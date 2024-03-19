using FluentValidation.Results;
using ICI.ProvaCandidato.Dados.Entities;
using ICI.ProvaCandidato.Dados.Exceptions;
using ICI.ProvaCandidato.Negocio;
using ICI.ProvaCandidato.Negocio.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

public class NoticiasController : Controller
{
    private readonly NoticiaService _noticiaService;
    private readonly TagService _tagService;
    private readonly UsuarioService _usuarioService;
    private readonly NoticiaValidator _noticiaValidator;

    public NoticiasController(NoticiaService noticiaService, TagService tagService, UsuarioService usuarioService,
                                NoticiaValidator noticiaValidator)
    {
        _noticiaService = noticiaService;
        _tagService = tagService;
        _usuarioService = usuarioService;
        _noticiaValidator = noticiaValidator;
    }

    public async Task<IActionResult> Noticias()
    {
        try
        {
            List<Noticia> noticias = await _noticiaService.GetAllNoticiasAsync();
            return View(noticias);
        }
        catch (CustomException ex)
        {
            TempData["Message"] = "Ocorreu um erro ao obter as notícias.";
            return RedirectToAction("Noticias", "Noticias");
        }
    }

    public async Task<IActionResult> Criar(int? id)
    {
        List<Tag> tags = new();
        List<Usuario> usuarios = new();

        try
        {
            tags = await _tagService.GetAllTagsAsync();
            usuarios = await _noticiaService.GetAllUsuariosAsync();
        }
        catch (CustomException ex)
        {
            TempData["Message"] = ex.Mensagem;
        }

        ViewBag.Tags = new SelectList(tags, "Id", "Descricao");
        ViewBag.Usuarios = new SelectList(usuarios, "Id", "Nome");

        if (id == null || id == 0)
        {
            ViewBag.Acao = "Criacao";
            return View(new Noticia());
        }
        else
        {
            ViewBag.Acao = "Edicao";
            Noticia noticia = null;

            try
            {
                noticia = await _noticiaService.GetNoticiaAsync(id);

                if (noticia == null)
                    return NotFound();
            }
            catch (CustomException ex)
            {
                TempData["Message"] = ex.Mensagem;
                return View(noticia);
            }

            ViewBag.UsuarioId = noticia.UsuarioId;
            ViewBag.TagsSelecionadas = await _noticiaService.GetTagsSelecionadasAsync(id);
            return View(noticia);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Criar(Noticia noticia, int[] TagsSelecionadas)
    {
        bool modelValido = validarModel(noticia, TagsSelecionadas);

        if (modelValido)
        {
            if (noticia.Id == 0)
                TempData["Message"] = "Notícia cadastrada com sucesso";
            else
                TempData["Message"] = "Notícia atualizada com sucesso";

            try
            {
                await _noticiaService.CreateOrUpdateNoticiaAsync(noticia, TagsSelecionadas);
                return RedirectToAction("Noticias", "Noticias");
            }
            catch (CustomException ex)
            {
                TempData["Message"] = ex.Message;
                return RedirectToAction("Noticias", "Noticias");
            }
        }

        var tags = await _tagService.GetAllTagsAsync();
        var usuarios = await _noticiaService.GetAllUsuariosAsync();

        ViewBag.Tags = new SelectList(tags, "Id", "Descricao");
        ViewBag.Usuarios = new SelectList(usuarios, "Id", "Nome");

        return View(noticia);
    }

    public async Task<IActionResult> ConfirmDelete(int? id)
    {
        if (id == null)
            return NotFound();

        var noticia = await _noticiaService.GetNoticiaAsync(id);
        if (noticia == null)
            return NotFound();

        try
        {
            var deletedNoticia = await _noticiaService.DeleteNoticiaAsync(id);
            TempData["Message"] = "Noticia excluída com sucesso.";
        }
        catch (CustomException ex)
        {
            TempData["Message"] = ex.Message;
        }

        return RedirectToAction("Noticias", "Noticias");
    }

    private bool validarModel(Noticia noticia, int[] tagsSelecionadas)
    {
        var validationResult = _noticiaValidator.Validate(noticia);

        if (tagsSelecionadas == null || tagsSelecionadas.Length == 0)
            validationResult.Errors.Add(new ValidationFailure("NoticiaTags", "Você deve selecionar pelo menos uma tag."));

        if (validationResult.Errors.Count > 0)
        {
            foreach (var error in validationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            return false;
        }

        return true;
    }

}

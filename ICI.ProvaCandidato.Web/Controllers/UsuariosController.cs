using ICI.ProvaCandidato.Dados;
using ICI.ProvaCandidato.Dados.Entities;
using ICI.ProvaCandidato.Dados.Exceptions;
using ICI.ProvaCandidato.Negocio;
using ICI.ProvaCandidato.Negocio.Validators;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

public class UsuariosController : Controller
{
    private readonly IciDbContext _context;
    private readonly UsuarioService _usuarioService;
    private readonly UsuarioValidator _usuarioValidator;

    public UsuariosController(UsuarioService usuarioService, IciDbContext context,
                              UsuarioValidator usuarioValidator)
    {
        _context = context;
        _usuarioService = usuarioService;
        _usuarioValidator = usuarioValidator;
    }

    public IActionResult Usuarios()
    {
        List<Usuario> usuarios = new();

        try
        {
            usuarios = _usuarioService.ObterTodosUsuarios();
        }
        catch (CustomException ex)
        {
            TempData["Message"] = ex.Mensagem;
        }

        return View(usuarios);
    }

    public IActionResult Criar(int? id)
    {
        Usuario usuario = new();

        if (id == null || id == 0)
        {
            ViewBag.Acao = "Criar";
            return View(new Usuario());
        }
        else
        {
            ViewBag.Acao = "Editar";

            try
            {
                usuario = _usuarioService.ObterUsuarioPorId(id.Value);
            }
            catch (CustomException ex)
            {
                TempData["Message"] = ex.Mensagem;
            }

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }
    }

    [HttpPost]
    public IActionResult Criar(Usuario usuario)
    {
        var validationResult = _usuarioValidator.Validate(usuario);

        if (validationResult.Errors.Count > 0)
        {
            foreach (var error in validationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
        }
        else
        {
            if (usuario.Id == 0)
            {
                try
                {
                    _usuarioService.AdicionarUsuario(usuario);
                    TempData["Message"] = "Usuário cadastrado com sucesso";
                }
                catch (CustomException ex)
                {
                    TempData["Message"] = ex.Mensagem;
                }
            }
            else
            {
                try
                {
                    _usuarioService.AtualizarUsuario(usuario);
                    TempData["Message"] = "Usuário atualizado com sucesso";
                }
                catch (CustomException ex)
                {
                    TempData["Message"] = ex.Mensagem;
                }
            }

            return RedirectToAction("Usuarios");
        }

        return View(usuario);
    }

    public IActionResult ConfirmDelete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        try
        {
            _usuarioService.ExcluirUsuario(id.Value);
            TempData["Message"] = "Usuário excluído com sucesso.";
        }
        catch (CustomException ex)
        {
            TempData["Message"] = ex.Mensagem;
        }

        return RedirectToAction("Usuarios");
    }
}

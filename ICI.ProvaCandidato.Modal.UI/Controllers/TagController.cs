using ICI.ProvaCandidato.Dados.Entities;
using ICI.ProvaCandidato.Dados.Exceptions;
using ICI.ProvaCandidato.Modal.UI.Models;
using ICI.ProvaCandidato.Negocio;
using ICI.ProvaCandidato.Negocio.Validators;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ICI.ProvaCandidato.Web.Controllers
{
    public class TagController : Controller
    {
        private readonly ILogger<TagController> _logger;
        private readonly TagService _tagService;
        private readonly TagValidator _tagValidator;

        public TagController(ILogger<TagController> logger, TagService tagService,
                              TagValidator tagValidator)
        {
            _logger = logger;
            _tagService = tagService;
            _tagValidator = tagValidator;
        }

        public async Task<IActionResult> Index()
        {
            List<Tag> tags = new();

            try
            {
                tags = await _tagService.GetAllTagsAsync();
            }
            catch (CustomException ex)
            {
                TempData["Message"] = ex.Mensagem;
            }

            return View(tags);
        }



        public async Task<IActionResult> Cadastro(int? id)
        {
            if (id == null)
            {
                ViewData["Acao"] = "Cadastro";
                return PartialView("_PartialViewCadastro", new Tag());
            }
            else
            {
                Tag tag = new();

                try
                {
                    tag = await _tagService.GetTagByIdAsync(id.Value);
                }
                catch (CustomException ex)
                {
                    TempData["Message"] = ex.Mensagem;
                    return View("Cadastro", tag);
                }

                if (tag == null)
                    return NotFound();

                ViewData["Acao"] = "Edicao";
                ViewData["Descricao"] = tag.Descricao;

                return View("Cadastro", tag);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Cadastro(Tag tag)
        {
            var validationResult = _tagValidator.Validate(tag);

            var message = "Tag cadastrada com sucesso";
            var titulo = "Cadastro";

            if (validationResult.Errors.Count > 0)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }
            else
            {
                if (tag.Id != 0)
                {
                    message = "Tag atualizada com sucesso";
                    titulo = "Edicao";
                }

                try
                {
                    await _tagService.CreateOrUpdateTagAsync(tag);

                    TempData["Message"] = message;
                    return RedirectToAction("Index");
                }
                catch (CustomException ex)
                {
                    TempData["Message"] = ex.Mensagem;
                    return RedirectToAction("Index");
                }
            }

            ViewData["Acao"] = titulo;
            return PartialView("_PartialViewCadastro", tag);
        }

        public async Task<IActionResult> ConfirmDelete(int id)
        {
            try
            {
                bool tagDeleted = await _tagService.DeleteTagAsync(id);
                if (!tagDeleted)
                    TempData["Message"] = "Não é possível excluir a Tag, pois ela está vinculada a uma ou mais notícias.";
                else
                    TempData["Message"] = "Tag excluída com sucesso.";

                return RedirectToAction("Index");

            }
            catch (CustomException ex)
            {
                TempData["Message"] = ex.Mensagem;
                return RedirectToAction("Index");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

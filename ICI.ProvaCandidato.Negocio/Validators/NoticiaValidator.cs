using FluentValidation;
using ICI.ProvaCandidato.Dados.Entities;

namespace ICI.ProvaCandidato.Negocio.Validators
{
    public class NoticiaValidator : AbstractValidator<Noticia>
    {
        public NoticiaValidator()
        {
            RuleFor(x => x.Titulo).
                NotEmpty().
                WithMessage("Titulo não pode estar vazio");

            RuleFor(x => x.Texto).
                NotEmpty().
                WithMessage("Texto não pode estar vazio");
        }
    }
}

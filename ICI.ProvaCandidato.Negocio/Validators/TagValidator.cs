using FluentValidation;
using ICI.ProvaCandidato.Dados.Entities;

namespace ICI.ProvaCandidato.Negocio.Validators
{
    public class TagValidator : AbstractValidator<Tag>
    {
        public TagValidator()
        {
            RuleFor(x => x.Descricao).
                NotEmpty().
                WithMessage("Descrição não pode estar vazio");
        }
    }
}

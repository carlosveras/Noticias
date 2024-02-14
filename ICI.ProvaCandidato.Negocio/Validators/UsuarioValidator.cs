using FluentValidation;
using ICI.ProvaCandidato.Dados.Entities;

namespace ICI.ProvaCandidato.Negocio.Validators
{
    public class UsuarioValidator : AbstractValidator<Usuario>
    {
        public UsuarioValidator()
        {
            RuleFor(x => x.Nome).
                NotEmpty().
                WithMessage("Nome não pode estar vazio");

            RuleFor(x => x.Senha).
                NotEmpty().
                WithMessage("Senha não pode estar vazio");

            RuleFor(x => x.Email).
                NotEmpty().
                WithMessage("Email não pode estar vazio");
        }
    }
}

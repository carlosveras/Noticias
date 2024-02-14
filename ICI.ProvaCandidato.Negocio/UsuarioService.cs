using ICI.ProvaCandidato.Dados;
using ICI.ProvaCandidato.Dados.Entities;
using ICI.ProvaCandidato.Dados.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ICI.ProvaCandidato.Negocio
{
    public class UsuarioService
    {
        private readonly IciDbContext _context;

        public UsuarioService(IciDbContext context)
        {
            _context = context;
        }

        public List<Usuario> ObterTodosUsuarios()
        {
            List<Usuario> usuarios = new();

            try
            {
                usuarios = _context.Usuarios.ToList();
            }
            catch (CustomException ex)
            {
                throw new CustomException("", ex);
            }

            return usuarios;
        }

        public Usuario ObterUsuarioPorId(int id)
        {
            Usuario usuario = new();

            try
            {
                usuario = _context.Usuarios.Find(id);
            }
            catch (CustomException ex)
            {
                throw new CustomException("", ex);
            }

            return usuario;
        }

        public void AdicionarUsuario(Usuario usuario)
        {
            try
            {
                _context.Usuarios.Add(usuario);
                _context.SaveChanges();
            }
            catch (CustomException ex)
            {
                throw new CustomException("", ex);
            }
        }

        public void AtualizarUsuario(Usuario usuario)
        {
            try
            {
                _context.Entry(usuario).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (CustomException ex)
            {
                throw new CustomException("", ex);
            }
        }

        public void ExcluirUsuario(int id)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == id);
            if (usuario != null)
            {
                try
                {
                    _context.Usuarios.Remove(usuario);
                    _context.SaveChanges();
                }
                catch (CustomException ex)
                {
                    throw new CustomException("", ex);
                }
            }
        }
    }
}

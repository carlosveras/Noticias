using ICI.ProvaCandidato.Dados;
using ICI.ProvaCandidato.Dados.Entities;
using ICI.ProvaCandidato.Dados.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICI.ProvaCandidato.Negocio
{
    public class NoticiaService : CustomException
    {
        private readonly IciDbContext _context;

        public NoticiaService(IciDbContext context)
        {
            _context = context;
        }

        public async Task<List<Noticia>> GetAllNoticiasAsync()
        {
            var lista = new List<Noticia>();

            try
            {
                lista = await _context.Noticias
                                      .Include(n => n.Usuario)
                                      .Include(n => n.NoticiaTags)
                                      .ThenInclude(nt => nt.Tag)
                                      .OrderBy(n => n.Id)
                                      .ThenBy(n => n.Titulo)
                                      .ToListAsync();
            }
            catch (CustomException ex)
            {
                throw new CustomException("", ex);
            }

            return lista;
        }

        public async Task<List<int>> GetTagsSelecionadasAsync(int? noticiaId)
        {
            var noticiaTags = await _context.NoticiaTags
                .Where(nt => nt.NoticiaId == noticiaId)
                .Select(nt => nt.TagId)
                .ToListAsync();

            return noticiaTags;
        }
        public async Task<List<Usuario>> GetAllUsuariosAsync()
        {
            try
            {
                var usuarios = await _context.Usuarios.ToListAsync();
                return usuarios;
            }
            catch (CustomException ex)
            {
                throw new CustomException("", ex);
            }
        }

        public async Task<Noticia> GetNoticiaAsync(int? id)
        {
            try
            {
                return await _context.Noticias.FindAsync(id);
            }
            catch (CustomException ex)
            {
                throw new CustomException("", ex);
            }
        }

        public async Task CreateOrUpdateNoticiaAsync(Noticia noticia, int[] TagsSelecionadas)
        {
            if (noticia.Id == 0)
            {
                try
                {
                    _context.Noticias.Add(noticia);
                    await _context.SaveChangesAsync();
                }
                catch (CustomException ex)
                {
                    throw new CustomException("", ex);
                }

                if (TagsSelecionadas != null)
                {
                    foreach (var tagId in TagsSelecionadas)
                    {
                        var noticiaTag = new NoticiaTag
                        {
                            NoticiaId = noticia.Id,
                            TagId = tagId
                        };
                        _context.NoticiaTags.Add(noticiaTag);
                    }
                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (CustomException)
                    {
                        throw new CustomException();
                    }
                }
            }
            else if (noticia.Id != 0)
            {
                try
                {
                    _context.Noticias.Update(noticia);
                    await _context.SaveChangesAsync();
                }
                catch (CustomException ex)
                {
                    throw new CustomException("", ex);
                }

                if (TagsSelecionadas != null)
                {
                    List<NoticiaTag> noticiaTagsExistentes = new();   

                    try
                    {
                        noticiaTagsExistentes = _context.NoticiaTags.Where(nt => nt.NoticiaId == noticia.Id).ToList();
                    }
                    catch (CustomException ex)
                    {
                        throw new CustomException("", ex);
                    }

                    foreach (var noticiaTag in noticiaTagsExistentes)
                    {
                        if (!TagsSelecionadas.Contains(noticiaTag.TagId))
                        {
                            _context.NoticiaTags.Remove(noticiaTag);
                        }
                    }

                    foreach (var tagId in TagsSelecionadas)
                    {
                        if (!noticiaTagsExistentes.Any(nt => nt.TagId == tagId))
                        {
                            var noticiaTag = new NoticiaTag
                            {
                                NoticiaId = noticia.Id,
                                TagId = tagId
                            };
                            _context.NoticiaTags.Add(noticiaTag);
                        }
                    }

                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (CustomException ex)
                    {
                        throw new CustomException("", ex);
                    }
                }
            }
        }

        public async Task<Noticia> DeleteNoticiaAsync(int? id)
        {
            var noticia = await _context.Noticias.FindAsync(id);
            if (noticia != null)
            {
                try
                {
                    _context.Noticias.Remove(noticia);
                    await _context.SaveChangesAsync();
                }
                catch (CustomException ex)
                {
                    throw new CustomException("", ex);
                }
            }

            return noticia;
        }
    }
}

using ICI.ProvaCandidato.Dados;
using ICI.ProvaCandidato.Dados.Entities;
using ICI.ProvaCandidato.Dados.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ICI.ProvaCandidato.Negocio
{
    public class TagService 
    {
        private readonly IciDbContext _context;

        public TagService(IciDbContext context)
        {
            _context = context;
        }

        public async Task<List<Tag>> GetAllTagsAsync()
        {
            List<Tag> lista = new();

            try
            {
                lista = await _context.Tags.ToListAsync();
                
            }
            catch (CustomException ex)
            {
                throw new CustomException("", ex);
            }

            return lista;
        }

        public async Task<Tag> GetTagByIdAsync(int id)
        {
            Tag tag = new();

            try
            {
                tag = await _context.Tags.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new CustomException("", ex);
            }

            return tag;
        }

        public async Task CreateOrUpdateTagAsync(Tag tag)
        {
            try
            {
                if (tag.Id == 0)
                    _context.Tags.Add(tag);
                else
                    _context.Tags.Update(tag);

                await _context.SaveChangesAsync();
            }
            catch (CustomException ex)
            {
                throw new CustomException("", ex);
            }
        }

        public async Task<bool> DeleteTagAsync(int id)
        {
            try
            {
                var tag = await _context.Tags.FindAsync(id);
                if (tag == null)
                    return false;

                var isTagLinkedToNoticia = await _context.NoticiaTags.AnyAsync(nt => nt.TagId == tag.Id);
                if (isTagLinkedToNoticia)
                    return false;

                _context.Tags.Remove(tag);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (CustomException ex)
            {
                throw new CustomException("", ex);
            }
        }
    }
}

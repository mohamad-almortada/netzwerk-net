using Microsoft.EntityFrameworkCore;
using Netzwerk.Data;
using Netzwerk.DTOs;
using Netzwerk.Model;
using AutoMapper;

namespace Netzwerk.Service {
    public class KeywordService : IKeywordService
    {

        private readonly ApiContext _context;
        private readonly IMapper _mapper;
        public KeywordService(ApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<KeywordDto> CreateKeywordAsync(KeywordDto keywordDto)
        {
            await _context.Keywords.AddAsync(new Keyword
            {
                Name = keywordDto.Name,
                Description = keywordDto.Description
            });
            await _context.SaveChangesAsync();
            var myKeywordDto = new KeywordDto
            {
                Name = keywordDto.Name,
                Description = keywordDto.Description
            };
            return myKeywordDto;
        }

        public async Task<bool> DeleteKeywordAsync(int id)
        {
            var keyword = _context.Keywords.Include(b=>b.UserKeywords).SingleOrDefault(b => b.Id == id);

            if (keyword == null)
            {
                return false;
            }

            _context.Keywords.Remove(keyword);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<KeywordDto> GetKeywordAsync(int id)
        {
            var keyword = await _context.Keywords.FindAsync(id);
            Console.WriteLine(keyword);
            if (keyword == null)
            {
                return null;
            }
            var myKeywordDto = _mapper.Map<KeywordDto>(keyword);
            
            return myKeywordDto;
        }

        public async Task<IEnumerable<KeywordDto>> GetKeywordsAsync()
        {
            var keywords = await _context.Keywords.ToListAsync();
            var keywordDto = keywords.Select(u => new KeywordDto
            {
                Id = u.Id,
                Name = u.Name,
                Description = u.Description,
            });
            return keywordDto;
          
        }

        public async Task UpdateKeywordAsync(Keyword keyword, int KeywordId)
        {
            if (KeywordId != keyword.Id)
            {
                throw new ArgumentException("KeywordId does not match keyword.Id");
            }

            _context.Entry(keyword).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }    
      
    }
}
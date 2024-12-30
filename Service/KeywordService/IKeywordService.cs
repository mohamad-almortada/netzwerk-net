using Netzwerk.DTOs;
using Netzwerk.Model;

namespace Netzwerk.Service
{
    public interface IKeywordService
    {
        Task<KeywordDto> CreateKeywordAsync(KeywordDto keyword);
        Task<KeywordDto> GetKeywordAsync(int id);
        Task<IEnumerable<KeywordDto>> GetKeywordsAsync();
        Task UpdateKeywordAsync(Keyword keyword, int KeywordId);
        Task<bool> DeleteKeywordAsync(int id);
    }
}
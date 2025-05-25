using Netzwerk.DTOs;

namespace Netzwerk.Interfaces;

public interface IVoteService
{
    Task<VoteDto> CreateVoteAsync(VoteDto voteDto);
    Task<VoteDto?> GetVoteAsync(int voteId);
    Task<IEnumerable<VoteDto>> GetVotesAsync();
    Task<VoteDto?> UpdateVoteAsync(int voteId, VoteDto voteDto);
    Task<bool> DeleteVoteAsync(int voteId);
}
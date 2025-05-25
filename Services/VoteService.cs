using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Netzwerk.Data;
using Netzwerk.DTOs;
using Netzwerk.Interfaces;
using Netzwerk.Model;

namespace Netzwerk.Services;

public class VoteService(ApiContext apiContext, IMapper mapper) : IVoteService
{
    public async Task<VoteDto> CreateVoteAsync(VoteDto voteDto)
    {
        var vote = mapper.Map<Vote>(voteDto);
        await apiContext.AddAsync(vote);
        vote.CreatedAt = DateTime.Now;
        await apiContext.SaveChangesAsync();
        return mapper.Map<VoteDto>(vote);
    }

    public async Task<VoteDto?> GetVoteAsync(int voteId)
    {
        var vote = await apiContext.Votes.FindAsync(voteId);
        if (vote == null) return null;
        return mapper.Map<VoteDto>(vote);
    }

    public async Task<IEnumerable<VoteDto>> GetVotesAsync()
    {
        var votes = await apiContext.Votes.ToListAsync();
        return mapper.Map<IEnumerable<VoteDto>>(votes);
    }

    public async Task<VoteDto?> UpdateVoteAsync(int voteId, VoteDto voteDto)
    {
        var vote = await apiContext.Votes.FindAsync(voteId);
        if (vote == null) return null;
        vote.VoteValue = voteDto.VoteValue;
        await apiContext.SaveChangesAsync();
        return mapper.Map<VoteDto>(vote);
    }

    public async Task<bool> DeleteVoteAsync(int voteId)
    {
        var vote = await apiContext.Votes.FindAsync(voteId);
        if (vote == null) return false;
        apiContext.Votes.Remove(vote);
        await apiContext.SaveChangesAsync();
        return true;
    }
}
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Netzwerk.Data;
using Netzwerk.DTOs;
using Netzwerk.Interfaces;
using Netzwerk.Model;

namespace Netzwerk.Services;

public class MarkerService(ApiContext apiContext, IMapper mapper) : IMarkerService
{
    public async Task<MarkerDto> CreateMarkerAsync(MarkerDto markerDto)
    {
        var markerToAdd = mapper.Map<Marker>(markerDto);
        var marker = await apiContext.Markers.AddAsync(markerToAdd);
        await apiContext.SaveChangesAsync();
        return markerDto;
    }

    public Task<MarkerDto?> GetMarkerAsync(int markerId)
    {
        throw new NotImplementedException();
    }

    public Task<MarkerDto?> GetMarkerAsync(decimal latitude, decimal longitude)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<MarkerDto>> GetMarkersAsync()
    {
        var markers = await apiContext.Markers.ToListAsync();
        var dtos = mapper.Map<IEnumerable<MarkerDto>>(markers);
        return dtos;
    }

    public Task<MarkerDto?> UpdateMarkerAsync(int markerId, MarkerDto markerDto)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteMarkerAsync(int markerId)
    {
        throw new NotImplementedException();
    }
}
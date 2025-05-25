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
        markerToAdd.CreatedAt = DateTime.Now;
        markerToAdd.UpdatedAt = DateTime.Now;

        await apiContext.SaveChangesAsync();
        return markerDto;
    }

    public async Task<MarkerDto?> GetMarkerAsync(int markerId)
    {
        var marker = await apiContext.Markers.FindAsync(markerId);
        return mapper.Map<MarkerDto>(marker);
    }

    public async Task<MarkerDto?> GetMarkerAsync(decimal latitude, decimal longitude)
    {
        var marker = await apiContext.Markers.FindAsync(latitude, longitude);
        return mapper.Map<MarkerDto>(marker);
    }

    public async Task<IEnumerable<MarkerDto>> GetMarkersAsync()
    {
        var markers = await apiContext.Markers.ToListAsync();
        var dtos = mapper.Map<IEnumerable<MarkerDto>>(markers);
        return dtos;
    }

    public async Task<MarkerDto?> UpdateMarkerAsync(int markerId, MarkerDto markerDto)
    {
        var marker = await apiContext.Markers.FindAsync(markerId);
        if (marker == null) return null;
        marker.Title = markerDto.Title;
        marker.Description = markerDto.Description;
        marker.Lat = markerDto.Lat;
        marker.Lon = markerDto.Lon;
        marker.UpdatedAt = DateTime.Now;
        await apiContext.SaveChangesAsync();
        return markerDto;
    }

    public async Task<bool> DeleteMarkerAsync(int markerId)
    {
        var marker = await apiContext.Markers.FindAsync(markerId);
        if (marker == null) return false;
        apiContext.Markers.Remove(marker);
        await apiContext.SaveChangesAsync();
        return true;
    }
}
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Netzwerk.Data;
using Netzwerk.DTOs;
using Netzwerk.Interfaces;
using Netzwerk.Model;

namespace Netzwerk.Services;

public class MapService(ApiContext apiContext, IMapper mapper) : IMapService
{
    public async Task<MapDto> CreateMapAsync(MapDto mapDto)
    {
        var mapToAdd = mapper.Map<Map>(mapDto);
        await apiContext.Maps.AddAsync(mapToAdd);
        await apiContext.SaveChangesAsync();
        return mapDto;
    }

    public async Task<MapDto?> GetMapAsync(int mapId)
    {
        var map = await apiContext.Maps.FindAsync(mapId);
        return map == null ? null : mapper.Map<MapDto>(map);
    }

    public async Task<IEnumerable<MapDto>> GetMapsAsync()
    {
        var map = await apiContext.Maps.ToListAsync();
        return mapper.Map<IEnumerable<MapDto>>(map);
    }

    public async Task<MapDto?> UpdateMapAsync(int mapId, MapDto mapDto)
    {
        var map = await apiContext.Maps.FindAsync(mapId);
        if (map == null) return null;
        map.UpdatedAt = DateTime.Now;
        map.Title = mapDto.Title;
        map.Description = mapDto.Description;
        map.IsPublic = mapDto.IsPublic;
        await apiContext.SaveChangesAsync();
        return mapDto;
    }

    public async Task<bool> DeleteMapAsync(int mapId)
    {
        var map = await apiContext.Maps.FindAsync(mapId);
        if (map == null) return false;
        apiContext.Maps.Remove(map);
        await apiContext.SaveChangesAsync();
        return true;
    }
}
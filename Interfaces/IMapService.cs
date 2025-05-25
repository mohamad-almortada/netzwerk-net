using Netzwerk.DTOs;

namespace Netzwerk.Interfaces;

public interface IMapService
{
    Task<MapDto> CreateMapAsync(MapDto mapDto);
    Task<MapDto?> GetMapAsync(int mapId);
    Task<IEnumerable<MapDto>> GetMapsAsync();
    Task<MapDto?> UpdateMapAsync(int mapId, MapDto mapDto);
    Task<bool> DeleteMapAsync(int mapId);
}
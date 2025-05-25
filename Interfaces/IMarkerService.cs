using Netzwerk.DTOs;

namespace Netzwerk.Interfaces;

public interface IMarkerService
{
    Task<MarkerDto> CreateMarkerAsync(MarkerDto markerDto);
    Task<MarkerDto?> GetMarkerAsync(int markerId);
    Task<MarkerDto?> GetMarkerAsync(decimal latitude, decimal longitude);
    Task<IEnumerable<MarkerDto>> GetMarkersAsync();
    Task<MarkerDto?> UpdateMarkerAsync(int markerId, MarkerDto markerDto);
    Task<bool> DeleteMarkerAsync(int markerId);
}
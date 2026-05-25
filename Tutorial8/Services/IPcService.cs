using Tutorial8.DTOs;

namespace Tutorial8.Services;

public interface IPcService
{
    Task<List<PcGetAllDto>> GetAllAsync();
    Task<PcGetComponentsDto?> GetComponentsAsync(int id);
    Task<PcGetAllDto> CreateAsync(PcCreateDto dto);
    Task<PcGetAllDto?> UpdateAsync(int id, PcUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}

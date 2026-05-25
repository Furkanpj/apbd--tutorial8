using Microsoft.EntityFrameworkCore;
using Tutorial8.Data;
using Tutorial8.DTOs;
using Tutorial8.Models;

namespace Tutorial8.Services;

public class PcService : IPcService
{
    private readonly AppDbContext _context;

    public PcService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<PcGetAllDto>> GetAllAsync()
    {
        return await _context.PCs
            .Select(pc => new PcGetAllDto
            {
                Id = pc.Id,
                Name = pc.Name,
                Weight = pc.Weight,
                Warranty = pc.Warranty,
                CreatedAt = pc.CreatedAt,
                Stock = pc.Stock
            })
            .ToListAsync();
    }

    public async Task<PcGetComponentsDto?> GetComponentsAsync(int id)
    {
        var pc = await _context.PCs
            .Include(p => p.PCComponents)
                .ThenInclude(pc => pc.Component)
                    .ThenInclude(c => c.Manufacturer)
            .Include(p => p.PCComponents)
                .ThenInclude(pc => pc.Component)
                    .ThenInclude(c => c.Type)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (pc == null) return null;

        return new PcGetComponentsDto
        {
            Id = pc.Id,
            Name = pc.Name,
            Weight = pc.Weight,
            Warranty = pc.Warranty,
            CreatedAt = pc.CreatedAt,
            Stock = pc.Stock,
            Components = pc.PCComponents.Select(pcComp => new PcComponentItemDto
            {
                Amount = pcComp.Amount,
                Component = new ComponentDetailDto
                {
                    Code = pcComp.Component.Code,
                    Name = pcComp.Component.Name,
                    Description = pcComp.Component.Description,
                    Manufacturer = new ManufacturerDto
                    {
                        Id = pcComp.Component.Manufacturer.Id,
                        Abbreviation = pcComp.Component.Manufacturer.Abbreviation,
                        FullName = pcComp.Component.Manufacturer.FullName,
                        FoundationDate = pcComp.Component.Manufacturer.FoundationDate
                    },
                    Type = new ComponentTypeDto
                    {
                        Id = pcComp.Component.Type.Id,
                        Abbreviation = pcComp.Component.Type.Abbreviation,
                        Name = pcComp.Component.Type.Name
                    }
                }
            }).ToList()
        };
    }

    public async Task<PcGetAllDto> CreateAsync(PcCreateDto dto)
    {
        var pc = new PC
        {
            Name = dto.Name,
            Weight = dto.Weight,
            Warranty = dto.Warranty,
            CreatedAt = dto.CreatedAt,
            Stock = dto.Stock
        };

        _context.PCs.Add(pc);
        await _context.SaveChangesAsync();

        return new PcGetAllDto
        {
            Id = pc.Id,
            Name = pc.Name,
            Weight = pc.Weight,
            Warranty = pc.Warranty,
            CreatedAt = pc.CreatedAt,
            Stock = pc.Stock
        };
    }

    public async Task<PcGetAllDto?> UpdateAsync(int id, PcUpdateDto dto)
    {
        var pc = await _context.PCs.FindAsync(id);
        if (pc == null) return null;

        pc.Name = dto.Name;
        pc.Weight = dto.Weight;
        pc.Warranty = dto.Warranty;
        pc.CreatedAt = dto.CreatedAt;
        pc.Stock = dto.Stock;

        await _context.SaveChangesAsync();

        return new PcGetAllDto
        {
            Id = pc.Id,
            Name = pc.Name,
            Weight = pc.Weight,
            Warranty = pc.Warranty,
            CreatedAt = pc.CreatedAt,
            Stock = pc.Stock
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var pc = await _context.PCs.FindAsync(id);
        if (pc == null) return false;

        _context.PCs.Remove(pc);
        await _context.SaveChangesAsync();
        return true;
    }
}

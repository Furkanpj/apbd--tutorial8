namespace Tutorial8.DTOs;

public class PcGetComponentsDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public double Weight { get; set; }
    public int Warranty { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Stock { get; set; }
    public List<PcComponentItemDto> Components { get; set; } = new();
}

public class PcComponentItemDto
{
    public int Amount { get; set; }
    public ComponentDetailDto Component { get; set; } = null!;
}

public class ComponentDetailDto
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ManufacturerDto Manufacturer { get; set; } = null!;
    public ComponentTypeDto Type { get; set; } = null!;
}

public class ManufacturerDto
{
    public int Id { get; set; }
    public string Abbreviation { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public DateOnly FoundationDate { get; set; }
}

public class ComponentTypeDto
{
    public int Id { get; set; }
    public string Abbreviation { get; set; } = null!;
    public string Name { get; set; } = null!;
}

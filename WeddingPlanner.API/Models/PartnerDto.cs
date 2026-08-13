namespace WeddingPlanner.API.Models;

public class PartnerDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public decimal CommissionPct { get; set; }

    public string? CategoryName { get; set; }
}
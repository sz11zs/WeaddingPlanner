namespace WeddingPlanner.Models;

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

public class Partner
{
    public int Id { get; set; }

    public int CategoryId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public decimal CommissionPct { get; set; }

    [ValidateNever]
    public PartnerCategory Category { get; set; } = null!;
}
namespace WeddingPlanner.Models;

public class PartnerCategory
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public ICollection<Partner> Partners { get; set; }
        = new List<Partner>();
}

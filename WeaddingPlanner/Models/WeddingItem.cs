namespace WeddingPlanner.Models;

public class WeddingItem
{
    public int Id { get; set; }

    public int WeddingId { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int? PartnerId { get; set; }

    public Wedding Wedding { get; set; } = null!;

    public Partner? Partner { get; set; }
}
namespace WeddingPlanner.Models;

public class Menu
{
    public int Id { get; set; }

    public int PartnerId { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public Partner Partner { get; set; } = null!;
}
namespace WeddingPlanner.Models;

public class WeddingTemplateItem
{
    public int Id { get; set; }

    public int WeddingTemplateId { get; set; }

    public string Name { get; set; } = string.Empty;

    public WeddingTemplate WeddingTemplate { get; set; } = null!;
}
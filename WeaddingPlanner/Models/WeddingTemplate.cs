namespace WeddingPlanner.Models;

public class WeddingTemplate
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public ICollection<WeddingTemplateItem> Items { get; set; }
        = new List<WeddingTemplateItem>();
}
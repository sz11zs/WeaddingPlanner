namespace WeddingPlanner.Models
{
    public class Wedding
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        public string? Location { get; set; }

        // Jedno vjenčanje može imati više stavki
        public ICollection<WeddingItem> Items { get; set; }
            = new List<WeddingItem>();
    }
}
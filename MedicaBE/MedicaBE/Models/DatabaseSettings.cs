namespace MedicaBE.Models
{
    public class DatabaseSettings
    {
        public Dictionary<string, string> CollectionNames { get; set; }
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; }
    }
}

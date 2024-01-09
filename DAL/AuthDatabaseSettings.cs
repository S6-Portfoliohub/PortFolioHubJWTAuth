namespace DAL
{
    public class AuthDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string ProjectCollectionName { get; set; } = null!;
    }
}

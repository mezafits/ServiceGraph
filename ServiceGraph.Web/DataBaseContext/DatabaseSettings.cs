public class DatabaseSettings
{
    public string DatabaseName { get; set; }
    public string ConnectionString { get; set; }
    public string AccountEndpoint { get; set; }
    public bool UseInMemoryDatabase { get; set; }
    public bool UseManagedIdentity { get; set; }
} 
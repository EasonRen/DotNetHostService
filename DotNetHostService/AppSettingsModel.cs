namespace DotNetHostService
{
    public class AppSettingsModel
    {
        public int RefreshInterval { get; set; }
        public string ConnectString { get; set; }
        public string StorageConnectionString { get; set; }
        public string BlobContainerName { get; set; }
        public string LocalFilePath { get; set; }
    }
}
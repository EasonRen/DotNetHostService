using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DotNetHostService
{
    public class AzureStorage
    {
        private readonly AppSettingsModel _appSettingsModel;
        //private CloudStorageAccount storageAccount = null;
        //private CloudBlobContainer cloudBlobContainer = null;

        public AzureStorage(IOptions<AppSettingsModel> settings)
        {
            _appSettingsModel = settings.Value;

            //InitAzureStorageConnectAsync().GetAwaiter().GetResult();
        }

        public async Task InitAzureStorageConnectAsync()
        {
            string localFileName = string.Empty;
            string sourceFile = string.Empty;

            await Task.Delay(TimeSpan.FromSeconds(5));
            //await Task.Yield();
            Console.WriteLine(_appSettingsModel.StorageConnectionString);

            DirectoryInfo directory = new DirectoryInfo(_appSettingsModel.LocalFilePath);

            if (directory.Exists)
            {
                var files = directory.GetFiles();
            }

            //if (CloudStorageAccount.TryParse(_appSettingsModel.StorageConnectionString, out storageAccount))
            //{
            //    CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();

            //    cloudBlobContainer = cloudBlobClient.GetContainerReference(_appSettingsModel.BlobContainerName);
            //    bool existBlobContainer = await cloudBlobContainer.ExistsAsync();

            //    if (existBlobContainer)
            //    {
            //        CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(localFileName);
            //        await cloudBlockBlob.UploadFromFileAsync(sourceFile);
            //        Console.WriteLine("success");
            //    }
            //}
        }
    }
}

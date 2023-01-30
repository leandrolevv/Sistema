using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Text.RegularExpressions;

namespace Main.Services.CloudFunctions
{
    public class AzureServices : ICloudServices
    {
        public async void DeleteFileIfExists(string containerName, string imageName)
        {
            var cloudBlobContainer = CreateBlobContainer(containerName);
            var blob = cloudBlobContainer.GetBlobReference(imageName);
            await blob.DeleteIfExistsAsync();
        }

        public async Task<string> UploadFile(string containerName, string imageName, string base64Image)
        {
            var cloudBlobContainer = CreateBlobContainer(containerName);
            var imageBase64 = new Regex(@"^data:image\/[a-z]+;base64,").Replace(base64Image, "");
            var file = Convert.FromBase64String(imageBase64);
            var blobUpload = cloudBlobContainer.GetBlockBlobReference(imageName);

            using (var stream = new MemoryStream(file))
            {
                await blobUpload.UploadFromStreamAsync(stream);
            }

            return blobUpload.Uri.AbsoluteUri;
        }

        private CloudBlobContainer CreateBlobContainer(string containerName)
        {
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(Configuration.AzureBlobConnectionString);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            return cloudBlobClient.GetContainerReference(containerName);
        }
    }
}

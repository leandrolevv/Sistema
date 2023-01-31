namespace Main.Services.CloudFunctions
{
    public interface ICloudServices
    {
        public void DeleteFileIfExists(string containerName, string imageName);

        public Task<string> UploadFile(string containerName, string imageName, string base64Image);
    }
}

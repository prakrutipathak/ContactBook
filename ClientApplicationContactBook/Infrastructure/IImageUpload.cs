namespace ClientApplicationContactBook.Infrastructure
{
    public interface IImageUpload
    {
        string AddImageFileToPath(IFormFile imageFile);
    }
}

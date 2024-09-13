using ClientApplicationContactBook.Infrastructure;

namespace ClientApplicationContactBook.Implementation
{
    public class ImageUpload: IImageUpload
    {
        public string AddImageFileToPath(IFormFile imageFile)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", imageFile.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                imageFile.CopyTo(stream);
                return imageFile.FileName;
            }
        }
    }
}

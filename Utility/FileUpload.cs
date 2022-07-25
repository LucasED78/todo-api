using System.Web;

namespace TodoAPI.Utility
{
  public static class FileUpload
  {
    public async static Task<string?> Upload(IFormFile file)
    {
      var folder = Path.Combine("Resources", "Files");
      var fileName = file.FileName;
      var destination = Path.Combine(Directory.GetCurrentDirectory(), folder, fileName);

      using var fs = new FileStream(destination, FileMode.Create);

      await file.CopyToAsync(fs);

      return destination;
    }
  }
}

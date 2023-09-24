using Microsoft.AspNetCore.Http;

namespace RealEstate.Mediator.Utilities
{
    public static class Util
    {
        public static byte[] ConvertFormFileToByteArray(IFormFile formFile)
        {
            using MemoryStream memoryStream = new();
            formFile.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }
}

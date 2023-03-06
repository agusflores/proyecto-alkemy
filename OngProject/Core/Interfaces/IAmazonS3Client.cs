using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace OngProject.Core.Interfaces
{
    public interface IAmazonS3Client
    {
        // Almacena un archivo en un Bucket(Carpeta remota)
        // Devuelve el nuevo nombre del archivo asignado automáticamente.
        // Dicho nombre puede ser usado para ser guardado en la base de datos local.
        Task<string> UploadObject(Stream stream);
        Task<string> UploadObject(Stream stream, string fileName);
        Task<string> UploadObject(IFormFile formFile);

        Task<GetObjectResponse> GetObject(string objectName);

        Task DeleteObject(string objectName);
    }
}
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace DatingApp.BL.Services.Interfaces;

public interface IPhotoService
{
    public Task<UploadResult> AddPhotoAsync(IFormFile file);
    public Task<DeletionResult> DeletePhotoAsync(string publicId);
}
using System.Net;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.BL.Infrastructure;
using DatingApp.BL.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace DatingApp.BL.Services;

public class PhotoService : IPhotoService
{
    private readonly Cloudinary _cloudinary;
    public PhotoService(IOptions<CloudinarySettings> options)
    {
        var account = new Account
        (
            options.Value.CloudName,
            options.Value.ApiKey,
            options.Value.ApiSecret
        );

        _cloudinary = new Cloudinary(account);
    }
    public async Task<UploadResult> AddPhotoAsync(IFormFile file)
    {
        ImageUploadResult uploadResult;

        if (file.Length > 0)
        {
            await using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face"),
                Folder = "DatingApp"
            };

            uploadResult = await _cloudinary.UploadAsync(uploadParams);
        }
        else
            throw new HttpException(HttpStatusCode.BadRequest, "Photo file is empty");

        return uploadResult;
    }

    public async Task<DeletionResult> DeletePhotoAsync(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);

        return await _cloudinary.DestroyAsync(deleteParams);
    }
}
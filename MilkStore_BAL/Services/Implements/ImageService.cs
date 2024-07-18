using Firebase.Storage;
using Microsoft.Extensions.Configuration;
using MilkStore_BAL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore_BAL.Services.Implements
{
    public class ImageService : IImageService
    {
        private readonly FirebaseStorage _firebaseStorage;

        public ImageService(IConfiguration configuration)
        {
            _firebaseStorage = new FirebaseStorage(
                configuration["Firebase:Storage:StorageBucket"],
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(configuration["Firebase:Storage:ApiKey"]),
                    ThrowOnCancel = true,
                }
            );
        }

        public async Task<string> UploadImageAsync(Stream imageStream, string fileName)
        {
            var task = _firebaseStorage
                .Child("images")
                .Child(fileName)
                .PutAsync(imageStream);

            return await task;
        }
    }
}

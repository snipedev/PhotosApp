using ClientApp.Model;
using RestEase;

namespace ClientApp
{

    [BasePath("api/Photos")]
    public interface IPhotoApi
    {
        [Header("Content-Type")]
        public string ContentType { get; set; }

        [Header("Accept")]
        public string Accept { get; set; }

        [Get("{tag}")]
        public Task<IList<Photos>> GetPhotos([Path("tag")] string tag, [Query("page")] int page, [Query("perPage")] int perPage );
    }
}

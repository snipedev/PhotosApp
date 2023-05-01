using ClientApp.Model;
using RestEase;

namespace ClientApp
{
    public class Client
    {
        public IPhotoApi _photoApiClient { get; set; }

        public Client()
        {
            _photoApiClient = RestClient.For<IPhotoApi>("https://localhost:44374");
            _photoApiClient.ContentType = "application/json";
        }

        public async Task<IList<Photos>> GetPhotosByTagAsync(string Tag, int page = 1,int perPage = 100)
        {
            var response = await _photoApiClient.GetPhotos(Tag,page,perPage);

            return response;

        }
    }
}
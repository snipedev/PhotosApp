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

        public async Task<IList<Photos>> GetPhotosByTagAsync(string Tag)
        {
            var response = await _photoApiClient.GetPhotos(Tag);

            return response;

        }
    }
}
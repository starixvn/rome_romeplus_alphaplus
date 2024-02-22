using AlphaPlus.Core.Models;

namespace AlphaPlus.Main.Site.Features.CustomerProfile
{
    public interface IProfilePictureService
    {
        Task<ActorProfilePictureModel> GetActorProfilePicture(int custId);

        Task<Result> UpdateActorProfilePicture(UpdateActorProfilePictureParam param);
    }

    public class ProfilePictureService(HttpClient httpClient) : IProfilePictureService
    {
        private readonly HttpClient httpClient = httpClient;

        public Task<ActorProfilePictureModel> GetActorProfilePicture(int custId)
            => httpClient.GetAsJsonAsync<ActorProfilePictureModel>($"customer/profile-picture?custId={custId}");

        public Task<Result> UpdateActorProfilePicture(UpdateActorProfilePictureParam param)
            => httpClient.PostAsJsonAsync<Result>("customer/profile-picture", param);
    }
}
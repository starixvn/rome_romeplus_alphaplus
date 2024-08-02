using AlphaPlus.AccountInfo.Site.Features.ProfilePicture.GetLibrary;
using AlphaPlus.AccountInfo.Site.Features.ProfilePicture.Update;
using AlphaPlus.Core.Configurations;
using AlphaPlus.Core.Models;
using Fanex.Data.Repository;

namespace AlphaPlus.AccountInfo.Site.Features.ProfilePicture
{
    public interface IProfilePictureService
    {
        Task<IEnumerable<ProfilePictureModel>> FetchProfilePictureLibrary();
        Task<Result> UpdateProfilePicture(int actorId, int imageId, byte[] image, string ip);
    }

    public class ProfilePictureService : IProfilePictureService
    {
        private readonly IDynamicRepository dynamicRepository;
        public ProfilePictureService(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<IEnumerable<ProfilePictureModel>> FetchProfilePictureLibrary()
        {
            var binaryProfilePictures = await dynamicRepository.FetchAsync<ProfilePictureDTO>(
                 new SelectProfilePictureLibraryCriteria(AppSettings.InsertProfilePictureSiteMode)).ConfigureAwait(false);

            return binaryProfilePictures.Select(profilePictureDto => new ProfilePictureModel(profilePictureDto.ImageId, profilePictureDto.Image));
        }

        public async Task<Result> UpdateProfilePicture(int custId, int imageId, byte[] image, string ip)
        {
            var command = new UpdateProfilePictureCommand(custId, imageId, image, AppSettings.InsertProfilePictureSiteMode, ip, true);
            await dynamicRepository.ExecuteAsync(command);

            return Result.CreateErrorResult(command.ErrMsg, command.ErrCode);
        }
    }
}
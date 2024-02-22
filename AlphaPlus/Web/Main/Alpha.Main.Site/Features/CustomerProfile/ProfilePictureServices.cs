using Alpha.Main.Configurations;
using AlphaPlus.Core.Models;
using Fanex.Data.Repository;

namespace Alpha.Main.Site.Features.CustomerProfile
{
    public interface IProfilePictureService
    {
        Task<ActorProfilePictureModel> GetActorProfilePicture(int custId);

        Task<Result> UpdateActorProfilePicture(UpdateActorProfilePictureParam param);
    }

    public class ProfilePictureService(IDynamicRepository dynamicRepository) : IProfilePictureService
    {
        private readonly IDynamicRepository dynamicRepository = dynamicRepository;

        public async Task<ActorProfilePictureModel> GetActorProfilePicture(int custId)
        {
            var model = await dynamicRepository.GetAsync<ActorProfilePictureModel>(new SelectActorProfilePictureCriteria(custId));
            return model ?? new ActorProfilePictureModel();
        }

        public async Task<Result> UpdateActorProfilePicture(UpdateActorProfilePictureParam param)
        {
            var command = new UpdateActorProfilePictureMainServerCommand(
                param.ActorId, AppSettings.IsMBCSite ? 1 : 2, param.IP);

            await dynamicRepository.ExecuteAsync(command);

            return Result.CreateErrorResult(command.ErrMsg, command.ErrCode);
        }
    }
}
using Alpha.Main.Configurations;
using Alpha.Main.ProfilePicture.Commands;
using Alpha.Main.ProfilePicture.Criteria;
using Alpha.Main.ProfilePicture.Models;
using Alpha.Main.ProfilePicture.Params;
using AlphaPlus.Core.Models;
using Fanex.Data.Repository;

namespace Alpha.Main.ProfilePicture.Services
{
    public interface ProfilePictureServices
    {
        Task<ActorProfilePictureModel> GetActorProfilePicture(int custId);

        Task<Result> UpdateActorProfilePicture(UpdateActorProfilePictureParam param);
    }

    public class ProfilePictureServicesImpl(IDynamicRepository dynamicRepository) : ProfilePictureServices
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
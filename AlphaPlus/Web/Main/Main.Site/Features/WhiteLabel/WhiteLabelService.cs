using Fanex.Data.Repository;

namespace AlphaPlus.Main.Site.Features.WhiteLabel
{
    public interface IWhiteLabelService
    {
        Task<bool> IsOfferedWhiteLabel(int custId);
    }

    public class WhiteLabelService : IWhiteLabelService
    {
        private readonly IDynamicRepository dynamicRepository;

        public WhiteLabelService(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<bool> IsOfferedWhiteLabel(int custId)
        {
            var commmand = new CheckExistsWhiteLabelCommand(custId);
            await dynamicRepository.ExecuteAsync(commmand);
            return commmand.IsExists;
        }
    }
}
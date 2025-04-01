using Fanex.Data.Repository;

namespace AlphaPlus.Main.Site.Features.Tour;

public interface IRewardTourService
{
    Task<bool> IsShow(int custId);
}

public class RewardTourService(IDynamicRepository dynamicRepository) : IRewardTourService
{
    private readonly IDynamicRepository dynamicRepository = dynamicRepository;

    public async Task<bool> IsShow(int custId)
    {
        var criteria = new CheckShowRewardTourCommand(custId);
        await dynamicRepository.ExecuteAsync(criteria);

        return criteria.IsShow;
    }
}
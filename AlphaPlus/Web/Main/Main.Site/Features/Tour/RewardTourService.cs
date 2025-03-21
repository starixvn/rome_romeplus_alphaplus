using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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
        var criteria = new ShowRewardTourCommand(custId);
        await dynamicRepository.ExecuteAsync(criteria);

        return criteria.IsShow;
    }
}
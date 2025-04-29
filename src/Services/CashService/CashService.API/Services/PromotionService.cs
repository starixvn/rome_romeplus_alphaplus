using AlphaPlus.CashService.API.Models;
using AlphaPlus.CashService.API.Models.Promotion;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Services;

public interface IPromotionService
{
    Task<IEnumerable<PromotionModel>> GetPromotion();

    Task<ResultModel> AddPromotion(AddPromotionModel addPromotionMdoel);
}
public class PromotionService(IDynamicRepository repository) : IPromotionService
{
    public Task<ResultModel> AddPromotion(AddPromotionModel addPromotionMdoel)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<PromotionModel>> GetPromotion()
    {
        var result = CreateFakePromotionModel();

        return result;
    }

    private static List<PromotionModel> CreateFakePromotionModel()
    {
        return new List<PromotionModel>()
        {
            new PromotionModel
            {
                PromotionTitle = "Promotion1",
                PromotionType = "Rebase on Turnover",
                SportsPercentage = 1,
                SportsMaxPayout = 10,
                VirtualSportsPercentage = 1,
                VirtualSportsMaxPayout = 10,
                GamesPercentage = 1,
                GamesMaxPayout = 10,
                LiveCasinoPercentage = 1,
                LiveCasinoMaxPayout = 10
            },
            new PromotionModel
            {
                PromotionTitle = "Promotion2",
                PromotionType = "Rebase on Turnover 1",
                SportsPercentage = 1,
                SportsMaxPayout = 8,
                VirtualSportsPercentage = 1,
                VirtualSportsMaxPayout = 10,
                GamesPercentage = 1,
                GamesMaxPayout = 10,
                LiveCasinoPercentage = 1,
                LiveCasinoMaxPayout = 10
            },
            new PromotionModel
            {
                PromotionTitle = "Promotion3",
                PromotionType = "Rebase on Turnover 2",
                SportsPercentage = 1,
                SportsMaxPayout = 8,
                VirtualSportsPercentage = 1,
                VirtualSportsMaxPayout = 9,
                GamesPercentage = 1,
                GamesMaxPayout = 10,
                LiveCasinoPercentage = 1,
                LiveCasinoMaxPayout = 9
            }
        };
    }

}

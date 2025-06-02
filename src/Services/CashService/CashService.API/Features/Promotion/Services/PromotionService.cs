namespace AlphaPlus.CashService.API.Features.Promotion.Services;

using Criteria;
using Fanex.Data.Repository;
using Models;
using Shared.Models;

public interface IPromotionService
{
    Task<IEnumerable<PromotionModel>> GetPromotion(string promotionType, string status);

    Task<ResultModel> AddPromotion(AddPromotionModel addPromotionMdoel);

    Task<ResultModel> EditPromotion(AddPromotionModel addPromotionMdoel);

    Task<ResultModel> DeletePromotion(int custId, int promotionId);
}

public class PromotionService(IDynamicRepository repository) : IPromotionService
{
    private List<PromotionModel> promotionModels = CreateFakePromotionModel();

    public async Task<ResultModel> AddPromotion(AddPromotionModel addPromotionMdoel)
    {
        var addPromotionCriteria = new AddPromotionCriteria
        {
            PromotionType = addPromotionMdoel.PromotionType,
            FromDate = addPromotionMdoel.FromDate,
            ToDate = addPromotionMdoel.ToDate,
            Provider = addPromotionMdoel.Provider,
            SportsPercentage = addPromotionMdoel.SportsPercentage,
            SportsMinTurnover = addPromotionMdoel.SportsMinTurnover,
            SportsMaxPayout = addPromotionMdoel.SportsMaxPayout,
            VirtualSportsPercentage = addPromotionMdoel.VirtualSportsPercentage,
            VirtualSportsMinTurnover = addPromotionMdoel.VirtualSportsMinTurnover,
            VirtualSportsMaxPayout = addPromotionMdoel.VirtualSportsMaxPayout,
            GamesPercentage = addPromotionMdoel.GamesPercentage,
            GamesMinTurnover = addPromotionMdoel.GamesMinTurnover,
            GamesMaxPayout = addPromotionMdoel.GamesMaxPayout,
            LiveCasinoPercentage = addPromotionMdoel.LiveCasinoPercentage,
            LiveCasinoMinTurnover = addPromotionMdoel.LiveCasinoMinTurnover,
            LiveCasinoMaxPayout = addPromotionMdoel.LiveCasinoMaxPayout
        };

        await repository.ExecuteAsync(addPromotionCriteria);

        var result = new ResultModel(addPromotionCriteria.ErrCode, addPromotionCriteria.ErrMsg);

        return result;
    }

    public async Task<IEnumerable<PromotionModel>> GetPromotion(string promotionType, string status)
    {
        var result = promotionModels;
        return result;
    }

    private static List<PromotionModel> CreateFakePromotionModel()
    {
        return new List<PromotionModel>()
        {
            new PromotionModel
            {
                PromotionId = 1,
                PromotionTitle = "Promotion1",
                PromotionType = "Rebase on Turnover",
                SportsPercentage = 1,
                SportsMaxPayout = 10,
                VirtualSportsPercentage = 1,
                VirtualSportsMaxPayout = 10,
                GamesPercentage = 1,
                GamesMaxPayout = 10,
                LiveCasinoPercentage = 1,
                LiveCasinoMaxPayout = 10,
                Status = "Active"
            },
            new PromotionModel
            {
                PromotionId = 2,
                PromotionTitle = "Promotion2",
                PromotionType = "Rebase on Turnover 1",
                SportsPercentage = 1,
                SportsMaxPayout = 8,
                VirtualSportsPercentage = 1,
                VirtualSportsMaxPayout = 10,
                GamesPercentage = 1,
                GamesMaxPayout = 10,
                LiveCasinoPercentage = 1,
                LiveCasinoMaxPayout = 10,
                 Status = "Active"
            },
            new PromotionModel
            {
                PromotionId = 3,
                PromotionTitle = "Promotion3",
                PromotionType = "Rebase on Turnover 2",
                SportsPercentage = 1,
                SportsMaxPayout = 8,
                VirtualSportsPercentage = 1,
                VirtualSportsMaxPayout = 9,
                GamesPercentage = 1,
                GamesMaxPayout = 10,
                LiveCasinoPercentage = 1,
                LiveCasinoMaxPayout = 9,
                 Status = "All"
            }
        };
    }

    public async Task<ResultModel> EditPromotion(AddPromotionModel addPromotionMdoel)
    {
        var editPromotionCriteria = new EditPromotionCriteria
        {
            PromotionType = addPromotionMdoel.PromotionType,
            FromDate = addPromotionMdoel.FromDate,
            ToDate = addPromotionMdoel.ToDate,
            Provider = addPromotionMdoel.Provider,
            SportsPercentage = addPromotionMdoel.SportsPercentage,
            SportsMinTurnover = addPromotionMdoel.SportsMinTurnover,
            SportsMaxPayout = addPromotionMdoel.SportsMaxPayout,
            VirtualSportsPercentage = addPromotionMdoel.VirtualSportsPercentage,
            VirtualSportsMinTurnover = addPromotionMdoel.VirtualSportsMinTurnover,
            VirtualSportsMaxPayout = addPromotionMdoel.VirtualSportsMaxPayout,
            GamesPercentage = addPromotionMdoel.GamesPercentage,
            GamesMinTurnover = addPromotionMdoel.GamesMinTurnover,
            GamesMaxPayout = addPromotionMdoel.GamesMaxPayout,
            LiveCasinoPercentage = addPromotionMdoel.LiveCasinoPercentage,
            LiveCasinoMinTurnover = addPromotionMdoel.LiveCasinoMinTurnover,
            LiveCasinoMaxPayout = addPromotionMdoel.LiveCasinoMaxPayout
        };

        await repository.ExecuteAsync(editPromotionCriteria);

        var result = new ResultModel(editPromotionCriteria.ErrCode, editPromotionCriteria.ErrMsg);

        return result;
    }

    public async Task<ResultModel> DeletePromotion(int custId, int promotionId)
    {
        var deletePromotionCriteria = new DeletePromotionCriteria(custId, promotionId);

        promotionModels.RemoveAll(p => p.PromotionId == promotionId);

        //await repository.ExecuteAsync(deletePromotionCriteria);

        var result = new ResultModel(deletePromotionCriteria.ErrCode, deletePromotionCriteria.ErrMsg);

        return result;
    }
}
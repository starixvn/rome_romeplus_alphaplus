namespace AlphaPlus.Settings.MemberInfo.API.Services.PositionTaking;

public interface IPositionTakingService
{
    IList<int> GetPageSizeList();

    PositionTakingViewModel GetPT(GetPositionTakingCriteria criteria);
}
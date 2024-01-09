namespace Alpha.Main.Services
{
    using AlphaPlus.Core.Models;
    using Params.Nickname;

    public interface NicknameService
    {
        Task<Result> CreateNickname(CreateNicknameParam param, bool nickNameLoginOnly);
    }
}

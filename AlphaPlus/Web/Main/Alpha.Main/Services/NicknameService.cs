namespace Alpha.Main.Services
{
    using Models;
    using Params.Nickname;

    public interface NicknameService
    {
        Task<Result> CreateNickname(CreateNicknameParam param, bool nickNameLoginOnly);
    }
}

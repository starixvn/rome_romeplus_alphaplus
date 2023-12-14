namespace Alpha.Main.Services
{
    using Models;
    using Params.Password;

    public interface PasswordService
    {
        Task<Result> ResetMainAccountPassword(ResetMainAccountPasswordParam param);

        Task<Result> ChangeMainAccountPassword(ChangeMainAccountPasswordParam param);

        Task<Result> ResetSubAccountPassword(ResetSubAccountPasswordParam param);

        Task<Result> ChangeSubAccountPassword(ChangeSubAccountPasswordParam param);

        Task<Result> SkipForceChangePassword(SkipForceChangePasswordParam param);

        Task UpdateLastChangePasswordIndirectAccount(UpdateLastChangePasswordIndirectAccountParam param);
    }
}

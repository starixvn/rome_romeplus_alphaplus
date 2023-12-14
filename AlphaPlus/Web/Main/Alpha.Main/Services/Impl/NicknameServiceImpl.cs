using System.Text.RegularExpressions;
using Alpha.Main.AppResources;
using Alpha.Main.Criteria.Nickname;
using Fanex.Data.Repository;
using Alpha.Main.Models;
using Alpha.Main.Params.Nickname;

namespace Alpha.Main.Services.Impl
{
    public class NicknameServiceImpl(IDynamicRepository dynamicRepository) : NicknameService
    {
        private const int Code = 100;
        private static readonly Regex NicknameRule = new Regex(@"^[a-zA-Z0-9]{5,20}$");

        private readonly IDynamicRepository dynamicRepository = dynamicRepository;

        public async Task<Result> CreateNickname(CreateNicknameParam param, bool nickNameLoginOnly)
        {
            if (param.IsEnableUpdateNickname)
            {
                var result = await ValidateAndCreateNickname(param);

                if (result.IsFailed)
                {
                    return result;
                }
            }

            return await UpdateNicknameLoginOnly(new UpdateNicknameLoginOnlyParam(param, nickNameLoginOnly));
        }

        private static bool IsValidNickName(string nickName)
            => NicknameRule.IsMatch(nickName);

        private Task<Result> ValidateAndCreateNickname(CreateNicknameParam param)
        {
            if (IsValidNickName(param.Nickname))
            {
                NicknameNonQueryCommand command = param.IsSubAccount
                    ? new CreateSubAccountNicknameCommand(param)
                    : new CreateNicknameCommand(param);

                return Execute(command);
            }

            return Task.FromResult(Result.CreateErrorResult(Resources.NicknameRule, Code));
        }

        private Task<Result> UpdateNicknameLoginOnly(UpdateNicknameLoginOnlyParam param)
        {
            NicknameNonQueryCommand command = param.IsSubAccount
                ? new UpdateSubAccountNicknameLoginOnlyCommand(param)
                : new UpdateNicknameLoginOnlyCommand(param);

            return Execute(command);
        }

        private async Task<Result> Execute(NicknameNonQueryCommand command)
        {
            await dynamicRepository.ExecuteAsync(command);

            return Result.CreateErrorResult(
                command.ErrCode == 0 ? Resources.ConfiguredNicknameSuccessfully : command.ErrMsg,
                command.ErrCode);
        }
    }
}

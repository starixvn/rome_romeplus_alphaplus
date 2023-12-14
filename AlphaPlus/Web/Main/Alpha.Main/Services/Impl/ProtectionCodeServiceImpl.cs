using Alpha.Main.AppResources;
using Alpha.Main.Criteria.ProtectionCode;
using Alpha.Main.Models;
using Alpha.Main.Params.ProtectionCode;
using AlphaPlus.Core.Utils;
using Fanex.Cryptography.Aes;
using Fanex.Data.Repository;

namespace Alpha.Main.Services.Impl
{
    public class ProtectionCodeServiceImpl : ProtectionCodeService
    {
        private const int Length = 4;
        private readonly IAesService aes;
        private readonly IEncryption encryption;
        private readonly IDynamicRepository repository;

        public ProtectionCodeServiceImpl(
            IAesService aes,
            IEncryption encryption,
            IDynamicRepository repository)
        {
            this.aes = aes;
            this.encryption = encryption;
            this.repository = repository;
        }

        public string GenerateHashKey()
            => encryption.RandomString(Length);

        public Task<string> GetAsync(int custId, int subAccId)
            => repository.GetAsync<string>(new GetProtectionCodeCriteria(custId, subAccId));

        public async Task<Result> UpdateAsync(UpdateProtectionCodeParam param)
        {
            if (string.IsNullOrWhiteSpace(param.ProtectionCode))
            {
                return Result.CreateErrorResult(Resources.RequiredNewProtectionCode);
            }

            var rawProtectionCode = aes.Decrypt(param.ProtectionCode, param.HashKey);
            var validateResult = ValidateProtectionCode(rawProtectionCode);

            if (validateResult.IsFailed)
            {
                return validateResult;
            }

            var criteria = new UpdateProtectionCodeCriteria(
                encryption.CfsEnCode(rawProtectionCode),
                param.CustId,
                param.SubAccId,
                param.LangKey,
                param.IP);
            await repository.ExecuteAsync(criteria);
            var result = new Result { Code = criteria.ErrCode, Message = criteria.ErrMsg };

            if (result.IsOk)
            {
                result.Message = Resources.UpdateProtectionCodeSuccessfully;
            }

            return result;
        }

        private static Result ValidateProtectionCode(string protectionCode)
        {
            if (protectionCode.Length < 6)
            {
                return Result.CreateErrorResult(Resources.MinLengthNewProtectionCodeError);
            }

            const int Space = 32;
            const int MaxAnsiCode = 127;

            if (protectionCode.Any(c => c == Space || c > MaxAnsiCode))
            {
                return Result.CreateErrorResult(Resources.ContainsSpaceOrUnicodeCharacterError);
            }

            return new Result();
        }
    }
}

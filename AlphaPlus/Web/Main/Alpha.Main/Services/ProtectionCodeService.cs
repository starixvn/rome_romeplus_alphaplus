namespace Alpha.Main.Services
{
    using System.Threading.Tasks;
    using Alpha.Main.Params.ProtectionCode;
    using AlphaPlus.Core.Models;

    public interface ProtectionCodeService
    {
        string GenerateHashKey();

        Task<string> GetAsync(int custId, int subAccId);

        Task<Result> UpdateAsync(UpdateProtectionCodeParam param);
    }
}

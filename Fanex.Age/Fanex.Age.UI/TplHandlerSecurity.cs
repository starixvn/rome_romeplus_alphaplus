using AlphaPlus.Web.Validators;

namespace Fanex.Age.UI
{
    public abstract class TplHandlerSecurity : TplHandler
    {
        public override object ValidateSecurityActions()
        {
            var antiXSRFResult = AntiForgeryValidator.ValidateRequest();

            return antiXSRFResult.ErrCode == (int)ErrorType.Success
                ? null
                : new Result(antiXSRFResult.ErrCode, antiXSRFResult.ErrMsg);
        }
    }
}
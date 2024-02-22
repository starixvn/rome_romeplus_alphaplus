namespace Alpha.Main.Site.Features.TelegramChatBot
{
    using AlphaPlus.Core.Models;
    using MediatR;

    public class UpdateOtpTelegramCommand : IRequest<Result>
    {
        public UpdateOtpTelegramCommand() : this(null)
        {
        }

        public UpdateOtpTelegramCommand(IUserSession userSession)
        {
            userSession ??= new UserSessionExt();
            CustId = userSession.CustId;
            SubAccId = userSession.SubAccId;
            Language = userSession.Language;
            IP = userSession.UserLoginIp;
        }

        public short FunctionId { get; set; }

        public string FunctionName { get; set; }

        public byte Type { get; set; }

        public bool Enabled { get; set; }

        public int CustId { get; }

        public int SubAccId { get; }

        public string Language { get; }

        public string IP { get; }
    }
}
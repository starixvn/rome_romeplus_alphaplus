using Alpha.Main.Configurations;
using Alpha.Main.Constants;
using Alpha.Main.Site.Helpers;
using Alpha.Main.Site.Requests.TelegramChatBot;
using Alpha.Main.Site.ViewModels.TelegramChatBot;
using AlphaPlus.Core.Models;
using MediatR;
using TelegramBot.Client.Services;

namespace Alpha.Main.Site.Handlers
{
    public class TelegramChatBotHandler(
        ISettingService settingService,
        IBotService botService) :
        IRequestHandler<SetUpChatBotQuery, ChatBotViewModel>,
        IRequestHandler<GenerateQRCodeCommand, VerifyAccountViewModel>,
        IRequestHandler<VerifyOTPCommand, Result>,
        IRequestHandler<ResendOTPCommand, Result>,
        IRequestHandler<UnsubscribeCommand, Result>,
        IRequestHandler<UpdateFunctionCommand, Result>,
        IRequestHandler<UpdateLanguageCommand, Result>
    {
        private const int UnlinkedErrorCode = 102;
        private readonly ISettingService settingService = settingService;
        private readonly IBotService botService = botService;

        public async Task<ChatBotViewModel> Handle(SetUpChatBotQuery message, CancellationToken cancellationToken)
        {
            var custBotMessenger = await settingService.GetCustBotMessengerAsync(
                message.CustId,
                message.SubAccId,
                Language.MapNewChineseLanguageCode(message.Language));

            return new ChatBotViewModel(custBotMessenger);
        }

        public async Task<VerifyAccountViewModel> Handle(GenerateQRCodeCommand message, CancellationToken cancellationToken)
        {
            var url = await settingService.GenerateStartUriAsync(
                message.CustId,
                message.SubAccId,
                AppSettings.SiteLabel,
                Language.MapNewChineseLanguageCode(message.Language));

            var logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"wwwroot/assets/styles/telegramchatbot/images/telegram-logo.png");
            var qrCodeDataUrl = QRCodeAdapter.Generate(url, logoPath);

            return new VerifyAccountViewModel(url, qrCodeDataUrl);
        }

        public async Task<Result> Handle(VerifyOTPCommand message, CancellationToken cancellationToken)
        {
            var bot = await GetTelegramBotAsync();
            var username = message.IsSubAcc ? message.SubUserName : message.Username;
            var result = await settingService.LinkAsync(bot.BotId, message.CustId, message.SubAccId, message.OTP, username, message.IP);
            return new Result(result.Code, result.Message);
        }

        public async Task<Result> Handle(ResendOTPCommand message, CancellationToken cancellationToken)
        {
            var result = await settingService.ResendOtpAsync(message.CustId, message.SubAccId);
            return new Result(result.Code, result.Message);
        }

        public async Task<Result> Handle(UnsubscribeCommand message, CancellationToken cancellationToken)
        {
            var result = await settingService.UnlinkAsync(message.CustId, message.SubAccId, message.IP);
            return new Result(result.Code, result.Message);
        }

        public async Task<Result> Handle(UpdateFunctionCommand message, CancellationToken cancellationToken)
        {
            var botAndCustMessenger = await GetTelegramBotAndCustMessengerAsync(
                message.CustId,
                message.SubAccId,
                message.Language);
            var bot = botAndCustMessenger.Item1;
            var custBotMessenger = botAndCustMessenger.Item2;
            if (custBotMessenger == null || !custBotMessenger.Enabled || !custBotMessenger.IsVerifyOTP)
            {
                return new Result(UnlinkedErrorCode);
            }

            var function = new TelegramBot.Client.Models.Function
            {
                FunctionId = message.FunctionId,
                FunctionName = message.FunctionName,
                Enabled = message.Enabled,
                Type = message.Type
            };
            var result = await settingService.UpdateUserFunction(custBotMessenger.UserId, bot.BotId, function, message.CustId, message.IP);
            return new Result(result.Code, result.Message);
        }

        public async Task<Result> Handle(UpdateLanguageCommand message, CancellationToken cancellationToken)
        {
            var botAndCustMessenger = await GetTelegramBotAndCustMessengerAsync(
                message.CustId,
                message.SubAccId,
                message.UserLanguage);
            var bot = botAndCustMessenger.Item1;
            var custBotMessenger = botAndCustMessenger.Item2;
            if (custBotMessenger == null || !custBotMessenger.Enabled || !custBotMessenger.IsVerifyOTP)
            {
                return new Result(UnlinkedErrorCode);
            }

            var result = await settingService.UpdateLanguage(
                custBotMessenger.UserId,
                bot.BotId,
                Language.MapNewChineseLanguageCode(message.Language),
                message.CustId,
                message.IP);

            return new Result(result.Code, result.Message);
        }

        private async Task<Tuple<TelegramBot.Client.Models.BotEntity, TelegramBot.Client.Models.CustBotMessenger>> GetTelegramBotAndCustMessengerAsync(int custId, int subAccId, string language)
        {
            var botTask = GetTelegramBotAsync();
            var custBotMessengerTask = settingService.GetCustBotMessengerAsync(
                custId,
                subAccId,
                Language.MapNewChineseLanguageCode(language));

            await Task.WhenAll(botTask, custBotMessengerTask);

            return new Tuple<TelegramBot.Client.Models.BotEntity, TelegramBot.Client.Models.CustBotMessenger>(await botTask, await custBotMessengerTask);
        }

        private async Task<TelegramBot.Client.Models.BotEntity> GetTelegramBotAsync()
        {
            return await botService.GetBySystem(AppSettings.SiteLabel);
        }
    }
}
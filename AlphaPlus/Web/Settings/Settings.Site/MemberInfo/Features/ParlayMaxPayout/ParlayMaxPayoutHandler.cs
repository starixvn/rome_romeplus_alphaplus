namespace AlphaPlus.Settings.Site.Features.ParlayMaxPayout
{
    using AlphaPlus.Settings.MemberInfo.API.Features.ParlayMaxPayout;
    using AlphaPlus.Settings.MemberInfo.API.Services;
    using AlphaPlus.Settings.MemberInfo.Domain.Constants;
    using AlphaPlus.Settings.MemberInfo.Domain.Features.ParlayMaxPayout.Params;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using AlphaPlus.Settings.MemberInfo.Domain.Params;
    using MediatR;
    using Requests;
    using System.Collections.Generic;
    using System.Linq;
    using ViewModels;

    public class ParlayMaxPayoutHandler :
        IRequestHandler<ParlayMaxPayoutSingleSelectQuery, ParlayMaxPayoutSinglePageViewModel>,
        IRequestHandler<ParlayMaxPayoutMultipleSelectQuery, ParlayMaxPayoutMultiplePageViewModel>,
        IRequestHandler<ParlayMaxPayoutUpdateSingleQuery, Result>,
        IRequestHandler<ParlayMaxPayoutUpdateMultipleQuery, IEnumerable<Result>>,
        IRequestHandler<ParlayMaxPayoutCheckDownlineQuery, Result>
    {
        private readonly ParlayMaxPayoutService parlayMaxPayoutService;
        private readonly CheckUMService checkUMService;

        public ParlayMaxPayoutHandler(
            ParlayMaxPayoutService parlayMaxPayoutService,
            CheckUMService checkUMService)
        {
            this.parlayMaxPayoutService = parlayMaxPayoutService;
            this.checkUMService = checkUMService;
        }

        public  async Task<ParlayMaxPayoutSinglePageViewModel> Handle(ParlayMaxPayoutSingleSelectQuery message, CancellationToken cancellation)
            => new ParlayMaxPayoutSinglePageViewModel(
                message,
                parlayMaxPayoutService.GetParlayMaxPayout(new ParlayMaxPayoutSelectParam(message.CustId)));

        public  async Task<ParlayMaxPayoutMultiplePageViewModel> Handle(ParlayMaxPayoutMultipleSelectQuery message, CancellationToken cancellation)
            => new ParlayMaxPayoutMultiplePageViewModel(
                message,
                parlayMaxPayoutService.GetParlayMaxPayout(new ParlayMaxPayoutSelectParam(message.SelectedCustId)));

        public  async Task<Result> Handle(ParlayMaxPayoutUpdateSingleQuery message, CancellationToken cancellation)
        {
            var checkUMResult = checkUMService.CheckUM(
                new CheckUMParam(message.SelectedCustId, NonDcsProduct.Sportsbook, message.Language));

            return checkUMResult.IsFailed
                ? checkUMResult
                : parlayMaxPayoutService.UpdateParlayMaxPayout(
                    new ParlayMaxPayoutUpdateParam(
                        message.SelectedCustId,
                        message.MaxPayoutLimit,
                        message.UserId,
                        message.SubAccId,
                        message.Language,
                        message.IP));
        }

        public  async Task<IEnumerable<Result>> Handle(ParlayMaxPayoutUpdateMultipleQuery message, CancellationToken cancellation)
        {
            var custIds = message.CustIdList.Split(',').Select(int.Parse);
            var results = new List<Result>();
            foreach (var custId in custIds)
            {
                var checkUMResult = checkUMService.CheckUM(
                    new CheckUMParam(custId, NonDcsProduct.Sportsbook, message.Language));

                results.Add( checkUMResult.IsFailed
                    ? checkUMResult
                    : parlayMaxPayoutService.UpdateParlayMaxPayout(
                        new ParlayMaxPayoutUpdateParam(
                            custId,
                            message.MaxPayoutLimit,
                            message.UserId,
                            message.SubAccId,
                            message.Language,
                            message.IP)));
            }

            return results;
        }

        public  async Task<Result> Handle(ParlayMaxPayoutCheckDownlineQuery message, CancellationToken cancellation)
        {
            var custId = message.CustIdList.Split(',').Select(int.Parse).FirstOrDefault();

            return parlayMaxPayoutService.CheckDownline(
                new ParlayMaxPayoutCheckDownlineParam(
                    custId,
                    message.MaxPayoutLimit,
                    message.Language));
        }
    }
}
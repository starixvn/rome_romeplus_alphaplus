namespace BetList.Status.ResultDetails {
    export class AllbetResultDetails extends BaseResultDetails implements ResultDetails {
        public ViewResultDetails = (
            matchId?: number,
            race?: number,
            betTypeId?: number,
            sportTypeId?: number,
            refNo?: string,
            userName?: string,
            winLostDate?: Date,
            refNoMixParlay?: number,
            league?: number,
            isOutright?: boolean,
            betId?: number,
            betCheck?: string,
            custId?: number,
            transDesc?: string
        ) => {
            const params = [
                '?transDesc=',
                transDesc,
                '&betCheck=',
                betCheck,
                '&bettypeId=',
                betTypeId,
                '&sportId=',
                sportTypeId
            ].join('');

            Common.PopupDetails.Instance(window.top).Show(this.ResultDetailsPath + params, '', 835, 500);
        };
    }

    export class RnGCasinoResultDetails extends BaseResultDetails implements ResultDetails {
        public ViewResultDetails = (
            matchId?: number,
            race?: number,
            betTypeId?: number,
            sportTypeId?: number,
            refNo?: string,
            userName?: string,
            winLostDate?: Date,
            refNoMixParlay?: number,
            league?: number,
            isOutright?: boolean,
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string
        ) => {
            var params = [
                '?custId=',
                custId,
                '&transId3rd=',
                transId3rd,
                '&gameId3rd=',
                gameId3rd,
                '&currencyId=',
                currencyId,
                '&siteId=',
                siteId,
                '&productId=6'
            ].join('');
            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.TicketsAppPath + 'ThirdParty/RngCasinoResult' + params,
                '',
                835,
                675
            );
        };
    }

    export class MacauGamesResultDetails extends BaseResultDetails implements ResultDetails {
        public ViewResultDetails = (
            matchId?: number,
            race?: number,
            betTypeId?: number,
            sportTypeId?: number,
            refNo?: string,
            userName?: string,
            winLostDate?: Date,
            refNoMixParlay?: number,
            league?: number,
            isOutright?: boolean,
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string
        ) => {
            var params = [
                '?custId=',
                custId,
                '&transId3rd=',
                transId3rd,
                '&gameId3rd=',
                gameId3rd,
                '&currencyId=',
                currencyId,
                '&siteId=',
                siteId,
                '&productId=22'
            ].join('');
            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.TicketsAppPath + 'ThirdParty/MacauGamesResult' + params,
                '',
                835,
                675
            );
        };
    }

    export class FishingWorldResultDetails extends BaseResultDetails implements ResultDetails {
        public ViewResultDetails = (
            matchId?: number,
            race?: number,
            betTypeId?: number,
            sportTypeId?: number,
            refNo?: string,
            userName?: string,
            winLostDate?: Date,
            refNoMixParlay?: number,
            league?: number,
            isOutright?: boolean,
            betId?: number,
            betCheck?: string
        ) => {
            var params = `?custId=${this.CustId}&roleId=${this.ViewerLevelId}&gameId=${betCheck}&productId=24`;
            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.TicketsAppPath + 'ThirdParty/Detail' + params,
                '',
                835,
                675
            );
        };
    }

    export class PPResultDetails extends BaseResultDetails implements ResultDetails {
        public ViewResultDetails = (
            matchId?: number,
            race?: number,
            betTypeId?: number,
            sportTypeId?: number,
            refNo?: string,
            userName?: string,
            winLostDate?: Date,
            refNoMixParlay?: number,
            league?: number,
            isOutright?: boolean,
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string
        ) => {
            var params = [
                '?custId=',
                custId,
                '&transId3rd=',
                transId3rd,
                '&gameId3rd=',
                gameId3rd,
                '&currencyId=',
                currencyId,
                '&BetType=',
                betTypeId,
                '&siteId=',
                siteId,
                '&productId=32'
            ].join('');
            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.TicketsAppPath + 'ThirdParty/PPResult' + params,
                '',
                835,
                675
            );
        };
    }

    export class SpadegamingResultDetails extends BaseResultDetails implements ResultDetails {
        public ViewResultDetails = (
            matchId?: number,
            race?: number,
            betTypeId?: number,
            sportTypeId?: number,
            refNo?: string,
            userName?: string,
            winLostDate?: Date,
            refNoMixParlay?: number,
            league?: number,
            isOutright?: boolean,
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string
        ) => {
            var params = [
                '?custId=',
                custId,
                '&transId3rd=',
                transId3rd,
                '&gameId3rd=',
                gameId3rd,
                '&currencyId=',
                currencyId,
                '&siteId=',
                siteId,
                '&productId=36'
            ].join('');
            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.TicketsAppPath + 'ThirdParty/SpadegamingResult' + params,
                '',
                835,
                675
            );
        };
    }

    export class VirtualGamesResultDetails extends BaseResultDetails implements ResultDetails {
        public ViewResultDetails = (
            matchId?: number,
            race?: number,
            betTypeId?: number,
            sportTypeId?: number,
            refNo?: string,
            userName?: string,
            winLostDate?: Date,
            refNoMixParlay?: number,
            league?: number,
            isOutright?: boolean,
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string
        ) => {
            var params = [
                '?custId=',
                custId,
                '&transId3rd=',
                transId3rd,
                '&gameId3rd=',
                gameId3rd,
                '&currencyId=',
                currencyId,
                '&siteId=',
                siteId,
                '&productId=38'
            ].join('');
            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.TicketsAppPath + 'ThirdParty/VirtualGamesResult' + params,
                '',
                835,
                675
            );
        };
    }

    export class RNGWarCasinoResultDetails extends BaseResultDetails implements ResultDetails {
        public ViewResultDetails = (
            matchId?: number,
            race?: number,
            betTypeId?: number,
            sportTypeId?: number,
            refNo?: string,
            userName?: string,
            winLostDate?: Date,
            refNoMixParlay?: number,
            league?: number,
            isOutright?: boolean,
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string
        ) => {
            var params = [
                '?custId=',
                custId,
                '&transId3rd=',
                transId3rd,
                '&gameId3rd=',
                gameId3rd,
                '&currencyId=',
                currencyId,
                '&siteId=',
                siteId,
                '&productId=50'
            ].join('');
            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.TicketsAppPath + 'ThirdParty/RNGWarResult' + params,
                '',
                835,
                675
            );
        };
    }
    export class IONCasinoResultDetails extends BaseResultDetails implements ResultDetails {
        public ViewResultDetails = (
            matchId?: number,
            race?: number,
            betTypeId?: number,
            sportTypeId?: number,
            refNo?: string,
            userName?: string,
            winLostDate?: Date,
            refNoMixParlay?: number,
            league?: number,
            isOutright?: boolean,
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string
        ) => {
            var params = [
                '?custId=',
                custId,
                '&transId3rd=',
                transId3rd,
                '&gameId3rd=',
                gameId3rd,
                '&currencyId=',
                currencyId,
                '&siteId=',
                siteId,
                '&productId=52'
            ].join('');
            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.TicketsAppPath + 'ThirdParty/IONCasinoResult' + params,
                '',
                835,
                675
            );
        };
    }

    export class SabaClubResultDetails extends BaseResultDetails implements ResultDetails {
        public ViewResultDetails = (
            matchId?: number,
            race?: number,
            betTypeId?: number,
            sportTypeId?: number,
            refNo?: string,
            userName?: string,
            winLostDate?: Date,
            refNoMixParlay?: number,
            league?: number,
            isOutright?: boolean,
            betId?: number,
            matchCode?: string,
            transDesc?: number,
            betcheck?: string
        ) => {
            var params = [
                '?custId=',
                transDesc || this.CustId,
                '&roleId=',
                this.ViewerLevelId,
                '&gameRoundId=',
                betcheck,
                '&gameId=',
                betTypeId,
                '&sportId=',
                sportTypeId,
                '&siteId=',
                matchCode,
                '&productId=42'
            ].join('');
            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.TicketsAppPath + 'SabaClub/Index' + params,
                '',
                835,
                675
            );
        };
    }

    export class PGSoftCasinoResultDetails extends BaseResultDetails implements ResultDetails {
        public ViewResultDetails = (
            matchId?: number,
            race?: number,
            betTypeId?: number,
            sportTypeId?: number,
            refNo?: string,
            userName?: string,
            winLostDate?: Date,
            refNoMixParlay?: number,
            league?: number,
            isOutright?: boolean,
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string
        ) => {
            var params = [
                '?custId=',
                custId,
                '&transId3rd=',
                transId3rd,
                '&gameId3rd=',
                gameId3rd,
                '&currencyId=',
                currencyId,
                '&siteId=',
                siteId,
                '&productId=55'
            ].join('');
            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.TicketsAppPath + 'ThirdParty/PGSoftResult' + params,
                '',
                835,
                675
            );
        };
    }

    export class JokerResultDetails extends BaseResultDetails implements ResultDetails {
        public ViewResultDetails = (
            matchId?: number,
            race?: number,
            betTypeId?: number,
            sportTypeId?: number,
            refNo?: string,
            userName?: string,
            winLostDate?: Date,
            refNoMixParlay?: number,
            league?: number,
            isOutright?: boolean,
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string
        ) => {
            var params = [
                '?custId=',
                custId,
                '&transId3rd=',
                transId3rd,
                '&gameId3rd=',
                gameId3rd,
                '&currencyId=',
                currencyId,
                '&siteId=',
                siteId,
                '&productId=56'
            ].join('');
            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.TicketsAppPath + 'ThirdParty/JokerResult' + params,
                '',
                835,
                675
            );
        };
    }

    export class GDGResultDetails extends BaseResultDetails implements ResultDetails {
        public ViewResultDetails = (
            matchId?: number,
            race?: number,
            betTypeId?: number,
            sportTypeId?: number,
            refNo?: string,
            userName?: string,
            winLostDate?: Date,
            refNoMixParlay?: number,
            league?: number,
            isOutright?: boolean,
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string
        ) => {
            var params = [
                '?custId=',
                custId,
                '&transId3rd=',
                transId3rd,
                '&gameId3rd=',
                gameId3rd,
                '&currencyId=',
                currencyId,
                '&siteId=',
                siteId,
                '&productId=59'
            ].join('');

            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.TicketsAppPath + 'ThirdParty/GDGResult' + params,
                '',
                835,
                675
            );
        };
    }

    export class Happy5LinkedGamesResultDetails extends BaseResultDetails implements ResultDetails {
        public ViewResultDetails = (
            matchId?: number,
            race?: number,
            betTypeId?: number,
            sportTypeId?: number,
            refNo?: string,
            userName?: string,
            winLostDate?: Date,
            refNoMixParlay?: number,
            league?: number,
            isOutright?: boolean,
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string
        ) => {
            var params = [
                '?custId=',
                custId,
                '&transId3rd=',
                refNo,
                '&gameId3rd=',
                gameId3rd,
                '&currencyId=',
                currencyId,
                '&siteId=',
                siteId,
                '&productId=3',
                '&BetTypeId=',
                betTypeId,
                '&SportTypeId=',
                sportTypeId
            ].join('');

            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.TicketsAppPath + 'ThirdParty/LinkedGamesResult' + params,
                '',
                835,
                675
            );
        };
    }

    export class SportsLotteryResultDetails extends BaseResultDetails implements ResultDetails {
        public ViewResultDetails = (
            matchId?: number,
            race?: number,
            betTypeId?: number,
            sportTypeId?: number,
            refNo?: string,
            userName?: string,
            winLostDate?: Date,
            refNoMixParlay?: number,
            league?: number,
            isOutright?: boolean,
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string
        ) => {
            var params = [
                '?custId=',
                custId,
                '&transId3rd=',
                transId3rd,
                '&gameId3rd=',
                gameId3rd,
                '&currencyId=',
                currencyId,
                '&siteId=',
                siteId,
                '&productId=63'
            ].join('');

            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.TicketsAppPath + 'ThirdParty/SportsLotteryDetailResult' + params,
                '',
                835,
                675
            );
        };
    }
    export class BGResultDetails extends BaseResultDetails implements ResultDetails {
        public ViewResultDetails = (
            matchId?: number,
            race?: number,
            betTypeId?: number,
            sportTypeId?: number,
            refNo?: string,
            userName?: string,
            winLostDate?: Date,
            refNoMixParlay?: number,
            league?: number,
            isOutright?: boolean,
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string
        ) => {
            var params = [
                '?custId=',
                custId,
                '&transId3rd=',
                transId3rd,
                '&gameId3rd=',
                gameId3rd,
                '&currencyId=',
                currencyId,
                '&siteId=',
                siteId,
                '&productId=57'
            ].join('');

            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.TicketsAppPath + 'ThirdParty/BGResult' + params,
                '',
                835,
                675
            );
        };
    }

    export class MaxGameResultDetails extends BaseResultDetails implements ResultDetails {
        public ViewResultDetails = (
            matchId?: number,
            race?: number,
            betTypeId?: number,
            sportTypeId?: number,
            refNo?: string,
            userName?: string,
            winLostDate?: Date,
            refNoMixParlay?: number,
            league?: number,
            isOutright?: boolean,
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string
        ) => {
            var params = [
                '?custId=',
                custId,
                '&transId3rd=',
                transId3rd,
                '&gameId3rd=',
                gameId3rd,
                '&currencyId=',
                currencyId,
                '&siteId=',
                siteId,
                '&BetType=',
                betTypeId,
                '&endtime=',
                winLostDate,
                '&productId=60'
            ].join('');
            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.TicketsAppPath + 'ThirdParty/MaxGameResult' + params,
                '',
                835,
                675
            );
        };
    }

    export class PPLiveCasinoResultDetails extends BaseResultDetails implements ResultDetails {
        public ViewResultDetails = (
            matchId?: number,
            race?: number,
            betTypeId?: number,
            sportTypeId?: number,
            refNo?: string,
            userName?: string,
            winLostDate?: Date,
            refNoMixParlay?: number,
            league?: number,
            isOutright?: boolean,
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string
        ) => {
            var params = [
                '?custId=',
                custId,
                '&transId3rd=',
                transId3rd,
                '&gameId3rd=',
                gameId3rd,
                '&currencyId=',
                currencyId,
                '&siteId=',
                siteId,
                '&BetType=',
                betTypeId,
                '&productId=65'
            ].join('');

            var mainDomain = location.origin.split(':')[0] + ':' + $('#MainDomain').val() + '/site-main';
            window.top.postMessage(
                {
                    event_id: 'pplivecasino-result',
                    url: 'ThirdParty/PPLiveCasinoResult' + params
                },
                mainDomain
            );
        };
    }

    export class HabaneroResultDetails extends BaseResultDetails implements ResultDetails {
        public ViewResultDetails = (
            matchId?: number,
            race?: number,
            betTypeId?: number,
            sportTypeId?: number,
            refNo?: string,
            userName?: string,
            winLostDate?: Date,
            refNoMixParlay?: number,
            league?: number,
            isOutright?: boolean,
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string
        ) => {
            var params = [
                '?custId=',
                custId,
                '&transId3rd=',
                transId3rd,
                '&gameId3rd=',
                gameId3rd,
                '&currencyId=',
                currencyId,
                '&siteId=',
                siteId,
                '&productId=61'
            ].join('');

            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.TicketsAppPath + 'ThirdParty/HabaneroResult' + params,
                '',
                835,
                675
            );
        };
    }

    export class CGResultDetails extends BaseResultDetails implements ResultDetails {
        public ViewResultDetails = (
            matchId?: number,
            race?: number,
            betTypeId?: number,
            sportTypeId?: number,
            refNo?: string,
            userName?: string,
            winLostDate?: Date,
            refNoMixParlay?: number,
            league?: number,
            isOutright?: boolean,
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string
        ) => {
            var params = [
                '?custId=',
                custId,
                '&transId3rd=',
                transId3rd,
                '&gameId3rd=',
                gameId3rd,
                '&currencyId=',
                currencyId,
                '&siteId=',
                siteId,
                '&productId=62'
            ].join('');

            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.TicketsAppPath + 'ThirdParty/CGResult' + params,
                '',
                835,
                675
            );
        };
    }

    export class VGamingResultDetails extends BaseResultDetails implements ResultDetails {
        public ViewResultDetails = (
            matchId?: number,
            race?: number,
            betTypeId?: number,
            sportTypeId?: number,
            refNo?: string,
            userName?: string,
            winLostDate?: Date,
            refNoMixParlay?: number,
            league?: number,
            isOutright?: boolean,
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string
        ) => {
            var params = [
                '?custId=',
                custId,
                '&transId3rd=',
                transId3rd,
                '&currencyId=',
                currencyId,
                '&siteId=',
                siteId,
                '&BetType=',
                betTypeId,
                '&productId=66'
            ].join('');
            Common.PopupDetails.Instance(window.top).Show(
                `${Common.AppPath.TicketsAppPath}ThirdParty/VGamingResult${params}`,
                '',
                835,
                675
            );
        };
    }

    export class AdvantPlayResultDetails extends BaseResultDetails implements ResultDetails {
        public ViewResultDetails = (
            matchId?: number,
            race?: number,
            betTypeId?: number,
            sportTypeId?: number,
            refNo?: string,
            userName?: string,
            winLostDate?: Date,
            refNoMixParlay?: number,
            league?: number,
            isOutright?: boolean,
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string
        ) => {
            var params = [
                '?custId=',
                custId,
                '&transId3rd=',
                transId3rd,
                '&gameId3rd=',
                gameId3rd,
                '&currencyId=',
                currencyId,
                '&siteId=',
                siteId,
                '&productId=67'
            ].join('');

            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.TicketsAppPath + 'ThirdParty/AdvantPlayResult' + params,
                '',
                835,
                675
            );
        };
    }

    export class AdvantPlayMiniResultDetails extends BaseResultDetails implements ResultDetails {
        public ViewResultDetails = (
            matchId?: number,
            race?: number,
            betTypeId?: number,
            sportTypeId?: number,
            refNo?: string,
            userName?: string,
            winLostDate?: Date,
            refNoMixParlay?: number,
            league?: number,
            isOutright?: boolean,
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string
        ) => {
            var params = [
                '?custId=',
                custId,
                '&transId3rd=',
                transId3rd,
                '&gameId3rd=',
                gameId3rd,
                '&currencyId=',
                currencyId,
                '&siteId=',
                siteId,
                '&productId=68'
            ].join('');

            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.TicketsAppPath + 'ThirdParty/AdvantPlayMiniResult' + params,
                '',
                835,
                675
            );
        };
    }

    export class BBINResultDetails extends BaseResultDetails implements ResultDetails {
        public ViewResultDetails = (
            matchId?: number,
            race?: number,
            betTypeId?: number,
            sportTypeId?: number,
            refNo?: string,
            userName?: string,
            winLostDate?: Date,
            refNoMixParlay?: number,
            league?: number,
            isOutright?: boolean,
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string
        ) => {
            var params = [
                '?custId=',
                custId,
                '&transId3rd=',
                transId3rd,
                '&gameId3rd=',
                gameId3rd,
                '&currencyId=',
                currencyId,
                '&siteId=',
                siteId,
                '&productId=47'
            ].join('');

            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.TicketsAppPath + 'ThirdParty/BBINResult' + params,
                '',
                835,
                900
            );
        };
    }

    export class PokerGamesResultDetails extends BaseResultDetails implements ResultDetails {
        public ViewResultDetails = (
            matchId?: number,
            race?: number,
            betTypeId?: number,
            sportTypeId?: number,
            refNo?: string,
            userName?: string,
            winLostDate?: Date,
            refNoMixParlay?: number,
            league?: number,
            isOutright?: boolean,
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string
        ) => {
            var params = [
                '?custId=',
                custId,
                '&transId3rd=',
                transId3rd,
                '&gameId3rd=',
                gameId3rd,
                '&currencyId=',
                currencyId,
                '&siteId=',
                siteId,
                '&BetType=',
                betTypeId,
                '&productId=34'
            ].join('');

            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.TicketsAppPath + 'ThirdParty/CardClubResult' + params,
                '',
                1099,
                500,
                { resizable: true }
            );
        };
    }

    export class FGGResultDetails extends BaseResultDetails implements ResultDetails {
        public ViewResultDetails = (
            matchId?: number,
            race?: number,
            betTypeId?: number,
            sportTypeId?: number,
            refNo?: string,
            userName?: string,
            winLostDate?: Date,
            refNoMixParlay?: number,
            league?: number,
            isOutright?: boolean,
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string
        ) => {
            var params = [
                '?custId=',
                custId,
                '&transId3rd=',
                transId3rd,
                '&gameId3rd=',
                gameId3rd,
                '&currencyId=',
                currencyId,
                '&siteId=',
                siteId,
                '&endtime=',
                winLostDate,
                '&productId=70'
            ].join('');
            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.TicketsAppPath + 'ThirdParty/FGGResult' + params,
                '',
                835,
                675
            );
        };
    }

    export class SEALResultDetails extends BaseResultDetails implements ResultDetails {
        public ViewResultDetails = (
            matchId?: number,
            race?: number,
            betTypeId?: number,
            sportTypeId?: number,
            refNo?: string,
            userName?: string,
            winLostDate?: Date,
            refNoMixParlay?: number,
            league?: number,
            isOutright?: boolean,
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string
        ) => {
            var params = [
                '?custId=',
                custId,
                '&transId3rd=',
                transId3rd,
                '&gameId3rd=',
                gameId3rd,
                '&currencyId=',
                currencyId,
                '&siteId=',
                siteId,
                '&BetType=',
                betTypeId,
                '&productId=72'
            ].join('');
            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.TicketsAppPath + 'ThirdParty/SEALResult' + params,
                '',
                835,
                675
            );
        };
    }

    export class MGResultDetails extends BaseResultDetails implements ResultDetails {
        public ViewResultDetails = (
            matchId?: number,
            race?: number,
            betTypeId?: number,
            sportTypeId?: number,
            refNo?: string,
            userName?: string,
            winLostDate?: Date,
            refNoMixParlay?: number,
            league?: number,
            isOutright?: boolean,
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string
        ) => {
            var params = [
                '?custId=',
                custId,
                '&transId3rd=',
                transId3rd,
                '&gameId3rd=',
                gameId3rd,
                '&currencyId=',
                currencyId,
                '&siteId=',
                siteId,
                '&BetType=',
                betTypeId,
                '&productId=73'
            ].join('');
            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.TicketsAppPath + 'ThirdParty/MGResult' + params,
                '',
                835,
                675
            );
        };
    }

    export class JiliResultDetails extends BaseResultDetails implements ResultDetails {
        public ViewResultDetails = (
            matchId?: number,
            race?: number,
            betTypeId?: number,
            sportTypeId?: number,
            refNo?: string,
            userName?: string,
            winLostDate?: Date,
            refNoMixParlay?: number,
            league?: number,
            isOutright?: boolean,
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string
        ) => {
            var params = [
                '?custId=',
                custId,
                '&transId3rd=',
                transId3rd,
                '&gameId3rd=',
                gameId3rd,
                '&currencyId=',
                currencyId,
                '&siteId=',
                siteId,
                '&BetType=',
                betTypeId,
                '&productId=75'
            ].join('');
            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.TicketsAppPath + 'ThirdParty/JiliResult' + params,
                '',
                835,
                675
            );
        };
    }

    export class Live22ResultDetails extends BaseResultDetails implements ResultDetails {
        public ViewResultDetails = (
            matchId?: number,
            race?: number,
            betTypeId?: number,
            sportTypeId?: number,
            refNo?: string,
            userName?: string,
            winLostDate?: Date,
            refNoMixParlay?: number,
            league?: number,
            isOutright?: boolean,
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string
        ) => {
            var params = [
                '?custId=',
                custId,
                '&transId3rd=',
                transId3rd,
                '&gameId3rd=',
                gameId3rd,
                '&currencyId=',
                currencyId,
                '&siteId=',
                siteId,
                '&BetType=',
                betTypeId,
                '&endtime=',
                winLostDate,
                '&productId=79'
            ].join('');
            Common.PopupDetails.Instance(window.top).Show(
                `${Common.AppPath.TicketsAppPath}ThirdParty/Live22Result${params}`,
                '',
                835,
                675
            );
        };
    }

    export class WEResultDetails extends BaseResultDetails implements ResultDetails {
        public ViewResultDetails = (
            matchId?: number,
            race?: number,
            betTypeId?: number,
            sportTypeId?: number,
            refNo?: string,
            userName?: string,
            winLostDate?: Date,
            refNoMixParlay?: number,
            league?: number,
            isOutright?: boolean,
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string
        ) => {
            var params = [
                '?custId=',
                custId,
                '&transId3rd=',
                transId3rd,
                '&gameId3rd=',
                gameId3rd,
                '&currencyId=',
                currencyId,
                '&siteId=',
                siteId,
                '&BetType=',
                betTypeId,
                '&productId=80'
            ].join('');
            Common.PopupDetails.Instance(window.top).Show(
                `${Common.AppPath.TicketsAppPath}ThirdParty/WEResult${params}`,
                '',
                835,
                675
            );
        };
    }

    export class UUSlotsResultDetails extends BaseResultDetails implements ResultDetails {
        public ViewResultDetails = (
            matchId?: number,
            race?: number,
            betTypeId?: number,
            sportTypeId?: number,
            refNo?: string,
            userName?: string,
            winLostDate?: Date,
            refNoMixParlay?: number,
            league?: number,
            isOutright?: boolean,
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string
        ) => {
            var params = [
                '?custId=',
                custId,
                '&transId3rd=',
                transId3rd,
                '&gameId3rd=',
                gameId3rd,
                '&currencyId=',
                currencyId,
                '&siteId=',
                siteId,
                '&BetType=',
                betTypeId,
                '&productId=78'
            ].join('');
            Common.PopupDetails.Instance(window.top).Show(
                `${Common.AppPath.TicketsAppPath}ThirdParty/UUSlotsResult${params}`,
                '',
                835,
                675
            );
        };
    }

    export class SabaxDResultDetails extends BaseResultDetails implements ResultDetails {
        public ViewResultDetails = (
            matchId?: number,
            race?: number,
            betTypeId?: number,
            sportTypeId?: number,
            refNo?: string,
            userName?: string,
            winLostDate?: Date,
            refNoMixParlay?: number,
            league?: number,
            isOutright?: boolean,
            currencyId?: number,
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string
        ) => {
            var params = [
                '?custId=',
                custId,
                '&transId3rd=',
                transId3rd,
                '&currencyId=',
                currencyId,
                '&siteId=',
                siteId,
                '&productId=86'
            ].join('');
            Common.PopupDetails.Instance(window.top).Show(
                `${Common.AppPath.TicketsAppPath}ThirdParty/SabaxDResult${params}`,
                '',
                835,
                675
            );
        };
    }
}

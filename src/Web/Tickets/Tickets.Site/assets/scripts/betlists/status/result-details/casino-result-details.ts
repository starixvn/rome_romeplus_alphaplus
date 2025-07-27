namespace BetList.Status.ResultDetails {
    const encodeParam = (value: string | number | undefined): string => {
        if (value === undefined || value === null) return '';
        return encodeURIComponent(value.toString());
    };
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
            const params = [
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
            const params = [
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
            const params = `?custId=${this.CustId}&roleId=${this.ViewerLevelId}&gameId=${betCheck}&productId=24`;
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
            const params = [
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
            const params = [
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
            const params = [
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
            const params = [
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
            const params = [
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
            const params = [
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
                Common.AppPath.TicketsAppPath + 'Results/SabaClubResult' + params,
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
            const params = [
                '?custId=',
                custId,
                '&transId3rd=',
                transId3rd,
                '&gameId3rd=',
                gameId3rd,
                '&currencyId=',
                currencyId,
                '&betType=',
                betTypeId,
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
            const params = [
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
            const params = [
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
            const params = [
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
            const params = [
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
            const params = [
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
            const params = [
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

            Common.PopupDetails.Instance(window.top).Show(
                Common.AppPath.TicketsAppPath + 'ThirdParty/PPLiveCasinoResult' + params,
                '',
                835,
                675
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
            const params = [
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
            const params = [
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
            const params = [
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
            const params = [
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
            const params = [
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
            const params = [
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
            const params = [
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
            const params = [
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
            const params = [
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
            const params = [
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
            siteId?: string,
            transDesc?: string
        ) => {
            const params = [
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
                '&TransDesc=',
                transDesc,
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
            const params = [
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
            const params = [
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

    export class SexyGamingResultDetails extends BaseResultDetails implements ResultDetails {
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
            site?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string
        ) => {
            const params = [
                '?custId=',
                custId,
                '&transId3rd=',
                transId3rd,
                '&site=',
                site,
                '&currencyId=',
                currencyId,
                '&siteId=',
                siteId,
                '&productId=45'
            ].join('');
            Common.PopupDetails.Instance(window.top).Show(
                `${Common.AppPath.TicketsAppPath}ThirdParty/SexyGamingResult${params}`,
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
            const params = [
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
            const params = [
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

    export class FastSpinResultDetails extends BaseResultDetails implements ResultDetails {
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
            const params = [
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
                '&productId=87'
            ].join('');
            Common.PopupDetails.Instance(window.top).Show(
                `${Common.AppPath.TicketsAppPath}ThirdParty/FastSpinResult${params}`,
                '',
                835,
                675
            );
        };
    }

    export class FACHAIResultDetails extends BaseResultDetails implements ResultDetails {
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
            const params = [
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
                '&productId=89'
            ].join('');
            Common.PopupDetails.Instance(window.top).Show(
                `${Common.AppPath.TicketsAppPath}ThirdParty/FACHAIResult${params}`,
                '',
                835,
                675
            );
        };
    }

    export class NextspinResultDetails extends BaseResultDetails implements ResultDetails {
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
            const params = [
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
                '&productId=85'
            ].join('');
            Common.PopupDetails.Instance(window.top).Show(
                `${Common.AppPath.TicketsAppPath}ThirdParty/NextspinResult${params}`,
                '',
                835,
                675
            );
        };
    }

    export class ONCasinoResultDetails extends BaseResultDetails implements ResultDetails {
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
            const params = [
                '?custId=',
                custId,
                '&transId3rd=',
                transId3rd,
                '&currencyId=',
                currencyId,
                '&siteId=',
                siteId,
                '&endtime=',
                winLostDate,
                '&productId=96'
            ].join('');
            Common.PopupDetails.Instance(window.top).Show(
                `${Common.AppPath.TicketsAppPath}ThirdParty/ONCasinoResult${params}`,
                '',
                835,
                675
            );
        };
    }

    export class PlaytechResultDetails extends BaseResultDetails implements ResultDetails {
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
            const params = [
                '?custId=',
                custId,
                '&transId3rd=',
                encodeParam(transId3rd),
                '&gameId3rd=',
                encodeParam(gameId3rd),
                '&currencyId=',
                currencyId,
                '&siteId=',
                siteId,
                '&BetType=',
                betTypeId,
                '&endtime=',
                winLostDate,
                '&productId=100'
            ].join('');
            Common.PopupDetails.Instance(window.top).Show(
                `${Common.AppPath.TicketsAppPath}ThirdParty/PlaytechResult${params}`,
                '',
                835,
                675
            );
        };
    }

    export class GPIResultDetails extends BaseResultDetails implements ResultDetails {
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
            const params = [
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
                '&productId=48'
            ].join('');
            Common.PopupDetails.Instance(window.top).Show(
                `${Common.AppPath.TicketsAppPath}ThirdParty/GPIResult${params}`,
                '',
                835,
                675
            );
        };
    }
    
    export class GPILiveCasinoResultDetails extends BaseResultDetails implements ResultDetails {
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
            siteId?: string) => {
            var params = '?custId=' + custId +
                '&transId3rd=' + transId3rd +
                '&currencyId=' + currencyId +
                '&siteId=' + siteId +
                '&endtime=' + winLostDate +
                '&productId=90';
            Common.PopupDetails.Instance(window.top).Show(
                `${Common.AppPath.TicketsAppPath}ThirdParty/GPILiveCasinoResult${params}`,
                "",
                835,
                675);
        }
    }

    export class SabaKenoResultDetails extends BaseResultDetails implements ResultDetails {
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
            siteId?: string) => {
            var params = '?custId=' + custId +
                '&transId3rd=' + transId3rd +
                '&currencyId=' + currencyId +
                '&siteId=' + siteId +
                '&productId=102';
            Common.PopupDetails.Instance(window.top).Show(Common.AppPath.TicketsAppPath + 'ThirdParty/SabaKenoResult' + params, "", 835, 675);
        }
    }

    export class PlayStarResultDetails extends BaseResultDetails implements ResultDetails {
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
            siteId?: string) => {
            var params = '?custId=' + custId +
                '&transId3rd=' + transId3rd +
                '&currencyId=' + currencyId +
                '&siteId=' + siteId +
                '&BetType=' + betTypeId +
                '&productId=91';
            Common.PopupDetails.Instance(window.top).Show(
                `${Common.AppPath.TicketsAppPath}ThirdParty/PlayStarResult${params}`,
                 "", 
                 835,
                 675);
        }
    }

    export class ASKMESLOTResultDetails extends BaseResultDetails implements ResultDetails {
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
            siteId?: string) => {
            var params = '?custId=' + custId +
                '&transId3rd=' + transId3rd +
                '&currencyId=' + currencyId +
                '&siteId=' + siteId +
                '&BetType=' + betTypeId +
                '&productId=103';
                
            Common.PopupDetails.Instance(window.top).Show(
                `${Common.AppPath.TicketsAppPath}ThirdParty/ASKMESLOTResult${params}`,
                 "", 
                 835,
                 675);
        }
    }
    
    export class FunkyGamesResultDetails extends BaseResultDetails implements ResultDetails {
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
            const params = [
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
                '&productId=104'
            ].join('');
            Common.PopupDetails.Instance(window.top).Show(
                `${Common.AppPath.TicketsAppPath}ThirdParty/FunkyGamesResult${params}`,
                '',
                835,
                675
            );
        };
    }

    export class HotDogResultDetails extends BaseResultDetails implements ResultDetails {
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
            const params = [
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
                '&productId=95'
            ].join('');
            Common.PopupDetails.Instance(window.top).Show(
                `${Common.AppPath.TicketsAppPath}ThirdParty/HotDogResult${params}`,
                '',
                835,
                675
            );
        };
    }

    export class MicrogamingLiveResultDetails extends BaseResultDetails implements ResultDetails {
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
            const params = [
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
                '&productId=107'
            ].join('');
            Common.PopupDetails.Instance(window.top).Show(
                `${Common.AppPath.TicketsAppPath}ThirdParty/MicrogamingLiveResult${params}`,
                '',
                835,
                675
            );
        };
    }

    export class Result3rdDetails extends BaseResultDetails implements ResultDetails {
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
            siteId?: string,
            sporttype?: string
        ) => {
            const params = [
                '?custId=',
                custId,
                '&transId3rd=',
                encodeParam(transId3rd),
                '&currencyId=',
                currencyId,
                '&siteId=',
                siteId,
                '&BetType=',
                betTypeId,
                '&SportTypeId=',
                sportTypeId,
                '&GameId3rd=',
                encodeParam(gameId3rd)
            ].join('');
            Common.PopupDetails.Instance(window.top).Show(
                `${Common.AppPath.TicketsAppPath}ThirdParty/ViewResult${params}`,
                '',
                835,
                675
            );
        };
    }
}

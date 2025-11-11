namespace BetList.Status.ResultDetails {
    export interface ResultDetails {
        ViewResultDetails(
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
            gameId3rd?: string,
            custId?: number,
            transId3rd?: string,
            siteId?: string,
            transdesc?: string
        ): void;
    }

    export class SportsbookResultDetails extends BaseResultDetails implements ResultDetails {
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
            transdesc?: string
        ) => {
            const params = [
                '?matchid=',
                matchId,
                '&bettypeid=',
                betTypeId,
                '&race=',
                race,
                '&sportId=',
                sportTypeId,
                '&leagueId=',
                league,
                '&isOutright=',
                isOutright,
                '&refnum=',
                refNo,
                '&betid=',
                betId,
                '&transdesc=',
                transdesc,
                '&isViewResultFromBetList=true'
            ].join('');
            const baseballSport = 8;
            const width = sportTypeId === baseballSport ? 1000 : 730;
            Common.PopupDetails.Instance(window.top).Show(this.ResultDetailsPath + params, '', width, 250);
        };
    }

    export class MixParlayResultDetails extends SportsbookResultDetails implements ResultDetails {
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
            displayBetTypeId?: string
        ) => {
            const params = [
                '?refnum=',
                refNoMixParlay,
                '&wldate=',
                winLostDate,
                '&bettypeid=',
                betTypeId,
                '&matchid=1&isViewResultFromBetList=true&displayBetTypeId=',
                displayBetTypeId
            ].join('');

            Common.PopupDetails.Instance(window.top).Show(this.ResultDetailsPath + params, '', 800, 500);
        };
    }

    export class FastMarketsResultDetails extends BaseResultDetails implements ResultDetails {
        public ViewResultDetails = (
            matchId: number,
            race: number,
            betTypeId: number,
            sportTypeId: number,
            refNo: string,
            userName: string,
            winLostDate: Date,
            refNoMixParlay: number,
            league: number,
            isOutright: boolean,
            betId: number,
            hdp2?: string
        ) => {
            const params = [
                '?matchid=',
                matchId,
                '&bettypeid=',
                betTypeId,
                '&race=',
                race,
                '&sportId=',
                sportTypeId,
                '&leagueId=',
                league,
                '&isOutright=',
                isOutright,
                '&refnum=',
                refNo,
                '&betid=',
                betId,
                '&isViewResultFromBetList=true',
                '&hdp2=',
                hdp2
            ].join('');

            Common.PopupDetails.Instance(window.top).Show(this.ResultDetailsPath + params, '', 730, 250);
        };
    }
}

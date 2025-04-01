namespace BetList.Status.ResultDetails {
    export class LeapGamingResultDetails extends BaseResultDetails implements ResultDetails {
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
            var params = [
                '?custId=',
                this.CustId,
                '&roleId=',
                this.ViewerLevelId,
                '&sportId=',
                sportTypeId,
                '&transDesc=',
                transDesc,
                '&productId=49'
            ].join('');
            Common.PopupDetails.Instance(window.top).Show(this.ResultDetailsPath + params, '', 835, 675);
        };
    }

    export class VirtualRacingResultDetails extends BaseResultDetails implements ResultDetails {
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
                '&ResultDate=',
                winLostDate,
                '&sportId=',
                sportTypeId,
                '&leagueId=',
                league,
                '&isViewResultFromBetList=true'
            ].join('');

            Common.PopupDetails.Instance(window.top).Show(this.ResultDetailsPath + params, '', 730, 250);
        };
    }
}

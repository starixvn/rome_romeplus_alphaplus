namespace BetList.Choice {
    export class ScoreMap {
        private static instance: ScoreMap;

        public static get Instance(): ScoreMap {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new ScoreMap() : this.instance;
            return this.instance;
        }

        ShowScoreMap(matchIds: string, betType: string, liveIndicator: number) {
            const scoreMapUrl = $('#hidden-ScoreMapUrl').val();
            const params =
                liveIndicator == 1
                    ? `matchIds=${matchIds}&liveBetTypes=${betType}&deadBetTypes=-1`
                    : `matchIds=${matchIds}&liveBetTypes=-1&deadBetTypes=${betType}`;

            Common.PopupDetails.Instance(window.top).Show(`${scoreMapUrl}?${params}`, '', 1008);
        }
    }
}

let OpenScoreMap: Function;

$(function () {
    OpenScoreMap = BetList.Choice.ScoreMap.Instance.ShowScoreMap;
});

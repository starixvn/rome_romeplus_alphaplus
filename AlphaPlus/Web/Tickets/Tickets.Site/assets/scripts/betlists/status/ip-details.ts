namespace BetList.Status {
    export class IpDetails {
        private static instance: IpDetails;

        public static get Instance(): IpDetails {
            this.instance = Common.Utils.IsNullOrUndefinedOrEmpty(this.instance) ? new IpDetails() : this.instance;
            return this.instance;
        }

        ShowIpInfo(ip: string) {
            Common.PopupDetails.Instance(window.top).Show(`/main/IPInfo?ip=${ip}`, 'IP Details', 400, 400);
        }
    }
}

let OpenIPInfo: Function;

$(function () {
    OpenIPInfo = BetList.Status.IpDetails.Instance.ShowIpInfo;
});

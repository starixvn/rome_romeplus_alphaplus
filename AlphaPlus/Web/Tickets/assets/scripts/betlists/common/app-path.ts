namespace Common {
    export class AppPath {
        public static OldMainAppPath = () => $('#old-main-path').val();
        public static readonly TicketsAppPath = $('#hidden-TicketBaseUrl').val();
        public static MainAppPath = () => $('#site-main-path').val();
    }
}

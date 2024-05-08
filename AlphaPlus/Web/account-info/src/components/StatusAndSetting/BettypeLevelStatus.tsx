import StatusItem from '../../models/status-item';

export default function BettypeLevelStatus({ status }: { status: StatusItem }) {
    return (
        <div className="col-xs-6">
            <span className="icon-ic_list" />
            <span className="link">{status.Text}</span>
        </div>
    );
}

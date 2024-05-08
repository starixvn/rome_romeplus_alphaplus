import classNames from 'classnames';

import StatusItem from '../../models/status-item';

export default function ConfirmStatus({ status }: { status: StatusItem }) {
    return (
        <div className={classNames('col-xs-6', 'checkbox', { ['disabled']: status.Disabled })}>
            <input type="checkbox" checked={status.Checked} />
            <label className="label-text" htmlFor={status.DisplayOrder}>
                {status.Text}
            </label>
        </div>
    );
}

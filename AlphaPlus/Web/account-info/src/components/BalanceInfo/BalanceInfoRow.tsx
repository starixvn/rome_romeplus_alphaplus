import classNames from 'classnames';

import CellContainNumberFormat from '../common/CellContainNumberFormat';
import styles from './BalanceInfo.module.scss';

type Props = {
    title: string;
    value: number;
    hrefLink?: string;
};

export default function BalanceInfoRow({ title, value, hrefLink }: Props) {
    return (
        <div className={classNames('row-info', styles['row-info'])}>
            <div className={classNames('col', styles['col'])}>{title}</div>
            <CellContainNumberFormat
                {...{
                    value,
                    hrefLink,
                    classNames: 'col col-right'
                }}
            />
        </div>
    );
}

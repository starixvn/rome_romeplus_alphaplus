import classNames from 'classnames';
import { Tooltip } from 'react-tippy';
import { NumericFormat } from 'react-number-format';
import styles from './BalanceInfo.module.scss';

type Props = {
    title: string;
    value: number;
    currency: string;
    tooltip: string;
};
export default function RewardRow({ title, value, currency, tooltip }: Props) {
    return <div className={classNames('row-info', styles['row-info'])}>
        <div className={classNames('col', styles['col'])}>{title}
        <Tooltip
            {...{
                className: classNames('icon-information-outline', styles['icon-information-outline']),
                title: tooltip,
                position: 'bottom',
                trigger: 'mouseenter',
                arrow: true
            }}></Tooltip>
        </div>
        <div className={classNames('col', styles['col'], 'col-right', styles['number'])}>
            <span className={styles['currency']}>{currency}</span>
            <NumericFormat value={value} displayType="text" decimalScale={2} fixedDecimalScale thousandSeparator />
        </div>
    </div>;
}

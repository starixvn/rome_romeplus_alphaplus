import classNames from 'classnames';
import { useTranslation } from 'react-i18next';
import { NumericFormat } from 'react-number-format';

import styles from './BalanceInfo.module.scss';
import BalanceInfoRow from './BalanceInfoRow';

type Props = {
    editable: boolean;
    givenCredit: number;
};
export default function GivenCreditRow({ editable, givenCredit }: Props) {
    const { t } = useTranslation();

    const handleOpenGivenCreditPopup = () => {
        // TODO:
    };

    if (editable) {
        return (
            <div className={classNames('row-info', styles['row-info'])}>
                <div className={classNames('col', styles['col'])}>{t('GivenCredit')}</div>
                <div className={classNames('col', styles['col'], 'col-right')} onClick={handleOpenGivenCreditPopup}>
                    <div className={styles['given-credit']}>
                        <span className="icon-border-color icon-edited" />
                        <NumericFormat
                            className={styles['number-link']}
                            value={givenCredit}
                            displayType="text"
                            decimalScale={2}
                            fixedDecimalScale
                            thousandSeparator
                        />
                    </div>
                </div>
            </div>
        );
    }

    return <BalanceInfoRow title={t('GivenCredit')} value={givenCredit} />;
}

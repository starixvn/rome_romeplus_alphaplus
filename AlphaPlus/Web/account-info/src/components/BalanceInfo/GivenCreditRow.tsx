import { useTranslation } from 'react-i18next';
import { NumericFormat } from 'react-number-format';

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
            <div className="row-info">
                <div className="col">{t('GivenCredit')}</div>
                <div id="given-credit-popup" className="col col-right" onClick={handleOpenGivenCreditPopup}>
                    <div className="given-credit">
                        <span className="icon-border-color icon-edited" />
                        <NumericFormat
                            className="number-link"
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

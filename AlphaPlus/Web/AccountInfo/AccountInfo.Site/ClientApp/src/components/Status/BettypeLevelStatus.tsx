import { useTranslation } from 'react-i18next';

import { ProductConstants } from '../../constants';
import { useGlobalContext } from '../../context/global-context';
import { mobileScale, resizePopupOnMobile } from '../../helpers';
import StatusItem from '../../models/status-item';

export default function BettypeLevelStatus({ status }: { status: StatusItem }) {
    const left = 200,
        top = 200,
        width = 475;
    const { t } = useTranslation();
    const { mobileView, username } = useGlobalContext();

    const getPopupHeightByProduct = (productId: number) => {
        switch (productId) {
            case ProductConstants.Keno:
                return 530;
            case ProductConstants.LiveCasino:
            case ProductConstants.RNGLottery:
                return 430;
            default:
                return 300;
        }
    };

    const handleOpenPopup = () => {
        const productName = status.Text;
        const popupTitle = `${t('Update')} ${productName} ${t('Status')} - ${username}`;
        const height = getPopupHeightByProduct(status.ProductId);
        window.top?.ageWnd.open(status.Url, popupTitle, left, top, width, mobileView ? mobileScale(height) : height);

        if (mobileView) {
            resizePopupOnMobile();
        }
    };
    return (
        <div className="col-xs-6" id={`product-${status.Text}`} onClick={handleOpenPopup}>
            <span className="icon-ic_list" />
            <span className="link">{status.Text}</span>
        </div>
    );
}

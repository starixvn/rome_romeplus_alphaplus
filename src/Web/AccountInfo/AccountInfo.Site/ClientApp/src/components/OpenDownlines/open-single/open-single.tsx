import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';

import { usernameRegExp } from '../../../helpers';
import Result from '../../../models/result';
import { handleError } from '../../../services/axios-helper';
import OpenAPI, { OpenSingleDataViewModel } from '../open-api';
import { viewInformation } from '../open-helper';
import { BodyRowContent, FooterContent } from './open-single-components';

type Props = {
    viewModel: OpenSingleDataViewModel;
    redirectViewListDetail: (isAffected: boolean, downlineRoleId: number) => void;
};

export default function OpenSingle({ viewModel, redirectViewListDetail }: Props) {
    const [message, setMessage] = useState<React.ReactNode>('');
    const { t } = useTranslation();

    const doUpdate = () => {
        $.blockUI?.();

        OpenAPI.updateClose<Result>('OpenDownlines/OpenSingle', { SearchAccountId: viewModel.SearchAccountId })
            .then((data) => {
                const message = (
                    <div className="msg-container">
                        <div className={`msg ${data.IsSuccessful ? 'msg-success' : 'msg msg-error'}`}>
                            {data.IsSuccessful ? t('OpenDownlines.UpdateSuccessfully') : data.Message.trim()}
                        </div>
                    </div>
                );

                setMessage(message);

                if (data.IsSuccessful) {
                    if (top?.customerListNew) {
                        try {
                            top.successfulUpdatingCallbackAndReloadMainFrame(true);
                        } catch (error) {
                            $.unblockUI?.();
                        }
                    }

                    if (
                        window.top?.reloadCustomerList &&
                        window.top.main.DelayReloadPage &&
                        typeof window.top.main.DelayReloadPage == 'function'
                    ) {
                        window.top.main.DelayReloadPage();
                    }

                    window.top?.confirmPopupPopup &&
                        window.top.confirmPopupPopup.onClosing &&
                        window.top.confirmPopupPopup.onClosing(viewModel.SearchAccountName, true);
                }

                $.unblockUI?.();
            })
            .catch(handleError);
    };

    const handleSubmit = () => {
        const message =
            viewModel.AffectedDownlinesTotal + viewModel.UnchangedDownlinesTotal === 0
                ? t('OpenedWithNoDownlinesMessages')
                : t('SingleConfirm');

        window.top?.bootboxConfirm(message, undefined, doUpdate);
    };

    const affectedDownlinesTotal = viewModel.AffectedDownlinesTotal,
        unchangedDownlinesTotal = viewModel.UnchangedDownlinesTotal,
        levels = viewModel.Levels;

    return (
        <div className="col-xs-12">
            {message}
            <p className="note">
                {usernameRegExp(t('OpenSingleNote'), viewModel.SearchAccountName)}
                <span onClick={() => viewInformation(t)} className="icon-information-outline" />
            </p>

            <div>
                <table className="tblRpt tblRpt-bordered width-100per">
                    <thead>
                        <tr>
                            {[t('Level'), t('AffectedDownlines'), t('RemainUnchangedDownlines')].map((text, index) => (
                                <th key={index}>{text} </th>
                            ))}
                        </tr>
                    </thead>
                    <tbody>
                        {affectedDownlinesTotal + unchangedDownlinesTotal > 0 &&
                            levels.map((level, index) => (
                                <BodyRowContent {...{ level, redirectViewListDetail, key: index }} />
                            ))}
                    </tbody>
                    <FooterContent
                        {...{
                            affectedDownlinesTotal,
                            unchangedDownlinesTotal,
                            redirectViewListDetail,
                            totalText: t('Total'),
                            noData: t('NoData')
                        }}
                    />
                </table>
            </div>
            <div className="row-info popup-bottom text-right">
                <button className="btn-submit" onClick={handleSubmit}>
                    {t('Open')}
                </button>
            </div>
        </div>
    );
}

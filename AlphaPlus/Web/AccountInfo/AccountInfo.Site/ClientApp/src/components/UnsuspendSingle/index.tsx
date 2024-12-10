import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';

import { usernameRegExp } from '../../helpers';
import Result from '../../models/result';
import { UnsuspendSingleDataViewModel } from '../../models/UnsuspenDownlines/UnsuspendSingleDataViewModel';
import { handleError } from '../../services/axios-helper';
import UnsuspendAPI from '../../services/UnsuspendAPI';
import { viewInformation } from '../unsuspend-helper';
import { BodyRowContent, FooterContent } from './unsuspend-single-components';

type Props = {
    viewModel: UnsuspendSingleDataViewModel;
    redirectViewListDetail: (isAffected: boolean, downlineRoleId: number) => void;
};

export default function UnsuspendSingle({ viewModel, redirectViewListDetail }: Props) {
    const { t } = useTranslation();
    const [message, setMessage] = useState<React.ReactNode>(null);

    const doUpdate = () => {
        $.blockUI?.();

        UnsuspendAPI.updateSuspend<Result>('UnsuspendDownlines/UnsuspendSingle', {
            CustId: viewModel.SearchAccountId
        })
            .then((data) => {
                const message = (
                    <div className="msg-container">
                        <div className={`msg ${data.IsSuccessful ? 'msg-success' : 'msg msg-error'}`}>
                            {data.IsSuccessful ? t('UpdateSuccessfully') : data.Message.trim()}
                        </div>
                    </div>
                );

                setMessage(message);

                if (data.IsSuccessful) {
                    if (top?.customerListNew) {
                        top.successfulUpdatingCallbackAndReloadMainFrame(true);
                    }

                    if (
                        window?.top?.reloadCustomerList &&
                        window.top?.main.DelayReloadPage &&
                        typeof window?.top?.main.DelayReloadPage == 'function'
                    ) {
                        window.top?.main.DelayReloadPage();
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
                ? t('UnsuspendedWithNoDownlinesMessages')
                : t('MultipleConfirm');

        window?.top?.bootboxConfirm(message, undefined, doUpdate);
    };

    const affectedDownlinesTotal = viewModel.AffectedDownlinesTotal,
        unchangedDownlinesTotal = viewModel.UnchangedDownlinesTotal,
        levels = viewModel.Levels;

    return (
        <div className="col-xs-12">
            {message}
            <p className="note">
                {usernameRegExp(t('UnsuspendSingleNote'), viewModel.SearchAccountName)}
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
                    {t('Unsuspended')}
                </button>
            </div>
        </div>
    );
}

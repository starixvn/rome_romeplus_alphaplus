import { Markup } from 'interweave';
import { isEqual } from 'lodash';
import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';

import {
    UnsuspendMultipleModel,
    UnsuspendMultipleResult
} from '../../models/UnsuspenDownlines/UnsuspendMultipleViewModel';
import UnsuspendAPI from '../../services/UnsuspendAPI';
import { getMultipleConfirmMessage, viewInformation } from '../unsuspend-helper';
import { RowContent } from './unsuspend-multiple-components';

type Props = {
    searchAccountRoleId: number;
    viewModel: UnsuspendMultipleModel[];
    originalViewModel: UnsuspendMultipleModel[];
    handleReset: () => void;
    toggleSuspendAll: (suspendAll: boolean) => void;
    handleChange: (index: number, value: boolean) => void;
    redirectViewListDetail: (
        searchAccountId: number,
        searchAccountName: string,
        downlineRoleId: number,
        isAffected: boolean
    ) => void;
};

export default function UnsuspendMultiple({
    searchAccountRoleId,
    viewModel,
    originalViewModel,
    handleReset,
    toggleSuspendAll,
    handleChange,
    redirectViewListDetail
}: Props) {
    const [message, setMessage] = useState<React.ReactNode>('');
    const { t } = useTranslation();
    const isNotMember = searchAccountRoleId > 1;
    const headers =
        searchAccountRoleId === 3
            ? [t('AffectedDownlinesAgentMember'), t('RemainUnchangedDownlinesAgentMember')]
            : [t('AffectedDownlinesMember'), t('RemainUnchangedDownlinesMember')];

    const doUpdate = () => {
        $.blockUI?.();
        const params: { SearchAccountId: number; Username: string; Suspended: boolean }[] = [];

        viewModel.map(function (item, index) {
            if (item.Suspended !== originalViewModel[index].Suspended) {
                params.push({
                    SearchAccountId: item.SearchAccountId,
                    Username: item.Username,
                    Suspended: item.Suspended
                });
            }
        });

        UnsuspendAPI.updateSuspend<UnsuspendMultipleResult>('UnsuspendDownlines/UnsuspendMultiple', {
            UnsuspendList: params,
            RoleId: searchAccountRoleId
        }).then((data) => {
            let isSuccess = true;
            const timeoutDefault = 3000;
            const results = data.Results;

            const message = (
                <div className="msg-container">
                    {results.map((result, index) => {
                        if (!result && isSuccess) {
                            isSuccess = false;
                        }

                        return (
                            <div key={index} className={result.IsSuccessful ? 'msg msg-success' : 'msg msg-error'}>
                                {result.Message}
                            </div>
                        );
                    })}
                </div>
            );

            setMessage(message);
            if (isSuccess) {
                if (top?.customerListNew) {
                    top?.successfulUpdatingCallbackAndReloadMainFrame(true);
                } else {
                    window.top?.main.DelayReloadPage();
                    setTimeout(() => {
                        window.top?.ageWnd.close();
                    }, timeoutDefault);
                }
            }

            $.unblockUI?.();
        });
    };

    const handleSubmit = () => {
        const message = getMultipleConfirmMessage(t, searchAccountRoleId, viewModel, originalViewModel);
        window.top?.bootboxConfirm(message, null, doUpdate);
    };

    return (
        <div className="col-xs-12">
            {message}
            <p className="note">
                <span onClick={() => viewInformation(t)} className="icon-information-outline" />
            </p>
            <table className="tblRpt tblRpt-bordered width-100per">
                <thead>
                    <tr>
                        <th rowSpan={2}>{t('Username')}</th>
                        <th>{t('SuspendStatus')}</th>
                        {isNotMember &&
                            headers.map((text, index) => (
                                <th rowSpan={2} key={index}>
                                    <Markup content={text} />
                                </th>
                            ))}
                    </tr>
                    <tr>
                        <th>
                            {[
                                {
                                    text: t('Suspended'),
                                    class: `btn-suspend-all`,
                                    suspended: true
                                },
                                {
                                    text: t('Unsuspended'),
                                    class: `btn-unsuspend-all`,
                                    suspended: false
                                }
                            ].map((button, index) => (
                                <button
                                    className={button.class}
                                    key={index}
                                    onClick={() => toggleSuspendAll(button.suspended)}
                                >
                                    {button.text}
                                </button>
                            ))}
                        </th>
                    </tr>
                </thead>
                <tbody>
                    {viewModel.map((dataRow, index) => (
                        <RowContent
                            {...{
                                t,
                                originaldataRow: originalViewModel[index],
                                dataRow,
                                searchAccountRoleId,
                                index,
                                handleChange,
                                redirectViewListDetail,
                                key: index
                            }}
                        />
                    ))}
                </tbody>
            </table>
            <div className="row-info popup-bottom list-button text-right">
                {[
                    {
                        text: t('Reset'),
                        class: `btn-reset`,
                        disabled: false,
                        onclick: handleReset
                    },
                    {
                        text: t('Submit'),
                        class: `btn-submit`,
                        disabled: isEqual(viewModel, originalViewModel),
                        onclick: handleSubmit
                    }
                ].map((button, index) => (
                    <button className={button.class} key={index} disabled={button.disabled} onClick={button.onclick}>
                        {button.text}
                    </button>
                ))}
            </div>
        </div>
    );
}

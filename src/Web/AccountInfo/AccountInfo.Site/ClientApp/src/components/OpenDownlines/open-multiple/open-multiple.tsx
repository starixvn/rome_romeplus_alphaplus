import { Markup } from 'interweave';
import { isEqual } from 'lodash';
import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';

import OpenAPI, { OpenMultipleModel, OpenMultipleResultViewModel } from '../open-api';
import { getMultipleConfirmMessage, viewInformation } from '../open-helper';
import { RowContent } from './open-multiple-components';

type Props = {
    searchAccountRoleId: number;
    viewModel: OpenMultipleModel[];
    originalViewModel: OpenMultipleModel[];
    handleReset: () => void;
    toggleCloseAll: (closeAll: boolean) => void;
    handleChange: (index: number, value: boolean) => void;
    redirectViewListDetail: (
        searchAccountId: number,
        searchAccountName: string,
        downlineRoleId: number,
        isAffected: boolean
    ) => void;
};

export default function OpenMultiple({
    viewModel,
    originalViewModel,
    searchAccountRoleId,
    redirectViewListDetail,
    handleChange,
    handleReset,
    toggleCloseAll
}: Props) {
    const [message, setMessage] = useState<React.ReactNode>('');
    const { t } = useTranslation();

    const doUpdate = () => {
        $.blockUI?.();
        const params: { SearchAccountId: number; Username: string; IsClosed: boolean }[] = [];

        viewModel.forEach(function (item, index) {
            if (item.IsClosed !== originalViewModel[index].IsClosed) {
                params.push({
                    SearchAccountId: item.SearchAccountId,
                    Username: item.Username,
                    IsClosed: item.IsClosed
                });
            }
        });

        OpenAPI.updateClose<OpenMultipleResultViewModel>('OpenDownlines/OpenMultiple', {
            OpenList: params,
            RoleId: searchAccountRoleId
        }).then((data) => {
            let isSuccess = true;
            const timeoutDefault = 3000;

            const message = (
                <div className="msg-container">
                    {data.Results.map((result, index) => {
                        if (!result.IsSuccessful && isSuccess) {
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
                    top.successfulUpdatingCallbackAndReloadMainFrame(true);
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

    const isNotMember = searchAccountRoleId > 1,
        headers =
            searchAccountRoleId === 3
                ? [t('AffectedDownlinesAgentMember'), t('RemainUnchangedDownlinesAgentMember')]
                : [t('AffectedDownlinesMember'), t('RemainUnchangedDownlinesMember')];

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
                        <th>{t('CloseStatus')}</th>
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
                                    text: t('Closed'),
                                    class: `btn-suspend-all`,
                                    isClosed: true
                                },
                                {
                                    text: t('Opened'),
                                    class: `btn-unsuspend-all`,
                                    isClosed: false
                                }
                            ].map((button, index) => (
                                <button
                                    className={button.class}
                                    key={index}
                                    onClick={() => toggleCloseAll(button.isClosed)}
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
                                originalDataRow: originalViewModel[index],
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

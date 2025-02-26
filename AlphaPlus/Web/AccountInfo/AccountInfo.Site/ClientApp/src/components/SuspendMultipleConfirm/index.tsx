import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';

import { UnsuspendMultipleResult } from '../../models/UnsuspenDownlines/UnsuspendMultipleViewModel';
import UnsuspendAPI from '../../services/UnsuspendAPI';
import { SuspendMultipleConfirmDataViewModel } from './SuspendMultipleConfirmDataViewModel';

export default function SuspendMultipleConfirmContainer({
    viewModel
}: {
    viewModel: SuspendMultipleConfirmDataViewModel;
}) {
    const { t } = useTranslation();
    const [hideMore, setHideMore] = useState(true);
    const [hideMessages, setHideMessages] = useState(true);
    const [message, setMessage] = useState<React.ReactNode>('');

    const handleCancel = () => {
        window.top?.popupManager.close();
    };

    const showMore = () => {
        setHideMore(!hideMore);
    };

    const handleSubmit = () => {
        $.blockUI?.();

        UnsuspendAPI.updateSuspend<UnsuspendMultipleResult>('UnsuspendDownlines/UnsuspendMultiple', {
            UnsuspendList: viewModel.UnsuspendList,
            RoleId: viewModel.RoleId
        }).then((data) => {
            let isSuccess = true;
            const timeoutDefault = 3000;
            const results = data.Results;

            const message = (
                <div className="msg-container">
                    {results.map((result, index) => {
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
            setHideMessages(false);
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

    return (
        <div className="content-page">
            <div id="message-container" className="message-container">
                {hideMessages ? <div></div> : message}
            </div>
            <div className="review-content">
                {viewModel.DisplayShowAllAccountOption ? (
                    <div>
                        <div className="shorten-content">
                            <span className="total">{viewModel.TotalSelectedText}&nbsp;</span>
                            {viewModel.ShortenUsernames.join(', ')}
                        </div>

                        <span style={hideMore ? { display: 'inline' } : { display: 'none' }} className="more-ellipses">
                            ...{' '}
                        </span>
                        <div className="more-content">
                            <span style={hideMore ? { display: 'none' } : { display: 'inline' }}>
                                , {viewModel.MoreDownlineUsernames.join(', ')} &nbsp;
                            </span>
                        </div>
                        <a className="toggle" href="javascript:void(0)" onClick={showMore}>
                            {hideMore ? <span>{t('ShowMore')}</span> : <span>{t('ShowLess')}</span>}
                        </a>
                    </div>
                ) : (
                    <div className="shorten-content">
                        <span className="total">{viewModel.TotalSelectedText}</span>
                        {viewModel.ShortenUsernames.join(', ')}
                    </div>
                )}
            </div>

            <div className="list-button text-right">
                {[
                    {
                        text: t('Cancel'),
                        class: `btn-cancel`,
                        onclick: handleCancel
                    },
                    {
                        text: t('Submit'),
                        class: `btn-submit`,
                        onclick: handleSubmit
                    }
                ].map((button, index) => (
                    <button className={button.class} key={index} onClick={button.onclick}>
                        {button.text}
                    </button>
                ))}
            </div>
        </div>
    );
}

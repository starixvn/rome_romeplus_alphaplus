import axios from 'axios';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import Result from '../../models/result';
import { handleError } from '../../services/axios-helper';
import styles from './RemarkInfo.module.scss';

export default function RemarkInfo() {
    const { t } = useTranslation();
    const {
        searchAccountId,
        searchAccountName,
        searchAccountRoleId,
        currentRemark
    } = remarkParams;

    const [hideMessages, setHideMessages] = useState(true);
    const [updateSuccess, setUpdateSuccess] = useState(true);
    const [message, setMessage] = useState('');
    const [newRemark, setNewRemark] = useState(currentRemark);
    const showErrorMessage = (msg: string) => {
        setHideMessages(false);
        setMessage(msg);
        setUpdateSuccess(false);
    };
    const handleSubmit = () => {
        if (newRemark === currentRemark) {
            showErrorMessage(t('ThereIsNoChange'));
            return;
        }
        $.blockUI?.();
        updateRemark(searchAccountId, searchAccountRoleId, newRemark);
    };
    const handleClose = (updateSuccessfully = false) => {
        if (
            window.top?.updateRemarkPopup &&
            window.top.updateRemarkPopup.onClosing &&
            typeof window.top.updateRemarkPopup.onClosing === 'function'
        ) {
            window.top.updateRemarkPopup.onClosing(
                searchAccountName,
                updateSuccessfully,
                newRemark,
                searchAccountId
            );
        }
    };

    const updateRemark = async (searchAccountID: number, searchAccountRoleID: number, remark: string) => {
        return axios
            .post<Result>(
                `${window.top?.alpha.urls['accountInfoRootPath']}SearchAccount/UpdateRemark`,
                {
                    searchAccountId: searchAccountID,
                    searchAccountRoleId: searchAccountRoleId,
                    remark: remark
                },
                {
                    headers: {
                        RequestVerificationToken: $("input[name='__RequestVerificationToken']").val(),
                        'X-Requested-With': 'XMLHttpRequest'
                    }
                }
            )
            .then((response) => response.data)
            .then((data) => {
                setHideMessages(false);
                setMessage(data.IsSuccessful ? t('UnsuspendedNote.UpdateSuccessfully') : data.Message.trim());
                setUpdateSuccess(data.IsSuccessful);

                if (data.IsSuccessful) {
                    handleClose(data.IsSuccessful);
                }

                $.unblockUI?.();
            })
            .catch(handleError)
            .finally(() => {
                $.unblockUI?.();
            });
    };
    const handleChangeRemark = (e: React.ChangeEvent<HTMLTextAreaElement>) => {
        setNewRemark(e.target.value);
    };

    return (
        <div className={styles['remark-info']}>
            <div className={hideMessages ? `hidden` : ``}>
                <span className={`col-xs-12 msg ${styles.msg} ${updateSuccess ? 'msg-success' : 'msg-error'}`}>
                    {message}
                </span>
            </div>
            <div>
                <label htmlFor="remark">{t('Remark')}</label>
                <textarea value={newRemark} onChange={handleChangeRemark} name="remark" id="remark" style={{ width: '100%', height: '100px' }} />
            </div>

            <div style={{ display: 'flex', justifyContent: 'flex-end', marginTop: '10px' }}>
                <button className="btn-cancel" onClick={() => handleClose()} >
                    {t('Cancel')}
                </button>
                <button className="btn-submit" onClick={handleSubmit} style={{ marginRight: '0' }}>
                    {t('Update')}
                </button>
            </div>
        </div>
    )
}

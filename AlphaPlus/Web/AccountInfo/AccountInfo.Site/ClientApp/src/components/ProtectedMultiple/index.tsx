import { cloneDeep, isEqual } from 'lodash';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';

import { CustomerProtectedStatus } from '../../models/ProtectedMultiple/CustomerProtectedStatus';
import ProtectedStatusAPI from '../../services/protected-status-api';
import RowContent from './RowContent';

type Props = {
    viewModel: CustomerProtectedStatus[];
};

export default function ProtectedMultiple(props: Props) {
    const [viewModel, setViewModel] = useState(props.viewModel);
    const [message, setMessage] = useState<React.ReactNode>('');
    const { t } = useTranslation();

    const toggleProtectedAll = (isProtected: boolean) => {
        const viewModelCopy = cloneDeep(viewModel);
        viewModelCopy.forEach((item) => {
            item.IsProtectedMode = isProtected;
        });

        setViewModel(viewModelCopy);
    };

    const handleChange = (index: number, value: boolean) => {
        const viewModelCopy = cloneDeep(viewModel);

        viewModelCopy[index].IsProtectedMode = value;
        setViewModel(viewModelCopy);
    };

    const handleReset = () => {
        setViewModel(props.viewModel);
    };

    const handleSubmit = () => {
        window.top?.bootboxConfirm(t('EditMultipleConfirmMessage'), null, handleEdit);
    };

    const handleEdit = () => {
        $.blockUI?.();
        const formData: CustomerProtectedStatus[] = [];

        viewModel.map(function (item, index) {
            if (item.IsProtectedMode !== props.viewModel[index].IsProtectedMode) {
                formData.push(item);
            }
        });

        ProtectedStatusAPI.editMultiple({ ProtectedStatus: formData }).then((data) => {
            let isSuccess = true;
            const dataResults = Array.isArray(data) ? data : [data];

            const message = (
                <div className="msg-container">
                    {dataResults.map((result, index) => {
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
                    }, 3000);
                }
            }

            $.unblockUI?.();
        });
    };

    return (
        <div className="col-xs-12">
            {message}
            <table className="tblRpt tblRpt-bordered width-100per">
                <thead>
                    <tr>
                        <th rowSpan={2}>{t('Username')}</th>
                        <th>{t('ProtectionStatus')}</th>
                    </tr>
                    <tr>
                        <th>
                            <button className="btn-protected-all" onClick={() => toggleProtectedAll(true)}>
                                {t('Protected')}
                            </button>
                            <button className="btn-unprotected-all" onClick={() => toggleProtectedAll(false)}>
                                {t('Unprotected')}
                            </button>
                        </th>
                    </tr>
                </thead>
                <tbody>
                    {viewModel.map((dataRow, index) => (
                        <RowContent
                            {...{
                                dataRow,
                                index,
                                handleChange,
                                t,
                                key: index
                            }}
                        />
                    ))}
                </tbody>
            </table>
            <div className="row-info popup-bottom list-button text-right">
                <button className="btn-reset" onClick={handleReset}>
                    {t('Reset')}
                </button>
                <button className="btn-submit" disabled={isEqual(viewModel, props.viewModel)} onClick={handleSubmit}>
                    {t('Submit')}
                </button>
            </div>
        </div>
    );
}

import { TFunction } from 'i18next';

import { CustomerProtectedStatus } from '../../models/ProtectedMultiple/CustomerProtectedStatus';

type Props = {
    dataRow: CustomerProtectedStatus;
    index: number;
    handleChange: (index: number, value: boolean) => void;
    t: TFunction;
};

export default function RowContent({ dataRow, index, handleChange, t }: Props) {
    let className, text;
    if (dataRow.IsProtectedMode) {
        className = 'protected-button';
        text = t('Protected');
    } else {
        className = 'unprotected-button';
        text = t('Unprotected');
    }

    return (
        <tr>
            <td>{dataRow.Username}</td>
            <td className="text-center">
                <div onClick={() => handleChange(index, !dataRow.IsProtectedMode)} className={className}>
                    {text}
                </div>
            </td>
        </tr>
    );
}

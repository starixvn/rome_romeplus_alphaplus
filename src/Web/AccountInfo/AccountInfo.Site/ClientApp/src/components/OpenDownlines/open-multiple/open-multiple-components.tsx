import { TFunction } from 'i18next';
import React from 'react';

import { OpenMultipleModel } from '../open-api';

export const ColumnContent = ({
    dataRow,
    isAffected,
    redirectViewListDetail
}: {
    dataRow: OpenMultipleModel;
    isAffected: boolean;
    redirectViewListDetail: (
        searchAccountId: number,
        searchAccountName: string,
        downlineRoleId: number,
        isAffected: boolean
    ) => void;
}) => {
    return (
        <div>
            {dataRow.Downlines.map((level, index) => {
                const downlines = isAffected ? level.AffectedDownlines : level.UnchangedDownlines;

                return (
                    <span key={index}>
                        {downlines === 0 ? (
                            downlines
                        ) : (
                            <a
                                key={index}
                                href="#"
                                onClick={() =>
                                    redirectViewListDetail(
                                        dataRow.SearchAccountId,
                                        dataRow.Username,
                                        level.DownlineRoleId,
                                        isAffected
                                    )
                                }
                            >
                                {downlines}
                            </a>
                        )}
                        {index < dataRow.Downlines.length - 1 ? ` / ` : ``}
                    </span>
                );
            })}
        </div>
    );
};

export const RowContent = ({
    t,
    originalDataRow,
    dataRow,
    searchAccountRoleId,
    index,
    handleChange,
    redirectViewListDetail
}: {
    t: TFunction;
    originalDataRow: OpenMultipleModel;
    dataRow: OpenMultipleModel;
    searchAccountRoleId: number;
    index: number;
    handleChange: (index: number, value: boolean) => void;
    redirectViewListDetail: (
        searchAccountId: number,
        searchAccountName: string,
        downlineRoleId: number,
        isAffected: boolean
    ) => void;
}) => {
    const isNotMember = searchAccountRoleId > 1,
        showValue = originalDataRow.IsClosed !== dataRow.IsClosed,
        header = dataRow.IsClosed
            ? { text: t('Closed'), buttonClass: 'suspend-button' }
            : { text: t('Opened'), buttonClass: 'unsuspend-button' };

    return (
        <tr>
            <td> {dataRow.Username} </td>
            <td className="text-center">
                <div onClick={() => handleChange(index, !dataRow.IsClosed)} className={header.buttonClass}>
                    {header.text}
                </div>
            </td>
            {isNotMember &&
                [true, false].map((isAffected, key) => {
                    const text = isAffected ? t('All') : '';

                    return (
                        <td className="text-center" key={key}>
                            {showValue &&
                                (originalDataRow.IsClosed ? (
                                    <ColumnContent {...{ dataRow, redirectViewListDetail, isAffected }} />
                                ) : (
                                    <span>{text}</span>
                                ))}
                        </td>
                    );
                })}
        </tr>
    );
};

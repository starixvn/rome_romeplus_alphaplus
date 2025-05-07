import React from 'react';
import { useTranslation } from 'react-i18next';
import { Promotion } from '../../../services/interfaces/Promotion';
import { NoRecordsIcon } from '../../../components/Icons';

interface TableProps {
    items: Promotion[];
    isLoading: boolean;
}

const Table: React.FC<TableProps> = ({
    items,
    isLoading
}) => {
    const { t } = useTranslation('promotion');

    const getStatusStyle = (status: string) => {

        var s = status ? status.toLocaleUpperCase() : '';
        switch (s) {
            case 'ACTIVE':
                return 'bg-[#DCFCE7] text-[#166534]';
            case 'SCHEDULED':
                return 'bg-[#F3F4F6] text-[#374151]';
            case 'EXPIRED':
                return 'bg-[#FEE2E2] text-[#991B1B]';
            default:
                return 'bg-gray-100 text-gray-800';
        }
    };

    const buildHeader = () => (
        <thead>
            <tr>
                <th
                    scope="col"
                    className="px-4 py-2 text-left text-sm font-medium text-gray-700 border border-gray-300 bg-gray-50 text-center"
                >
                    {t('table.action')}
                </th>
                <th
                    scope="col"
                    className="px-4 py-2 text-left text-sm font-medium text-gray-700 border border-gray-300 bg-gray-50"
                >
                    {t('table.promotionTitle')}
                </th>
                <th
                    scope="col"
                    className="px-4 py-2 text-left text-sm font-medium text-gray-700 border border-gray-300 bg-gray-50"
                >
                    {t('table.promotionType')}
                </th>
                <th
                    scope="col"
                    className="px-4 py-2 text-left text-sm font-medium text-gray-700 border border-gray-300 bg-gray-50"
                >
                    {t('table.sports')}
                </th>
                <th
                    scope="col"
                    className="px-4 py-2 text-left text-sm font-medium text-gray-700 border border-gray-300 bg-gray-50"
                >
                    {t('table.virtualSports')}
                </th>
                <th
                    scope="col"
                    className="px-4 py-2 text-left text-sm font-medium text-gray-700 border border-gray-300 bg-gray-50"
                >
                    {t('table.games')}
                </th>
                <th
                    scope="col"
                    className="px-4 py-2 text-left text-sm font-medium text-gray-700 border border-gray-300 bg-gray-50"
                >
                    {t('table.liveCasino')}
                </th>
                <th
                    scope="col"
                    className="px-4 py-2 text-left text-sm font-medium text-gray-700 border border-gray-300 bg-gray-50"
                >
                    {t('table.period')}
                </th>
                <th
                    scope="col"
                    className="px-4 py-2 text-left text-sm font-medium text-gray-700 border border-gray-300 bg-gray-50"
                >
                    {t('table.provider')}
                </th>
                <th
                    scope="col"
                    className="px-4 py-2 text-left text-sm font-medium text-gray-700 border border-gray-300 bg-gray-50"
                >
                    {t('table.status')}
                </th>
                <th
                    scope="col"
                    className="px-4 py-2 text-left text-sm font-medium text-gray-700 border border-gray-300 bg-gray-50"
                >
                    {t('table.lastUpdated')}
                </th>

            </tr>
        </thead>
    );

    const buildRow = (item: Promotion, index: number) => (
        <tr className={index % 2 === 0 ? 'bg-white' : 'bg-gray-50'}>
            <td className="px-4 py-0.5 text-sm text-gray-700 border border-gray-300 text-center">
                {index + 1}
            </td>
            <td className="px-4 py-0.5 text-sm text-gray-700 border border-gray-300">{item.promotionTitle}</td>
            <td className="px-4 py-0.5 text-sm text-gray-700 border border-gray-300">{item.promotionType}</td>
            <td className="px-4 py-0.5 text-sm text-gray-700 border border-gray-300">{item.sportsPercentage}<br />{item.sportsMaxPayout}</td>
            <td className="px-4 py-0.5 text-sm text-gray-700 border border-gray-300">{item.virtualSportsPercentage}<br />{item.virtualSportsMaxPayout}</td>
            <td className="px-4 py-0.5 text-sm text-gray-700 border border-gray-300">{item.gamesPercentage}<br />{item.gamesMaxPayout}</td>
            <td className="px-4 py-0.5 text-sm text-gray-700 border border-gray-300">{item.liveCasinoPercentage}<br />{item.liveCasinoMaxPayout}</td>
            <td className="px-4 py-0.5 text-sm text-gray-700 border border-gray-300">{item.provider}</td>
            <td className="px-4 py-0.5 text-sm border border-gray-300 text-center">
                <span
                    className={`px-2 py-1 rounded-full text-xs font-medium ${getStatusStyle(item.status)}`}
                >
                    {item.status.toUpperCase()}
                </span>
            </td>
            <td className="px-4 py-0.5 text-sm text-gray-700 border border-gray-300">{item.lastUpdated}</td>
        </tr>
    );

    const buildEmptyState = () => (
        <tr>
            <td colSpan={12} className="border border-gray-300">
                <div className="flex flex-col items-center justify-center py-16">
                    <NoRecordsIcon className="mb-4 text-gray-400" />
                    <p className="text-gray-500 text-sm">{t('table.noRecords')}</p>
                </div>
            </td>
        </tr>
    );

    const buildBody = () => (
        <tbody>{items && items.length > 0 ? items.map(buildRow) : buildEmptyState()}</tbody>
    );

    if (isLoading) {
        return (
            <div className="flex justify-center items-center h-64">
                <div className="loading-spinner"></div>
            </div>
        );
    }

    return (
        <>
            <div className="overflow-x-auto">
                <table className="min-w-full border-collapse border border-gray-300">
                    {buildHeader()}
                    {buildBody()}
                </table>
            </div>
        </>
    );
};

export default Table;

import React from 'react';
import { useTranslation } from 'react-i18next';
import { Promotion } from '../../../services/interfaces/Promotion';
import { IconDelete, IconPen, NoRecordsIcon } from '../../../components/Icons';

interface TableProps {
    items: Promotion[];
    isLoading: boolean;
}

const Table: React.FC<TableProps> = ({
    items,
    isLoading
}) => {
    const { t } = useTranslation('promotion');
    const { t: tGlobal } = useTranslation('global');

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
                <th scope="col" className='w-[40px] sticky z-10 left-0 top-[0] bg-[#ffffff] '>
                    {t('table.no')} 
                </th>
                <th scope="col" className='w-[75px] sticky z-10 left-[40px] top-[0] bg-[#ffffff]'>
                    {t('table.action')}
                </th>
                <th className='sticky z-10 w-[110px]  left-[115px] top-[0] bg-[#ffffff] shadow-[3px_0_4px_0_rgba(99,84,84,0.3)]' scope="col">
                    <span className="relative z-3">{t('table.promotionTitle')}</span>
                </th>
                <th scope="col">
                    {t('table.promotionType')}
                </th>
                <th scope="col">
                    {t('table.sports')}
                </th>
                <th scope="col">
                    {t('table.virtualSports')}
                </th>
                <th scope="col">
                    {t('table.games')}
                </th>
                <th scope="col">
                    {t('table.liveCasino')}
                </th>
                <th scope="col">
                    {t('table.period')}
                </th>
                <th scope="col">
                    {t('table.provider')}
                </th>
                <th scope="col">
                    {t('table.status')}
                </th>
                <th scope="col">
                    {t('table.lastUpdated')}
                </th>

            </tr>
        </thead>
    );

    const buildRow = (item: Promotion, index: number) => (
        <tr className={index % 2 === 0 ? 'bg-white' : 'bg-gray-50'}>
            <td className='w-[40px] sticky left-0 bg-[#ffffff]'>
                {index + 1}
            </td>
            <td className='w-[75px] sticky left-[40px] bg-[#ffffff]'>
                <div className="flex gap-[4px]">
                    <button className="cursor-pointer p-[4px] rounded-[4px] bg-[#ffffff] hover:bg-[#EAEDF2] text-[var(--primary-color)]"
                        title={tGlobal('edit')}
                    >                            
                        <IconPen />
                    </button>
                    <button className="cursor-pointer p-[4px] rounded-[4px] bg-[#ffffff] hover:bg-[#EAEDF2] text-[var(--primary-color)]"
                        title={tGlobal('delete')}
                    >                            
                        <IconDelete />
                    </button>
                </div>
            </td>
            <td className="w-[110px] sticky left-[115px] bg-[#ffffff] shadow-[3px_0_4px_0_rgba(99,84,84,0.3)]">{item.promotionTitle}</td>
            <td className="">{item.promotionType}</td>
            <td className="">{item.sportsPercentage}<br />{item.sportsMaxPayout}</td>
            <td className="">{item.virtualSportsPercentage}<br />{item.virtualSportsMaxPayout}</td>
            <td className="">{item.gamesPercentage}<br />{item.gamesMaxPayout}</td>
            <td className="">{item.liveCasinoPercentage}<br />{item.liveCasinoMaxPayout}</td>
            <td className="">{item.period}</td>
            <td className="">{item.provider}</td>
            <td className="">
                <span
                    className={`rounded-[20px] px-[10px] py-[3px] ${getStatusStyle(item.status)}`}
                >
                    {item.status.toUpperCase()}
                </span>
            </td>
            <td className="">{item.lastUpdated}</td>
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
        <tbody className='font-[500]'>{items && items.length > 0 ? items.map(buildRow) : buildEmptyState()}</tbody>
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
            <div className="table-fixed-header table-fixed-body bg-[#ffffff] h-full">
                <table className="text-[12px] max-[600px]:text-[17px] min-w-[1300px] whitespace-nowrap border-collapse border border-[#0000001f] border-t-[0px]  border-l-[0px] text-[#333333] shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f]">
                    {buildHeader()}
                    {buildBody()}
                    
                </table>
            </div>
        </>
    );
};

export default Table;

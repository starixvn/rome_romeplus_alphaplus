import React, { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';

import LoadingComponent from '../../../../components/LoadingComponent';
import NoRecordFound from '../../../../components/NoRecordFoundComponent';
import { PromotionTransaction } from '../../../../services/interfaces/Promotion';
import { displayAmount, formatDate } from '../../../../utils/commonFunctions';
import { IconPen } from '../../../../components/Icons';
import EditPendingPromotion from './EditPendingPromotion';

interface DataGridProps {
  items: PromotionTransaction[];
  pageIndex: number;
  pageSize: number;
  isLoading: boolean;
  onSelectionChange?: (selectedIds: (string | number)[]) => void;
}

const DataGrid: React.FC<DataGridProps> = ({
  items,
  pageIndex,
  pageSize,
  isLoading,
  onSelectionChange,
}) => {
  const { t } = useTranslation('promotion');
  const [selectedItems, setSelectedItems] = useState<Set<string | number>>(new Set());
  const [selectAll, setSelectAll] = useState(false);
  const [editModal, setEditModal] = useState<{
    isOpen: boolean;
    transId: number;
    currentAmount: number;
  }>({
    isOpen: false,
    transId: 0,
    currentAmount: 0,
  });

  useEffect(() => {
    setSelectAll(false);
    setSelectedItems(new Set());
  }, [items]);

  useEffect(() => {
    if (onSelectionChange) {
      onSelectionChange(Array.from(selectedItems));
    }
  }, [selectedItems, onSelectionChange]);

  const handleSelectAll = (checked: boolean) => {
    setSelectAll(checked);
    if (checked) {
      const allTransIds = items.map((item) => item.transId);
      setSelectedItems(new Set(allTransIds));
    } else {
      setSelectedItems(new Set());
    }
  };

  const handleSelectItem = (transId: string | number, checked: boolean) => {
    const newSelectedItems = new Set(selectedItems);
    if (checked) {
      newSelectedItems.add(transId);
    } else {
      newSelectedItems.delete(transId);
    }
    setSelectedItems(newSelectedItems);

    if (checked && newSelectedItems.size === items.length) {
      setSelectAll(true);
    } else if (!checked) {
      setSelectAll(false);
    }
  };

  const onEditSinglePromotion = (transId: number, amount: number) => {
    setEditModal({
      isOpen: true,
      transId,
      currentAmount: amount,
    });
  };

  const handleCloseEditModal = () => {
    setEditModal({
      isOpen: false,
      transId: 0,
      currentAmount: 0,
    });
  };

  const handleApprovePromotion = async (transId: number, amount: number) => {
    console.log('Approving promotion:', transId, 'with amount:', amount);
    // TODO: Implement API call to approve promotion
  };

  const handleRejectPromotion = async (transId: number) => {
    console.log('Rejecting promotion:', transId);
    // TODO: Implement API call to reject promotion
  };

  if (isLoading) {
    return (
      <div className="flex justify-center items-center h-64">
        <LoadingComponent />
      </div>
    );
  }

  return (
    <>
      <div className="table-fixed-header table-fixed-body min-h-0">
        <table className="w-full whitespace-nowrap border-collapse border border-[#0000001f] border-t-[0px]  border-l-[0px]">
          <thead className="z-11!">
            <tr>
              <th
                scope="col"
                className="w-[32px] min-w-[32px] bg-[var(--table-header-background)] sticky left-0 z-11"
              >
                {t('table.no')}
              </th>
              <th scope="col" className="w-[50px] min-w-[50px] left-[32px]">
                <input
                  type="checkbox"
                  className="w-[16px] h-[16px] cursor-pointer"
                  name="selectAll"
                  disabled={items.length === 0}
                  checked={selectAll}
                  onChange={(e) => handleSelectAll(e.target.checked)}
                />
              </th>
              <th scope="col" className="w-[150px] min-w-[150px] left-[32px]">
                {t('table.username')}
                <br />
                {t('table.nickname')}
              </th>
              <th scope="col">
                {t('table.firstName')}
                <br />
                {t('table.lastName')}
              </th>
              <th scope="col" className="w-[200px]">
                {t('table.title')}
              </th>
              <th scope="col">{t('table.promotionType')}</th>
              <th scope="col" className="min-w-[80px]">
                {t('table.amount')}
              </th>
              <th scope="col">{t('table.dateRange')}</th>
              <th scope="col" className="w-[60px]">
                {t('table.actions')}
              </th>
            </tr>
          </thead>
          <tbody className="font-[500]">
            {items && items.length > 0 ? (
              items.map((item, index) => (
                <tr key={item.transId}>
                  <td className="w-[32px] min-w-[32px]">
                    {index + 1 + (pageIndex - 1) * pageSize}
                  </td>
                  <td className="w-[50px] min-w-[50px]">
                    <input
                      type="checkbox"
                      className="w-[16px] h-[16px] cursor-pointer"
                      checked={selectedItems.has(item.transId)}
                      onChange={(e) => handleSelectItem(item.transId, e.target.checked)}
                    />
                  </td>
                  <td className="w-[150px] min-w-[150px]">
                    <div>{item.username}</div>
                    <div>{item.nickname}</div>
                  </td>
                  <td>
                    <div>{item.firstName}</div>
                    <div>{item.lastName}</div>
                  </td>
                  <td className="text-center">{item.title}</td>
                  <td className="text-center">{item.promotionTypeName}</td>
                  <td className="text-right">
                    <div className="flex w-full justify-end relative">
                      {displayAmount(item.amount)}
                    </div>
                  </td>
                  <td className="text-center">
                    <div>
                      {formatDate(item.fromDate)} - {formatDate(item.toDate)}
                    </div>
                  </td>
                  <td className="text-center">
                    <div
                      className="flex justify-center items-center cursor-pointer"
                      onClick={() => onEditSinglePromotion(item.transId, item.amount)}
                    >
                      <IconPen className="w-[20px] h-[20px] fill-[#333333] dark:fill-[#ededed]" />
                    </div>
                  </td>
                </tr>
              ))
            ) : (
              <NoRecordFound colSpan={9} />
            )}
          </tbody>
        </table>
      </div>

      <EditPendingPromotion
        isOpen={editModal.isOpen}
        onClose={handleCloseEditModal}
        transId={editModal.transId}
        currentAmount={editModal.currentAmount}
        onApprove={handleApprovePromotion}
        onReject={handleRejectPromotion}
      />
    </>
  );
};

export default DataGrid;

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
  onSelectionChange?: (selectedItems: PromotionTransaction[]) => void;
  refreshGrid?: () => void;
  isCashServiceEnabled: boolean;
}

const DataGrid: React.FC<DataGridProps> = ({
  items,
  pageIndex,
  pageSize,
  isLoading,
  onSelectionChange,
  refreshGrid,
  isCashServiceEnabled,
}) => {
  const { t } = useTranslation('promotion');
  const [selectedItems, setSelectedItems] = useState<PromotionTransaction[]>([]);
  const [selectAll, setSelectAll] = useState(false);
  const [editModal, setEditModal] = useState<{
    isOpen: boolean;
    item: PromotionTransaction | null;
  }>({
    isOpen: false,
    item: null,
  });

  useEffect(() => {
    setSelectAll(false);
    setSelectedItems([]);
  }, [items]);

  useEffect(() => {
    if (onSelectionChange) {
      onSelectionChange(selectedItems);
    }
  }, [selectedItems, onSelectionChange]);

  const handleSelectAll = (checked: boolean) => {
    setSelectAll(checked);
    if (checked) {
      setSelectedItems(items);
    } else {
      setSelectedItems([]);
    }
  };

  const handleSelectItem = (item: PromotionTransaction, checked: boolean) => {
    const newSelectedItems = [...selectedItems];

    if (checked) {
      newSelectedItems.push(item);
    } else {
      newSelectedItems.splice(
        newSelectedItems.findIndex((i) => i.transId === item.transId && i.custId === item.custId),
        1
      );
    }
    setSelectedItems(newSelectedItems);
    if (checked && newSelectedItems.length === items.length) {
      setSelectAll(true);
    } else if (!checked) {
      setSelectAll(false);
    }
  };

  const onEditSinglePromotion = (item: PromotionTransaction) => {
    setEditModal({
      isOpen: true,
      item: item,
    });
  };

  const handleCloseEditModal = () => {
    setEditModal({
      isOpen: false,
      item: null,
    });
    refreshGrid?.();
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
      <div className="table">
        <table className="w-full border-collapse border border-[#0000001f] border-t-[0px]  border-l-[0px]">
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
                  disabled={items.length === 0 || !isCashServiceEnabled}
                  checked={selectAll}
                  onChange={(e) => handleSelectAll(e.target.checked)}
                />
              </th>
              <th scope="col" className="w-[150px] min-w-[150px] left-[32px]">
                {t('table.username')}
                <br />
                {t('table.nickname')}
              </th>
              <th scope="col">{t('table.firstLastName')}</th>
              <th scope="col" className="w-[200px]">
                {t('table.title')}
              </th>
              <th scope="col">{t('table.promotionType')}</th>
              <th scope="col" className="min-w-[80px]">
                {t('table.amount')}
              </th>
              <th scope="col">{t('table.rebatePeriod')}</th>
              <th scope="col">{t('table.refNo')}</th>
              {isCashServiceEnabled && (
                <th scope="col" className="w-[60px]">
                  {t('table.actions')}
                </th>
              )}
            </tr>
          </thead>
          <tbody className="font-[500]">
            {items && items.length > 0 ? (
              items.map((item, index) => (
                <tr key={index}>
                  <td className="w-[32px] min-w-[32px]">
                    {index + 1 + (pageIndex - 1) * pageSize}
                  </td>
                  <td className="w-[50px] min-w-[50px]">
                    <input
                      type="checkbox"
                      className="w-[16px] h-[16px] cursor-pointer"
                      checked={
                        selectedItems.findIndex(
                          (selectedItem) =>
                            selectedItem.transId === item.transId &&
                            selectedItem.custId === item.custId
                        ) !== -1
                      }
                      onChange={(e) => handleSelectItem(item, e.target.checked)}
                      disabled={!isCashServiceEnabled}
                    />
                  </td>
                  <td className="w-[150px] min-w-[150px] text-left">
                    <div>{item.username}</div>
                    <div>{item.nickname}</div>
                  </td>
                  <td className="text-left">
                    <div>{item.firstName}</div>
                    <div>{item.lastName}</div>
                  </td>
                  <td className="text-left">
                    <div className="w-[100%] max-w-[400px] min-w-[300px] max-[768px]:max-w-[200px] max-[768px]:min-w-[150px]">
                      {item.title}
                    </div>
                  </td>
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
                  <td className="text-center">{item.transId}</td>
                  {isCashServiceEnabled && (
                    <td className="text-center">
                      <div
                        className="flex justify-center items-center cursor-pointer"
                        onClick={() => onEditSinglePromotion(item)}
                      >
                        <IconPen className="w-[20px] h-[20px] fill-[#333333] dark:fill-[#ededed]" />
                      </div>
                    </td>
                  )}
                </tr>
              ))
            ) : (
              <NoRecordFound colSpan={isCashServiceEnabled ? 9 : 8} />
            )}
          </tbody>
        </table>
      </div>

      <EditPendingPromotion
        isOpen={editModal.isOpen}
        onClose={handleCloseEditModal}
        item={editModal.item}
      />
    </>
  );
};

export default DataGrid;

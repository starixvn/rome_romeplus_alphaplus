import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import i18n from '../../../../i18n/i18n-promotion';
import { useCookieLanguage } from '../../../../hooks/useCookieLanguage';
import {
  Product,
  ProductSetting,
  PromotionDetail,
} from '../../../../services/interfaces/Promotion';
import { IconDelete, IconPen } from '../../../../components/Icons';
import NoRecordFoundComponent from '../../../../components/NoRecordFoundComponent';
import { formatDate } from '../../../../utils/commonFunctions';
import { ButtonDeleteComponent, ButtonPenComponent } from '../../../../components/Buttons/ButtonIconComponent';

interface TableProps {
  items: PromotionDetail[];
  isLoading: boolean;
  onEdit?: (item: PromotionDetail) => void;
  onDelete?: (item: PromotionDetail) => void;
}

const Table: React.FC<TableProps> = ({ items, isLoading, onEdit, onDelete }) => {
  const [gridProducts, setGridProducts] = useState<ProductSetting[]>([]);
  const { t } = useTranslation('promotion');
  useCookieLanguage(i18n);

  useEffect(() => {
    setGridProducts(displayGridProducts(items));
  }, [items]);

  const getStatusStyle = (status: string) => {
    var s = status ? status.toLocaleUpperCase() : '';
    switch (s) {
      case 'ACTIVE':
        return 'bg-[#D4E7D9]';
      case 'SCHEDULED':
        return 'bg-[#FBD8D8]';
      case 'EXPIRED':
        return 'bg-[#FBD8D8]';
      case 'INACTIVE':
        return 'bg-[#FBD8D8]';
      default:
        return '';
    }
  };

  const displayProducts = (products: Product[]) => {
    const maxDisplay = 5;
    if (!products || products.length === 0) return '';
    const names = products.slice(0, maxDisplay).map((p) => p.productName);
    const remain = products.length - maxDisplay;
    return remain > 0 ? `${names.join(', ')} + ${remain} more` : names.join(', ');
  };

  const displayGridProducts = (promotions: PromotionDetail[]): ProductSetting[] => {
    const productMap = new Map<number, ProductSetting>();
    promotions.forEach((promotion) => {
      promotion.productSettings.forEach((product) => {
        if (!productMap.has(product.productId)) {
          productMap.set(product.productId, product);
        }
      });
    });
    return Array.from(productMap.values());
  };
  const buildHeader = () => (
    <thead>
      <tr>
        <th rowSpan={2} scope="col" className="w-[32px] sticky left-0 z-10 bg-[var(--table-header-background)] text-center">
          {t('table.no')}
        </th>
        <th rowSpan={2} className="w-[75px] sticky left-[32px] z-10 bg-[var(--table-header-background)] text-center">
          {t('table.action')}
        </th>
        <th
          rowSpan={2}
          className="w-[110px] sticky left-[117px] z-10 bg-[var(--table-header-background)] text-center"
          scope="col"
        >
          <span className="relative z-3">{t('table.promotionTitle')}</span>
        </th>
        <th rowSpan={2} scope="col" className="sticky left-[225px] z-10 text-center bg-[var(--table-header-background)] shadow-[1px_0_0_#a6a6a6]">
          {t('table.promotionType')}
        </th>
        <th colSpan={gridProducts.length} className="text-center" scope="col">
          {t('table.percentage')}
          <br />
          {t('table.maxPayout')}
        </th>
        <th
          rowSpan={2}
          scope="col"
          className="w-[110px] sticky right-[253px] z-10 bg-[var(--table-header-background)] text-center"
        >
          {t('table.products')}
        </th>
        <th
          rowSpan={2}
          scope="col"
          className="w-[110px] sticky right-[87px] z-10 bg-[var(--table-header-background)] text-center"
        >
          {t('table.period')}
        </th>
        <th rowSpan={2} scope="col" className="w-[110px] sticky right-[1px] z-10 bg-[var(--table-header-background)] text-center">
          {t('table.status')}
        </th>
      </tr>
      <tr>
        {gridProducts.map((product) => (
          <th key={product.productId} scope="col" className="text-center">
            {product.productName}
          </th>
        ))}
      </tr>
    </thead>
  );
  const buildRow = (item: PromotionDetail, index: number) => (
    <tr key={item.promotionId}>
      <td className="sticky left-0 bg-white z-10 text-center">{index + 1}</td>
      <td className="sticky left-[32px] bg-white z-10 text-center">
        <div className="flex justify-center">
          <ButtonPenComponent onClick={() => onEdit?.(item)} />
          <ButtonDeleteComponent onClick={() => onDelete?.(item)} />
        </div>
      </td>
      <td className="sticky left-[117px] bg-white z-10 text-left">{item.promotionTitle}</td>
      <td className="sticky left-[225px] bg-white z-10 text-center shadow-[1px_0_0_#a6a6a6]">{item.promotionType}</td>
      {gridProducts.map((product) =>
        displayPercentageMaxPayout(item.productSettings, product.productId)
      )}
      <td className="w-[110px] sticky right-[253px] bg-white z-10">
        {displayProducts(item.productSettings)}
      </td>
      <td className="w-[110px] sticky right-[87px] bg-white z-10">{`${formatDate(item.fromDate?.toString() || '')} - ${formatDate(item.toDate?.toString() || '')}`}</td>
      <td className="w-[110px] sticky right-[1px] bg-white z-10">
        <span
          className={`inline-block px-2 py-1 rounded text-xs font-semibold ${getStatusStyle(item.enabled ? 'ACTIVE' : 'INACTIVE')}`}
        >
          {item.enabled ? 'ACTIVE' : 'INACTIVE'}
        </span>
      </td>
    </tr>
  );

  const displayPercentageMaxPayout = (productSettings: ProductSetting[], productId: number) => {
    const product = productSettings.find((p) => p.productId === productId);
    if (!product)
      return (
        <td key={productId} className="text-right">
          -
          <br />-
        </td>
      );

    let displayMaxPayout: number | string = product.maxPayout;
    if (product.unlimited) displayMaxPayout = 'Unlimited';
    return (
      <td key={product.productId} className="text-right">
        {product.percentage + '%'}
        <br />
        {displayMaxPayout || ''}
      </td>
    );
  };

  const buildBody = () => (
    <tbody className="font-[500]">
      {items && items.length > 0 ? (
        items.map(buildRow)
      ) : (
        <NoRecordFoundComponent colSpan={12} message="No pending promotions found" />
      )}
    </tbody>
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
      <div className="table-fixed-header table-fixed-body bg-[#ffffff] max-h-[calc(100vh-140px)] overflow-y-auto">
        <table className="min-w-[1300px] w-full whitespace-nowrap border-collapse border border-[#0000001f] border-t-[0px]  border-l-[0px] text-[#333333] shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f]">
          {buildHeader()}
          {buildBody()}
        </table>
      </div>
    </>
  );
};

export default Table;

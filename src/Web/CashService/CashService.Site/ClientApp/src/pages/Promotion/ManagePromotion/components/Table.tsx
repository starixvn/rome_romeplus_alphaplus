import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import {
  ButtonDeleteComponent,
  ButtonPenComponent,
} from '../../../../components/Buttons/ButtonIconComponent';
import NoRecordFoundComponent from '../../../../components/NoRecordFoundComponent';
import { useCookieLanguage } from '../../../../hooks/useCookieLanguage';
import i18n from '../../../../i18n/i18n-promotion';
import {
  Product,
  ProductSetting,
  PromotionDetail,
} from '../../../../services/interfaces/Promotion';
import { formatDate } from '../../../../utils/commonFunctions';

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
      case 'ENABLE':
        return 'bg-[#D4E7D9]';
      case 'DISABLE':
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
    return remain > 0 ? `${names.join('/ ')} + ${remain} more` : names.join('/ ');
  };

  const displayGridProducts = (promotions: PromotionDetail[]): ProductSetting[] => {
    const productMap = new Map<number, ProductSetting>();
    const sortOrder = [1, 44, 41, 53];
    
    promotions.forEach((promotion) => {
      // Sort productSettings by the specified order
      const sortedProductSettings = promotion.productSettings.sort((a, b) => {
        const aIndex = sortOrder.indexOf(a.productCategoryId);
        const bIndex = sortOrder.indexOf(b.productCategoryId);
        
        // If both are in the sort order, sort by their position
        if (aIndex !== -1 && bIndex !== -1) {
          return aIndex - bIndex;
        }
        // If only a is in the sort order, a comes first
        if (aIndex !== -1) {
          return -1;
        }
        // If only b is in the sort order, b comes first
        if (bIndex !== -1) {
          return 1;
        }
        // If neither is in the sort order, maintain original order
        return 0;
      });
      
      sortedProductSettings.forEach((product) => {
        if (!productMap.has(product.productCategoryId)) {
          productMap.set(product.productCategoryId, product);
        }
      });
    });
    return Array.from(productMap.values());
  };
  const buildHeader = () => (
    <thead>
      <tr>
        <th
          rowSpan={2}
          scope="col"
          className="w-[32px] bg-[var(--table-header-background)] text-center"
        >
          {t('table.no')}
        </th>
        <th rowSpan={2} className="w-[75px] bg-[var(--table-header-background)] text-center">
          {t('table.action')}
        </th>
        <th
          rowSpan={2}
          className="w-[110px] bg-[var(--table-header-background)] text-center"
          scope="col"
        >
          <span className="relative z-3">{t('table.promotionTitle')}</span>
        </th>
        <th rowSpan={2} scope="col" className="text-center bg-[var(--table-header-background)]">
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
          className="w-[110px] bg-[var(--table-header-background)] text-center"
        >
          {t('table.products')}
        </th>
        <th
          rowSpan={2}
          scope="col"
          className="w-[110px] bg-[var(--table-header-background)] text-center"
        >
          {t('table.period')}
        </th>
        <th
          rowSpan={2}
          scope="col"
          className="w-[110px] bg-[var(--table-header-background)] text-center"
        >
          {t('table.status')}
        </th>
      </tr>
      <tr>
        {gridProducts.map((product) => (
          <th key={product.productCategoryId} scope="col" className="text-center">
            {product.productCategoryName}
          </th>
        ))}
      </tr>
    </thead>
  );
  const buildRow = (item: PromotionDetail, index: number) => (
    <tr key={item.promotionId}>
      <td className="bg-white text-center">{index + 1}</td>
      <td className="bg-white text-center">
        <div className="flex justify-center">
          <ButtonPenComponent onClick={() => onEdit?.(item)} />
          <ButtonDeleteComponent onClick={() => onDelete?.(item)} />
        </div>
      </td>
      <td className="bg-white text-left">{item.promotionTitle}</td>
      <td className="bg-white text-center">{item.promotionTypeName}</td>
      {gridProducts.map((product) =>
        displayPercentageMaxPayout(item.productSettings, product.productCategoryId)
      )}
      <td className="w-[110px] bg-white">
        {displayProducts(item.productSettings.flatMap((p) => p.products))}
      </td>
      <td className="w-[110px] bg-white">{`${formatDate(item.fromDate?.toString() || '')} - ${formatDate(item.toDate?.toString() || '')}`}</td>
      <td className="w-[110px] bg-white">
        <span
          className={`inline-block px-2 py-1 rounded text-xs font-semibold ${getStatusStyle(item.enabled ? 'ENABLE' : 'DISABLE')}`}
        >
          {item.enabled ? 'ENABLE' : 'DISABLE'}
        </span>
      </td>
    </tr>
  );

  const displayPercentageMaxPayout = (productSettings: ProductSetting[], productId: number) => {
    const product = productSettings.find((p) => p.productCategoryId === productId);
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
      <td key={product.productCategoryId} className="text-right">
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

import React, { useState, useEffect } from 'react';
import {
  ProductDropdown,
  ProductSetting,
  PromotionRequest,
} from '../../../../services/interfaces/Promotion';
import {
  BackButtonComponent,
  ButtonApplyComponent,
  ButtonCancelComponent,
} from '../../../../components/Buttons/index';
import { ErrorMessageComponent, SuccessMessageComponent } from '../../../../components/Message';
import { TitleComponent } from '../../../../components/Title';
import { TreeNodeOption } from '../../../../components/TreeSelect/TreeSelect';
import { promotionService } from '../../../../services/promotionAPI';
import PromotionInfo, { PromotionLangFields } from './Form/LanguageInfo';
import ProductSettings from './Form/ProductSettings';
import Information from './Form/Information';

interface AddPromotionProps {
  onClose: () => void;
}

const PROMOTION_TYPES = [{ value: 1, label: 'Rebate on Turnover' }];

export const LANGUAGES = [
  { key: 'en', label: 'English (default)', isDefault: true },
  { key: 'th', label: 'ภาษาไทย', isDefault: false },
  { key: 'vi', label: 'Tiếng Việt', isDefault: false },
];

function convertProductToTreeNodeOption(products: ProductDropdown[]): TreeNodeOption[] {
  return products.map((product) => ({
    label: product.productCategoryName,
    value: product.productCategoryId,
    children:
      product.products && product.products.length > 0
        ? product.products.map((p) => ({
            label: p.productName,
            value: p.productId,
          }))
        : [],
  }));
}

const AddPromotion: React.FC<AddPromotionProps> = ({ onClose }) => {
  const [error, setError] = useState<string | null>(null);
  const [successMessage, setSuccessMessage] = useState<string>('');
  const [loading, setLoading] = useState<boolean>(false);
  const [isActive, setIsActive] = useState<boolean>(true);
  const [promotionType, setPromotionType] = useState<number>(PROMOTION_TYPES[0].value);

  const today = new Date();
  const nextWeek = new Date();
  nextWeek.setDate(today.getDate() + 7);
  const [period, setPeriod] = useState<[Date | null, Date | null]>([today, nextWeek]);

  const [productOptions, setProductOptions] = useState<ProductDropdown[]>([]);
  const [productSettings, setProductSettings] = useState<ProductSetting[]>([]);
  const [selectedProductIds, setSelectedProductIds] = useState<number[]>([]);
  const [selectedProductCategories, setSelectedProductCategories] = useState<number[]>([]);
  const [activeLang, setActiveLang] = useState(LANGUAGES[0].key);
  const [promotionInfo, setPromotionInfo] = useState<Record<string, PromotionLangFields>>({});

  useEffect(() => {
    promotionService.getPromotionProducts().then((apiProducts) => {
      if (apiProducts && apiProducts.length > 0) {
        setProductOptions(apiProducts);
        setProductSettings(
          apiProducts.map((p) => ({
            productCategoryId: p.productCategoryId,
            productCategoryName: p.productCategoryName,
            products: p.products,
            percentage: 0,
            minTurnover: 0,
            maxPayout: 0,
            unlimited: false,
          }))
        );
      }
    });
  }, []);

  const handleProductChange = (values: (string | number)[], highestParents: TreeNodeOption[]) => {
    setSelectedProductIds(values.map((v) => v as number));
    setSelectedProductCategories(highestParents.map((h) => h.value as number));
  };

  const handleProductFieldChange = (productId: number, field: string, value: number | boolean) => {
    setProductSettings((prev) =>
      prev.map((p) => (p.productCategoryId === productId ? { ...p, [field]: value } : p))
    );
  };

  const handlePromotionInfoChange = (
    lang: string,
    field: keyof PromotionLangFields,
    value: any
  ) => {
    setPromotionInfo((prev) => ({ ...prev, [lang]: { ...prev[lang], [field]: value } }));
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setLoading(true);

    setSuccessMessage('Promotion saved successfully');
    const [fromDate, toDate] = period.map((date) => (date ? new Date(date) : null));
    const promotionData: PromotionRequest = {
      promotionType,
      fromDate,
      toDate,
      products: productSettings,
      enabled: isActive,
      promotionInfo: Object.entries(promotionInfo).map(([lang, info]) => ({
        lang,
        title: info.title,
        banner: info.banner as string | null,
        content: info.content,
      })),
    };
    console.log('promotionData', promotionData);
  };

  return (
    <>
      <div className="flex items-center justify-flex-start mb-[8px] py-[8px] border-[var(--border-control)] border-b-[1px] [&>:nth-child(2)]:border-0 [&>:nth-child(2)]:mb-[0] [&>:nth-child(1)]:mb-[0]">
        <BackButtonComponent onClick={onClose} />
        <TitleComponent title={`Add Promotion`} />
      </div>

      <div className="pb-[16px]">
        <div className="modal-body">
          <ErrorMessageComponent className="mb-[16px] max-[768px]:mb-[0]" message={error} />
          <SuccessMessageComponent
            className="mb-[16px] max-[768px]:mb-[0]"
            message={successMessage}
          />

          <Information
            isActive={isActive}
            setIsActive={setIsActive}
            promotionType={promotionType}
            setPromotionType={setPromotionType}
            period={period}
            setPeriod={setPeriod}
            productOptions={convertProductToTreeNodeOption(productOptions)}
            selectedProductIds={selectedProductIds}
            handleProductChange={handleProductChange}
            PROMOTION_TYPES={PROMOTION_TYPES}
          />
        </div>
        <ProductSettings
          productCategories={productSettings}
          handleProductFieldChange={handleProductFieldChange}
          selectedProductCategories={selectedProductCategories}
        />

        <PromotionInfo
          languages={LANGUAGES}
          value={promotionInfo}
          setValue={handlePromotionInfoChange}
          error={error || ''}
          success={successMessage || ''}
          activeLang={activeLang}
          setActiveLang={setActiveLang}
        />

        <div className="flex justify-start py-[8px] mt-[8px] gap-[8px]">
          <ButtonCancelComponent text="Cancel" onClick={onClose} disabled={loading} />
          <ButtonApplyComponent text="Create" onClick={handleSubmit} disabled={loading} />
        </div>
      </div>
    </>
  );
};

export default AddPromotion;

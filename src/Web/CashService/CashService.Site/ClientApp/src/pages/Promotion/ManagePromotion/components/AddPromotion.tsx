import React, { useEffect, useState } from 'react';
import {
  BackButtonComponent,
  ButtonComponent,
} from '../../../../components/Buttons/index';
import { ErrorMessageComponent, SuccessMessageComponent } from '../../../../components/Message';
import { TitleComponent } from '../../../../components/Title';
import { TreeNodeOption } from '../../../../components/TreeSelect/TreeSelect';
import { PROMOTION_TYPES } from '../../../../constants/promotionConstants';
import {
  Language,
  ProductDropdown,
  ProductSetting,
  PromotionRequest,
} from '../../../../services/interfaces/Promotion';
import { promotionService } from '../../../../services/promotionAPI';
import Information from './Form/Information';
import PromotionInfo, { PromotionLangFields } from './Form/LanguageInfo';
import ProductSettings from './Form/ProductSettings';
import { useTranslation } from 'react-i18next';

interface AddPromotionProps {
  onClose: () => void;
}

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
  const { t: tGlobal } = useTranslation('global');
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
  const [activeLang, setActiveLang] = useState("en");
  const [promotionInfo, setPromotionInfo] = useState<Record<string, PromotionLangFields>>({});
  const [languages, setLanguages] = useState<Language[]>([]);

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
            productIds: [],
          }))
        );
      }
    });
    promotionService.getLanguages().then((apiLanguages: Record<string, string>) => {
      if (apiLanguages) {
        const transformedLanguages: Language[] = Object.entries(apiLanguages).map(([key, value]) => ({
          key,
          label: value,
          isDefault: key === 'en'
        }));
        setLanguages(transformedLanguages);
      }
    });
  }, []);

  const handleProductChange = (values: (string | number)[], highestParents: TreeNodeOption[]) => {
    
    const selectedIds = values.map((v) => v as number);
    setSelectedProductIds(selectedIds);
    setSelectedProductCategories(highestParents.map((h) => h.value as number));
    
    // Update productIds in productSettings for selected categories
    setProductSettings((prev) =>
      prev.map((setting) => {
        const categoryProducts = setting.products || [];
        const selectedProductsInCategory = selectedIds.filter((id) =>
          categoryProducts.some((product) => product.productId === id)
        );
        
        return {
          ...setting,
          productIds: selectedProductsInCategory,
        };
      })
    );
  };

  const handleProductFieldChange = (productId: number, field: string, value: number | boolean) => {
    
    setProductSettings((prev) =>
      prev.map((p) => {
        if (p.productCategoryId === productId) {
          if (field === 'unlimited' && value === true) {
            return { ...p, [field]: value, maxPayout: 0 };
          }
          return { ...p, [field]: value };
        }
        return p;
      })
    );
  };

  const handlePromotionInfoChange = (
    lang: string,
    field: keyof PromotionLangFields,
    value: any
  ) => {
   
    setPromotionInfo((prev) => ({ ...prev, [lang]: { ...prev[lang], [field]: value } }));
  };
const validateForm = () => {
  const errors = [
    {
      condition: selectedProductCategories.length === 0,
      message: 'Please select at least one Product'
    },
    { 
      condition: !promotionInfo.en, 
      message: 'Title is required'
    },
    {
      condition: promotionInfo.en && !promotionInfo.en.title,
      message: 'Title is required'
    },
    /*{
      condition: promotionInfo.en && !promotionInfo.en.banner,
      message: 'Banner is required'
    },*/
    {
      condition: promotionInfo.en && !promotionInfo.en.content,
      message: 'Content is required'
    }    
  ];
  const failedValidation = errors.find(check => check.condition);
  if (failedValidation) {
    setError(failedValidation.message);
    return false;
  }
  return true;
}
  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (!validateForm())
      {
        window.scrollTo({ top: 0, behavior: 'smooth' });
        return;
      }
    setError(null);
    setLoading(true);
    
    const [fromDate, toDate] = period.map((date) => (date ? new Date(date) : null));
    const promotionData: PromotionRequest = {
      promotionType,
      fromDate,
      toDate,
      productSettings: productSettings,
      enabled: isActive,
      contents: Object.entries(promotionInfo).map(([lang, info]) => ({
        language: lang,
        title: info.title,
        banner: info.banner as string | null,
        content: info.content,
      })),
    };
    
    promotionService.addPromotion(promotionData).then((result) => {
      window.scrollTo({ top: 0, behavior: 'smooth' });
      if (result.isSuccessful) {
        setSuccessMessage('Promotion saved successfully');        
        setTimeout(() => {
          onClose();
        }, 3000);
      } else {
        setError(result.message);
      }
    }).finally(() => {
      setLoading(false);
    });
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
            readOnlyMode={false}
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
          readOnlyMode={false}
        />

        <PromotionInfo
          languages={languages}
          value={promotionInfo}
          setValue={handlePromotionInfoChange}
          error={error || ''}
          success={successMessage || ''}
          activeLang={activeLang}
          setActiveLang={setActiveLang}
        />

        <div className="flex justify-start py-[8px] mt-[8px] gap-[8px]">
          <ButtonComponent variant="cancel" text={tGlobal('cancel')} onClick={onClose} disabled={loading} />
          <ButtonComponent variant="apply" text={tGlobal('save')} onClick={handleSubmit} disabled={loading} />
        </div>
      </div>
    </>
  );
};

export default AddPromotion;

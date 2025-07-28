import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { ButtonIconComponent } from '../../../../components/Buttons/ButtonIconComponent';
import {
  ButtonComponent,
} from '../../../../components/Buttons/index';
import { MessageComponent } from '../../../../components/Message';
import { TitleComponent } from '../../../../components/Title';
import { TreeNodeOption } from '../../../../components/TreeSelect/TreeSelect';
import { PROMOTION_TYPES } from '../../../../constants/promotionConstants';
import {
  Language,
  ProductDropdown,
  ProductSetting
} from '../../../../services/interfaces/Promotion';
import { promotionService } from '../../../../services/promotionAPI';
import Information from './Form/Information';
import PromotionInfo, { PromotionLangFields } from './Form/LanguageInfo';
import ProductSettings from './Form/ProductSettings';

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
  const { t } = useTranslation('promotion');
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
    const selectedCategories = highestParents.map((h) => h.value as number);
    setSelectedProductCategories(selectedCategories);
    
    // Update productIds in productSettings for selected categories and set minimum values
    setProductSettings((prev) =>
      prev.map((setting) => {
        const categoryProducts = setting.products || [];
        const selectedProductsInCategory = selectedIds.filter((id) =>
          categoryProducts.some((product) => product.productId === id)
        );
        
        const isCategorySelected = selectedCategories.includes(setting.productCategoryId);
        
        return {
          ...setting,
          productIds: selectedProductsInCategory,
          // Set default values to 1 for selected categories if current value is 0
          // Reset values to 0 for unselected categories
          percentage: isCategorySelected 
            ? (setting.percentage === 0 ? 1 : setting.percentage)
            : 0,
          minTurnover: isCategorySelected 
            ? (setting.minTurnover === 0 ? 0 : setting.minTurnover)
            : 0,
          maxPayout: isCategorySelected 
            ? (setting.maxPayout === 0 ? 1 : setting.maxPayout)
            : 0,
          unlimited: isCategorySelected ? setting.unlimited : false,
        };
      })
    );
  };

  const handleProductFieldChange = (productId: number, field: string, value: number | boolean) => {
    setProductSettings((prev) =>
      prev.map((p) => {
        if (p.productCategoryId === productId) {
          if (field === 'unlimited') {
            const boolValue = value as boolean;
            if (boolValue === true) {
              // When checking unlimited, set maxPayout to 0
              return { ...p, unlimited: boolValue, maxPayout: 0 };
            } else {
              // When unchecking unlimited, set maxPayout to 1
              return { ...p, unlimited: boolValue, maxPayout: 1 };
            }
          } else {
            // For numeric fields, prevent setting to 0 or negative values
            let adjustedValue = value;
            if (typeof value === 'number' && value <= 0) {
              adjustedValue = 1;
            }
            return { ...p, [field]: adjustedValue };
          }
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
      message: t('addPromotionForm.requiredProductCategory')
    },
    
    { 
      condition: !promotionInfo.en, 
      message: t('addPromotionForm.requiredTitle')
    },
    {
      condition: promotionInfo.en && !promotionInfo.en.title,
      message: t('addPromotionForm.requiredTitle')
    },
    {
      condition: promotionInfo.en && !promotionInfo.en.banner,
      message: t('addPromotionForm.requiredBanner')
    },
    {
      condition: promotionInfo.en && !promotionInfo.en.content,
      message: t('addPromotionForm.requiredContent')
    }    
  ];
  const failedValidation = errors.find(check => check.condition);
  if (failedValidation) {
    setError(failedValidation.message);
    return false;
  }
  return true;
}

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!validateForm()) {
      window.scrollTo({ top: 0, behavior: 'smooth' });
      return;
    }
    setError(null);
    setLoading(true);
    
    try {
      const [fromDate, toDate] = period.map((date) => (date ? new Date(date) : null));      
      const formData = new FormData();
      formData.append('PromotionType', promotionType.toString());
      formData.append('StatusId', '1');
      formData.append('FromDate', fromDate?.toISOString() || '');
      formData.append('ToDate', toDate?.toISOString() || '');
      formData.append('Enabled', isActive.toString());
      if (productSettings && productSettings.length > 0) {
        formData.append('ProductSettingsJson', JSON.stringify(productSettings));
      }
      Object.entries(promotionInfo).forEach(([lang, info], index) => {
        formData.append(`Contents[${index}].Language`, lang);
        formData.append(`Contents[${index}].Title`, info.title || '');
        formData.append(`Contents[${index}].Content`, info.content || '');
        
        if (info.banner instanceof File) {
          formData.append(`Contents[${index}].Banner`, info.banner);
        } else if (info.banner) {
          formData.append(`Contents[${index}].BannerName`, info.banner as string);
        }
      });
      
      console.log('FormData entries:');
      for (let [key, value] of formData.entries()) {
        console.log(`${key}:`, value);
      }
      
      const result = await promotionService.addPromotionWithFormData(formData);
      
      window.scrollTo({ top: 0, behavior: 'smooth' });
      if (result.isSuccessful) {
        setSuccessMessage(t('addPromotionForm.promotionSavedSuccessfully'));        
        setTimeout(() => {
          onClose();
        }, 3000);
      } else {
        setError(result.message);
      }
    } catch (error) {
      console.error('Error adding promotion:', error);
      setError('An error occurred while saving the promotion');
    } finally {
      setLoading(false);
    }
  };

  return (
    <>
      <div className="flex items-center justify-flex-start mb-[8px] py-[8px] border-[var(--border-control)] border-b-[1px] [&>:nth-child(2)]:border-0 [&>:nth-child(2)]:mb-[0] [&>:nth-child(1)]:mb-[0]">
        <ButtonIconComponent style='Back' onClick={onClose} />
        <TitleComponent text={t('managePromotion.addPromotion')} type='main'/>
      </div>

      <div className="pb-[16px]">
        <div className="modal-body">
          <MessageComponent className="mb-[16px] max-[768px]:mb-[0]" message={error} type='error' />
          <MessageComponent className="mb-[16px] max-[768px]:mb-[0]" message={successMessage} type='success'/>

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

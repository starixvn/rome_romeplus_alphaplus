import React, { useEffect, useRef, useState } from 'react';
import {
  BackButtonComponent,
  ButtonApplyComponent,
  ButtonCancelComponent,
} from '../../../../components/Buttons/index';
import { ErrorMessageComponent, SuccessMessageComponent } from '../../../../components/Message';
import { TitleComponent } from '../../../../components/Title';
import { TreeNodeOption } from '../../../../components/TreeSelect/TreeSelect';
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

interface EditPromotionProps {
  onClose: () => void;
  promotionId: number;
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

const EditPromotion: React.FC<EditPromotionProps> = ({ onClose, promotionId }) => {
  const [error, setError] = useState<string | null>(null);
  const [successMessage, setSuccessMessage] = useState<string>('');
  const [loading, setLoading] = useState<boolean>(false);
  const [isActive, setIsActive] = useState<boolean>(true);
  const promotionTypeRef = useRef<number>(PROMOTION_TYPES[0].value);
  const periodRef = useRef<[Date | null, Date | null]>([null, null]);
  const [productOptions, setProductOptions] = useState<ProductDropdown[]>([]);
  const [productSettings, setProductSettings] = useState<ProductSetting[]>([]);
  const [selectedProductIds, setSelectedProductIds] = useState<number[]>([]);
  const [activeLang, setActiveLang] = useState(LANGUAGES[0].key);
  const [promotionInfo, setPromotionInfo] = useState<Record<string, PromotionLangFields>>({});
  const [languages, setLanguages] = useState<Language[]>([]);

  useEffect(() => {
    promotionService.getPromotionProducts().then((apiProducts) => {
      if (apiProducts && apiProducts.length > 0) {
        setProductOptions(apiProducts);
      }
    });

    promotionService.getLanguages().then((apiLanguages: Record<string, string>) => {
      if (apiLanguages) {
        const transformedLanguages: Language[] = Object.entries(apiLanguages).map(
          ([key, value]) => ({
            key,
            label: value,
            isDefault: key === 'en',
          })
        );
        setLanguages(transformedLanguages);
      }

      const loadPromotionData = async () => {
        try {
          setLoading(true);
          const promotionData = await promotionService.getPromotionById(promotionId);

          if (promotionData) {
            promotionTypeRef.current = promotionData.promotionTypeId;
            if (promotionData.fromDate && promotionData.toDate) {
              periodRef.current = [
                new Date(promotionData.fromDate),
                new Date(promotionData.toDate),
              ];
            }

            setIsActive(promotionData.enabled);

            if (promotionData.productSettings) {
              setProductSettings(promotionData.productSettings);
              setSelectedProductIds(
                promotionData.productSettings.map((p: ProductSetting) => p.productCategoryId)
              );
            }

            if (promotionData.promotionInfo) {
              const infoMap: Record<string, PromotionLangFields> = {};
              promotionData.promotionInfo.forEach((info) => {
                infoMap[info.language] = {
                  title: info.title || '',
                  banner: info.banner,
                  prevBanner: info.banner,
                  content: info.content || '',
                };
              });
              setPromotionInfo(infoMap);
            }
          }
        } catch (err: any) {
          setError('Failed to load promotion data');
          console.error('Error loading promotion:', err);
        } finally {
          setLoading(false);
        }
      };
      loadPromotionData();
    });
  }, [promotionId]);

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

    const [fromDate, toDate] = periodRef.current.map((date) => (date ? new Date(date) : null));
    const promotionData: PromotionRequest = {
      promotionType: promotionTypeRef.current,
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

    console.log(promotionData);

    setTimeout(() => {
      setSuccessMessage('Promotion updated successfully');
      setTimeout(() => {
        onClose();
      }, 1500);
    }, 1000);

    setLoading(false);
  };

  if (loading && !promotionInfo) {
    return (
      <div className="flex items-center justify-center h-64">
        <div>Loading promotion data...</div>
      </div>
    );
  }

  return (
    <>
      <div className="flex items-center justify-flex-start mb-[8px] py-[8px] border-[var(--border-control)] border-b-[1px] [&>:nth-child(2)]:border-0 [&>:nth-child(2)]:mb-[0] [&>:nth-child(1)]:mb-[0]">
        <BackButtonComponent onClick={onClose} />
        <TitleComponent title={`Edit Promotion`} />
      </div>

      <form onSubmit={handleSubmit} className="pb-[16px]">
        <div className="modal-body">
          <ErrorMessageComponent className="mb-[16px] max-[768px]:mb-[0]" message={error} />
          <SuccessMessageComponent
            className="mb-[16px] max-[768px]:mb-[0]"
            message={successMessage}
          />

          <Information
            isActive={isActive}
            setIsActive={setIsActive}
            promotionType={promotionTypeRef.current}
            setPromotionType={(newPromotionType) => {
              promotionTypeRef.current = newPromotionType;
            }}
            period={periodRef.current}
            setPeriod={(newPeriod) => {
              periodRef.current = newPeriod;
            }}
            productOptions={convertProductToTreeNodeOption(productOptions)}
            selectedProductIds={selectedProductIds}
            handleProductChange={() => {
              void 0;
            }}
            PROMOTION_TYPES={PROMOTION_TYPES}
          />
        </div>
        <ProductSettings
          productCategories={productSettings}
          selectedProductCategories={[]}
          handleProductFieldChange={() => {
            void 0;
          }}
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
          <ButtonCancelComponent text="Cancel" onClick={onClose} />
          <ButtonApplyComponent text="Update" type="submit" disabled={loading} />
        </div>
      </form>
    </>
  );
};

export default EditPromotion;

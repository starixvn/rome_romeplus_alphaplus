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
  EditPromotionRequest,
  Language,
  ProductDropdown,
  ProductSetting,
  PromotionDetail
} from '../../../../services/interfaces/Promotion';
import { promotionService } from '../../../../services/promotionAPI';
import Information from './Form/Information';
import PromotionInfo, { PromotionLangFields } from './Form/LanguageInfo';
import ProductSettings from './Form/ProductSettings';
import { useTranslation } from 'react-i18next';

interface EditPromotionProps {
  onClose: () => void;
  promotionId: number;
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

const EditPromotion: React.FC<EditPromotionProps> = ({ onClose, promotionId }) => {
  const { t: tGlobal } = useTranslation('global');
  const [error, setError] = useState<string | null>(null);
  const [successMessage, setSuccessMessage] = useState<string>('');
  const [loading, setLoading] = useState<boolean>(false);
  const [isActive, setIsActive] = useState<boolean>(true);
  const [originalStatus, setOriginalStatus] = useState<boolean>(true);
  const [promotion, setPromotion] = useState<PromotionDetail | null>(null);
  const [productOptions, setProductOptions] = useState<ProductDropdown[]>([]);
  const [activeLang, setActiveLang] = useState("en");
  const [promotionInfo, setPromotionInfo] = useState<Record<string, PromotionLangFields>>({});
  const [originalPromotionInfo, setOriginalPromotionInfo] = useState<Record<string, PromotionLangFields>>({});
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
          const promotion = await promotionService.getPromotionById(promotionId);

          setPromotion(promotion);
          setIsActive(promotion.enabled);
          setOriginalStatus(promotion.enabled);

          if (promotion.languageSettings) {

            const infoMap: Record<string, PromotionLangFields> = {};
            promotion.languageSettings.forEach((info) => {
              infoMap[info.language] = {
                title: info.title || '',
                banner: info.banner,
                prevBanner: info.banner,
                content: info.content || '',
              };
            });

            setPromotionInfo(infoMap);
            setOriginalPromotionInfo({ ...infoMap });

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

  const validateForm = () => {
    const errors = [
      {
        condition: !hasChanges(),
        message: 'There is no change to update'
      },
      {
        condition: !promotionInfo.en,
        message: 'Title is required'
      },
      {
        condition: promotionInfo.en && !promotionInfo.en.title,
        message: 'Title is required'
      },
      //{
      //condition: promotionInfo.en && !promotionInfo.en.banner,
      //message: 'Banner is required'
      //},
      {
        condition: promotionInfo.en && !promotionInfo.en.content,
        message: 'Content is required'
      }
    ];
    debugger;
    const failedValidation = errors.find(check => check.condition);
    if (failedValidation) {
      setError(failedValidation.message);
      return false;
    }
    return true;
  }

  const hasChanges = () => {
    const statusChanged = originalStatus !== isActive;

    const contentChanged = JSON.stringify(promotionInfo) !== JSON.stringify(originalPromotionInfo);

    return statusChanged || contentChanged;
  }
  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (!validateForm()) {
      window.scrollTo({ top: 0, behavior: 'smooth' });
      return;
    }
    setError(null);
    setLoading(true);
    const promotionData: EditPromotionRequest = {
      promotionId: promotionId,
      enabled: isActive,
      originalEnabled: originalStatus,
      contents: Object.entries(promotionInfo).map(([lang, info]) => ({
        language: lang,
        title: info.title,
        banner: "Razor harcode",//info.banner as string | null,
        content: info.content,
      })),
      originalContents: Object.entries(originalPromotionInfo).map(([lang, info]) => ({
        language: lang,
        title: info.title,
        banner: "Razor harcode",//info.banner as string | null,
        content: info.content,
      })),
    };

    promotionService.editPromotion(promotionId, promotionData).then((result) => {
      window.scrollTo({ top: 0, behavior: 'smooth' });
      if (result.isSuccessful) {
        setSuccessMessage('Promotion updated successfully');
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
            readOnlyMode={true}
            isActive={isActive}
            setIsActive={(value: boolean) => { setIsActive(value); }}
            promotionType={promotion?.promotionTypeId || 0}
            setPromotionType={() => { void (0); }}
            period={promotion?.fromDate && promotion?.toDate ? [new Date(promotion.fromDate), new Date(promotion.toDate)] : [null, null]}
            setPeriod={() => { void (0); }}
            productOptions={convertProductToTreeNodeOption(productOptions)}
            selectedProductIds={promotion?.productSettings?.map((p: ProductSetting) => p.productCategoryId) || []}
            handleProductChange={() => { void (0) }}
            PROMOTION_TYPES={PROMOTION_TYPES}
          />
        </div>
        <ProductSettings
          productCategories={promotion?.productSettings || []}
          selectedProductCategories={promotion?.productSettings.map((p: ProductSetting) => p.productCategoryId) || []}
          handleProductFieldChange={() => { void (0) }}
          readOnlyMode={true}
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
          <ButtonComponent variant="cancel" text={tGlobal('cancel')} onClick={onClose} />
          <ButtonComponent variant="apply" text={tGlobal('update')} type="submit" disabled={loading} />
        </div>
      </form>
    </>
  );
};

export default EditPromotion;

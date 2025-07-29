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
  ProductSetting,
  PromotionDetail
} from '../../../../services/interfaces/Promotion';
import { promotionService } from '../../../../services/promotionAPI';
import Information from './Form/Information';
import PromotionInfo, { PromotionLangFields } from './Form/LanguageInfo';
import ProductSettings from './Form/ProductSettings';

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

// Function to get banner URL from banner name
function getBannerUrl(bannerName: string): string {
  const baseUrl = window.location.origin + window.location.pathname.split('/').slice(0, -2).join('/');
  return `${baseUrl}/PromotionImages/${bannerName}`;
}

// Function to load image from URL and create File object
async function loadImageAsFile(imageUrl: string, fileName: string): Promise<File | null> {
  try {
    const response = await fetch(imageUrl, {
      mode: 'cors',
      credentials: 'same-origin'
    });
    if (!response.ok) {
      console.warn(`Failed to load image from ${imageUrl}: ${response.status} ${response.statusText}`);
      return null;
    }
    
    const blob = await response.blob();
    const file = new File([blob], fileName, { type: blob.type || 'image/jpeg' });
    return file;
  } catch (error) {
    console.error('Error loading image as file:', error);
    return null;
  }
}

const EditPromotion: React.FC<EditPromotionProps> = ({ onClose, promotionId }) => {
  const { t: tGlobal } = useTranslation('global');
  const { t } = useTranslation('promotion');
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
            
            // Load images and create File objects for each language
            for (const info of promotion.languageSettings) {
              let bannerFile: File | string | null = null;
              
              if (info.bannerName) {
                // Load the image as a File object
                const imageUrl = getBannerUrl(info.bannerName);

                bannerFile = await loadImageAsFile(imageUrl, info.bannerName);
                
                // If loading failed, keep as null - will be handled by UI
                if (!bannerFile) {
                  console.warn(`Failed to load image for ${info.bannerName}`);
                  bannerFile = null;
                }
              }
              
              infoMap[info.language] = {
                title: info.title || '',
                banner: bannerFile,
                bannerName: info.bannerName,
                content: info.content || '',
              };
            }
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
        message: tGlobal('thereNoChangeToUpdate')
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

  const hasChanges = () => {
    const statusChanged = originalStatus !== isActive;

    const contentChanged = JSON.stringify(promotionInfo) !== JSON.stringify(originalPromotionInfo);

    return statusChanged || contentChanged;
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
      const formData = new FormData();
      formData.append('PromotionId', promotionId.toString());
      formData.append('Enabled', isActive.toString());
      formData.append('OriginalEnabled', originalStatus.toString());     
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

      Object.entries(originalPromotionInfo).forEach(([lang, info], index) => {
        formData.append(`OriginalContents[${index}].Language`, lang);
        formData.append(`OriginalContents[${index}].Title`, info.title || '');
        formData.append(`OriginalContents[${index}].Content`, info.content || '');
        
        if (info.banner instanceof File) {
          formData.append(`OriginalContents[${index}].Banner`, info.banner);
        } else if (info.banner) {
          formData.append(`OriginalContents[${index}].BannerName`, info.banner as string);
        }
      });

      const result = await promotionService.editPromotionWithFormData(promotionId, formData);
      
      window.scrollTo({ top: 0, behavior: 'smooth' });
      if (result.isSuccessful) {
        setSuccessMessage(t('editPromotionForm.promotionUpdatedSuccessfully'));
        setTimeout(() => {
          onClose();
        }, 3000);
      } else {
        setError(result.message);
      }
    } catch (error) {
      console.error('Error editing promotion:', error);
      setError('An error occurred while updating the promotion');
    } finally {
      setLoading(false);
    }
  };

  if (loading && !promotionInfo) {
    return (
      <div className="flex items-center justify-center h-64">
        <div>{tGlobal('loading')}</div>
      </div>
    );
  }

  return (
    <>
      <div className="flex items-center justify-flex-start mb-[8px] py-[8px] border-[var(--border-control)] border-b-[1px] [&>:nth-child(2)]:border-0 [&>:nth-child(2)]:mb-[0] [&>:nth-child(1)]:mb-[0]">
        <ButtonIconComponent style='Back' onClick={onClose} />
        <TitleComponent text={t('editPromotionForm.editPromotion')} type='main'/>
      </div>

      <form onSubmit={handleSubmit} className="pb-[16px]">
        <div className="modal-body">
          <MessageComponent className="mb-[16px] max-[768px]:mb-[0]" message={error} type='error' />
          <MessageComponent className="mb-[16px] max-[768px]:mb-[0]" message={successMessage} type='success'/>

          <Information
            readOnlyMode={true}
            isActive={isActive}
            setIsActive={(value: boolean) => { setIsActive(value); }}
            promotionType={promotion?.promotionTypeId || 0}
            setPromotionType={() => { void (0); }}
            period={promotion?.fromDate && promotion?.toDate ? [new Date(promotion.fromDate), new Date(promotion.toDate)] : [null, null]}
            setPeriod={() => { void (0); }}
            productOptions={convertProductToTreeNodeOption(productOptions)}
            selectedProductIds={promotion?.productSettings?.flatMap((p: ProductSetting) => 
              p.products && p.products.length > 0 
                ? p.products.map(product => product.productId)
                : []
            ) || []}
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

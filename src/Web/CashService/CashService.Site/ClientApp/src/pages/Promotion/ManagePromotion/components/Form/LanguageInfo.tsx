import React from 'react';
import { useTranslation } from 'react-i18next';
import { ButtonTabComponent } from '../../../../../components/Buttons/ButtonTabComponent';
import { TitleComponent } from '../../../../../components/Title';
import UploadImage from '../../../../../components/UploadImage/UploadImage';
import CkEditorWrapper from '../../../../../components/CkEditorWrapper/CkEditorWrapper';
const BANNER_LOCATION = `${window.location.origin + window.location.pathname.split('/').slice(0, -2).join('/')}/PromotionImages`;

export interface PromotionLangFields {
  title: string;
  banner: File | string | null;
  bannerName: string | null;
  content: string;
}

export interface PromotionInfoProps {
  languages: { key: string; label: string; isDefault: boolean }[];
  value: Record<string, PromotionLangFields>;
  setValue: (lang: string, field: keyof PromotionLangFields, val: any) => void;
  error?: string;
  success?: string;
  activeLang: string;
  setActiveLang: (lang: string) => void;
}

export const LanguageTabs: React.FC<{
  languages: { key: string; label: string; isDefault: boolean }[];
  activeLang: string;
  setActiveLang: (lang: string) => void;
}> = ({ languages, activeLang, setActiveLang }) => {
  const sortedLanguages = languages.sort((a, b) => {
    if (a.isDefault && !b.isDefault) return -1;
    if (!a.isDefault && b.isDefault) return 1;
    return 0;
  });
  return (
    <div className="flex border-b border-[var(--border-color)] mb-[15px]">
      {sortedLanguages.map((lang) => (
        <ButtonTabComponent
          key={lang.key}
          text={lang.label}
          onClick={() => setActiveLang(lang.key)}
          isActive={activeLang === lang.key}
          type="button"
        />
      ))}
    </div>
  );
};

export const TitleField: React.FC<{
  value: string;
  onChange: (v: string) => void;
  lang: string;
  isDefault: boolean;
}> = ({ value, onChange, lang, isDefault }) => {
  const { t } = useTranslation('promotion');
  return (
    <div className="mb-4">
      <TitleComponent text={t('addPromotionForm.title')} required={isDefault} type="label" />
      <input
        className="control-field w-full"
        key={`title-${lang}`}
        value={value}
        onChange={(e) => onChange(e.target.value)}
        placeholder=""
      />
    </div>
  );
};

export const BannerField: React.FC<{
  bannerName?: string | null;
  setBanner: (f: File | string | null) => void;
  setBannerName?: (name: string | null) => void;
  banner?: File | string | null;
  lang: string;
  isDefault: boolean;
}> = ({ bannerName, setBanner, setBannerName, banner, lang, isDefault }) => {
  const { t } = useTranslation('promotion');

  const handleSetImage = (file: File | null) => {
    setBanner(file);
  };

  const handleRemoveImage = () => {
    bannerName && setBannerName && setBannerName(null);
  };

  return (
    <div className="mb-4">
      <TitleComponent text={t('addPromotionForm.banner')} required={isDefault} type="label" />
      <UploadImage
        key={`upload-image-${lang}`}
        setImage={handleSetImage}
        prevImage={bannerName ? `${BANNER_LOCATION}/${bannerName}` : ''}
        image={banner ?? null}
        inputId={`image-upload-${lang}`}
        maxSize={20}
        customClassName="w-full h-[300px]"
        removeImage={handleRemoveImage}
      />
    </div>
  );
};

export const ContentField: React.FC<{
  value: string;
  onChange: (v: string) => void;
  lang: string;
  isDefault: boolean;
}> = ({ value, onChange, lang, isDefault }) => {
  const { t } = useTranslation('promotion');
  return (
    <div>
      <TitleComponent text={t('addPromotionForm.content')} required={isDefault} type="label" />
      <CkEditorWrapper key={`content-${lang}`} data={value} onChange={onChange} isPaddingContent />
    </div>
  );
};

const PromotionInfo: React.FC<PromotionInfoProps> = ({
  languages,
  value,
  setValue,
  activeLang,
  setActiveLang,
}) => {
  const langFields = value[activeLang] || {
    title: '',
    banner: null,
    bannerName: null,
    content: '',
  };
  const { t } = useTranslation('promotion');
  return (
    <div className="max-w-[620px] ">
      <div className="flex items-center pb-[5px] border-b border-[var(--border-control)] mb-[5px]">
        <div className="inline-block mr-[10px]">
          <TitleComponent text={t('addPromotionForm.promotionContent')} type="medium" />
        </div>
      </div>
      <LanguageTabs languages={languages} activeLang={activeLang} setActiveLang={setActiveLang} />
      <TitleField
        value={langFields.title}
        onChange={(v) => setValue(activeLang, 'title', v)}
        lang={activeLang}
        isDefault={languages.find((lang) => lang.key === activeLang)?.isDefault || false}
      />
      <BannerField
        key={`banner-${activeLang}`}
        setBanner={(f) => setValue(activeLang, 'banner', f)}
        setBannerName={(name) => setValue(activeLang, 'bannerName', name)}
        bannerName={langFields.bannerName}
        banner={langFields.banner}
        lang={activeLang}
        isDefault={languages.find((lang) => lang.key === activeLang)?.isDefault || false}
      />
      <ContentField
        value={langFields.content}
        onChange={(v) => {
          setValue(activeLang, 'content', v);
        }}
        lang={activeLang}
        isDefault={languages.find((lang) => lang.key === activeLang)?.isDefault || false}
      />
    </div>
  );
};

export default PromotionInfo;

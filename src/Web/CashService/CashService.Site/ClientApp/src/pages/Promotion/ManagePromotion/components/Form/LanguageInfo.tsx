import React from 'react';
import { useTranslation } from 'react-i18next';
import { ButtonTabComponent } from '../../../../../components/Buttons/ButtonTabComponent';
import { TitleComponent } from '../../../../../components/Title';
import UploadImage from '../../../Components/UploadImage';
import CKEditor4Simple from './CKEditor4Simple';

export interface PromotionLangFields {
  title: string;
  banner: File | string | null;
  prevBanner: string | null;
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

const getBannerUrl = (bannerPath: string | null): string | null => {
  if (!bannerPath) return null;

  if (
    bannerPath.startsWith('http://') ||
    bannerPath.startsWith('https://') ||
    bannerPath.startsWith('data:')
  ) {
    return bannerPath;
  }

  if (bannerPath.startsWith('PromotionImages/')) {
    const baseUrl =
      window.location.origin + window.location.pathname.split('/').slice(0, -2).join('/');
    return `${baseUrl}/${bannerPath}`;
  }

  return bannerPath;
};

export const LanguageTabs: React.FC<{
  languages: { key: string; label: string }[];
  activeLang: string;
  setActiveLang: (lang: string) => void;
}> = ({ languages, activeLang, setActiveLang }) => {
  return (
    <div className="flex mt-[-12px] border-b border-[var(--border-color)] mb-[15px]">
      {languages.map((lang) => (
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
  prevBanner: string | null;
  setBanner: (f: File | string | null) => void;
  lang: string;
  isDefault: boolean;
}> = ({ prevBanner, setBanner, lang, isDefault }) => {
  const { t } = useTranslation('promotion');

  const handleSetImage = (file: File | null) => {
    setBanner(file);
  };

  const fullBannerUrl = getBannerUrl(prevBanner);

  return (
    <div className="mb-4">
      <TitleComponent text={t('addPromotionForm.banner')} required={isDefault} type="label" />
      <UploadImage
        key={`banner-${lang}`}
        setImage={handleSetImage}
        prevImage={fullBannerUrl}
        maxSize={500}
        customClassName="w-full h-[300px]"
      />
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
  const { t } = useTranslation('promotion');

  // Get current language fields
  const langFields = value[activeLang] || {
    title: '',
    banner: null,
    prevBanner: null,
    content: '',
  };

  // Handle content change for current active language
  const handleContentChange = (content: string) => {
    setValue(activeLang, 'content', content);
  };

  return (
    <div className="max-w-3xl">
      <LanguageTabs languages={languages} activeLang={activeLang} setActiveLang={setActiveLang} />

      <TitleField
        value={langFields.title}
        onChange={(v) => setValue(activeLang, 'title', v)}
        lang={activeLang}
        isDefault={languages.find((lang) => lang.key === activeLang)?.isDefault || false}
      />

      <BannerField
        prevBanner={langFields.prevBanner}
        setBanner={(f) => setValue(activeLang, 'banner', f)}
        lang={activeLang}
        isDefault={languages.find((lang) => lang.key === activeLang)?.isDefault || false}
      />

      {/* Single CKEditor instance that changes content based on active language */}
      <div className="mb-4">
        <CKEditor4Simple
          value={langFields.content}
          onChange={handleContentChange}
          placeholder={t('addPromotionForm.contentPlaceholder') || 'Enter content here...'}
          required={languages.find((lang) => lang.key === activeLang)?.isDefault || false}
          label={t('addPromotionForm.content')}
          minHeight="200px"
          maxHeight="400px"
          showToolbar={true}
        />
      </div>
    </div>
  );
};

export default PromotionInfo;

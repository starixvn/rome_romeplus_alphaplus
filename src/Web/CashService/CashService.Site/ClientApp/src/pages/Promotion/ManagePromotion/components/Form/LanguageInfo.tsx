import React from 'react';
import { useTranslation } from 'react-i18next';
import { ButtonTabComponent } from '../../../../../components/Buttons/ButtonTabComponent';
import { TitleComponent } from '../../../../../components/Title';
import UploadImage from '../../../Components/UploadImage';

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

// Function to get image URL from File object only
const getImageUrl = (banner: File | string | null): string | null => {
  if (banner instanceof File) {
    // If banner is a File object, create object URL
    return URL.createObjectURL(banner);
  }
  // If banner is not a File object, return null (no image to display)
  return null;
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
  banner: File | string | null;
  bannerName: string | null;
  setBanner: (f: File | string | null) => void;
  setBannerName?: (name: string | null) => void;
  lang: string;
  isDefault: boolean;
}> = ({ banner, setBanner, setBannerName, lang, isDefault }) => {
  const { t } = useTranslation('promotion');

  const handleSetImage = (file: File | null) => {
    setBanner(file);
    // Update bannerName when a new file is uploaded
    if (file && setBannerName) {
      // Set to null because backend will generate new filename
      setBannerName(null);
    } else if (!file && setBannerName) {
      setBannerName(null);
    }
  };
  
  const fullBannerUrl = getImageUrl(banner);
  
  // Cleanup object URL when component unmounts or banner changes
  React.useEffect(() => {
    return () => {
      if (banner instanceof File && fullBannerUrl && fullBannerUrl.startsWith('blob:')) {
        URL.revokeObjectURL(fullBannerUrl);
      }
    };
  }, [banner, fullBannerUrl]);

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

export const ContentField: React.FC<{
  value: string;
  onChange: (v: string) => void;
  lang: string;
  isDefault: boolean;
}> = ({ value, onChange, lang, isDefault }) => {
  const { t } = useTranslation('promotion');
  return (
    <div className="mb-4">
      <TitleComponent text={t('addPromotionForm.content')} required={isDefault} type="label" />
      <div className="border border-[var(--border-control)] rounded">
        <textarea
          key={`content-${lang}`}
          className="w-full min-h-[120px] p-2 resize-y border-0 focus:ring-0 focus:outline-none block"
          value={value}
          onChange={(e) => onChange(e.target.value)}
          placeholder=""
        />
      </div>
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
        banner={langFields.banner}
        bannerName={langFields.bannerName}
        setBanner={(f) => setValue(activeLang, 'banner', f)}
        setBannerName={(name) => setValue(activeLang, 'bannerName', name)}
        lang={activeLang}
        isDefault={languages.find((lang) => lang.key === activeLang)?.isDefault || false}
      />
      <ContentField
        value={langFields.content}
        onChange={(v) => setValue(activeLang, 'content', v)}
        lang={activeLang}
        isDefault={languages.find((lang) => lang.key === activeLang)?.isDefault || false}
      />
    </div>
  );
};

export default PromotionInfo;

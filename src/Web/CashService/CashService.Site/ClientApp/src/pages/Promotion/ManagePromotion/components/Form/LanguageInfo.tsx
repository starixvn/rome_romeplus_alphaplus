import React from 'react';
import { LabelTitleComponent } from '../../../../../components/Title/LabelTitleComponent';
import UploadImage from '../../../../Transactions/Components/UploadImage';
import { ButtonTabComponent } from '../../../../../components/Buttons/ButtonTabComponent';

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
}> = ({ value, onChange, lang, isDefault }) => (
  <div className="mb-4">
    <LabelTitleComponent text={`Title`} required={isDefault} />
    <input
      className="control-field w-full"
      key={`title-${lang}`}
      value={value}
      onChange={(e) => onChange(e.target.value)}
      placeholder=""
    />
  </div>
);

export const BannerField: React.FC<{
  prevBanner: string | null;
  setBanner: (f: File | string | null) => void;
  lang: string;
  isDefault: boolean;
}> = ({ prevBanner, setBanner, lang, isDefault }) => (
  <div className="mb-4">
    <LabelTitleComponent text={`Banner`} required={isDefault} />
    <UploadImage
      key={`banner-${lang}`}
      setImage={setBanner}
      prevImage={prevBanner}
      maxSize={500}
      customClassName="w-full h-[300px]"
    />
  </div>
);

export const ContentField: React.FC<{
  value: string;
  onChange: (v: string) => void;
  lang: string;
  isDefault: boolean;
}> = ({ value, onChange, lang, isDefault }) => (
  <div className="mb-4">
    <LabelTitleComponent text={`Content`} required={isDefault} />
    <div className="border border-[var(--border-control)] rounded">
      <textarea
        key={`content-${lang}`}
        className="w-full min-h-[120px] p-2 resize-y border-0 focus:ring-0 focus:outline-none"
        value={value}
        onChange={(e) => onChange(e.target.value)}
        placeholder=""
      />
    </div>
  </div>
);

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
    prevBanner: null,
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
        prevBanner={langFields.prevBanner}
        setBanner={(f) => setValue(activeLang, 'banner', f)}
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

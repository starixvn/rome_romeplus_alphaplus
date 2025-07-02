import React from 'react';
import { LabelTitleComponent } from '../../../../../components/Title/LabelTitleComponent';
import { ErrorMessageComponent, SuccessMessageComponent } from '../../../../../components/Message';
import UploadImage from '../../../../Transactions/Components/UploadImage';
import { ButtonTabComponent } from '../../../../../components/Buttons/ButtonTabComponent';

export interface PromotionLangFields {
  title: string;
  banner: File | string | null;
  prevBanner: string | null;
  content: string;
}

export interface PromotionInfoProps {
  languages: { key: string; label: string }[];
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
    <div className="flex gap-2 mb-4">
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
}> = ({ value, onChange, lang }) => (
  <div className="mb-4">
    <LabelTitleComponent text={`Title (${lang})`} required />
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
}> = ({ prevBanner, setBanner, lang }) => (
  <div className="mb-4">
    <LabelTitleComponent text={`Banner (${lang})`} required />
    <UploadImage
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
}> = ({ value, onChange, lang }) => (
  <div className="mb-4">
    <LabelTitleComponent text={`Content (${lang})`} required />
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
  error,
  success,
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
    <div className="bg-white max-w-3xl pt-[16px]">
      <LanguageTabs languages={languages} activeLang={activeLang} setActiveLang={setActiveLang} />
      <TitleField
        value={langFields.title}
        onChange={(v) => setValue(activeLang, 'title', v)}
        lang={activeLang}
      />
      <BannerField
        prevBanner={langFields.prevBanner}
        setBanner={(f) => setValue(activeLang, 'banner', f)}
        lang={activeLang}
      />
      <ContentField
        value={langFields.content}
        onChange={(v) => setValue(activeLang, 'content', v)}
        lang={activeLang}
      />
      <ErrorMessageComponent message={error || null} />
      <SuccessMessageComponent message={success || null} />
    </div>
  );
};

export default PromotionInfo;

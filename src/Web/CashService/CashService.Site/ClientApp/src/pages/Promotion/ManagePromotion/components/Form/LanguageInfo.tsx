import React from 'react';
import { LabelTitleComponent } from '../../../../../components/Title/LabelTitleComponent';
import { ErrorMessageComponent, SuccessMessageComponent } from '../../../../../components/Message';
import UploadImage from '../../../../Transactions/Components/UploadImage';

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
}> = ({ languages, activeLang, setActiveLang }) => (
  <div className="flex gap-2 mb-4">
    {languages.map((lang) => (
      <button
        key={lang.key}
        className={`px-4 py-2 rounded-t font-semibold border-b-2 ${
          activeLang === lang.key
            ? 'bg-[#1967D2] text-white border-[#1967D2]'
            : 'bg-[#F5F5F5] text-[#333] border-transparent'
        }`}
        onClick={() => setActiveLang(lang.key)}
        type="button"
      >
        {lang.label}
      </button>
    ))}
  </div>
);

export const TitleField: React.FC<{
  value: string;
  onChange: (v: string) => void;
}> = ({ value, onChange }) => (
  <div className="mb-4">
    <LabelTitleComponent text="Title" required />
    <input
      className="control-field w-full"
      value={value}
      onChange={(e) => onChange(e.target.value)}
      placeholder=""
    />
  </div>
);

export const BannerField: React.FC<{
  prevBanner: string | null;
  setBanner: (f: File | string | null) => void;
}> = ({ prevBanner, setBanner }) => (
  <div className="mb-4">
    <LabelTitleComponent text="Banner" required />
    <UploadImage setImage={setBanner} prevImage={prevBanner} />
  </div>
);

export const ContentField: React.FC<{
  value: string;
  onChange: (v: string) => void;
}> = ({ value, onChange }) => (
  <div className="mb-4">
    <LabelTitleComponent text="Content" required />
    <div className="border border-[var(--border-control)] rounded">
      <textarea
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
      <TitleField value={langFields.title} onChange={(v) => setValue(activeLang, 'title', v)} />
      <BannerField
        prevBanner={langFields.prevBanner}
        setBanner={(f) => setValue(activeLang, 'banner', f)}
      />
      <ContentField
        value={langFields.content}
        onChange={(v) => setValue(activeLang, 'content', v)}
      />
      <ErrorMessageComponent message={error || null} />
      <SuccessMessageComponent message={success || null} />
    </div>
  );
};

export default PromotionInfo;

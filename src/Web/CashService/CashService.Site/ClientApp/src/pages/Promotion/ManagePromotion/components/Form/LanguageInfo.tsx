import React from 'react';
import { LabelTitleComponent } from '../../../../../components/Title/LabelTitleComponent';
import { ErrorMessageComponent, SuccessMessageComponent } from '../../../../../components/Message';
import UploadImage from '../../../../Transactions/Components/UploadImage';
import { ButtonTabComponent } from '../../../../../components/Buttons/ButtonTabComponent';
import { useQuill } from 'react-quilljs';
import 'quill/dist/quill.snow.css';

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
}> = ({ value, onChange, lang, isDefault }) => {
  const { quill, quillRef } = useQuill({
    theme: 'snow',
    modules: {
      toolbar: [
        [{ header: [1, 2, false] }],
        ['bold', 'italic', 'underline'],
        ['link', 'image'],
        [{ list: 'ordered' }, { list: 'bullet' }],
        ['clean'],
      ],
    },
    placeholder: '',
  });

  React.useEffect(() => {
    if (quill && value !== quill.root.innerHTML) {
      quill.root.innerHTML = value || '';
    }
  }, [quill, value]);

  React.useEffect(() => {
    if (!quill) return;
    const handler = () => {
      onChange(quill.root.innerHTML);
    };
    quill.on('text-change', handler);
    return () => {
      quill.off('text-change', handler);
    };
  }, [quill, onChange]);

  return (
    <div className="mb-4">
      <LabelTitleComponent text={`Content`} required={isDefault} />
      <div className="border border-[var(--border-control)] rounded">
        <div key={`content-${lang}`} ref={quillRef} style={{ minHeight: 180, padding: 2 }} />
      </div>
    </div>
  );
};

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
      <ErrorMessageComponent message={error || null} />
      <SuccessMessageComponent message={success || null} />
    </div>
  );
};

export default PromotionInfo;

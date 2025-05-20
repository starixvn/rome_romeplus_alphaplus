import { useTranslation } from "react-i18next";
import { Contact as ContactType } from "../../../services/interfaces/MemberSites";
import { FieldError, useFormContext } from "react-hook-form";

interface ContactProps {
  contact: ContactType;
  handleContactChange: (contact: ContactType) => void;
}

interface ContactErrors {
  [key: string]: FieldError;
}

export const Contact = (props: ContactProps) => {
  const { t } = useTranslation('memberSite');
  const { contact } = props;
  const { register, formState: { errors } } = useFormContext();
  const getValidationRules = (type: string) => {
    const rules: any = {
      required: contact.isRequired ? t('validation.required') : false,
    };

    switch (type) {
      case 'line':
        rules.pattern = {
          value: /^[a-zA-Z0-9._-]+$/,
          message: t('validation.lineId')
        };
        break;
      case 'telegram':
        rules.pattern = {
          value: /^[a-zA-Z0-9_]+$/,
          message: t('validation.telegram')
        };
        break;
      case 'email':
        rules.pattern = {
          value: /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i,
          message: t('validation.email')
        };
        break;
    }
    return rules;
  };

  const getErrorMessage = (error: FieldError | undefined) => {
    if (!error) return '';
    if (typeof error.message === 'string') return error.message;
    if (Array.isArray(error.message)) return error.message[0];
    return '';
  };

  const fieldName = `contacts.${contact.type}`;
  const contactErrors = errors.contacts as ContactErrors | undefined;
  const fieldError = contactErrors?.[contact.type];

  return (
    <div key={contact.type} className='w-[230px] pt-[4px] max-[768px]:w-[48%]'>
      <label htmlFor={contact.type} className="block pt-[4px] mb-[8px] font-semibold max-[768px]:mb-[8px]">
        {t(`message.${contact.type}`)}
        {contact.isRequired && <span className='text-[#E25757]'>*</span>}
      </label>
      <div className="relative">
        <input
          id={contact.type}
          type="text"
          {...register(fieldName, getValidationRules(contact.type))}
          className={`h-[32px] max-[768px]:h-[40px] w-[100%] border-[1px] rounded-[4px] px-[8px] text-[12px] focus:outline-none max-[768px]:text-[16px] max-[768px]:leading-[16px] ${
            fieldError 
              ? 'border-[#E25757] focus:border-[#E25757]' 
              : 'border-[var(--border-control)] focus:border-[var(--border-control-focus)]'
          }`}
        />
        {fieldError && (
          <p className="text-[#E25757] text-[12px] mt-[4px]">
            {getErrorMessage(fieldError)}
          </p>
        )}
      </div>
    </div>
  );
};

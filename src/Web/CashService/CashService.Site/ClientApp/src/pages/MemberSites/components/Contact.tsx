import { useTranslation } from "react-i18next";
import { Contact as ContactType } from "../../../services/interfaces/MemberSites";
import { FieldError, useFormContext } from "react-hook-form";
import { CONTACT_MAX_LENGTH, CONTACT_REGEX, LINE_REGEX } from "../../../constants";
import { LabelTitleComponent } from "../../../components/Title";
import { MessageComponent } from "../../../components/Message";

interface ContactProps {
  index: number;
  contact: ContactType;
}

interface ContactErrors {
  [key: string]: {
    value?: FieldError;
  };
}

export const Contact = (props: ContactProps) => {
  const { t } = useTranslation('memberSite');
  const { contact, index } = props;
  const { register, formState: { errors }, setValue } = useFormContext();
  const getValidationRules = (contact: ContactType) => {
    const rules: any = {
      required: contact.isRequired ? t('validation.required') : false,
    };

    switch (contact.type) {
      case 'line':
        rules.pattern = {
          value: LINE_REGEX,
          message: t('validation.onlyAz09')
        };
        break;
      case 'telegram':
      case 'tele':
        rules.pattern = {
          value: CONTACT_REGEX,
          message: t('validation.onlyAz09')
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

  const field = `contacts.${index}.value`;

  const contactErrors = errors.contacts as ContactErrors | undefined;
  const fieldError = contactErrors?.[index];

  return (
    <div key={contact.type} className='w-[230px] pt-[4px] max-[768px]:w-[48%]'>
       
      <LabelTitleComponent text={t(`message.${contact.type}`)}  required={contact.isRequired}/>
      <div className="relative">
        <input
          id={contact.type}
          type="text"
          {...register(field, {
            ...getValidationRules(contact),
            onChange: (e) => {
              e.preventDefault();
              setValue(field, e.target.value, { shouldDirty: true });
            }
          })}
          maxLength={CONTACT_MAX_LENGTH}
          className={`control-field ${fieldError ? 'error' : '' }`}
        />       
        <MessageComponent message={getErrorMessage(fieldError?.value)} type='validate' />
       
      </div>
    </div>
  );
};

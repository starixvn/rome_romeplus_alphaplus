import React from 'react';
import { useTranslation } from 'react-i18next';
import { Contact, MobilePhone } from '../../../services/interfaces/MemberSites';
import { Contact as ContactComponent } from './Contact';
import { MobilePhone as MobilePhoneComponent } from './MobiPhone';
import { useFormContext } from 'react-hook-form';
import { ANNOUNCEMENT_MAX_LENGTH } from '../../../constants';
import { MediumTitleComponent } from '../../../components/TitleComponents';

interface MessageFormProps {
  loading: boolean;
}

const MessageFormSkeleton = () => {
  return (
    <div>
      {/* Title skeleton */}
      <div className="h-[22px] w-[160px] bg-gray-200 dark:bg-gray-700 rounded mb-2 animate-pulse"></div>
      {/* Subtitle skeleton */}
      <div className="h-[16px] w-[340px] bg-gray-100 dark:bg-gray-700 rounded mb-4 animate-pulse"></div>
      {/* Input skeletons */}
      <div className="flex gap-4 mb-4">
        <div className="h-[38px] w-[190px] bg-gray-200 dark:bg-gray-700 rounded animate-pulse"></div>
        <div className="h-[38px] w-[190px] bg-gray-200 dark:bg-gray-700 rounded animate-pulse"></div>
      </div>
      {/* Textarea skeleton */}
      <div className="h-[80px] w-[400px] bg-gray-200 dark:bg-gray-700 rounded animate-pulse"></div>
    </div>
  );
};

const MessageForm: React.FC<MessageFormProps> = ({loading}) => {
  const { t } = useTranslation('memberSite');
  const { setValue, watch, register, formState: { errors } } = useFormContext();

  if (loading) {
    return <MessageFormSkeleton />
  }

  const handleMobilePhoneChange = (data: Partial<MobilePhone>) => {
    setValue('mobilePhone', { ...watch('mobilePhone'), ...data }, { shouldDirty: true });
  }

  return (
    <div className="">
      <MediumTitleComponent text={t('message.title')} />
      <p className="pt-[4px] mb-[8px] font-semibold">{t('message.description')}</p>

      <div className="flex flex-nowrap gap-[16px] mb-[4px] max-[768px]:justify-between max-[768px]:gap-[0] max-[768px]:flex-wrap">
        <MobilePhoneComponent
          mobilePhone={watch('mobilePhone')}
          handleMobilePhoneChange={handleMobilePhoneChange}
        />
        {watch('contacts').map((contact: Contact, index: number) => (
          <ContactComponent
            index={index}
            key={contact.type}
            contact={contact}
          />
        ))}
      </div>

      <div className='w-[476px] max-[768px]:w-[100%]'>
        <label className="block pt-[4px] mb-[8px] font-semibold max-[768px]:mb-[8px]" htmlFor="announcement">
          {t('message.announcement')}
        </label>
        <div className="relative">
          <textarea
            id="announcement"
            {...register('announcement', {
              maxLength: {
                value: ANNOUNCEMENT_MAX_LENGTH,
                message: t('validation.maxLength', { maxLength: ANNOUNCEMENT_MAX_LENGTH })
              }
            })}
            rows={4}
            className={`p-[8px] h-[100px] w-[100%] border-[1px] rounded-[4px] px-[8px] text-[12px] focus:outline-none max-[768px]:text-[16px] max-[768px]:leading-[16px] max-[768px]:w-[100%] ${errors.announcement
              ? 'border-[#E25757] focus:border-[#E25757]'
              : 'border-[var(--border-control)] focus:border-[var(--border-control-focus)]'
              }`}
            placeholder={t('message.placeholder.message')}
            maxLength={ANNOUNCEMENT_MAX_LENGTH}
          />
          {errors.announcement && (
            <p className="text-[#E25757] text-[12px] mt-[4px]">
              {errors.announcement?.message as string}
            </p>
          )}
        </div>
      </div>
    </div>
  );
};

export default MessageForm; 
import React from 'react';
import { useTranslation } from 'react-i18next';
import { Contact, MobilePhone } from '../../../services/interfaces/MemberSites';
import { Contact as ContactComponent } from './Contact';
import { MobilePhone as MobilePhoneComponent } from './MobiPhone';
import { useFormContext } from 'react-hook-form';
import { ANNOUNCEMENT_MAX_LENGTH } from '../../../constants';
import { LabelTitleComponent, MediumTitleComponent } from '../../../components/Title';
import { ValidateMessageComponent } from '../../../components/Message';

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
      <p className="pt-[4px] mb-[8px] font-semibold text-[var(--text-color)]">{t('message.description')}</p>
      <div className=''>
        <div className="flex flex-nowrap gap-[16px] mb-[10px] max-[768px]:justify-between max-[768px]:gap-[0] max-[768px]:flex-wrap">
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

        <div className='w-full max-[768px]:w-[100%]'>        
          <LabelTitleComponent text={t('message.announcement')}  />
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
              className={`p-[8px] h-[100px] w-[100%] border-[1px] rounded-[4px] px-[8px]  focus:outline-none max-[768px]:w-[100%] bg-[var(--background-control)] resize ${errors.announcement
                ? 'border-[var(--border-control-error)]'
                : 'border-[var(--border-control)]'
                }`}
              placeholder={t('message.placeholder.message')}
              maxLength={ANNOUNCEMENT_MAX_LENGTH}
            />
            {errors.announcement && (
              <ValidateMessageComponent message={errors.announcement?.message as string} />
            )}
          </div>
        </div>
      </div>
    </div>
  );
};

export default MessageForm; 
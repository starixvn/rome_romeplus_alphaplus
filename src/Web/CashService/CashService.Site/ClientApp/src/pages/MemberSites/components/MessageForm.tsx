import React from 'react';
import { useTranslation } from 'react-i18next';
import { Contact, MobilePhone } from '../../../services/interfaces/MemberSites';
import { Contact as ContactComponent } from './Contact';
import { MobilePhone as MobilePhoneComponent } from './MobiPhone';
interface MessageFormProps {
  contacts: Contact[];
  mobilePhone: MobilePhone;
  announcement: string;
  loading: boolean;
  onMessageChange: (messages: {
    mobilePhone?: MobilePhone;
    contacts?: Contact[];
    announcement?: string
  }) => void;
}

const MessageFormSkeleton = () => {
  return (
    <div>
      {/* Title skeleton */}
      <div className="h-[22px] w-[160px] bg-gray-200 rounded mb-2 animate-pulse"></div>
      {/* Subtitle skeleton */}
      <div className="h-[16px] w-[340px] bg-gray-100 rounded mb-4 animate-pulse"></div>
      {/* Input skeletons */}
      <div className="flex gap-4 mb-4">
        <div className="h-[38px] w-[190px] bg-gray-200 rounded animate-pulse"></div>
        <div className="h-[38px] w-[190px] bg-gray-200 rounded animate-pulse"></div>
      </div>
      {/* Textarea skeleton */}
      <div className="h-[80px] w-[400px] bg-gray-200 rounded animate-pulse"></div>
    </div>
  );
};

const MessageForm: React.FC<MessageFormProps> = ({
  contacts,
  mobilePhone,
  announcement,
  onMessageChange,
  loading
}) => {
  const { t } = useTranslation('memberSite');

  if (loading) {
    return <MessageFormSkeleton />
  }


  const handleContactChange = (contact: Partial<Contact>) => {
    const newContact = contacts.find(c => c.type === contact.type);
    if (newContact) {
      newContact.value = contact.value || '';
    } else {
      contacts.push(contact as Contact);
    }
    onMessageChange({ contacts: contacts });
  }

  const handleMobilePhoneChange = (data: Partial<MobilePhone>) => {
    const newMobilePhone = { ...mobilePhone, ...data };
    onMessageChange({ mobilePhone: newMobilePhone });
  }

  return (
    <div className="">
      <h2 className="text-[16px] text-[var(--text-color-primary)] font-semibold leading-[21.6px] max-[768px]:text-[20px] max-[768px]:leading-[26px] ">{t('message.title')}</h2>
      <p className="text-[12px] pt-[4px] mb-[8px] font-semibold max-[768px]:text-[16px] max-[768px]:leading-[16px]">{t('message.description')}</p>
      <div className="max-w-[785px]">
        <div className="flex flex-nowrap gap-[16px] mb-[4px] max-[768px]:justify-between max-[768px]:gap-[0] max-[768px]:flex-wrap">
          <div className='w-[230px] pt-[4px] max-[768px]:w-[48%]'>
            <MobilePhoneComponent
              mobilePhone={mobilePhone}
              handleMobilePhoneChange={handleMobilePhoneChange}
            />
          </div>
          {
            contacts.map((contact: Contact) => {
              return (
                <ContactComponent
                  key={contact.type}
                  contact={contact}
                  handleContactChange={handleContactChange}
                />)
            })
          }


        </div>

        <div className='w-[476px] max-[768px]:w-[100%]'>
          <label className="block text-[12px] pt-[4px] mb-[8px] font-semibold max-[768px]:text-[16px] max-[768px]:leading-[16px] max-[768px]:mb-[8px]" htmlFor="announcement">
            {t('message.announcement')}
          </label>
          <textarea
            id="announcement"
            value={announcement}
            onChange={(e) => onMessageChange({ announcement: e.target.value })}
            rows={4}
            maxLength={1000}
            className="p-[8px] h-[100px] w-[100%] border-[1px] border-[var(--border-control)] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[var(--border-control-focus)] max-[768px]:text-[16px] max-[768px]:leading-[16px] max-[768px]:w-[100%]"
            placeholder={t('message.placeholder.message')}
          />
        </div>
      </div>
    </div>
  );
};

export default MessageForm; 
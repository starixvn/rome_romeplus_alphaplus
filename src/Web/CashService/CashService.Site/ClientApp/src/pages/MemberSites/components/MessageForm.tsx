import React from 'react';
import { useTranslation } from 'react-i18next';

interface MessageFormProps {
  countryCodes: string[];
  mobileCode: string;
  mobilePhone: string;
  lineId: string;
  announcement: string;
  loading: boolean;
  onMessageChange: (messages: { mobileCode?: string; mobilePhone?: string; lineId?: string; announcement?: string }) => void;
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
  countryCodes,
  mobileCode,
  mobilePhone,
  lineId,
  announcement,
  onMessageChange,
  loading
}) => {
  const { t } = useTranslation('memberSite');

  if (loading) {
    return <MessageFormSkeleton />
  }

  return (
    <div className="">
      <h2 className="text-[16px] text-[var(--primary-color)] font-semibold leading-[21.6px] max-[768px]:text-[20px] max-[768px]:leading-[26px] ">{t('message.title')}</h2>
      <p className="text-[12px] pt-[4px] mb-[8px] font-semibold max-[768px]:text-[16px] max-[768px]:leading-[16px]">{t('message.description')}</p>
      <div className="">
          <div className="flex gap-[16px] mb-[4px] max-[768px]:justify-between max-[768px]:gap-[0]">
            <div className='w-[24%] min-w-[200px] pt-[4px] max-[768px]:w-[48%]'>
              <label className="block text-[12px] pt-[4px] mb-[8px] font-semibold max-[768px]:text-[16px] max-[768px]:leading-[16px] max-[768px]:mb-[8px]" htmlFor="mobile">
                {t('message.mobile')}
              </label>
              <div className="flex items-center h-[35px]  border-[1px] border-[#0000003D] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[var(--primary-color)] cursor-pointer">
                <select
                  value={mobileCode}
                  onChange={(e) =>{
                    onMessageChange({ mobileCode: e.target.value });
                  }}
                  className="focus:outline-none focus:border-[var(--primary-color)] h-[100%]  cursor-pointer mr-[5px] max-[768px]:text-[16px] max-[768px]:leading-[16px]"
                >
                  {countryCodes.map((code) => (
                    <option key={code} value={code} selected={code === mobileCode}>+{code}</option>
                  ))}
                </select>
                <input                     
                  className="focus:outline-none w-[100%] h-[100%] pl-[10px] border-l-[1px] border-[#0000003D] max-[768px]:text-[16px] max-[768px]:leading-[16px]"
                  id="mobile"
                  type="text"
                  value={mobilePhone}
                  onChange={(e) => {
                    onMessageChange({ mobilePhone: e.target.value });
                  }}
                  placeholder="XXXXXXXXX"
                />
              </div>
            </div>
            <div className='w-[24%] min-w-[200px] pt-[4px] max-[768px]:w-[48%]'>
              <label className="block text-[12px] pt-[4px] mb-[8px] font-semibold max-[768px]:text-[16px] max-[768px]:leading-[16px] max-[768px]:mb-[8px]" htmlFor="lineId">
                {t('message.lineId')}
                <span className='text-[#E25757]'>*</span>
              </label>
              <input
                id="lineId"
                type="text"
                value={lineId}
                onChange={(e) => onMessageChange({ lineId: e.target.value })}
                className="h-[35px] w-[100%] border-[1px] border-[#0000003D] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[var(--primary-color)] max-[768px]:text-[16px] max-[768px]:leading-[16px]"
              />
            </div>
      
        </div>

        <div className='w-[50%] min-w-[400px] max-[768px]:w-[100%]'>
          <label className="block text-[12px] pt-[4px] mb-[8px] font-semibold max-[768px]:text-[16px] max-[768px]:leading-[16px] max-[768px]:mb-[8px]" htmlFor="announcement">
            {t('message.announcement')}
          </label> 
          <textarea
            id="announcement"
            value={announcement}
            onChange={(e) => onMessageChange({ announcement: e.target.value })}
            rows={4}
            maxLength={1000}
            className="p-[8px] h-[100px] w-[100%] border-[1px] border-[#0000003D] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[var(--primary-color)] max-[768px]:text-[16px] max-[768px]:leading-[16px] max-[768px]:w-[100%]"
            placeholder={t('message.placeholder.message')}
          />
        </div>
      </div>
    </div>
  );
};

export default MessageForm; 
import React from 'react';
import { useTranslation } from 'react-i18next';

interface MessageFormProps {
  mobile: string;
  lineId: string;
  announcement: string;
  onMessageChange: (messages: { mobile?: string; lineId?: string; announcement?: string }) => void;
}

const MessageForm: React.FC<MessageFormProps> = ({
  mobile,
  lineId,
  announcement,
  onMessageChange
}) => {
  const { t } = useTranslation('memberSite');

  return (
    <div className="">
      <h2 className="w-full text-[16px] text-[var(--primary-color)] font-semibold leading-[21.6px] tracking-[0.2px] ">{t('message.title')}</h2>
      <p className="text-[12px] pt-[4px] mb-[8px] font-semibold">{t('message.description')}</p>
      <div className="">
          <div className="flex gap-[16px] mb-[4px]">
            <div className='w-[24%] min-w-[200px] pt-[4px]'>
              <label className="text-[12px] pt-[4px] mb-[8px] font-semibold" htmlFor="mobile">
                {t('message.mobile')}
              </label>
              <div className="flex items-center h-[35px]  border-[1px] border-[#0000003D] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[var(--primary-color)] cursor-pointer">
                <select
                  className="focus:outline-none focus:border-[var(--primary-color)] h-[100%]  cursor-pointer mr-[5px]"
                >
                  <option value="+66">+66</option>
                  <option value="+84">+84</option>
                </select>
                <input                     
                  className="focus:outline-none w-[100%] h-[100%] pl-[10px] border-l-[1px] border-[#0000003D]"
                  id="mobile"
                  type="text"
                  value={mobile}
                  onChange={(e) => onMessageChange({ mobile: e.target.value })}
                  placeholder="XXXXXXXXX"
                />
              </div>
            </div>
            <div className='w-[24%] min-w-[200px] pt-[4px]'>
              <label className="text-[12px] pt-[4px] mb-[8px] font-semibold" htmlFor="lineId">
                {t('message.lineId')}
                <span className='text-[#E25757]'>*</span>
              </label>
              <input
                id="lineId"
                type="text"
                value={lineId}
                onChange={(e) => onMessageChange({ lineId: e.target.value })}
                className="h-[35px] w-[100%] border-[1px] border-[#0000003D] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[var(--primary-color)]"
              />
            </div>
      
        </div>

        <div className='w-[50%] min-w-[400px]'>
          <label className="text-[12px] pt-[4px] mb-[8px] font-semibold" htmlFor="announcement">
            {t('message.announcement')}
          </label> 
          <textarea
            id="announcement"
            value={announcement}
            onChange={(e) => onMessageChange({ announcement: e.target.value })}
            rows={4}
            maxLength={1000}
            className="p-[8px] h-[100px] w-[100%] border-[1px] border-[#0000003D] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[var(--primary-color)]"
            placeholder={t('message.placeholder.message')}
          />
        </div>
      </div>
    </div>
  );
};

export default MessageForm; 
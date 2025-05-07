import Tippy from "@tippyjs/react";
import { useTranslation } from 'react-i18next';
import { IconInfo } from "../Icons";
import '../../i18n/i18n';

interface TitleCommonProps {
  title: string;
}

export default function TitleCommon({ title }: TitleCommonProps) {
    const { t } = useTranslation('managePayments');
    return (
      <>
        <div className="mb-[8px]">
            <h1 className="flex items-center font-bold text-[15px] text-[#333333]">
            {title}
            <Tippy
                content={
                <div>
                    <div>
                    <i className="bi bi-info-circle fs-4 ico-info inline-elm"></i> 
                    {t('managePayments')}
                    </div>
                    <div>• {t('managePaymentsTip1')}</div>
                    <div>• {t('managePaymentsTip2')}</div>
                </div>
                }
                placement="top"
                theme="light"
                arrow={true}
            >
                <span><IconInfo className="w-[20px] h-[20px] ml-[4px] fill-[#163f78]" /></span>
            </Tippy>
            </h1>
        </div>
      </>
    );
  }
  
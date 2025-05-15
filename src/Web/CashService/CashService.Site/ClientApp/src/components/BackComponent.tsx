import { IconBack } from "./Icons";
import { useTranslation } from 'react-i18next';

interface BackCommonProps {
  onClose: () => void;
}

export default function BackCommon({ onClose }: BackCommonProps) {
    const { t: tGlobal } = useTranslation('global');
    return (
      <>
        <div className='text-[var(--text-color-primary)] px-[8px] py-[4px] border-b-[2px] border-[#cfcfcf] border-dotted mb-[4px]'>
            <span className='cursor-pointer inline-flex items-center py-[8px]' onClick={onClose}>
            <IconBack className='w-[18px] h-[18px] fill-[var(--primary-color)] mr-[8px]'  />
            <span className='text-[12px] text-[var(--text-color-primary)] font-semibold'>{tGlobal('back')}</span>
            </span>
        </div>
      </>
    );
  }
  
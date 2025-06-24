import React from 'react';
import { MediumTitleComponent } from './TitleComponents';
import { NoDataIcon } from './Icons';
import { useTranslation } from 'react-i18next';

interface NoRecordFoundProps {
  colSpan: number;
  message?: string;
}

const NoRecordFound: React.FC<NoRecordFoundProps> = ({ colSpan }) => {  
  const { t } = useTranslation('global');
  return (
    <tr>
      <td colSpan={colSpan} className="p-3 text-center">
        <div className="flex justify-center items-center flex-col py-[20px]" role="alert">
          <NoDataIcon />
          <MediumTitleComponent text={t('noData')} />
        </div>
      </td>
    </tr>
  );
};

export default NoRecordFound;

import React, { useMemo } from 'react';
import { IconCheck, IconWarning, IconError } from '../Icons';
import { ResultMessageProps, ResultType } from './types';

interface MessageStyles {
  container: string;
  containerIcon?: string;
  iconComponent?: React.ReactNode;
}

export const MessageComponent: React.FC<ResultMessageProps> = ({
  message,
  type = 'success',
  className = '',
}) => {
  const styles = useMemo((): MessageStyles => {
    switch (type) {
      case 'validate':
        return {
          container: 'message-validate',
        };
      case 'error':
        return {
          container: 'message-error', 
          iconComponent: <IconError className="w-[20px] h-[20px] fill-[#CC0000] dark:fill-[#FF6D6D] absolute top-[7px] left-[10px]" />
        };
      case 'warning':
        return {
          container: 'message-warning',    
          iconComponent: <IconWarning className="w-[20px] h-[20px] fill-[#806A26] dark:fill-[#CCB84D] absolute top-[7px] left-[10px]" />
        };
      case 'success':
      default:
        return {
          container: 'message-success',  
          iconComponent: <IconCheck className="w-[20px] h-[20px] fill-[#268026] dark:fill-[#7ECC4D] absolute top-[7px] left-[10px]" />
        };
    }
  }, [type]);

  const validTypes: ResultType[] = ['error', 'success', 'warning', 'validate'];
  if (!validTypes.includes(type)) {
    console.warn(`Invalid message type: ${type}`);
    type = 'success';
  }

  if (!message) return null;

  return (
    <div className={`${className} max-[768px]:py-[12px]`} role="alert">
      <div className={`${styles.container} ${styles.containerIcon || ''}`}>
        {styles.iconComponent}
        <div dangerouslySetInnerHTML={{ __html: message }} />
      </div>
    </div>
  );
}; 
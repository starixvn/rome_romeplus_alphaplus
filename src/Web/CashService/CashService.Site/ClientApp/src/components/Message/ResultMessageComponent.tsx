import React from 'react';
import { ResultMessageProps } from './types';
import { ErrorMessageComponent } from './ErrorMessageComponent';
import { WarningMessageComponent } from './WarningMessageComponent';
import { SuccessMessageComponent } from './SuccessMessageComponent';

export const ResultMessageComponent: React.FC<ResultMessageProps> = ({
  type,
  isError,
  isSuccess,
  isWarning,
  message,
  className = '',
}) => {
  if (isError || type === 'error') {
    return <ErrorMessageComponent message={message} className={className} />;
  }

  if (isWarning || type === 'warning') {
    return <WarningMessageComponent message={message} className={className} />;
  }

  if (isSuccess || type === 'success') {
    return <SuccessMessageComponent message={message} className={className} />;
  }

  return null;
}; 
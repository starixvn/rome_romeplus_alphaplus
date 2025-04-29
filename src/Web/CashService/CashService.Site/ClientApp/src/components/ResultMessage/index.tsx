import React from 'react';

export type ResultType = 'error' | 'success';

interface ResultMessageProps {
  type: ResultType;
  message: string;
  className?: string;
}

const getResultStyles = (type: ResultType) => {
  const styles = {
    error: 'bg-red-50 border-red-100 text-red-500 dark:bg-red-900/10 dark:border-red-800/20 dark:text-red-400',
    success: 'bg-emerald-50 border-emerald-100 text-emerald-600 dark:bg-emerald-900/10 dark:border-emerald-800/20 dark:text-emerald-400'
  };
  return styles[type];
};

const getResultTitle = (type: ResultType) => {
  const titles = {
    error: 'Error!',
    success: 'Success!'
  };
  return titles[type];
};

const getResultIcon = (type: ResultType) => {
  if (type === 'error') {
    return (
      <svg className="w-5 h-5 mr-2" fill="currentColor" viewBox="0 0 20 20">
        <path fillRule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zM8.707 7.293a1 1 0 00-1.414 1.414L8.586 10l-1.293 1.293a1 1 0 101.414 1.414L10 11.414l1.293 1.293a1 1 0 001.414-1.414L11.414 10l1.293-1.293a1 1 0 00-1.414-1.414L10 8.586 8.707 7.293z" clipRule="evenodd" />
      </svg>
    );
  }
  return (
    <svg className="w-5 h-5 mr-2" fill="currentColor" viewBox="0 0 20 20">
      <path fillRule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z" clipRule="evenodd" />
    </svg>
  );
};

export const ResultMessage: React.FC<ResultMessageProps> = ({
  type,
  message,
  className = ''
}) => {
  return (
    <div 
      className={`${getResultStyles(type)} px-4 py-3 rounded border ${className}`}
      role="alert"
    >
      <div className="flex items-center">
        {getResultIcon(type)}
        <span className="font-semibold mr-2">{getResultTitle(type)}</span>
        {message}
      </div>
    </div>
  );
}; 
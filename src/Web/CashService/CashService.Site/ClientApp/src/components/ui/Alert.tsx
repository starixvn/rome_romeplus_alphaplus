import * as React from 'react';

export interface AlertProps {
  type?: 'success' | 'error' | 'warning' | 'info';
  message: string;
  className?: string;
  showIcon?: boolean;
  style?: React.CSSProperties;
}

const typeStyles = {
  success: 'bg-green-50 border-green-400 text-green-800',
  error: 'bg-red-50 border-red-400 text-red-800',
  warning: 'bg-yellow-50 border-yellow-400 text-yellow-800',
  info: 'bg-blue-50 border-blue-400 text-blue-800',
};

const icons = {
  success: (
    <svg className="w-5 h-5 text-green-500" fill="none" viewBox="0 0 24 24" stroke="currentColor">
      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M5 13l4 4L19 7" />
    </svg>
  ),
  error: (
    <svg className="w-5 h-5 text-red-500" fill="none" viewBox="0 0 24 24" stroke="currentColor">
      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" />
    </svg>
  ),
  warning: (
    <svg className="w-5 h-5 text-yellow-500" fill="none" viewBox="0 0 24 24" stroke="currentColor">
      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 8v4m0 4h.01" />
    </svg>
  ),
  info: (
    <svg className="w-5 h-5 text-blue-500" fill="none" viewBox="0 0 24 24" stroke="currentColor">
      <path
        strokeLinecap="round"
        strokeLinejoin="round"
        strokeWidth={2}
        d="M13 16h-1v-4h-1m1-4h.01"
      />
    </svg>
  ),
};

export const Alert: React.FC<AlertProps> = ({
  type = 'info',
  message,
  className = '',
  showIcon = true,
  style,
}) => {
  return (
    <div
      className={`flex items-start gap-2 border-l-4 rounded p-3 mb-2 ${typeStyles[type]} ${className}`}
      style={style}
      role="alert"
    >
      {showIcon && <span className="mt-0.5">{icons[type]}</span>}
      <span>{message}</span>
    </div>
  );
};

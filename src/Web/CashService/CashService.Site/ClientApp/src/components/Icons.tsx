import React from 'react';
import classNames from 'classnames';
export interface IconProps {
  className?: string;
}

export const NoRecordsIcon: React.FC<{ className?: string }> = ({ className }) => (
  <svg
    width="70"
    height="70"
    viewBox="0 0 70 70"
    fill="none"
    xmlns="http://www.w3.org/2000/svg"
    className={className}
  >
    <g opacity="0.5">
      <path
        d="M55 15H15C12.2386 15 10 17.2386 10 20V50C10 52.7614 12.2386 55 15 55H55C57.7614 55 60 52.7614 60 50V20C60 17.2386 57.7614 15 55 15Z"
        stroke="currentColor"
        strokeWidth="2"
        strokeLinecap="round"
        strokeLinejoin="round"
      />
      <circle cx="35" cy="35" r="5" stroke="currentColor" strokeWidth="2" />
      <path d="M35 35L45 45" stroke="currentColor" strokeWidth="2" strokeLinecap="round" />
      <path d="M20 26V26.01" stroke="currentColor" strokeWidth="2" strokeLinecap="round" />
      <path d="M26 26V26.01" stroke="currentColor" strokeWidth="2" strokeLinecap="round" />
      <path d="M32 26V26.01" stroke="currentColor" strokeWidth="2" strokeLinecap="round" />
    </g>
  </svg>
);

// Search Icon
export const SearchIcon: React.FC<{ className?: string }> = ({ className }) => (
  <svg
    xmlns="http://www.w3.org/2000/svg"
    width="16"
    height="16"
    viewBox="0 0 24 24"
    fill="none"
    stroke="currentColor"
    strokeWidth="2"
    strokeLinecap="round"
    strokeLinejoin="round"
    className={className ?? 'text-gray-500'}
  >
    <circle cx="11" cy="11" r="8"></circle>
    <line x1="21" y1="21" x2="16.65" y2="16.65"></line>
  </svg>
);

// Download Icon
export const DownloadIcon: React.FC<{ className?: string }> = ({ className }) => (
  <svg
    xmlns="http://www.w3.org/2000/svg"
    width="18"
    height="18"
    viewBox="0 0 24 24"
    fill="none"
    stroke="currentColor"
    strokeWidth="2"
    strokeLinecap="round"
    strokeLinejoin="round"
    className={className ?? 'text-gray-600'}
  >
    <path d="M21 15v4a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2v-4"></path>
    <polyline points="7 10 12 15 17 10"></polyline>
    <line x1="12" y1="15" x2="12" y2="3"></line>
  </svg>
);

// Copy Icon
export const CopyIcon: React.FC<{ className?: string }> = ({ className }) => (
  <svg
    xmlns="http://www.w3.org/2000/svg"
    width="18"
    height="18"
    viewBox="0 0 24 24"
    fill="none"
    stroke="currentColor"
    strokeWidth="2"
    strokeLinecap="round"
    strokeLinejoin="round"
    className={className ?? 'text-gray-600'}
  >
    <rect x="9" y="9" width="13" height="13" rx="2" ry="2"></rect>
    <path d="M5 15H4a2 2 0 0 1-2-2V4a2 2 0 0 1 2-2h9a2 2 0 0 1 2 2v1"></path>
  </svg>
);

// Edit Icon
export const IconEdit: React.FC<{ className?: string }> = ({ className }) => (
  <svg
    xmlns="http://www.w3.org/2000/svg"
    width="18"
    height="18"
    viewBox="0 0 24 24"
    fill="none"
    stroke="currentColor"
    strokeWidth="2"
    strokeLinecap="round"
    strokeLinejoin="round"
    className={className ?? 'text-gray-600'}
  >
    <rect x="9" y="9" width="13" height="13" rx="2" ry="2"></rect>
    <path d="M5 15H4a2 2 0 0 1-2-2V4a2 2 0 0 1 2-2h9a2 2 0 0 1 2 2v1"></path>
  </svg>
);

export const InfoIcon: React.FC<{ className?: string }> = ({ className }) => (
  <svg
    xmlns="http://www.w3.org/2000/svg"
    width="18"
    height="18"
    viewBox="0 0 24 24"
    fill="none"
    stroke="currentColor"
    strokeWidth="2"
    strokeLinecap="round"
    strokeLinejoin="round"
    className={className ?? 'text-gray-600'}
  >
    <rect x="9" y="9" width="13" height="13" rx="2" ry="2"></rect>
    <path d="M5 15H4a2 2 0 0 1-2-2V4a2 2 0 0 1 2-2h9a2 2 0 0 1 2 2v1"></path>
  </svg>
);

export const ChevronDownIcon: React.FC<IconProps> = ({ className = '' }) => (
  <svg className={className} fill="currentColor" viewBox="0 0 20 20">
    <path
      fillRule="evenodd"
      d="M5.293 7.293a1 1 0 011.414 0L10 10.586l3.293-3.293a1 1 0 111.414 1.414l-4 4a1 1 0 01-1.414 0l-4-4a1 1 0 010-1.414z"
      clipRule="evenodd"
    />
  </svg>
);
export const IconPen: React.FC = () => (
  <svg
    className="w-[16px] h-[16px] fill-[#163f78]"
    xmlns="http://www.w3.org/2000/svg"
    version="1.1"
    viewBox="0 0 32 32"
  >
    <path d="M0 25.2v6.8h6.8l19.6-19.8-6.8-6.6-19.6 19.6zM31.4 7.2c0.8-0.8 0.8-1.8 0-2.4l-4-4c-0.8-0.8-1.8-0.8-2.4 0l-3.2 3.2 6.8 6.8 2.8-3.6z" />
  </svg>
);
export const IconInfo: React.FC = () => (
  <svg
    className="w-[20px] h-[20px] ml-[4px] fill-[#163f78]"
    version="1.1"
    xmlns="http://www.w3.org/2000/svg"
    viewBox="0 0 32 32"
  >
    <path d="M14.667 12h2.667v-2.667h-2.667zM16 26.667c-5.88 0-10.667-4.787-10.667-10.667s4.787-10.667 10.667-10.667c5.88 0 10.667 4.787 10.667 10.667s-4.787 10.667-10.667 10.667zM16 2.667c-7.364 0-13.333 5.97-13.333 13.333s5.97 13.333 13.333 13.333c7.364 0 13.333-5.97 13.333-13.333s-5.97-13.333-13.333-13.333zM14.667 22.667h2.667v-8h-2.667v8z"></path>
  </svg>
);
export const IconClose: React.FC = () => (
  <svg
    className="w-[22px] h-[22px] fill-[#163f78]"
    version="1.1"
    xmlns="http://www.w3.org/2000/svg"
    viewBox="0 0 32 32"
  >
    <path d="M25.333 8.547l-1.88-1.88-7.453 7.453-7.453-7.453-1.88 1.88 7.453 7.453-7.453 7.453 1.88 1.88 7.453-7.453 7.453 7.453 1.88-1.88-7.453-7.453 7.453-7.453z"></path>
  </svg>
);

export const Spinner: React.FC<{ size?: 'small' | 'large'; className?: string }> = ({
  size = 'large',
  className = '',
}) => {
  const spinnerSize = size === 'small' ? 'h-5 w-5' : 'h-8 w-8';
  return (
    <svg
      className={`animate-spin text-blue-600 ${spinnerSize} ${className}`}
      xmlns="http://www.w3.org/2000/svg"
      fill="none"
      viewBox="0 0 24 24"
    >
      <circle
        className="opacity-25"
        cx="12"
        cy="12"
        r="10"
        stroke="currentColor"
        strokeWidth="4"
      ></circle>
      <path
        className="opacity-75"
        fill="currentColor"
        d="M4 12a8 8 0 018-8v4a4 4 0 00-4 4H4z"
      ></path>
    </svg>
  );
};

export const CalendarIcon: React.FC<{ className?: string }> = ({ className = '' }) => (
  <svg
    xmlns="http://www.w3.org/2000/svg"
    fill="none"
    viewBox="0 0 24 24"
    strokeWidth={1.5}
    stroke="currentColor"
    className={`w-5 h-5 ${className}`}
  >
    <path
      strokeLinecap="round"
      strokeLinejoin="round"
      d="M6.75 3v2.25M17.25 3v2.25M3.75 7.5h16.5M4.5 21h15a1.5 1.5 0 001.5-1.5V6a1.5 1.5 0 00-1.5-1.5h-15A1.5 1.5 0 003 6v13.5A1.5 1.5 0 004.5 21z"
    />
  </svg>
);
export const IconSearch: React.FC = () => (
  <svg className='w-[16px] h-[16px] fill-[#828282] ' version="1.1" xmlns="http://www.w3.org/2000/svg" width="32" height="32" viewBox="0 0 32 32">
    <path  d="M23 20.2h-1.4l-0.6-0.6c1.8-2 3-4.8 3-7.6 0-6.6-5.4-12-12-12-6.4 0-11.8 5.4-11.8 11.8 0 6.6 5.4 11.8 11.8 11.8 3 0 5.6-1 7.6-3l0.6 0.6v1.4l9.2 9.2 2.8-2.8-9.2-8.8zM12 20.2c-4.6 0-8.2-3.6-8.2-8.2s3.8-8.4 8.2-8.4 8.2 3.6 8.2 8.2-3.6 8.4-8.2 8.4z"></path>
  </svg>
);
export const IconDelete: React.FC = () => (
  <svg className='w-[16px] h-[16px] fill-[#163f78]' version="1.1" xmlns="http://www.w3.org/2000/svg" width="32" height="32" viewBox="0 0 32 32">
    <path  d="M4 10v20c0 1.1 0.9 2 2 2h18c1.1 0 2-0.9 2-2v-20h-22zM10 28h-2v-14h2v14zM14 28h-2v-14h2v14zM18 28h-2v-14h2v14zM22 28h-2v-14h2v14z"></path>
    <path  d="M26.5 4h-6.5v-2.5c0-0.825-0.675-1.5-1.5-1.5h-7c-0.825 0-1.5 0.675-1.5 1.5v2.5h-6.5c-0.825 0-1.5 0.675-1.5 1.5v2.5h26v-2.5c0-0.825-0.675-1.5-1.5-1.5zM18 4h-6v-1.975h6v1.975z"></path>
  </svg>
);
export const IconCheck  = ({ className }: { className?: string }) => (
  <svg className={classNames(className)} version="1.1" xmlns="http://www.w3.org/2000/svg" width="32" height="32" viewBox="0 0 32 32">
    <path d="M16 2.667c7.364 0 13.333 5.97 13.333 13.333s-5.97 13.333-13.333 13.333c-7.364 0-13.333-5.97-13.333-13.333s5.97-13.333 13.333-13.333zM14.667 22l9.333-9.333-1.88-1.88-7.453 7.44-4.12-4.107-1.88 1.88 6 6z"></path>
  </svg>
);
export const IconError = ({ className }: { className?: string }) => (
  <svg className={classNames(className)} version="1.1" xmlns="http://www.w3.org/2000/svg" width="32" height="32" viewBox="0 0 32 32">
    <path d="M16 2.667c7.373 0 13.333 5.96 13.333 13.333s-5.96 13.333-13.333 13.333c-7.373 0-13.333-5.96-13.333-13.333s5.96-13.333 13.333-13.333zM20.787 9.333l-4.787 4.787-4.787-4.787-1.88 1.88 4.787 4.787-4.787 4.787 1.88 1.88 4.787-4.787 4.787 4.787 1.88-1.88-4.787-4.787 4.787-4.787-1.88-1.88z"></path>
  </svg>
);
export const IconSync = ({ className }: { className?: string }) => (
  <svg className={classNames(className)} viewBox="0 0 701 534" fill="none" xmlns="http://www.w3.org/2000/svg">
    <path fill-rule="evenodd" clip-rule="evenodd" d="M624.867 228.226C625.85 239.619 635.04 248.675 646.84 248.675L688.888 248.675C698.586 248.675 704.114 259.443 698.281 266.984L601.232 391.845C592.45 403.14 574.986 403.174 566.204 391.88L468.849 266.984C462.983 259.476 468.51 248.675 478.208 248.675L517.001 248.675C530.531 248.675 541.043 236.887 539.042 223.881C524.766 132.572 434.126 65.6284 332.329 86.5047C311.916 90.6865 292.791 98.6883 275.734 109.653C257.05 121.639 231.957 116.865 219.004 99.1166L218.529 98.4249C205.27 80.2816 209 54.4011 228.058 42.0202C268.613 15.7438 317.273 0.333235 369.698 0.333237C503.912 0.333243 613.982 100.598 624.867 228.226ZM76.4662 305.773C75.4828 294.38 66.2933 285.325 54.4927 285.325L12.4449 285.325C2.74673 285.325 -2.7804 274.557 3.05206 267.016L100.101 142.155C108.884 130.86 126.347 130.826 135.13 142.12L232.484 267.016C238.35 274.524 232.823 285.325 223.125 285.325L184.332 285.325C170.802 285.325 160.291 297.113 162.291 310.119C176.567 401.428 267.207 468.371 369.004 447.495C389.417 443.313 408.542 435.311 425.599 424.346C444.283 412.361 469.376 417.135 482.33 434.883L482.804 435.575C496.063 453.718 492.333 479.599 473.276 491.98C432.72 518.256 384.06 533.667 331.636 533.667C197.422 533.667 87.3511 433.402 76.4662 305.773Z"/>
  </svg>
);
import React from 'react';

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
export const IconPen: React.FC = () => (
  <svg className='w-[16px] h-[16px] fill-[#163f78]' xmlns="http://www.w3.org/2000/svg" version="1.1" viewBox="0 0 32 32">
    <path d="M0 25.2v6.8h6.8l19.6-19.8-6.8-6.6-19.6 19.6zM31.4 7.2c0.8-0.8 0.8-1.8 0-2.4l-4-4c-0.8-0.8-1.8-0.8-2.4 0l-3.2 3.2 6.8 6.8 2.8-3.6z"/>
  </svg>
);
export const IconInfo: React.FC = () => (
  <svg className='w-[20px] h-[20px] ml-[4px] fill-[#163f78]' version="1.1" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 32 32">
    <path
      d="M14.667 12h2.667v-2.667h-2.667zM16 26.667c-5.88 0-10.667-4.787-10.667-10.667s4.787-10.667 10.667-10.667c5.88 0 10.667 4.787 10.667 10.667s-4.787 10.667-10.667 10.667zM16 2.667c-7.364 0-13.333 5.97-13.333 13.333s5.97 13.333 13.333 13.333c7.364 0 13.333-5.97 13.333-13.333s-5.97-13.333-13.333-13.333zM14.667 22.667h2.667v-8h-2.667v8z"
    ></path>
  </svg>
);
export const IconClose: React.FC = () => (
  <svg className='w-[22px] h-[22px] fill-[#163f78]' version="1.1" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 32 32">
<path  d="M25.333 8.547l-1.88-1.88-7.453 7.453-7.453-7.453-1.88 1.88 7.453 7.453-7.453 7.453 1.88 1.88 7.453-7.453 7.453 7.453 1.88-1.88-7.453-7.453 7.453-7.453z"></path>
</svg>

);
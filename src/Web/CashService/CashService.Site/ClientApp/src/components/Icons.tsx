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
    className="w-[16px] h-[16px] fill-[#0b599c] dark:fill-[#41a4ff]"
    xmlns="http://www.w3.org/2000/svg"
    version="1.1"
    viewBox="0 0 32 32"
  >
    <path d="M0 25.2v6.8h6.8l19.6-19.8-6.8-6.6-19.6 19.6zM31.4 7.2c0.8-0.8 0.8-1.8 0-2.4l-4-4c-0.8-0.8-1.8-0.8-2.4 0l-3.2 3.2 6.8 6.8 2.8-3.6z" />
  </svg>
);

export const IconClose: React.FC = () => (
  <svg
    className="w-[22px] h-[22px] fill-[#333333] dark:fill-[#ededed]"
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
  <svg className={`w-5 h-5 ${className}`} viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
    <path fillRule="evenodd" clipRule="evenodd" d="M16 2V4H8V2H6V4H5C3.89 4 3.01 4.9 3.01 6L3 20C3 21.1 3.89 22 5 22H19C20.1 22 21 21.1 21 20V6C21 4.9 20.1 4 19 4H18V2H16ZM17 13H12V18H17V13ZM5.00001 20H19V9.00001H5.00001V20Z" />
  </svg>
);
export const IconSearch: React.FC = () => (
  <svg className='w-[16px] h-[16px] fill-[#828282] ' version="1.1" xmlns="http://www.w3.org/2000/svg" width="32" height="32" viewBox="0 0 32 32">
    <path  d="M23 20.2h-1.4l-0.6-0.6c1.8-2 3-4.8 3-7.6 0-6.6-5.4-12-12-12-6.4 0-11.8 5.4-11.8 11.8 0 6.6 5.4 11.8 11.8 11.8 3 0 5.6-1 7.6-3l0.6 0.6v1.4l9.2 9.2 2.8-2.8-9.2-8.8zM12 20.2c-4.6 0-8.2-3.6-8.2-8.2s3.8-8.4 8.2-8.4 8.2 3.6 8.2 8.2-3.6 8.4-8.2 8.4z"></path>
  </svg>
);
export const IconDelete: React.FC = () => (
  <svg className='w-[16px] h-[16px] fill-[#0b599c] dark:fill-[#41a4ff]' version="1.1" xmlns="http://www.w3.org/2000/svg" width="32" height="32" viewBox="0 0 32 32">
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
export const IconBack = ({ className }: { className?: string }) => (
  <svg className={classNames(className)} xmlns="http://www.w3.org/2000/svg" xmlSpace="preserve" viewBox="0 0 18 18" width="18px" height="18px" data-v-75a2fae7="" ><path d="M5.5 9.5c-.3-.3-.3-.8 0-1.1l6-6c.3-.3.8-.3 1.1 0s.3.8 0 1.1L7.1 9l5.5 5.5c.3.3.3.8 0 1.1s-.8.3-1.1 0l-6-6.1z" ></path></svg>
);
export const IconArrowDown = ({ className }: { className?: string }) => (
  <svg className={`${className}`} viewBox="0 0 32 32" xmlns="http://www.w3.org/2000/svg">
  <path d="M15.8909 23.6457C15.3613 23.6435 14.8543 23.4313 14.4809 23.0557L4.48091 13.0557C4.15326 12.6731 3.98205 12.1809 4.00149 11.6776C4.02093 11.1742 4.2296 10.6967 4.58579 10.3406C4.94198 9.98437 5.88727 9.16523 6.39062 9.14579C6.39062 9.14579 7.00802 8.81802 7.39062 9.14567L15.8906 17.6456L24.3906 9.14559C24.7732 8.81794 25.8906 9.14569 25.8906 9.14569C26.394 9.16513 26.8399 9.98437 27.196 10.3406C27.5522 10.6967 27.7609 11.1742 27.7803 11.6776C27.7998 12.1809 27.6286 12.6731 27.3009 13.0557L17.3009 23.0557C16.9276 23.4313 16.4205 23.6435 15.8909 23.6457Z" />
  </svg>
);
export const IconInfo = ({ className }: { className?: string }) => (
  <svg
   
    className={`w-[20px] h-[20px] ml-[4px] fill-[#118AF2] ${className}`}
    version="1.1"
    xmlns="http://www.w3.org/2000/svg"
    viewBox="0 0 32 32"
  >
    <path d="M14.667 12h2.667v-2.667h-2.667zM16 26.667c-5.88 0-10.667-4.787-10.667-10.667s4.787-10.667 10.667-10.667c5.88 0 10.667 4.787 10.667 10.667s-4.787 10.667-10.667 10.667zM16 2.667c-7.364 0-13.333 5.97-13.333 13.333s5.97 13.333 13.333 13.333c7.364 0 13.333-5.97 13.333-13.333s-5.97-13.333-13.333-13.333zM14.667 22.667h2.667v-8h-2.667v8z"></path>
  </svg>
);

export const IconWarning = ({ className }: { className?: string }) => (
  <svg className={classNames(className)} version="1.1" xmlns="http://www.w3.org/2000/svg" width="32" height="32" viewBox="0 0 32 32">
    <path d="M29.013 21.227l-8.533-15.36c-1.147-2.067-2.733-3.2-4.48-3.2s-3.333 1.133-4.48 3.2l-8.533 15.36c-1.080 1.96-1.2 3.84-0.333 5.32s2.573 2.293 4.813 2.293h17.067c2.24 0 3.947-0.813 4.813-2.293s0.747-3.373-0.333-5.32zM15 12c0-0.547 0.453-1 1-1s1 0.453 1 1v6.667c0 0.547-0.453 1-1 1s-1-0.453-1-1v-6.667zM16.947 23.613c-0.067 0.053-0.133 0.107-0.2 0.16-0.080 0.053-0.16 0.093-0.24 0.12-0.080 0.040-0.16 0.067-0.253 0.080-0.080 0.013-0.173 0.027-0.253 0.027s-0.173-0.013-0.267-0.027c-0.080-0.013-0.16-0.040-0.24-0.080-0.080-0.027-0.16-0.067-0.24-0.12-0.067-0.053-0.133-0.107-0.2-0.16-0.24-0.253-0.387-0.6-0.387-0.947s0.147-0.693 0.387-0.947c0.067-0.053 0.133-0.107 0.2-0.16 0.080-0.053 0.16-0.093 0.24-0.12 0.080-0.040 0.16-0.067 0.24-0.080 0.173-0.040 0.36-0.040 0.52 0 0.093 0.013 0.173 0.040 0.253 0.080 0.080 0.027 0.16 0.067 0.24 0.12 0.067 0.053 0.133 0.107 0.2 0.16 0.24 0.253 0.387 0.6 0.387 0.947s-0.147 0.693-0.387 0.947z"></path>
  </svg>
);
export const NoDataIcon = () => (
  <svg width="114" height="109" viewBox="0 0 114 109" fill="none" xmlns="http://www.w3.org/2000/svg">
      <circle cx="60.4434" cy="54.6797" r="53.4532" fill="#B4D1E4" fill-opacity="0.3"></circle>
      <path d="M100.732 52.1567L98.4882 54.4004L96.248 52.1602L94.6601 53.7481L96.9003 55.9883L94.6601 58.2285L96.248 59.8163L98.4882 57.5761L100.728 59.8163L102.316 58.2285L100.076 55.9883L102.32 53.7445L100.732 52.1567Z" fill="#A1BCD6"></path>
      <path d="M62.9995 3.10588L60.7557 5.34965L58.5155 3.10946L56.9277 4.69731L59.1679 6.9375L56.9277 9.17769L58.5155 10.7655L60.7557 8.52535L62.9959 10.7655L64.5838 9.17769L62.3436 6.9375L64.5874 4.69373L62.9995 3.10588Z" fill="#A1BCD6"></path>
      <path d="M0.83436 32.2036C0.781745 32.0078 0.717226 31.8128 0.68043 31.6128C0.400254 30.0475 0.78537 29.488 2.58284 28.8553C2.77521 28.7867 2.97709 28.7445 3.1737 28.6917C17.1325 24.9399 31.0891 21.1887 45.0491 17.4415C45.4468 17.3346 45.8733 17.0462 46.2784 17.4536C49.1677 19.2128 57.5414 23.9527 60.6707 25.7089C61.1091 25.7986 61.1459 26.1699 61.236 26.5051C66.8779 47.4963 72.5204 68.4898 78.1601 89.4815C78.6348 91.2479 78.1347 92.0916 76.3382 92.5745C58.7287 97.3123 41.118 102.046 23.5062 106.774C21.7526 107.246 20.8929 106.77 20.4236 105.06C19.6984 102.425 1.73518 35.5732 0.83436 32.2036Z" fill="url(#paint0_radial_12052_32750)"></path>
      <path d="M24.4732 15.3458C24.4732 15.1431 24.4615 14.9381 24.4778 14.7354C24.6136 13.151 25.1307 12.7107 27.0308 12.5662C27.2344 12.5499 27.4403 12.5615 27.6439 12.5615C42.0981 12.5615 56.55 12.5615 71.0042 12.5662C71.416 12.5662 71.9027 12.3984 72.1882 12.8971C74.5218 15.3458 81.3782 22.0968 83.9444 24.6051C84.3446 24.8055 84.2837 25.1736 84.2837 25.5208C84.2837 47.2569 84.2837 68.9954 84.2814 90.7316C84.2814 92.5606 83.5794 93.2456 81.7191 93.2456C63.4834 93.2503 45.2477 93.2503 27.0121 93.2456C25.1962 93.2456 24.4895 92.5629 24.4802 90.7898C24.4638 88.0568 24.4685 18.8338 24.4732 15.3458Z" fill="url(#paint1_radial_12052_32750)"></path>
      <path d="M83.9411 24.6045C83.7492 25.0099 83.3795 24.954 83.0308 24.954C80.2883 24.954 77.5435 24.9633 74.801 24.947C74.2534 24.9424 73.6731 24.9261 73.1677 24.749C72.5312 24.5253 72 24.1012 72 23.3021C72.0024 20.0192 72 16.7363 72 13.451C72 13.246 71.9953 13.0409 72.1849 12.8965C75.1871 15.8648 78.1917 18.8309 81.1869 21.8039C82.1159 22.7266 83.0238 23.6702 83.9387 24.6045H83.9411Z" fill="url(#paint2_radial_12052_32750)"></path>
      <path d="M68.6605 78.7758L78.013 69.502L83.1091 74.6409L81.1009 76.632L97.0653 92.6252C98.5347 94.0973 98.5287 96.4831 97.0518 97.9478C95.5817 99.4057 93.2092 99.3997 91.7465 97.9343L75.7648 81.9238L73.7567 83.9148L68.6605 78.7758Z" fill="#50579B"></path>
      <path d="M55.7942 36.1328C50.0339 36.1328 44.6178 38.3767 40.5443 42.4501C36.4709 46.5236 34.227 51.9397 34.227 57.7C34.227 63.4602 36.4709 68.8763 40.5443 72.9498C44.6178 77.0233 50.0339 79.2671 55.7942 79.2671C61.5545 79.2671 66.9705 77.0233 71.044 72.9498C75.1175 68.8763 77.3613 63.4602 77.3613 57.7C77.3613 51.9397 75.1175 46.5236 71.044 42.4501C66.9705 38.3767 61.5545 36.1328 55.7942 36.1328Z" fill="url(#paint3_linear_12052_32750)"></path>
      <g filter="url(#filter0_i_12052_32750)">
          <path d="M55.795 36.132C50.0347 36.132 44.6186 38.3759 40.5451 42.4493C36.4717 46.5228 34.2278 51.9389 34.2278 57.6992C34.2278 63.4594 36.4717 68.8755 40.5451 72.949C44.6186 77.0225 50.0347 79.2663 55.795 79.2663C61.5553 79.2663 66.9713 77.0225 71.0448 72.949C75.1183 68.8755 77.3621 63.4594 77.3621 57.6992C77.3621 51.9389 75.1183 46.5228 71.0448 42.4493C66.9713 38.3759 61.5553 36.132 55.795 36.132ZM55.795 32.1328C69.915 32.1328 81.3613 43.5791 81.3613 57.6992C81.3613 71.8192 69.915 83.2655 55.795 83.2655C41.6749 83.2655 30.2286 71.8192 30.2286 57.6992C30.2286 43.5791 41.6749 32.1328 55.795 32.1328Z" fill="url(#paint4_radial_12052_32750)"></path>
      </g>
      <path d="M55.6563 62.3801C55.655 62.3813 55.653 62.3813 55.6518 62.3801C53.9761 60.7031 53.3387 58.7346 53.7264 56.4701L53.9205 55.3862C54.1324 54.3823 54.2291 53.6359 54.2106 53.1472C54.1922 52.6584 53.9961 52.2227 53.6179 51.8445C52.6657 50.8923 51.3954 51.2105 49.8069 52.799C49.0703 53.5356 48.405 54.3967 47.8198 55.3823C47.0227 56.7088 45.1214 57.1565 44.0271 56.0622L43.6958 55.7309C43.0432 55.0783 42.817 54.0976 43.2231 53.2688C44.042 51.5974 45.1831 50.0278 46.6508 48.5602C48.6652 46.5457 50.6853 45.371 52.711 45.0361C54.7367 44.7012 56.497 45.2812 57.992 46.7762C58.8285 47.6127 59.3229 48.4798 59.4619 49.3731C59.6009 50.2664 59.5403 51.4838 59.2847 53.0297C59.1218 54.0825 59.0206 54.86 58.9814 55.3798C58.9421 55.8996 59.0005 56.3751 59.1566 56.8063C59.3172 57.242 59.6066 57.6734 60.0337 58.1005C60.2917 58.3585 60.2917 58.7769 60.0337 59.0349L56.4327 62.6359C56.2902 62.7785 56.0591 62.7785 55.9166 62.6359L55.6607 62.3801C55.6595 62.3789 55.6575 62.3789 55.6563 62.3801ZM62.0598 67.7764C61.1889 67.7752 60.4331 67.4542 59.7924 66.8135C59.1517 66.1728 58.8085 65.3947 58.8162 64.5328C58.8239 63.6708 59.1561 62.9114 59.8084 62.2591C60.4606 61.6069 61.2111 61.2924 62.0953 61.2803C62.9795 61.2682 63.7442 61.5803 64.3849 62.221C65.0256 62.8617 65.3377 63.6264 65.3256 64.5106C65.309 65.3992 64.9857 66.1586 64.3468 66.7975C63.7078 67.4365 62.9262 67.782 62.0598 67.7764Z" fill="url(#paint5_radial_12052_32750)"></path>
      <defs>
          <filter id="filter0_i_12052_32750" x="30.2285" y="32.1328" width="51.1328" height="51.1328" filterUnits="userSpaceOnUse" color-interpolation-filters="sRGB">
              <feFlood flood-opacity="0" result="BackgroundImageFix"></feFlood>
              <feBlend mode="normal" in="SourceGraphic" in2="BackgroundImageFix" result="shape"></feBlend>
              <feColorMatrix in="SourceAlpha" type="matrix" values="0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 127 0" result="hardAlpha"></feColorMatrix>
              <feOffset dy="1.25772"></feOffset>
              <feComposite in2="hardAlpha" operator="arithmetic" k2="-1" k3="1"></feComposite>
              <feColorMatrix type="matrix" values="0 0 0 0 1 0 0 0 0 1 0 0 0 0 1 0 0 0 0.25 0"></feColorMatrix>
              <feBlend mode="normal" in2="shape" result="effect1_innerShadow_12052_32750"></feBlend>
          </filter>
          <radialGradient id="paint0_radial_12052_32750" cx="0" cy="0" r="1" gradientUnits="userSpaceOnUse" gradientTransform="translate(37.2971 66.8942) rotate(-29.9136) scale(52.9626)">
              <stop stop-color="#50749B"></stop>
              <stop offset="1" stop-color="#D1DFE8"></stop>
          </radialGradient>
          <radialGradient id="paint1_radial_12052_32750" cx="0" cy="0" r="1" gradientUnits="userSpaceOnUse" gradientTransform="translate(59.5926 63.3565) rotate(-18.166) scale(50.3371 50.3371)">
              <stop stop-color="#9CB6D2"></stop>
              <stop offset="1" stop-color="#C1DEF0"></stop>
          </radialGradient>
          <radialGradient id="paint2_radial_12052_32750" cx="0" cy="0" r="1" gradientUnits="userSpaceOnUse" gradientTransform="translate(79.0115 20.4899) rotate(-17.6471) scale(18.8324 14.7262)">
              <stop stop-color="#85ABC5"></stop>
              <stop offset="1" stop-color="#B7DAF1"></stop>
          </radialGradient>
          <linearGradient id="paint3_linear_12052_32750" x1="77.3667" y1="57.6993" x2="34.2324" y2="57.6993" gradientUnits="userSpaceOnUse">
              <stop stop-color="white"></stop>
              <stop offset="1" stop-color="#D4E6F9"></stop>
          </linearGradient>
          <radialGradient id="paint4_radial_12052_32750" cx="0" cy="0" r="1" gradientUnits="userSpaceOnUse" gradientTransform="translate(51.3375 64.3275) rotate(-166.332) scale(42.0751 32.6127)">
              <stop stop-color="#A5C6E9"></stop>
              <stop offset="1" stop-color="#50579B"></stop>
          </radialGradient>
          <radialGradient id="paint5_radial_12052_32750" cx="0" cy="0" r="1" gradientUnits="userSpaceOnUse" gradientTransform="translate(59.7264 58.1092) rotate(-65.5982) scale(14.0433 15.6119)">
              <stop stop-color="#A5C6E9"></stop>
              <stop offset="1" stop-color="#50579B"></stop>
          </radialGradient>
      </defs>
  </svg>
);

export const IconCopied = ({ className }: { className?: string }) => (
  <svg className={classNames(className)} xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M5 13l4 4L19 7" />
  </svg>
);
export const IconCopy = ({ className }: { className?: string }) => (
  <svg xmlns="http://www.w3.org/2000/svg" className={classNames(className)} fill="none" viewBox="0 0 24 24" stroke="currentColor">
    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M8 16H6a2 2 0 01-2-2V6a2 2 0 012-2h8a2 2 0 012 2v2m-6 12h8a2 2 0 002-2v-8a2 2 0 00-2-2h-8a2 2 0 00-2 2v8a2 2 0 002 2z" />
    </svg>
);

export const IconFirst = ({ className }: { className?: string }) => (
  <svg className={`w-[20px] h-[20px] fill-[var(--pager-color)]  sf ${className}`} version="1.1" xmlns="http://www.w3.org/2000/svg" width="32" height="32" viewBox="0 0 32 32">
    <path d="M24.547 22.12l-6.12-6.12 6.12-6.12-1.88-1.88-8 8 8 8 1.88-1.88zM8 8h2.667v16h-2.667v-16z"></path>
  </svg>
)
export const IconPrev = ({ className }: { className?: string }) => (
  <svg className={`w-[20px] h-[20px] fill-[var(--pager-color)] ${className}`} version="1.1" xmlns="http://www.w3.org/2000/svg" width="32" height="32" viewBox="0 0 32 32">
    <path d="M20.547 9.88l-1.88-1.88-8 8 8 8 1.88-1.88-6.107-6.12 6.107-6.12z"></path>
  </svg>
)
export const IconNext = ({ className }: { className?: string }) => (
  <svg className={`w-[20px] h-[20px] fill-[var(--pager-color)] ${className}`} version="1.1" xmlns="http://www.w3.org/2000/svg" width="32" height="32" viewBox="0 0 32 32">
    <path d="M13.333 8l-1.88 1.88 6.107 6.12-6.107 6.12 1.88 1.88 8-8-8-8z"></path>
  </svg>
)
export const IconLast = ({ className }: { className?: string }) => (
  <svg className={`w-[20px] h-[20px] fill-[var(--pager-color)] ${className}`} version="1.1" xmlns="http://www.w3.org/2000/svg" width="32" height="32" viewBox="0 0 32 32">
    <path d="M7.453 9.88l6.12 6.12-6.12 6.12 1.88 1.88 8-8-8-8-1.88 1.88zM21.333 8h2.667v16h-2.667v-16z"></path>
  </svg>

)
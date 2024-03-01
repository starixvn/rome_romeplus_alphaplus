import '../styles/globals.scss';
import 'react-tooltip/dist/react-tooltip.css';
import '../i18n/product-setting-info';

import React from 'react';
import ReactDOM from 'react-dom/client';

import ProductSettingInfo from '@/features/ProductSettingInfo';

ReactDOM.createRoot(document.getElementById('root') as HTMLElement).render(
  <React.StrictMode>
    <ProductSettingInfo />
  </React.StrictMode>,
);

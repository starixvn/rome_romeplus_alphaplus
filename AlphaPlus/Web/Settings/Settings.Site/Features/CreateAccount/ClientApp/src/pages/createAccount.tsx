import '../styles/globals.scss';
import 'react-tooltip/dist/react-tooltip.css';
import '../i18n/config';

import React from 'react';
import ReactDOM from 'react-dom/client';
import { RouterProvider } from 'react-router-dom';

import createAccountRouter from '@/routers/createAccountRouter';

ReactDOM.createRoot(document.getElementById('root') as HTMLElement).render(
  <React.StrictMode>
    <RouterProvider router={createAccountRouter} />
  </React.StrictMode>,
);

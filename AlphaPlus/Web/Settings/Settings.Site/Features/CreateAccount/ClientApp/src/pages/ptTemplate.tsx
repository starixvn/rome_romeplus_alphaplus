import 'react-tooltip/dist/react-tooltip.css';
import '../styles/globals.scss';
import '../i18n/pt-template';

import React from 'react';
import ReactDOM from 'react-dom/client';

import PTTemplateTable from '../features/PTTemplateTable';

ReactDOM.createRoot(document.getElementById('root') as HTMLElement).render(
  <React.StrictMode>
    <PTTemplateTable />
  </React.StrictMode>,
);

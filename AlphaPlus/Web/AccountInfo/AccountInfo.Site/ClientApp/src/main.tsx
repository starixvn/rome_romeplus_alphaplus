import 'react-tippy/dist/tippy.css';
import './index.scss';
import './i18n/config';
import 'react-virtualized/styles.css';

import React from 'react';
import ReactDOM from 'react-dom/client';

import App from './App.tsx';

ReactDOM.createRoot(document.getElementById('root')!).render(
    <React.StrictMode>
        <App />
    </React.StrictMode>
);

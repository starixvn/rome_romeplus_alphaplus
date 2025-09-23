import 'react-tippy/dist/tippy.css';
import './index.scss';
import './i18n/config';
import 'react-virtualized/styles.css';

import React from 'react';
import ReactDOM from 'react-dom/client';

import RemarkInfo from './components/RemarkInfo';

ReactDOM.createRoot(document.getElementById('root') as HTMLElement).render(
    <React.StrictMode>
        <RemarkInfo />
    </React.StrictMode>
);

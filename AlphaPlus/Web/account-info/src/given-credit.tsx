import 'react-tippy/dist/tippy.css';
import './index.scss';
import './i18n/config';
import 'react-virtualized/styles.css';

import React from 'react';
import ReactDOM from 'react-dom/client';

import GivenCredit from './components/GivenCredit';

ReactDOM.createRoot(document.getElementById('root')!).render(
    <React.StrictMode>
        <GivenCredit />
    </React.StrictMode>
);

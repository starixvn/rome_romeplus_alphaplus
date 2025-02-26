import './i18n/config';
import 'react-image-crop/dist/ReactCrop.css';
import './index.scss';

import React from 'react';
import ReactDOM from 'react-dom/client';

import ProfilePicture from './components/ProfilePicture';

ReactDOM.createRoot(document.getElementById('root')!).render(
    <React.StrictMode>
        <ProfilePicture />
    </React.StrictMode>
);

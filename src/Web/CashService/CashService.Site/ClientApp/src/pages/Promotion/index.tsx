import * as React from 'react';
import { createRoot } from 'react-dom/client';
import 'tippy.js/dist/tippy.css';
import '../../i18n/i18n';

function Promotion() {
    return(<p>Hello</p>)
}

createRoot(document.getElementById('root')!).render(
    <React.StrictMode>
        <Promotion />
    </React.StrictMode>
); 
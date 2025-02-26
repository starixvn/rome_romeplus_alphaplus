import './unsuspend-downlines.scss';
import './i18n/config';

import ReactDOM from 'react-dom/client';

import { OpenSingleDataViewModel } from './components/OpenDownlines/open-api';
import { OpenSingleContainer } from './components/OpenDownlines/open-single/open-single-container';

export const renderOpenSingle = (viewModel: OpenSingleDataViewModel) => {
    ReactDOM.createRoot(document.getElementById('container')!).render(<OpenSingleContainer {...{ viewModel }} />);
};

window.renderOpenSingle = renderOpenSingle;

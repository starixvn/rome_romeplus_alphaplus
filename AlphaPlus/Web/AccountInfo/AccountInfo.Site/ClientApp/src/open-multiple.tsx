import './unsuspend-downlines.scss';
import './i18n/config';

import ReactDOM from 'react-dom/client';

import { OpenMultipleViewModel } from './components/OpenDownlines/open-api';
import OpenMultipleContainer from './components/OpenDownlines/open-multiple/open-multiple-container';

export const renderOpenMultiple = (viewModel: OpenMultipleViewModel) => {
    ReactDOM.createRoot(document.getElementById('container')!).render(
        <OpenMultipleContainer {...{ viewModel: viewModel.Data, searchAccountRoleId: viewModel.SearchAccountRoleId }} />
    );
};

window.renderOpenMultiple = renderOpenMultiple;

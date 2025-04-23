import './unsuspend-downlines.scss';
import './i18n/config';

import ReactDOM from 'react-dom/client';

import SuspendMultipleConfirmContainer from './components/SuspendMultipleConfirm';
import { SuspendMultipleConfirmDataViewModel } from './components/SuspendMultipleConfirm/SuspendMultipleConfirmDataViewModel';

export const renderSuspendMultipleConfirm = (viewModel: SuspendMultipleConfirmDataViewModel) => {
    ReactDOM.createRoot(document.getElementById('container')!).render(
        <SuspendMultipleConfirmContainer {...{ viewModel }} />
    );
};

window.renderSuspendMultipleConfirm = renderSuspendMultipleConfirm;

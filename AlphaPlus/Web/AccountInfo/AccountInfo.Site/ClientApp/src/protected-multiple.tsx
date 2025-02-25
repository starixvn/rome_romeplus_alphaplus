import './i18n/config';
import './protected-status.scss';

import ReactDOM from 'react-dom/client';

import ProtectedMultiple from './components/ProtectedMultiple';
import { CustomerProtectedStatus } from './models/ProtectedMultiple/CustomerProtectedStatus';

export const renderProtectedMultiple = (viewModel: CustomerProtectedStatus[]) =>
    ReactDOM.createRoot(document.getElementById('container')!).render(<ProtectedMultiple viewModel={viewModel} />);

window.renderProtectedMultiple = renderProtectedMultiple;

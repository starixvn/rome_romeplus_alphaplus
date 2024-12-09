import ReactDOM from 'react-dom/client';

import UnsuspendSingleContainer from './components/UnsuspendSingle/container';
import { UnsuspendSingleDataViewModel } from './models/UnsuspenDownlines/UnsuspendSingleDataViewModel';

export const renderUnsuspendSingle = (viewModel: UnsuspendSingleDataViewModel) => {
    ReactDOM.createRoot(document.getElementById('container')!).render(<UnsuspendSingleContainer {...{ viewModel }} />);
};

window.renderUnsuspendSingle = renderUnsuspendSingle;

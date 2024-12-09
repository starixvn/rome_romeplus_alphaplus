import ReactDOM from 'react-dom/client';

import UnsuspendMultipleContainer from './components/UnsuspendMultiple/container';
import { UnsuspendMultipleViewModel } from './models/UnsuspenDownlines/UnsuspendMultipleViewModel';

export const renderUnsuspendMultiple = (viewModel: UnsuspendMultipleViewModel) => {
    ReactDOM.createRoot(document.getElementById('container')!).render(
        <UnsuspendMultipleContainer
            {...{ viewModel: viewModel.Data, searchAccountRoleId: viewModel.SearchAccountRoleId }}
        />
    );
};

window.renderUnsuspendMultiple = renderUnsuspendMultiple;

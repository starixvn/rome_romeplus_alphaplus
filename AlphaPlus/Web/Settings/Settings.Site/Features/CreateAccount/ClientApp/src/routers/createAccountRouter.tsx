import { createBrowserRouter, RouteObject } from 'react-router-dom';

import { createAccountRootUrl } from '@/constants';
import NewAccount from '@/features/NewAccount';
import ProductSetting from '@/features/NewAccount/ProductGroupSetting';
import ErrorElement from '@/features/Shared/ErrorElement';

const route: RouteObject[] = [
  {
    path: '/',
    element: <NewAccount />,
    children: [
      { index: true, element: <ProductSetting /> },
      {
        path: 'products/:productId',
        element: <ProductSetting />,
      },
    ],
    errorElement: <ErrorElement />,
  },
];

const option = {
  basename: createAccountRootUrl,
};

const createAccountRouter = createBrowserRouter(route, option);
export default createAccountRouter;

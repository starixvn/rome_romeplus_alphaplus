import { useState } from 'react';

import { UnsuspendSingleDataViewModel } from '../../models/UnsuspenDownlines/UnsuspendSingleDataViewModel';
import UnsuspendAPI from '../../services/UnsuspendAPI';
import UnsuspendViewList from '../UnsuspendViewList';
import UnsuspendSingle from './';

export default function UnsuspendSingleContainer({ viewModel }: { viewModel: UnsuspendSingleDataViewModel }) {
    const [isAffected, setIsAffected] = useState(false);
    const [showViewList, setShowViewList] = useState(false);
    const [downlineRoleId, setDownlineRoleId] = useState(1);

    const redirectViewListDetail = (isAffected: boolean, downlineRoleId: number) => {
        setShowViewList(true);
        setIsAffected(isAffected);
        setDownlineRoleId(downlineRoleId);
    };

    const redirectMainPage = () => setShowViewList(false);

    if (showViewList) {
        return (
            <UnsuspendViewList
                {...{
                    isAffected,
                    downlineRoleId,
                    searchAccountId: viewModel.SearchAccountId,
                    searchAccountName: viewModel.SearchAccountName,
                    redirectMainPage,
                    fetchCustomerList: UnsuspendAPI.fetchCustomerList
                }}
            />
        );
    }

    return <UnsuspendSingle viewModel={viewModel} redirectViewListDetail={redirectViewListDetail} />;
}

import { useState } from 'react';

import { OpenSingleDataViewModel } from '../open-api';
import OpenViewList from '../open-view-list';
import OpenSingle from './open-single';

export function OpenSingleContainer({ viewModel }: { viewModel: OpenSingleDataViewModel }) {
    const [isAffected, setIsAffected] = useState(false);
    const [showViewList, setShowViewList] = useState(false);
    const [downlineRoleId, setDownlineRoleId] = useState(1);

    const redirectViewListDetail = (isAffected: boolean, downlineRoleId: number) => {
        setShowViewList(true);
        setIsAffected(isAffected);
        setDownlineRoleId(downlineRoleId);
    };

    const redirectMainPage = () => {
        setShowViewList(false);
    };

    if (showViewList) {
        return (
            <OpenViewList
                {...{
                    isAffected,
                    downlineRoleId,
                    searchAccountId: viewModel.SearchAccountId,
                    searchAccountName: viewModel.SearchAccountName,
                    redirectMainPage: redirectMainPage
                }}
            />
        );
    }

    return (
        <OpenSingle
            {...{
                viewModel,
                redirectViewListDetail: redirectViewListDetail
            }}
        />
    );
}

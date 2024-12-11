import { cloneDeep, forEach } from 'lodash';
import { useEffect, useState } from 'react';

import { OpenMultipleModel } from '../open-api';
import OpenViewList from '../open-view-list';
import OpenMultiple from './open-multiple';

export default function OpenMultipleContainer(props: { viewModel: OpenMultipleModel[]; searchAccountRoleId: number }) {
    const [showViewList, setShowViewList] = useState(false);
    const [searchAccountId, setSearchAccountId] = useState(0);
    const [searchAccountName, setSearchAccountName] = useState('');
    const [downlineRoleId, setDownlineRoleId] = useState(1);
    const [isAffected, setIsAffected] = useState(false);
    const [viewModel, setViewModel] = useState(props.viewModel);

    useEffect(() => {
        const correctData = () => {
            forEach(props.viewModel, (dataRow) => {
                let currentLevel = dataRow.Downlines.length;
                while (currentLevel < props.searchAccountRoleId - 1) {
                    dataRow.Downlines.push({
                        SearchAccountId: 0,
                        DownlineRoleId: 0,
                        AffectedDownlines: 0,
                        UnchangedDownlines: 0
                    });

                    currentLevel++;
                }
            });
        };

        correctData();
    }, [props]);

    const handleReset = () => {
        setViewModel(props.viewModel);
    };

    const toggleCloseAll = (closeAll: boolean) => {
        const viewModelCopy = cloneDeep(viewModel);

        viewModelCopy.forEach((item) => {
            item.IsClosed = closeAll;
        });

        setViewModel(viewModelCopy);
    };

    const handleChange = (index: number, value: boolean) => {
        const viewModelCopy = cloneDeep(viewModel);

        viewModelCopy[index].IsClosed = value;

        setViewModel(viewModelCopy);
    };

    const redirectViewListDetail = (
        searchAccountId: number,
        searchAccountName: string,
        downlineRoleId: number,
        isAffected: boolean
    ) => {
        setSearchAccountId(searchAccountId);
        setSearchAccountName(searchAccountName);
        setDownlineRoleId(downlineRoleId);
        setIsAffected(isAffected);
        setShowViewList(true);
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
                    searchAccountId,
                    searchAccountName,
                    redirectMainPage: redirectMainPage
                }}
            />
        );
    }

    return (
        <OpenMultiple
            {...{
                viewModel,
                originalViewModel: props.viewModel,
                searchAccountRoleId: props.searchAccountRoleId,
                handleReset: handleReset,
                toggleCloseAll: toggleCloseAll,
                handleChange: handleChange,
                redirectViewListDetail: redirectViewListDetail
            }}
        />
    );
}

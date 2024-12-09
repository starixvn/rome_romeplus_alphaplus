import { cloneDeep, forEach } from 'lodash';
import { useEffect, useState } from 'react';

import { UnsuspendMultipleModel } from '../../models/UnsuspenDownlines/UnsuspendMultipleViewModel';
import UnsuspendViewList from '../UnsuspendViewList';
import UnsuspendMultiple from './';

export default function UnsuspendMultipleContainer(props: {
    viewModel: UnsuspendMultipleModel[];
    searchAccountRoleId: number;
}) {
    const [showViewList, setShowViewList] = useState(false);
    const [searchAccountId, setSearchAccountId] = useState(0);
    const [searchAccountName, setSearchAccountName] = useState('');
    const [downlineRoleId, setDownlineRoleId] = useState(1);
    const [isAffected, setIsAffected] = useState(false);
    const [viewModel, setViewModel] = useState(props.viewModel);

    useEffect(() => {
        const correctData = () => {
            const { viewModel, searchAccountRoleId } = props;

            forEach(viewModel, (dataRow) => {
                let currentLevel = dataRow.Downlines.length;
                while (currentLevel < searchAccountRoleId - 1) {
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

    const redirectMainPage = () => {
        setShowViewList(false);
    };

    const handleReset = () => {
        setViewModel(props.viewModel);
    };

    const toggleSuspendAll = (suspendAll: boolean) => {
        const viewModelCopy = cloneDeep(viewModel);

        viewModelCopy.forEach((item) => {
            item.Suspended = suspendAll;
        });

        setViewModel(viewModelCopy);
    };

    const handleChange = (index: number, value: boolean) => {
        const viewModelCopy = cloneDeep(viewModel);

        viewModelCopy[index].Suspended = value;
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

    if (showViewList) {
        return (
            <UnsuspendViewList
                {...{
                    isAffected,
                    downlineRoleId,
                    searchAccountId,
                    searchAccountName,
                    redirectMainPage
                }}
            />
        );
    }

    return (
        <UnsuspendMultiple
            {...{
                viewModel,
                originalViewModel: props.viewModel,
                searchAccountRoleId: props.searchAccountRoleId,
                handleReset,
                toggleSuspendAll,
                handleChange,
                redirectViewListDetail
            }}
        />
    );
}

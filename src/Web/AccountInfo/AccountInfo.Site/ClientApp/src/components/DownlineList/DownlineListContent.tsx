import classNames from 'classnames';
import { useEffect, useState } from 'react';
import { Tooltip } from 'react-tippy';
import { AutoSizer, Index, IndexRange, InfiniteLoader, List, ListRowProps, WindowScroller } from 'react-virtualized';

import { useGlobalContext } from '../../context/global-context';
import Downline from '../../models/downline';
import NoInformation from '../common/NoInformation';
import styles from './DownlineList.module.scss';

const Scale = 0.7;
const STATUS_LOADING = 1;
const STATUS_LOADED = 2;
const HEIGHT_DESKTOP = 180;
const ROW_HEIGHT_DESKTOP = 30;
const MAX_DOM_ROWS_DESKTOP = HEIGHT_DESKTOP / ROW_HEIGHT_DESKTOP;
const ROW_HEIGHT_MOBILE = ROW_HEIGHT_DESKTOP / Scale;
const COLUMNS = 2;

type Props = {
    statusId: number;
    downlineList: Downline[];
    totalPages: number;
    pageIndex: number;
    fetchDownlineListFromServer: (
        pageIndex?: number,
        reloadByStatus?: boolean,
        statusId?: number,
        totalPages?: number
    ) => void;
};

function chunkDownlineList(downlineList: Downline[]) {
    const results = [];
    while (downlineList.length) {
        results.push(downlineList.splice(0, COLUMNS));
    }
    return results;
}

export default function DownlineListContent({
    downlineList,
    fetchDownlineListFromServer,
    pageIndex,
    totalPages,
    statusId
}: Props) {
    const { mobileView, getCustomerInfo } = useGlobalContext();
    const [list, setList] = useState(chunkDownlineList([...downlineList]));
    const [loadedRowsMap, setLoadedRowsMap] = useState<{ [key: number]: number }>({});
    const timeoutIdMap: { [key: number]: boolean } = {};
    let currentPageIndex = 0;

    useEffect(() => {
        setList(chunkDownlineList([...downlineList]));
    }, [downlineList]);

    const loadMoreRows = ({ startIndex, stopIndex }: IndexRange) => {
        let promiseResolver: any;
        fectchMoreData();
        setLoadedStatus(startIndex, stopIndex, STATUS_LOADING);
        const timeOutDefault = 200;

        const timeoutId = setTimeout(() => {
            delete timeoutIdMap[timeoutId];
            setLoadedStatus(startIndex, stopIndex, STATUS_LOADED);
            promiseResolver();
        }, timeOutDefault);

        timeoutIdMap[timeoutId] = true;

        return new Promise((resolve) => {
            promiseResolver = resolve;
        });
    };

    const setLoadedStatus = (startIndex: number, stopIndex: number, loadedStatus: number) => {
        for (let i = startIndex; i <= stopIndex; i++) {
            loadedRowsMap[i] = loadedStatus;
        }

        setLoadedRowsMap(loadedRowsMap);
    };

    const fectchMoreData = () => {
        if (currentPageIndex !== pageIndex && pageIndex < totalPages) {
            currentPageIndex = pageIndex;
            fetchDownlineListFromServer(pageIndex + 1, false, statusId, totalPages);
        }
    };

    const isRowLoaded = ({ index }: Index) => {
        return !!loadedRowsMap[index];
    };

    const rowRenderer = ({ key, index, style }: ListRowProps) => {
        const content = list[index].map((item, itemIndex) => {
            const { Username } = item,
                maxLength = 13,
                isTruncated = Username.length > maxLength,
                displayedUsername = isTruncated ? `${Username.substring(0, maxLength)}...` : Username;

            const rowContent =
                loadedRowsMap[index] === STATUS_LOADED ? (
                    <a
                        href="#"
                        onClick={() => {
                            getCustomerInfo?.(Username);
                        }}
                    >
                        {displayedUsername}
                    </a>
                ) : (
                    <div className="placeholder" style={{ width: Username.length }} />
                );

            const rowContentWithTooltip = (
                <Tooltip {...{ title: Username, position: 'bottom', trigger: 'mouseenter', arrow: true }}>
                    {rowContent}
                </Tooltip>
            );

            return (
                <div className="col-xs-6" key={itemIndex}>
                    {isTruncated ? rowContentWithTooltip : rowContent}
                </div>
            );
        });

        return <div {...{ style, key, className: 'col-xs-12' }}>{content}</div>;
    };

    const buildInfiniteLoader = (extraProps: any, mobileView = false) => {
        const length = list.length;

        return (
            <InfiniteLoader isRowLoaded={isRowLoaded} loadMoreRows={loadMoreRows} rowCount={length}>
                {({ onRowsRendered, registerChild }) => (
                    <AutoSizer>
                        {({ width }) => (
                            <List
                                ref={registerChild}
                                autoHeight={mobileView}
                                {...{
                                    onRowsRendered,
                                    width,
                                    rowCount: length,
                                    rowRenderer: rowRenderer
                                }}
                                {...extraProps}
                            />
                        )}
                    </AutoSizer>
                )}
            </InfiniteLoader>
        );
    };

    if (list.length === 0) {
        return <NoInformation />;
    }

    if (mobileView) {
        return (
            <WindowScroller key={statusId}>
                {({ height, isScrolling, scrollTop }) => {
                    return buildInfiniteLoader(
                        {
                            rowHeight: ROW_HEIGHT_MOBILE,
                            className: 'customer-list',
                            height,
                            isScrolling,
                            scrollTop
                        },
                        true
                    );
                }}
            </WindowScroller>
        );
    }

    return (
        <div className={classNames('col-xs-12', styles['customer-list'])} key={statusId}>
            {buildInfiniteLoader({
                rowHeight: ROW_HEIGHT_DESKTOP,
                height: (list.length > MAX_DOM_ROWS_DESKTOP ? MAX_DOM_ROWS_DESKTOP : list.length) * ROW_HEIGHT_DESKTOP,
                className: 'col-xs-12'
            })}
        </div>
    );
}

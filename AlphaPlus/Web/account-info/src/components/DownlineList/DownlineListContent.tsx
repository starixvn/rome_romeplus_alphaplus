import { useState } from 'react';
import { AutoSizer, Index, InfiniteLoader, List, WindowScroller } from 'react-virtualized';

import { useGlobalContext } from '../../context/global-context';
import Downline from '../../models/downline';

const Scale = 0.7;
const STATUS_LOADING = 1;
const STATUS_LOADED = 2;
const HEIGHT_DESKTOP = 240;
const ROW_HEIGHT_DESKTOP = 30;
const MAX_DOM_ROWS_DESKTOP = HEIGHT_DESKTOP / ROW_HEIGHT_DESKTOP;
const ROW_HEIGHT_MOBILE = ROW_HEIGHT_DESKTOP / Scale;
const COLUMNS = 2;

type Props = {
    statusId: number;
    downlineList: Downline[];
};

const chunkDownlineList = (downlineList: Downline[]) => {
    const results = [];
    while (downlineList.length) {
        results.push(downlineList.splice(0, COLUMNS));
    }
    return results;
};

export default function DownlineListContent({ statusId, downlineList }: Props) {
    const { mobileView } = useGlobalContext();
    const [list, setList] = useState(chunkDownlineList([...downlineList]));
    const [loadedRowsMap, setLoadedRowsMap] = useState({});

    const isRowLoaded = ({ index }: Index) => {
        return !!loadedRowsMap[index];
    };

    const buildInfiniteLoader = (extraProps, autoHeight = false) => {
        const length = list.length;

        return (
            <InfiniteLoader isRowLoaded={isRowLoaded} loadMoreRows={this.loadMoreRows} rowCount={length}>
                {({ onRowsRendered, registerChild }) => (
                    <AutoSizer>
                        {({ width }) => (
                            <List
                                ref={registerChild}
                                autoHeight={autoHeight}
                                {...{
                                    onRowsRendered,
                                    width,
                                    rowCount: length,
                                    rowRenderer: this.rowRenderer
                                }}
                                {...extraProps}
                            />
                        )}
                    </AutoSizer>
                )}
            </InfiniteLoader>
        );
    };

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
        <div className="col-xs-12 customer-list" key={statusId}>
            {buildInfiniteLoader({
                rowHeight: ROW_HEIGHT_DESKTOP,
                height: (length > MAX_DOM_ROWS_DESKTOP ? MAX_DOM_ROWS_DESKTOP : length) * ROW_HEIGHT_DESKTOP,
                className: 'col-xs-12'
            })}
        </div>
    );
}

import React from 'react';

import { BlockIds } from '../constants';

export default interface ContentItem {
    blockId: BlockIds;
    featureText: string;
    iconClass: string;
    pathname: string;
    visible: boolean;
    children: React.ReactNode;
    fetchDataFromServer?: (...arg: any[]) => void;
}

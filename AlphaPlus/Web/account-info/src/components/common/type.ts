import { BlockIds } from '../../constants';

export type BlockProps = {
    blockId: BlockIds;
    onClick: (blockId: BlockIds) => void;
};

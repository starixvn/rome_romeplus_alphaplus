import { Tooltip } from 'react-tippy';

import { useGlobalContext } from '../../context/global-context';
import styles from './CustomerInfo.module.scss';

type Props = {
    username: string;
};

export default function UplineInfo({ username }: Props) {
    const { getCustomerInfo } = useGlobalContext();
    const maxLength = 15;
    const isTruncated = username && username.length > maxLength;

    const content = (
        <span
            id="upline-info"
            className={styles.enabled}
            onClick={() => {
                getCustomerInfo?.(username);
            }}
        >
            {isTruncated ? `${username.substring(0, maxLength)}...` : username}
        </span>
    );

    const contentWithTooltip = (
        <Tooltip
            {...{
                title: username,
                position: 'bottom',
                trigger: 'mouseenter',
                arrow: true
            }}
        >
            {content}
        </Tooltip>
    );

    return isTruncated ? contentWithTooltip : content;
}

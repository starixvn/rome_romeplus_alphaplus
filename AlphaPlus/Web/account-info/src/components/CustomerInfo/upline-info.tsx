import { Tooltip } from 'react-tippy';

import { useGlobalContext } from '../../context/global-context';

export default function UplineInfo() {
    const { username } = useGlobalContext();
    const maxLength = 15;
    const isTruncated = username && username.length > maxLength;

    const content = (
        <span
            id="upline-info"
            className="enabled"
            // onClick={() => {
            //     fetchCustomerInfoFromServer(username);
            // }}
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

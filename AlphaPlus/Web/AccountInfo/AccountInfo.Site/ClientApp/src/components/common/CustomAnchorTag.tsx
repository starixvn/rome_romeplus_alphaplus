import React from 'react';

import { useGlobalContext } from '../../context/global-context';
import { closeLeftPanelOnMobile } from '../../helpers';

type Props = {
    children: React.ReactNode;
    title?: string;
    classNames?: string;
    href: string;
};

export default function CustomAnchorTag({ title = undefined, classNames = '', children, href }: Props) {
    const { mobileView } = useGlobalContext();
    const handleOnclick = (e: React.MouseEvent<HTMLAnchorElement>) => {
        e.preventDefault();

        if (mobileView) {
            closeLeftPanelOnMobile();
        }

        const mainFrame = window.top?.$('#main');
        // if (!window.top?.alpha.core.isIe()) {
        //     try {
        //         mainFrame?.contents().find('head').append($('#AssetCommonCssLink').val());
        //         mainFrame?.contents().find('body').empty().block();
        //     } catch (e) {}
        // }

        mainFrame?.attr('src', href);
    };

    return (
        <a
            {...{
                title,
                href: '#',
                target: 'main',
                className: classNames,
                onClick: handleOnclick
            }}
        >
            {children}
        </a>
    );
}

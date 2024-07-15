import React from 'react';

type Props = {
    children: React.ReactNode;
    title?: string;
    classNames?: string;
    href: string;
};

// TODO: CustomAnchorTag
export default function CustomAnchorTag({ title = undefined, classNames = '', children, href }: Props) {
    const handleOnclick = (e: React.MouseEvent<HTMLAnchorElement>) => {
        e.preventDefault();

        const mainFrame = window.top?.$('#main');
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

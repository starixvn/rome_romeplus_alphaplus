import React from 'react';

type Props = {
    children: React.ReactNode;
    title?: string;
    classNames?: string;
    href?: string;
};

export default function CustomAnchorTag({ title = undefined, classNames = '', children }: Props) {
    const handleOnclick = (e: React.MouseEvent<HTMLAnchorElement>) => {
        e.preventDefault();
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

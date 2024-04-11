export const buildTooltipProps = (tooltip?: string) => {
    return {
        title: tooltip ?? '',
        position: 'bottom',
        trigger: 'mouseenter',
        arrow: true
    };
};

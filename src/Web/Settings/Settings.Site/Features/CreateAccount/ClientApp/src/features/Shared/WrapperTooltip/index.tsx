import classNames from 'classnames';
import { ITooltip, Tooltip } from 'react-tooltip';

import styles from './WrapperTooltip.module.scss';

type Type = 'error' | 'warning' | 'success' | 'default';

interface IProps extends ITooltip {
  type?: Type;
}
export default function WrapperTooltip({ children, type, ...rest }: IProps) {
  const isOpening = type === 'error' && rest.isOpen;
  return (
    // eslint-disable-next-line react/jsx-props-no-spreading
    <Tooltip className={classNames(styles.tooltip, styles[`${type}`], { errTooltipOpen: isOpening })} {...rest}>
      {children}
    </Tooltip>
  );
}

WrapperTooltip.defaultProps = {
  type: 'default',
};

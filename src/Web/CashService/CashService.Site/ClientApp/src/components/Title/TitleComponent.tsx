import React, { useMemo } from 'react';
import Tippy from '@tippyjs/react';
import { IconInfo } from '../Icons';
import { TitleComponentProps } from './types';
import { validateTitleType, getTitleClasses, isValidText } from './utils';
import './TitleComponents.scss';

interface TitleStyles {
  container: string;
  element: 'h1' | 'h2' | 'h3' | 'label';
}

export const TitleComponent: React.FC<TitleComponentProps> = ({
  type = 'main',
  text,
  className = '',
  required = false,
  showTippy = false,
  children,
  classNameTippy,
}) => {
  // Validate and get safe type
  const safeType = validateTitleType(type);

  const styles = useMemo((): TitleStyles => {
    switch (safeType) {
      case 'main':
        return {
          container: getTitleClasses(safeType, className),
          element: 'h2',
        };
      case 'medium':
        return {
          container: getTitleClasses(safeType, className),
          element: 'h3',
        };
      case 'normal':
        return {
          container: getTitleClasses(safeType, className),
          element: 'h3',
        };
      case 'label':
        return {
          container: getTitleClasses(safeType, className),
          element: 'label',
        };
      default:
        return {
          container: getTitleClasses('main', className),
          element: 'h2',
        };
    }
  }, [safeType, className]);

  // Handle empty or null text gracefully
  if (!isValidText(text)) {
    return null;
  }

  const renderRequired = () => {
    if (!required) return null;
    return <span className="text-[#FF0000] dark:text-[#FF4F50] ml-[4px]">*</span>;
  };

  const renderTippy = () => {
    if (!showTippy || !children) return null;
    return (
      <Tippy
        content={children}
        placement="top"
        className={classNameTippy}
        theme="light"
        arrow={true}
      >
        <span>
          <IconInfo className="cursor-pointer ml-[4px] " />
        </span>
      </Tippy>
    );
  };

  const Element = styles.element;

  return (
    <Element className={styles.container}>
      {text}
      {renderRequired()}
      {renderTippy()}
    </Element>
  );
};

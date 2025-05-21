import React from 'react';
import { components, OptionProps } from 'react-select';

interface OptionType {
  value: string;
  label: string;
  disabled?: boolean;
}

export const CheckboxOption = (props: OptionProps<OptionType, true>) => {
  const { children, isSelected, isDisabled } = props;
  return (
    <components.Option {...props}>
      <div
        style={{
          display: 'flex',
          alignItems: 'center',
          cursor: 'pointer',
          opacity: isDisabled ? 0.5 : 1,
        }}
      >
        <input
          type="checkbox"
          checked={isSelected}
          disabled={isDisabled}
          onChange={() => null}
          style={{ marginRight: '8px', pointerEvents: 'none' }}
        />
        {children}
      </div>
    </components.Option>
  );
};

import React, { useMemo, useCallback } from 'react';
import Select, { MultiValue, ActionMeta, StylesConfig, components } from 'react-select';
import { CheckboxOption } from './Multiselect/CheckboxOption';
import { CustomValueContainer } from './Multiselect/CustomValueContainer';

interface OptionType {
  value: string;
  label: string;
  disabled?: boolean;
}

interface MultiSelectProps {
  options: OptionType[];
  isCheckAll?: boolean;
  isSearchable?: boolean;
  onChange: (selectedValues: string[]) => void;
  placeholder?: string;
  className?: string;
  initialValues?: string[];
  styles?: StylesConfig<OptionType, true>;
  onMenuClose?: () => void;
}

const ALL_OPTION_VALUE = '__ALL__';

const allOption: OptionType = { value: ALL_OPTION_VALUE, label: 'All', disabled: false };

const MultiSelect: React.FC<MultiSelectProps> = ({
  options,
  isCheckAll = false,
  isSearchable = true,
  onChange,
  placeholder = 'Select...',
  className,
  initialValues = [],
  styles,
  onMenuClose,
}) => {
  const componentOptions = useMemo(() => {
    return isCheckAll ? [allOption, ...options] : options;
  }, [options, isCheckAll]);

  const enabledOptions = useMemo(
    () => componentOptions.filter((opt) => !opt.disabled),
    [componentOptions]
  );

  const enabledOriginalOptions = useMemo(() => options.filter((opt) => !opt.disabled), [options]);

  const getOptionsFromValues = useCallback(
    (values: string[]): OptionType[] => {
      const allEnabledOriginalOptionsSelected =
        isCheckAll &&
        enabledOriginalOptions.length > 0 &&
        enabledOriginalOptions.every((opt) => values.includes(opt.value));

      const effectiveValues = allEnabledOriginalOptionsSelected
        ? [...values, ALL_OPTION_VALUE]
        : values;

      return componentOptions.filter((opt) => effectiveValues.includes(opt.value));
    },
    [isCheckAll, enabledOriginalOptions, componentOptions]
  );

  const currentValue = useMemo(() => {
    return getOptionsFromValues(initialValues);
  }, [initialValues, getOptionsFromValues]);

  const handleChange = useCallback(
    (selected: MultiValue<OptionType>, actionMeta: ActionMeta<OptionType>) => {
      let newSelectedOptions: readonly OptionType[] = selected;

      if (isCheckAll) {
        const isAllSelected = selected.some((opt) => opt.value === ALL_OPTION_VALUE);
        const wasAllPreviouslySelected = currentValue.some((opt) => opt.value === ALL_OPTION_VALUE);

        if (
          actionMeta.action === 'select-option' &&
          actionMeta.option?.value === ALL_OPTION_VALUE
        ) {
          newSelectedOptions = enabledOptions;
        } else if (
          actionMeta.action === 'deselect-option' &&
          actionMeta.option?.value === ALL_OPTION_VALUE
        ) {
          newSelectedOptions = [];
        } else if (actionMeta.action === 'deselect-option' && wasAllPreviouslySelected) {
          newSelectedOptions = selected.filter((opt) => opt.value !== ALL_OPTION_VALUE);
        } else if (
          actionMeta.action === 'select-option' &&
          !isAllSelected &&
          !actionMeta.option?.disabled
        ) {
          const currentSelectedEnabledOriginalCount = selected.filter(
            (opt) => opt.value !== ALL_OPTION_VALUE && !opt.disabled
          ).length;

          if (
            enabledOriginalOptions.length > 0 &&
            currentSelectedEnabledOriginalCount === enabledOriginalOptions.length
          ) {
            newSelectedOptions = enabledOptions;
          }
        } else if (actionMeta.action === 'clear') {
          newSelectedOptions = [];
        } else if (actionMeta.action === 'remove-value') {
          if (actionMeta.removedValue?.value === ALL_OPTION_VALUE) {
            newSelectedOptions = [];
          } else if (wasAllPreviouslySelected) {
            newSelectedOptions = selected.filter((opt) => opt.value !== ALL_OPTION_VALUE);
          }
          const currentSelectedEnabledOriginalCount = newSelectedOptions.filter(
            (opt) => opt.value !== ALL_OPTION_VALUE && !opt.disabled
          ).length;
          if (
            isCheckAll &&
            enabledOriginalOptions.length > 0 &&
            currentSelectedEnabledOriginalCount === enabledOriginalOptions.length
          ) {
            newSelectedOptions = enabledOptions;
          }
        }
      }

      const finalSelectedOptions = newSelectedOptions.filter(
        (opt) =>
          !opt.disabled || initialValues.includes(opt.value) || opt.value === ALL_OPTION_VALUE
      );

      const outputValues = finalSelectedOptions
        .map((option) => option.value)
        .filter((value) => value !== ALL_OPTION_VALUE);
      onChange(outputValues);
    },
    [isCheckAll, enabledOptions, enabledOriginalOptions, currentValue, initialValues, onChange]
  );

  const defaultStyles: StylesConfig<OptionType, true> = {
    option: (base, state) => ({
      ...base,
      backgroundColor: state.isSelected ? 'transparent' : base.backgroundColor,
      color: state.isDisabled
        ? '#aaa'
        : state.isSelected
          ? state.isFocused
            ? '#333'
            : '#555'
          : base.color,
      paddingLeft: '10px',
      paddingRight: '10px',
      ':active': {
        backgroundColor: state.isSelected
          ? 'transparent'
          : state.isDisabled
            ? base.backgroundColor
            : '#eee',
      },
    }),
    control: (base) => ({
      ...base,
      minHeight: '32px',
      maxHeight: '32px',
      cursor: 'pointer',
      paddingTop: '7px',
      '@media (max-width: 768px)': {
        minHeight: '40px',
        maxHeight: '40px',
      },
    }),
    menu: (base) => ({
      ...base,
      zIndex: 100,
      marginTop: '0',
    }),
    valueContainer: (base) => ({
      ...base,
      padding: '0 8px',
      cursor: 'pointer',
    }),
    dropdownIndicator: (base) => ({
      ...base,
      cursor: 'pointer',
      padding: '0px 4px',
      '& svg': {
        width: '16px',
        height: '16px',
      },
    }),
    clearIndicator: (base) => ({
      ...base,
      cursor: 'pointer',
      padding: '0px 4px',
      '& svg': {
        width: '16px',
        height: '16px',
      },
    }),
    input: (base) => ({
      ...base,
      margin: 0,
      padding: 0,
    }),
  };

  const combinedStyles = { ...defaultStyles, ...styles };

  return (
    <Select<OptionType, true>
      isMulti
      options={componentOptions}
      value={currentValue}
      onChange={handleChange}
      isSearchable={isSearchable}
      placeholder={placeholder}
      closeMenuOnSelect={false}
      hideSelectedOptions={false}
      className={`react-select-container ${className || ''}`}
      classNamePrefix="react-select"
      components={{
        Option: CheckboxOption,
        ValueContainer: CustomValueContainer,
        MultiValue: () => null,
        IndicatorSeparator: () => null,
        Control: (props) => <components.Control {...props} />,
      }}
      styles={combinedStyles}
      isOptionDisabled={(option) => !!option.disabled}
      onMenuClose={onMenuClose}
    />
  );
};

export default MultiSelect;

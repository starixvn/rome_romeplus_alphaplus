import React, { useState, useMemo, useEffect } from 'react';
import Select, {
  MultiValue,
  ActionMeta,
  StylesConfig,
  components,
  OptionProps,
  ValueContainerProps,
} from 'react-select';

interface OptionType {
  value: string;
  label: string;
  disabled?: boolean;
}

const ALL_OPTION_VALUE = '__ALL__';

const allOption: OptionType = { value: ALL_OPTION_VALUE, label: 'All', disabled: false };

const CheckboxOption = (props: OptionProps<OptionType, true>) => {
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

const CustomValueContainer = (props: ValueContainerProps<OptionType, true>) => {
  const { children, getValue, hasValue, selectProps } = props;
  const selectedOptions = getValue();

  const childrenToRender = React.Children.toArray(children).filter((child) => {
    if (!React.isValidElement(child)) return false;
    const typeName = (child.type as any)?.name;
    return typeName === 'Input' || typeName === 'Placeholder';
  });

  const allComponentOptions = useMemo(() => selectProps.options || [], [selectProps.options]);

  const isCheckAllEnabled = useMemo(
    () => allComponentOptions.some((opt) => 'value' in opt && opt.value === ALL_OPTION_VALUE),
    [allComponentOptions]
  );

  const originalOptions = useMemo<OptionType[]>(
    () =>
      allComponentOptions.flatMap((optOrGroup) => {
        if ('options' in optOrGroup) {
          return optOrGroup.options;
        } else if ('value' in optOrGroup && optOrGroup.value !== ALL_OPTION_VALUE) {
          return [optOrGroup];
        }
        return [];
      }),
    [allComponentOptions]
  );

  const enabledOriginalOptions = useMemo(
    () => originalOptions.filter((opt) => !opt.disabled),
    [originalOptions]
  );

  const nonAllSelectedCount = selectedOptions.filter(
    (opt) => opt.value !== ALL_OPTION_VALUE
  ).length;

  const selectedEnabledOriginalCount = selectedOptions.filter(
    (opt) =>
      opt.value !== ALL_OPTION_VALUE &&
      !opt.disabled &&
      originalOptions.some((o) => o.value === opt.value)
  ).length;

  let valueText: string | null = null;

  if (hasValue) {
    if (
      isCheckAllEnabled &&
      enabledOriginalOptions.length > 0 &&
      selectedEnabledOriginalCount === enabledOriginalOptions.length
    ) {
      valueText = 'All Selected';
    } else if (nonAllSelectedCount > 0) {
      valueText = `${nonAllSelectedCount} selected`;
    }
  }

  return (
    <components.ValueContainer {...props}>
      {childrenToRender}
      {hasValue && valueText && (
        <div
          style={{
            position: 'relative',
            top: '5px',
            transform: 'translateY(-50%)',
            marginLeft: '2px',
            marginRight: '2px',
            maxWidth: 'calc(100% - 14px)',
            overflow: 'hidden',
            textOverflow: 'ellipsis',
            whiteSpace: 'nowrap',
            boxSizing: 'border-box',
          }}
        >
          {valueText}
        </div>
      )}
    </components.ValueContainer>
  );
};

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
  const [isOpen, setIsOpen] = useState(false);

  const componentOptions = useMemo(() => {
    return isCheckAll ? [allOption, ...options] : options;
  }, [options, isCheckAll]);

  const enabledOptions = useMemo(
    () => componentOptions.filter((opt) => !opt.disabled),
    [componentOptions]
  );

  const enabledOriginalOptions = useMemo(() => options.filter((opt) => !opt.disabled), [options]);

  const getOptionsFromValues = (values: string[]): OptionType[] => {
    const allEnabledOriginalOptionsSelected =
      isCheckAll &&
      enabledOriginalOptions.length > 0 &&
      enabledOriginalOptions.every((opt) => values.includes(opt.value));

    const effectiveValues = allEnabledOriginalOptionsSelected
      ? [...values, ALL_OPTION_VALUE]
      : values;

    return componentOptions.filter((opt) => effectiveValues.includes(opt.value));
  };

  const [selectedOptions, setSelectedOptions] = useState<readonly OptionType[]>(() =>
    getOptionsFromValues(initialValues)
  );

  useEffect(() => {
    setSelectedOptions(getOptionsFromValues(initialValues));
  }, [initialValues, componentOptions, enabledOriginalOptions.length]);

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      const target = event.target as HTMLElement;
      const menuElement = document.querySelector('.react-select__menu');
      const controlElement = document.querySelector('.react-select__control');

      if (controlElement?.contains(target)) {
        return;
      }

      if (menuElement && !menuElement.contains(target) && isOpen) {
        setIsOpen(false);
        onMenuClose?.();
      }
    };

    document.addEventListener('mousedown', handleClickOutside);
    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
    };
  }, [isOpen, onMenuClose]);

  const handleMenuOpen = () => {
    setIsOpen(true);
  };

  const handleMenuClose = () => {
    setIsOpen(false);
    onMenuClose?.();
  };

  const handleChange = (selected: MultiValue<OptionType>, actionMeta: ActionMeta<OptionType>) => {
    let newSelectedOptions: readonly OptionType[] = selected;

    if (isCheckAll) {
      const isAllSelected = selected.some((opt) => opt.value === ALL_OPTION_VALUE);
      const wasAllPreviouslySelected = selectedOptions.some(
        (opt) => opt.value === ALL_OPTION_VALUE
      );

      if (actionMeta.action === 'select-option' && actionMeta.option?.value === ALL_OPTION_VALUE) {
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
      (opt) => !opt.disabled || initialValues.includes(opt.value) || opt.value === ALL_OPTION_VALUE
    );

    setSelectedOptions(finalSelectedOptions);

    const outputValues = finalSelectedOptions
      .map((option) => option.value)
      .filter((value) => value !== ALL_OPTION_VALUE);
    onChange(outputValues);
  };

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
      '@media (max-width: 768px)': {
        minHeight: '40px',
        maxHeight: '40px',
      },
    }),
    menu: (base) => ({
      ...base,
      zIndex: 100,
    }),
    valueContainer: (base) => ({
      ...base,
      padding: '0 8px',
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
      value={selectedOptions}
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
      }}
      styles={combinedStyles}
      isOptionDisabled={(option) => !!option.disabled}
      onMenuOpen={handleMenuOpen}
      onMenuClose={handleMenuClose}
      menuIsOpen={isOpen}
    />
  );
};

export default MultiSelect;

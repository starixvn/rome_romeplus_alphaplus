import React, { useMemo } from 'react';
import { useTranslation } from 'react-i18next';
import { components, ValueContainerProps } from 'react-select';

interface OptionType {
  value: string;
  label: string;
  disabled?: boolean;
}

const ALL_OPTION_VALUE = '__ALL__';

export const CustomValueContainer = (props: ValueContainerProps<OptionType, true>) => {
  const { children, getValue, hasValue, selectProps } = props;
  const selectedOptions = getValue();
  const { t } = useTranslation('global');

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
      valueText = t('allSelected');
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

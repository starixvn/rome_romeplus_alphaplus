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

// --- Component ValueContainer tùy chỉnh (giữ nguyên) ---
const CustomValueContainer = (props: ValueContainerProps<OptionType, true>) => {
  const { children, getValue, hasValue, selectProps } = props;
  const selectedOptions = getValue();

  const childrenToRender = React.Children.toArray(children).filter((child) => {
    if (!React.isValidElement(child)) return false;
    const typeName = (child.type as any)?.name;
    return typeName === 'Input' || typeName === 'Placeholder';
  });

  // --- Sửa lỗi TypeScript bằng cách kiểm tra kiểu ---
  const allComponentOptions = useMemo(() => selectProps.options || [], [selectProps.options]);

  // Kiểm tra xem option "All" có tồn tại trong danh sách không (chỉ kiểm tra ở cấp cao nhất)
  const isCheckAllEnabled = useMemo(
    () => allComponentOptions.some((opt) => 'value' in opt && opt.value === ALL_OPTION_VALUE),
    [allComponentOptions]
  );

  // Tính toán danh sách phẳng các options gốc (không bao gồm "All" và loại bỏ cấu trúc group)
  const originalOptions = useMemo<OptionType[]>(
    () =>
      allComponentOptions.flatMap((optOrGroup) => {
        if ('options' in optOrGroup) {
          // Nếu là group, trả về mảng options con của nó
          return optOrGroup.options;
        } else if ('value' in optOrGroup && optOrGroup.value !== ALL_OPTION_VALUE) {
          // Nếu là option thường và không phải "All", trả về mảng chứa chính nó
          return [optOrGroup];
        }
        // Bỏ qua các trường hợp khác (vd: option "All" ở cấp cao nhất, hoặc kiểu không mong muốn)
        return [];
      }),
    [allComponentOptions]
  );

  // Lọc ra các options gốc có thể chọn (không bị disabled)
  const enabledOriginalOptions = useMemo(
    () => originalOptions.filter((opt) => !opt.disabled),
    [originalOptions]
  );
  // --- Kết thúc sửa lỗi ---

  // Đếm số lượng option thực tế đã chọn (không tính "All")
  const nonAllSelectedCount = selectedOptions.filter(
    (opt) => opt.value !== ALL_OPTION_VALUE
  ).length;

  // Đếm số lượng option gốc *đã chọn* và *có thể chọn*
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
            top: '12px',
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

// --- Component chính ---
interface MultiSelectProps {
  options: OptionType[]; // options gốc, chưa bao gồm "All"
  isCheckAll?: boolean;
  isSearchable?: boolean;
  onChange: (selectedValues: string[]) => void;
  placeholder?: string;
  className?: string;
  initialValues?: string[];
  styles?: StylesConfig<OptionType, true>;
}

const MultiSelect: React.FC<MultiSelectProps> = ({
  options, // Nhận vào options gốc (chưa có "All")
  isCheckAll = false,
  isSearchable = true,
  onChange,
  placeholder = 'Select...',
  className,
  initialValues = [],
  styles,
}) => {
  // Chỉ thêm option "All" nếu isCheckAll là true
  const componentOptions = useMemo(() => {
    return isCheckAll ? [allOption, ...options] : options;
  }, [options, isCheckAll]);

  // Lấy danh sách các options có thể chọn (không bị disabled) từ componentOptions
  // Bao gồm cả "All" nếu isCheckAll là true
  const enabledOptions = useMemo(
    () => componentOptions.filter((opt) => !opt.disabled),
    [componentOptions]
  );

  // Lấy danh sách các options gốc có thể chọn (không bao gồm "All")
  const enabledOriginalOptions = useMemo(() => options.filter((opt) => !opt.disabled), [options]);

  // --- Cập nhật getOptionsFromValues ---
  const getOptionsFromValues = (values: string[]): OptionType[] => {
    // Kiểm tra xem tất cả các option gốc *có thể chọn* có nằm trong `values` không
    const allEnabledOriginalOptionsSelected =
      isCheckAll &&
      enabledOriginalOptions.length > 0 &&
      enabledOriginalOptions.every((opt) => values.includes(opt.value));

    // Bao gồm ALL_OPTION_VALUE nếu tất cả các option gốc có thể chọn được chọn
    const effectiveValues = allEnabledOriginalOptionsSelected
      ? [...values, ALL_OPTION_VALUE]
      : values;

    // Lọc từ componentOptions (có thể chứa cả option disabled)
    // để giữ lại các option có value trong effectiveValues
    // Điều này bảo tồn các lựa chọn ban đầu của các option bị disabled
    return componentOptions.filter((opt) => effectiveValues.includes(opt.value));
  };

  const [selectedOptions, setSelectedOptions] = useState<readonly OptionType[]>(() =>
    getOptionsFromValues(initialValues)
  );

  useEffect(() => {
    setSelectedOptions(getOptionsFromValues(initialValues));
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [initialValues, componentOptions, enabledOriginalOptions.length]); // Thêm dependency

  // --- Cập nhật handleChange ---
  const handleChange = (selected: MultiValue<OptionType>, actionMeta: ActionMeta<OptionType>) => {
    let newSelectedOptions: readonly OptionType[] = selected;

    if (isCheckAll) {
      const isAllSelected = selected.some((opt) => opt.value === ALL_OPTION_VALUE);
      const wasAllPreviouslySelected = selectedOptions.some(
        (opt) => opt.value === ALL_OPTION_VALUE
      );

      // 1. Hành động: Chọn option "All"
      if (actionMeta.action === 'select-option' && actionMeta.option?.value === ALL_OPTION_VALUE) {
        // Chọn "All" và tất cả các option *không bị disabled*
        newSelectedOptions = enabledOptions;
      }
      // 2. Hành động: Bỏ chọn option "All"
      else if (
        actionMeta.action === 'deselect-option' &&
        actionMeta.option?.value === ALL_OPTION_VALUE
      ) {
        // Bỏ chọn tất cả
        newSelectedOptions = [];
      }
      // 3. Hành động: Bỏ chọn một option khác KHI "All" đang được chọn
      else if (actionMeta.action === 'deselect-option' && wasAllPreviouslySelected) {
        // Bỏ chọn "All", giữ lại các option còn lại (bao gồm cả disabled nếu có)
        newSelectedOptions = selected.filter((opt) => opt.value !== ALL_OPTION_VALUE);
      }
      // 4. Hành động: Chọn một option khác dẫn đến tất cả các option *có thể chọn* đều được chọn
      else if (
        actionMeta.action === 'select-option' && // Phải là hành động chọn
        !isAllSelected && // "All" chưa được chọn trước đó
        !actionMeta.option?.disabled // Option vừa chọn không bị disabled
      ) {
        // Đếm số lượng option *có thể chọn* (không phải "All") đang được chọn trong `selected` mới
        const currentSelectedEnabledOriginalCount = selected.filter(
          (opt) => opt.value !== ALL_OPTION_VALUE && !opt.disabled
        ).length;

        // Nếu số lượng này bằng tổng số option gốc có thể chọn -> tự động chọn "All"
        if (
          enabledOriginalOptions.length > 0 &&
          currentSelectedEnabledOriginalCount === enabledOriginalOptions.length
        ) {
          newSelectedOptions = enabledOptions; // Chọn "All" và tất cả các option không bị disabled
        }
      }
      // 5. Hành động: Clear
      else if (actionMeta.action === 'clear') {
        newSelectedOptions = [];
      }
      // 6. Hành động: Remove value (xóa tag)
      else if (actionMeta.action === 'remove-value') {
        // Nếu xóa tag "All"
        if (actionMeta.removedValue?.value === ALL_OPTION_VALUE) {
          newSelectedOptions = []; // Xóa tất cả
        }
        // Nếu xóa tag khác KHI "All" đang được chọn
        else if (wasAllPreviouslySelected) {
          // Bỏ chọn "All", giữ lại các option còn lại trong `selected`
          newSelectedOptions = selected.filter((opt) => opt.value !== ALL_OPTION_VALUE);
        }
        // Kiểm tra lại xem có cần tự động chọn "All" không sau khi xóa tag
        const currentSelectedEnabledOriginalCount = newSelectedOptions.filter(
          (opt) => opt.value !== ALL_OPTION_VALUE && !opt.disabled
        ).length;
        if (
          isCheckAll &&
          enabledOriginalOptions.length > 0 &&
          currentSelectedEnabledOriginalCount === enabledOriginalOptions.length
        ) {
          // Nếu sau khi xóa, tất cả enabled options còn lại -> chọn lại All
          newSelectedOptions = enabledOptions;
        }
      }
    }

    // Lọc lại lần cuối để đảm bảo không có option nào bị disabled được thêm vào (trừ khi nó có trong initialValues)
    // Điều này quan trọng nếu logic ở trên chưa hoàn hảo
    const finalSelectedOptions = newSelectedOptions.filter(
      (opt) => !opt.disabled || initialValues.includes(opt.value) || opt.value === ALL_OPTION_VALUE
    );

    setSelectedOptions(finalSelectedOptions);

    // Chuẩn bị giá trị output (luôn loại bỏ "All")
    const outputValues = finalSelectedOptions
      .map((option) => option.value)
      .filter((value) => value !== ALL_OPTION_VALUE);
    onChange(outputValues);
  };

  // Styles tùy chỉnh
  const defaultStyles: StylesConfig<OptionType, true> = {
    option: (base, state) => ({
      ...base,
      // Giữ style làm mờ cho option disabled (opacity đã xử lý ở CheckboxOption)
      // react-select tự xử lý cursor và pointer-events cho option disabled
      backgroundColor: state.isSelected ? 'transparent' : base.backgroundColor, // Bỏ nền khi selected
      color: state.isDisabled
        ? '#aaa'
        : state.isSelected
          ? state.isFocused
            ? '#333'
            : '#555'
          : base.color, // Màu chữ cho disabled/selected/normal
      paddingLeft: '10px',
      paddingRight: '10px',
      ':active': {
        // Ghi đè hiệu ứng active nếu không muốn
        backgroundColor: state.isSelected
          ? 'transparent'
          : state.isDisabled
            ? base.backgroundColor
            : '#eee', // Không hiệu ứng active khi selected hoặc disabled
      },
    }),
    control: (base) => ({
      ...base,
      minHeight: '38px',
    }),
    // Không cần style cho multiValue vì đã ẩn
  };
  const combinedStyles = { ...defaultStyles, ...styles };

  return (
    <Select<OptionType, true>
      isMulti
      // Truyền options gốc vào đây, react-select sẽ đọc cờ `disabled`
      options={componentOptions}
      value={selectedOptions}
      onChange={handleChange}
      isSearchable={isSearchable}
      placeholder={placeholder}
      closeMenuOnSelect={false} // Giữ menu mở khi chọn item
      // closeMenuOnScroll={false} // Giữ menu mở khi cuộn trang (tùy chọn)
      hideSelectedOptions={false}
      className={`react-select-container ${className || ''}`}
      classNamePrefix="react-select"
      components={{
        Option: CheckboxOption, // Option với checkbox (đã cập nhật)
        ValueContainer: CustomValueContainer, // ValueContainer hiển thị tóm tắt
        MultiValue: () => null, // Ẩn tag MultiValue
        // IndicatorSeparator: () => null,
      }}
      styles={combinedStyles}
      // --- Thêm prop isOptionDisabled ---
      // Cung cấp hàm để react-select biết option nào bị disabled
      // Dựa trên thuộc tính `disabled` trong dữ liệu option
      isOptionDisabled={(option) => !!option.disabled}
    />
  );
};

export default MultiSelect;

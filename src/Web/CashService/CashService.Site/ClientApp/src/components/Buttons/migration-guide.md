# Migration Guide: Button Components

## Tổng quan

Chúng ta đã tạo một `ButtonComponent` duy nhất để thay thế 5 button components cũ:
- `ButtonApplyComponent` → `ButtonComponent` với `variant="apply"`
- `ButtonCancelComponent` → `ButtonComponent` với `variant="cancel"`
- `ButtonFullComponent` → `ButtonComponent` với `variant="full"`
- `ButtonOutlineComponent` → `ButtonComponent` với `variant="outline"`
- `ButtonRejectComponent` → `ButtonComponent` với `variant="reject"`

## Cách Migration

### Bước 1: Cập nhật Import

**Trước:**
```tsx
import {
  ButtonApplyComponent,
  ButtonCancelComponent,
  ButtonFullComponent,
  ButtonOutlineComponent,
  ButtonRejectComponent,
} from '../../components/Buttons/index';
```

**Sau:**
```tsx
import { ButtonComponent } from '../../components/Buttons/index';
```

### Bước 2: Cập nhật JSX

**Trước:**
```tsx
<ButtonApplyComponent text="Apply" onClick={handleApply} />
<ButtonCancelComponent text="Cancel" onClick={handleCancel} />
<ButtonFullComponent text="View" onClick={handleView} />
<ButtonOutlineComponent text="Edit" onClick={handleEdit} />
<ButtonRejectComponent text="Reject" onClick={handleReject} />
```

**Sau:**
```tsx
<ButtonComponent text="Apply" onClick={handleApply} variant="apply" />
<ButtonComponent text="Cancel" onClick={handleCancel} variant="cancel" />
<ButtonComponent text="View" onClick={handleView} variant="full" />
<ButtonComponent text="Edit" onClick={handleEdit} variant="outline" />
<ButtonComponent text="Reject" onClick={handleReject} variant="reject" />
```

### Bước 3: Sử dụng Default Variant (Tùy chọn)

Vì `apply` là variant mặc định, bạn có thể bỏ qua prop `variant`:

```tsx
<ButtonComponent text="Apply" onClick={handleApply} />
<ButtonComponent text="Cancel" onClick={handleCancel} variant="cancel" />
<ButtonComponent text="View" onClick={handleView} variant="full" />
<ButtonComponent text="Edit" onClick={handleEdit} variant="outline" />
<ButtonComponent text="Reject" onClick={handleReject} variant="reject" />
```

## Backward Compatibility

Các components cũ vẫn hoạt động bình thường nhờ wrapper components trong `index.tsx`. Bạn có thể migrate từng file một mà không ảnh hưởng đến các file khác.

## Files cần Migration

Dựa trên kết quả tìm kiếm, các file sau sử dụng các button components cũ:

### High Priority (Files chính):
- `src/pages/PaymentProviderSettings/index.tsx`
- `src/pages/PaymentProviderSettings/UpdatePaymentMethodModal.tsx`
- `src/pages/PaymentProviderSettings/AddPaymentMethodModal.tsx`
- `src/pages/ManagePayments/BankAccountComponent.tsx`
- `src/pages/ManagePayments/SetTransactionLimitModal.tsx`
- `src/pages/ManagePayments/UpdateBankAccountComponent.tsx`

### Medium Priority:
- `src/pages/Transactions/PendingWithdrawal/index.tsx`
- `src/pages/Transactions/PendingDeposit/index.tsx`
- `src/pages/Transactions/History/components/DataGrid.tsx`
- `src/pages/CustomerSites/pages/UpdateInfoCustomer/AddBankModal.tsx`
- `src/pages/Promotion/ManagePromotion/components/AddPromotion.tsx`

### Low Priority (Test files):
- `src/pages/Transactions/PendingWithdrawal/components/__test__/ManualAddWithdrawalForm.test.tsx`
- `src/pages/Transactions/PendingDeposit/components/__test__/ManualAddDepositForm.test.tsx`

## Script Migration (Tùy chọn)

Bạn có thể sử dụng các lệnh find & replace để migration nhanh:

### 1. Cập nhật Import:
```bash
# Tìm và thay thế import statements
find . -name "*.tsx" -exec sed -i 's/ButtonApplyComponent,//g' {} \;
find . -name "*.tsx" -exec sed -i 's/ButtonCancelComponent,//g' {} \;
find . -name "*.tsx" -exec sed -i 's/ButtonFullComponent,//g' {} \;
find . -name "*.tsx" -exec sed -i 's/ButtonOutlineComponent,//g' {} \;
find . -name "*.tsx" -exec sed -i 's/ButtonRejectComponent,//g' {} \;
```

### 2. Thêm ButtonComponent import:
```bash
# Thêm ButtonComponent vào import statements
find . -name "*.tsx" -exec sed -i 's/import {/import { ButtonComponent, /g' {} \;
```

### 3. Cập nhật JSX:
```bash
# Thay thế các button components
find . -name "*.tsx" -exec sed -i 's/<ButtonApplyComponent/<ButtonComponent variant="apply"/g' {} \;
find . -name "*.tsx" -exec sed -i 's/<ButtonCancelComponent/<ButtonComponent variant="cancel"/g' {} \;
find . -name "*.tsx" -exec sed -i 's/<ButtonFullComponent/<ButtonComponent variant="full"/g' {} \;
find . -name "*.tsx" -exec sed -i 's/<ButtonOutlineComponent/<ButtonComponent variant="outline"/g' {} \;
find . -name "*.tsx" -exec sed -i 's/<ButtonRejectComponent/<ButtonComponent variant="reject"/g' {} \;
```

## Testing

Sau khi migration, hãy chạy tests để đảm bảo mọi thứ hoạt động đúng:

```bash
npm test
# hoặc
yarn test
```

## Lợi ích của Migration

1. **Giảm code duplication**: Từ 5 components xuống 1 component
2. **Dễ maintain**: Chỉ cần sửa 1 file thay vì 5 files
3. **Consistent styling**: Đảm bảo tất cả buttons có cùng base styling
4. **Type safety**: Tất cả variants được định nghĩa trong TypeScript
5. **Flexibility**: Dễ dàng thêm variants mới trong tương lai 
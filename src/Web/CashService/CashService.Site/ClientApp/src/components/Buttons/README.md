# Button Components

## ButtonComponent (Component chính)

`ButtonComponent` là component duy nhất để thay thế tất cả các button components cũ:
- `ButtonApplyComponent`
- `ButtonCancelComponent` 
- `ButtonFullComponent`
- `ButtonOutlineComponent`
- `ButtonRejectComponent`

### Cách sử dụng

```tsx
import { ButtonComponent } from '../../components/Buttons';

// Button Apply (mặc định)
<ButtonComponent 
  text="Apply" 
  onClick={handleApply} 
  variant="apply" 
/>

// Button Cancel
<ButtonComponent 
  text="Cancel" 
  onClick={handleCancel} 
  variant="cancel" 
/>

// Button Full
<ButtonComponent 
  text="View Details" 
  onClick={handleView} 
  variant="full" 
/>

// Button Outline
<ButtonComponent 
  text="Edit" 
  onClick={handleEdit} 
  variant="outline" 
/>

// Button Reject
<ButtonComponent 
  text="Reject" 
  onClick={handleReject} 
  variant="reject" 
/>
```

### Props

| Prop | Type | Default | Description |
|------|------|---------|-------------|
| `text` | `string` | - | Text hiển thị trên button |
| `onClick` | `function` | - | Function được gọi khi click |
| `disabled` | `boolean` | `false` | Trạng thái disabled |
| `type` | `'button' \| 'submit' \| 'reset'` | `'button'` | Type của button |
| `className` | `string` | `''` | CSS classes bổ sung |
| `title` | `string` | `''` | Tooltip text |
| `variant` | `'apply' \| 'cancel' \| 'full' \| 'outline' \| 'reject'` | `'apply'` | Kiểu button |

### Migration từ components cũ

Các components cũ vẫn hoạt động bình thường (backward compatibility), nhưng bạn có thể migrate sang component mới:

```tsx
// Cũ
<ButtonApplyComponent text="Apply" onClick={handleApply} />

// Mới
<ButtonComponent text="Apply" onClick={handleApply} variant="apply" />

// Hoặc sử dụng default variant
<ButtonComponent text="Apply" onClick={handleApply} />
```

## Các components khác

Các components sau vẫn được giữ nguyên:
- `ButtonIconComponent` (ButtonClosePopupComponent, ButtonPenComponent, ButtonDeleteComponent)
- `ButtonToggleComponent` (EnableButtonComponent, DisableButtonComponent, ButtonOnComponent, ButtonOffComponent)
- `ButtonTabComponent` (ButtonTabActiveComponent, ButtonTabInactiveComponent, ButtonTabComponent)
- `BackButtonComponent` 
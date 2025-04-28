import * as React from 'react';
import * as DialogPrimitive from '@radix-ui/react-dialog';

interface DialogProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  children: React.ReactNode;
  title?: React.ReactNode;
  description?: React.ReactNode;
  footer?: React.ReactNode;
  size?: 'sm' | 'md' | 'lg' | 'xl' | '2xl' | '3xl' | '4xl' | '5xl' | '6xl' | '7xl' | 'full';
}

const sizeClassMap = {
  sm: 'max-w-sm',
  md: 'max-w-md',
  lg: 'max-w-lg',
  xl: 'max-w-xl',
  '2xl': 'max-w-2xl',
  '3xl': 'max-w-3xl',
  '4xl': 'max-w-4xl',
  '5xl': 'max-w-5xl',
  '6xl': 'max-w-6xl',
  '7xl': 'max-w-7xl',
  full: 'max-w-full',
};

export function Dialog({
  open,
  onOpenChange,
  children,
  title,
  description,
  footer,
  size = '2xl',
}: DialogProps) {
  const maxWidthClass = sizeClassMap[size] || sizeClassMap['7xl'];
  return (
    <DialogPrimitive.Root open={open} onOpenChange={onOpenChange}>
      <DialogPrimitive.Portal>
        <DialogPrimitive.Overlay className="fixed inset-0 z-50 bg-black/50" />
        <DialogPrimitive.Content
          className={`fixed left-1/2 top-1/2 z-50 w-full ${maxWidthClass} -translate-x-1/2 -translate-y-1/2 rounded-lg bg-white p-6 shadow-lg focus:outline-none`}
        >
          {title && (
            <DialogPrimitive.Title className="text-lg font-semibold mb-2">
              {title}
            </DialogPrimitive.Title>
          )}
          {description && (
            <DialogPrimitive.Description className="mb-4 text-gray-500">
              {description}
            </DialogPrimitive.Description>
          )}
          <div>{children}</div>
          {footer && <div className="mt-6 flex justify-end gap-2">{footer}</div>}
          <DialogPrimitive.Close asChild>
            <button
              className="absolute right-4 top-4 rounded p-1 hover:bg-gray-100 focus:outline-none"
              aria-label="Close"
            >
              <svg width="20" height="20" fill="none" viewBox="0 0 20 20">
                <path d="M6 6l8 8M6 14L14 6" stroke="#888" strokeWidth="2" strokeLinecap="round" />
              </svg>
            </button>
          </DialogPrimitive.Close>
        </DialogPrimitive.Content>
      </DialogPrimitive.Portal>
    </DialogPrimitive.Root>
  );
}

import React, { useEffect, useRef, useCallback } from 'react';

declare global {
  interface Window {
    CKEDITOR: any;
  }
}

export interface CKEditor4Props {
  value: string;
  onChange: (value: string) => void;
  placeholder?: string;
  required?: boolean;
  label?: string;
  className?: string;
  minHeight?: string;
  maxHeight?: string;
  readOnly?: boolean;
  showToolbar?: boolean;
  config?: any;
}

export const CKEditor4Wrapper: React.FC<CKEditor4Props> = ({
  value,
  onChange,
  placeholder = '',
  required = false,
  label,
  className = '',
  minHeight = '200px',
  maxHeight = '400px',
  readOnly = false,
  showToolbar = true,
  config = {},
}) => {
  const editorRef = useRef<HTMLTextAreaElement>(null);
  const editorInstanceRef = useRef<any>(null);
  const isInitializedRef = useRef(false);

  // Default CKEditor configuration
  const defaultConfig = {
    height: '200px',
    removePlugins: 'elementspath,resize',
    toolbar: showToolbar
      ? [
          {
            name: 'document',
            items: ['Source', '-', 'Save', 'NewPage', 'Preview', 'Print', '-', 'Templates'],
          },
          {
            name: 'clipboard',
            items: ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'],
          },
          {
            name: 'editing',
            items: ['Find', 'Replace', '-', 'SelectAll', '-', 'SpellChecker', 'Scayt'],
          },
          {
            name: 'forms',
            items: [
              'Form',
              'Checkbox',
              'Radio',
              'TextField',
              'Textarea',
              'Select',
              'Button',
              'ImageButton',
              'HiddenField',
            ],
          },
          '/',
          {
            name: 'basicstyles',
            items: [
              'Bold',
              'Italic',
              'Underline',
              'Strike',
              'Subscript',
              'Superscript',
              '-',
              'RemoveFormat',
            ],
          },
          {
            name: 'paragraph',
            items: [
              'NumberedList',
              'BulletedList',
              '-',
              'Outdent',
              'Indent',
              '-',
              'Blockquote',
              'CreateDiv',
              '-',
              'JustifyLeft',
              'JustifyCenter',
              'JustifyRight',
              'JustifyBlock',
              '-',
              'BidiLtr',
              'BidiRtl',
            ],
          },
          { name: 'links', items: ['Link', 'Unlink', 'Anchor'] },
          {
            name: 'insert',
            items: [
              'Image',
              'Flash',
              'Table',
              'HorizontalRule',
              'Smiley',
              'SpecialChar',
              'PageBreak',
              'Iframe',
            ],
          },
          '/',
          { name: 'styles', items: ['Styles', 'Format', 'Font', 'FontSize'] },
          { name: 'colors', items: ['TextColor', 'BGColor'] },
          { name: 'tools', items: ['Maximize', 'ShowBlocks'] },
        ]
      : [],
    readOnly: readOnly,
    placeholder: placeholder,
    ...config,
  };

  // Initialize CKEditor
  const initEditor = useCallback(() => {
    if (!editorRef.current || isInitializedRef.current || !window.CKEDITOR) return;

    try {
      // Destroy existing instance if any
      if (editorInstanceRef.current) {
        editorInstanceRef.current.destroy();
        editorInstanceRef.current = null;
      }

      // Create new instance
      editorInstanceRef.current = window.CKEDITOR.replace(editorRef.current, defaultConfig);

      // Set initial value
      if (value) {
        editorInstanceRef.current.setData(value);
      }

      // Handle change events
      editorInstanceRef.current.on('change', () => {
        const data = editorInstanceRef.current.getData();
        onChange(data);
      });

      // Handle blur events
      editorInstanceRef.current.on('blur', () => {
        const data = editorInstanceRef.current.getData();
        onChange(data);
      });

      isInitializedRef.current = true;
    } catch (error) {
      console.error('Failed to initialize CKEditor:', error);
    }
  }, [value, onChange, defaultConfig]);

  // Load CKEditor script
  useEffect(() => {
    if (window.CKEDITOR) {
      initEditor();
      return;
    }

    const script = document.createElement('script');
    script.src = 'https://cdn.ckeditor.com/4.25.1/standard/ckeditor.js';
    script.async = true;
    script.onload = () => {
      // Wait a bit for CKEditor to fully load
      setTimeout(initEditor, 100);
    };
    script.onerror = () => {
      console.error('Failed to load CKEditor');
    };

    document.head.appendChild(script);

    return () => {
      if (script.parentNode) {
        script.parentNode.removeChild(script);
      }
    };
  }, [initEditor]);

  // Update editor value when prop changes
  useEffect(() => {
    if (editorInstanceRef.current && isInitializedRef.current) {
      const currentData = editorInstanceRef.current.getData();
      if (currentData !== value) {
        editorInstanceRef.current.setData(value || '');
      }
    }
  }, [value]);

  // Update readOnly state
  useEffect(() => {
    if (editorInstanceRef.current && isInitializedRef.current) {
      editorInstanceRef.current.setReadOnly(readOnly);
    }
  }, [readOnly]);

  // Cleanup on unmount
  useEffect(() => {
    return () => {
      if (editorInstanceRef.current) {
        try {
          editorInstanceRef.current.destroy();
        } catch (error) {
          console.error('Error destroying CKEditor:', error);
        }
        editorInstanceRef.current = null;
        isInitializedRef.current = false;
      }
    };
  }, []);

  return (
    <div className={`ckeditor4-wrapper ${className}`}>
      {label && (
        <div className="mb-2">
          <label className="text-sm font-medium text-[var(--text-color)]">
            {label}
            {required && <span className="text-red-500 ml-1">*</span>}
          </label>
        </div>
      )}

      <div
        className="border border-[var(--border-control)] rounded overflow-hidden"
        style={{ minHeight, maxHeight }}
      >
        <textarea ref={editorRef} name="editor" style={{ display: 'none' }} defaultValue={value} />
      </div>
    </div>
  );
};

export default CKEditor4Wrapper;

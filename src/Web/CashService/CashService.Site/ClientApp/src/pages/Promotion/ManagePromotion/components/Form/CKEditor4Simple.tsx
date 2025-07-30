import React, { useEffect, useRef, useCallback } from 'react';

declare global {
  interface Window {
    CKEDITOR: any;
  }
}

export interface CKEditor4SimpleProps {
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
}

export const CKEditor4Simple: React.FC<CKEditor4SimpleProps> = ({
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
}) => {
  const editorRef = useRef<HTMLTextAreaElement>(null);
  const editorInstanceRef = useRef<any>(null);
  const isInitializedRef = useRef(false);

  // Simple CKEditor configuration
  const editorConfig = {
    height: '200px',
    removePlugins: 'elementspath,resize',
    toolbar: showToolbar
      ? [
          { name: 'basicstyles', items: ['Bold', 'Italic', 'Underline', 'Strike'] },
          { name: 'paragraph', items: ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent'] },
          { name: 'links', items: ['Link', 'Unlink'] },
          { name: 'insert', items: ['Image', 'Table'] },
          { name: 'styles', items: ['Format'] },
          { name: 'colors', items: ['TextColor', 'BGColor'] },
        ]
      : [],
    readOnly: readOnly,
    placeholder: placeholder,
    // React 19 compatibility settings
    autoUpdateElement: false,
    allowedContent: true,
    extraAllowedContent: 'span(*)',
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
      editorInstanceRef.current = window.CKEDITOR.replace(editorRef.current, editorConfig);

      // Set initial value
      if (value) {
        editorInstanceRef.current.setData(value);
      }

      // Handle change events with debounce
      let changeTimeout: NodeJS.Timeout;
      editorInstanceRef.current.on('change', () => {
        clearTimeout(changeTimeout);
        changeTimeout = setTimeout(() => {
          const data = editorInstanceRef.current.getData();
          onChange(data);
        }, 300);
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
  }, [value, onChange, editorConfig]);

  // Load CKEditor script
  useEffect(() => {
    if (window.CKEDITOR) {
      initEditor();
      return;
    }

    const script = document.createElement('script');
    script.src = 'https://cdn.ckeditor.com/4.16.2/standard/ckeditor.js';
    script.async = true;
    script.onload = () => {
      // Wait for CKEditor to fully load
      setTimeout(initEditor, 200);
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
    <div className={`ckeditor4-simple ${className}`}>
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

export default CKEditor4Simple;

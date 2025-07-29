import React, { useState, useEffect, useCallback, useRef } from 'react';
import { useTranslation } from 'react-i18next';

export interface RichContentEditorProps {
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

const Toolbar: React.FC<{
  onToggle: (style: string) => void;
  onBlockToggle: (blockType: string) => void;
}> = ({ onToggle, onBlockToggle }) => {
  const { t } = useTranslation('common');

  const handleStyleToggle = (style: string) => {
    onToggle(style);
  };

  const handleBlockToggle = (blockType: string) => {
    onBlockToggle(blockType);
  };

  const unselectedStyle = 'bg-gray-100 hover:bg-gray-200 text-gray-700';

  return (
    <div className="flex flex-wrap gap-1 p-2 border-b border-[var(--border-control)] bg-[var(--background-control)]">
      <button
        type="button"
        onClick={() => handleStyleToggle('bold')}
        className={`px-2 py-1 rounded text-sm cursor-pointer ${unselectedStyle}`}
        title={t('editor.bold') || 'Bold'}
      >
        <strong>B</strong>
      </button>
      <button
        type="button"
        onClick={() => handleStyleToggle('italic')}
        className={`px-2 py-1 rounded text-sm cursor-pointer ${unselectedStyle}`}
        title={t('editor.italic') || 'Italic'}
      >
        <em>I</em>
      </button>
      <button
        type="button"
        onClick={() => handleStyleToggle('underline')}
        className={`px-2 py-1 rounded text-sm cursor-pointer ${unselectedStyle}`}
        title={t('editor.underline') || 'Underline'}
      >
        <u>U</u>
      </button>

      <div className="w-px h-6 bg-[var(--border-control)] mx-1" />
      <button
        type="button"
        onClick={() => handleBlockToggle('h1')}
        className={`px-2 py-1 rounded text-sm cursor-pointer ${unselectedStyle}`}
        title={t('editor.heading1') || 'Heading 1'}
      >
        H1
      </button>
      <button
        type="button"
        onClick={() => handleBlockToggle('h2')}
        className={`px-2 py-1 rounded text-sm cursor-pointer ${unselectedStyle}`}
        title={t('editor.heading2') || 'Heading 2'}
      >
        H2
      </button>
      <button
        type="button"
        onClick={() => handleBlockToggle('ul')}
        className={`px-2 py-1 rounded text-sm cursor-pointer ${unselectedStyle}`}
        title={t('editor.bulletList') || 'Bullet List'}
      >
        •
      </button>
      <button
        type="button"
        onClick={() => handleBlockToggle('ol')}
        className={`px-2 py-1 rounded text-sm cursor-pointer ${unselectedStyle}`}
        title={t('editor.numberedList') || 'Numbered List'}
      >
        1.
      </button>
    </div>
  );
};

export const RichContentEditor: React.FC<RichContentEditorProps> = ({
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
  const editorRef = useRef<HTMLDivElement>(null);
  const [isComposing, setIsComposing] = useState(false);

  // Update content when value prop changes
  useEffect(() => {
    if (editorRef.current && !isComposing) {
      const currentContent = editorRef.current.innerHTML;
      if (currentContent !== value) {
        editorRef.current.innerHTML = value || '';
      }
    }
  }, [value, isComposing]);

  const handleInput = useCallback(() => {
    if (editorRef.current && !isComposing) {
      const content = editorRef.current.innerHTML;
      onChange(content);
    }
  }, [onChange, isComposing]);

  const handleCompositionStart = useCallback(() => {
    setIsComposing(true);
  }, []);

  const handleCompositionEnd = useCallback(() => {
    setIsComposing(false);
    // Trigger onChange after composition ends
    if (editorRef.current) {
      const content = editorRef.current.innerHTML;
      onChange(content);
    }
  }, [onChange]);

  const handleKeyDown = useCallback(
    (e: React.KeyboardEvent) => {
      // Handle Enter key for new lines
      if (e.key === 'Enter' && !e.shiftKey) {
        e.preventDefault();
        document.execCommand('insertLineBreak', false);
        handleInput();
      }
    },
    [handleInput]
  );

  const handleStyleToggle = useCallback(
    (style: string) => {
      if (editorRef.current) {
        document.execCommand(style, false);
        editorRef.current.focus();
        handleInput();
      }
    },
    [handleInput]
  );

  const handleBlockToggle = useCallback(
    (blockType: string) => {
      if (editorRef.current) {
        switch (blockType) {
          case 'h1':
            document.execCommand('formatBlock', false, '<h1>');
            break;
          case 'h2':
            document.execCommand('formatBlock', false, '<h2>');
            break;
          case 'ul':
            document.execCommand('insertUnorderedList', false);
            break;
          case 'ol':
            document.execCommand('insertOrderedList', false);
            break;
          default:
            document.execCommand('formatBlock', false, '<p>');
        }
        editorRef.current.focus();
        handleInput();
      }
    },
    [handleInput]
  );

  const focusEditor = useCallback(() => {
    if (editorRef.current) {
      editorRef.current.focus();
    }
  }, []);

  return (
    <div className={`rich-content-editor ${className}`}>
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
        onClick={focusEditor}
      >
        {showToolbar && !readOnly && (
          <Toolbar onToggle={handleStyleToggle} onBlockToggle={handleBlockToggle} />
        )}

        <div className="editor-container w-full p-2">
          <div
            ref={editorRef}
            contentEditable={!readOnly}
            className="rich-editor-content w-full min-h-[100px] outline-none"
            style={{
              minHeight: '100px',
              fontSize: '14px',
              lineHeight: '1.5',
              fontFamily: 'inherit',
            }}
            onInput={handleInput}
            onCompositionStart={handleCompositionStart}
            onCompositionEnd={handleCompositionEnd}
            onKeyDown={handleKeyDown}
            data-placeholder={placeholder}
            suppressContentEditableWarning={true}
          />
        </div>
      </div>
    </div>
  );
};

export default RichContentEditor;

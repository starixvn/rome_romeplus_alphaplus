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
  activeStyles: Set<string>;
  activeBlock: string;
}> = ({ onToggle, onBlockToggle, activeStyles, activeBlock }) => {
  const { t } = useTranslation('common');

  const handleStyleToggle = (style: string) => {
    onToggle(style);
  };

  const handleBlockToggle = (blockType: string) => {
    onBlockToggle(blockType);
  };

  const getStyleButtonClass = (style: string) => {
    const baseClass = 'px-2 py-1 rounded text-sm cursor-pointer';
    return activeStyles.has(style)
      ? `${baseClass} bg-blue-500 text-white`
      : `${baseClass} bg-gray-100 hover:bg-gray-200 text-gray-700`;
  };

  const getBlockButtonClass = (blockType: string) => {
    const baseClass = 'px-2 py-1 rounded text-sm cursor-pointer';
    return activeBlock === blockType
      ? `${baseClass} bg-blue-500 text-white`
      : `${baseClass} bg-gray-100 hover:bg-gray-200 text-gray-700`;
  };

  return (
    <div className="flex flex-wrap gap-1 p-2 border-b border-[var(--border-control)] bg-[var(--background-control)]">
      <button
        type="button"
        onClick={() => handleStyleToggle('bold')}
        className={getStyleButtonClass('bold')}
        title={t('editor.bold') || 'Bold'}
      >
        <strong>B</strong>
      </button>
      <button
        type="button"
        onClick={() => handleStyleToggle('italic')}
        className={getStyleButtonClass('italic')}
        title={t('editor.italic') || 'Italic'}
      >
        <em>I</em>
      </button>
      <button
        type="button"
        onClick={() => handleStyleToggle('underline')}
        className={getStyleButtonClass('underline')}
        title={t('editor.underline') || 'Underline'}
      >
        <u>U</u>
      </button>

      <div className="w-px h-6 bg-[var(--border-control)] mx-1" />
      <button
        type="button"
        onClick={() => handleBlockToggle('h1')}
        className={getBlockButtonClass('h1')}
        title={t('editor.heading1') || 'Heading 1'}
      >
        H1
      </button>
      <button
        type="button"
        onClick={() => handleBlockToggle('h2')}
        className={getBlockButtonClass('h2')}
        title={t('editor.heading2') || 'Heading 2'}
      >
        H2
      </button>
      <button
        type="button"
        onClick={() => handleBlockToggle('ul')}
        className={getBlockButtonClass('ul')}
        title={t('editor.bulletList') || 'Bullet List'}
      >
        •
      </button>
      <button
        type="button"
        onClick={() => handleBlockToggle('ol')}
        className={getBlockButtonClass('ol')}
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
  const [activeStyles, setActiveStyles] = useState<Set<string>>(new Set());
  const [activeBlock, setActiveBlock] = useState<string>('p');

  // Helper function to apply style to selection
  const applyStyle = useCallback((style: string) => {
    const selection = window.getSelection();
    if (!selection || selection.rangeCount === 0) return;

    const range = selection.getRangeAt(0);
    const span = document.createElement('span');

    switch (style) {
      case 'bold':
        span.style.fontWeight = 'bold';
        break;
      case 'italic':
        span.style.fontStyle = 'italic';
        break;
      case 'underline':
        span.style.textDecoration = 'underline';
        break;
    }

    // Wrap the selected content with the styled span
    const contents = range.extractContents();
    span.appendChild(contents);
    range.insertNode(span);

    // Normalize the DOM
    span.normalize();

    // Update the selection
    selection.removeAllRanges();
    selection.addRange(range);
  }, []);

  // Helper function to apply block formatting
  const applyBlockFormat = useCallback((blockType: string) => {
    const selection = window.getSelection();
    if (!selection || selection.rangeCount === 0) return;

    const range = selection.getRangeAt(0);
    const container = range.commonAncestorContainer;

    // Find the block element
    let blockElement =
      container.nodeType === Node.TEXT_NODE ? container.parentElement : (container as Element);
    while (blockElement && !['H1', 'H2', 'P', 'UL', 'OL', 'LI'].includes(blockElement.tagName)) {
      blockElement = blockElement.parentElement;
    }

    if (!blockElement) return;

    // Create new block element
    let newBlock: HTMLElement;
    switch (blockType) {
      case 'h1':
        newBlock = document.createElement('h1');
        break;
      case 'h2':
        newBlock = document.createElement('h2');
        break;
      case 'ul':
        newBlock = document.createElement('ul');
        const li = document.createElement('li');
        li.appendChild(document.createTextNode(''));
        newBlock.appendChild(li);
        break;
      case 'ol':
        newBlock = document.createElement('ol');
        const oli = document.createElement('li');
        oli.appendChild(document.createTextNode(''));
        newBlock.appendChild(oli);
        break;
      default:
        newBlock = document.createElement('p');
    }

    // Replace the block element
    if (blockElement.tagName === 'LI') {
      // If we're in a list item, replace the entire list
      const list = blockElement.closest('ul, ol');
      if (list) {
        list.parentNode?.replaceChild(newBlock, list);
      }
    } else {
      blockElement.parentNode?.replaceChild(newBlock, blockElement);
    }

    // Move content to new block
    if (blockType !== 'ul' && blockType !== 'ol') {
      newBlock.innerHTML = blockElement.innerHTML;
    }
  }, []);

  // Update content when value prop changes
  useEffect(() => {
    if (editorRef.current && !isComposing) {
      const currentContent = editorRef.current.innerHTML;
      if (currentContent !== value) {
        editorRef.current.innerHTML = value || '';
      }
    }
  }, [value, isComposing]);

  // Update active styles and block when selection changes
  const updateActiveStyles = useCallback(() => {
    if (!editorRef.current) return;

    const selection = window.getSelection();
    if (!selection || selection.rangeCount === 0) return;

    const range = selection.getRangeAt(0);
    const container = range.commonAncestorContainer;

    // Check active styles by looking at the DOM
    const newActiveStyles = new Set<string>();
    let element =
      container.nodeType === Node.TEXT_NODE ? container.parentElement : (container as Element);

    while (element && element !== editorRef.current) {
      const htmlElement = element as HTMLElement;
      if (htmlElement.style.fontWeight === 'bold' || htmlElement.style.fontWeight === '700') {
        newActiveStyles.add('bold');
      }
      if (htmlElement.style.fontStyle === 'italic') {
        newActiveStyles.add('italic');
      }
      if (htmlElement.style.textDecoration.includes('underline')) {
        newActiveStyles.add('underline');
      }
      element = element.parentElement;
    }

    setActiveStyles(newActiveStyles);

    // Check active block
    let blockElement =
      container.nodeType === Node.TEXT_NODE ? container.parentElement : (container as Element);
    while (blockElement && !['H1', 'H2', 'P', 'UL', 'OL', 'LI'].includes(blockElement.tagName)) {
      blockElement = blockElement.parentElement;
    }

    if (blockElement) {
      const tagName = blockElement.tagName.toLowerCase();
      if (tagName === 'h1') setActiveBlock('h1');
      else if (tagName === 'h2') setActiveBlock('h2');
      else if (tagName === 'ul' || blockElement.closest('ul')) setActiveBlock('ul');
      else if (tagName === 'ol' || blockElement.closest('ol')) setActiveBlock('ol');
      else setActiveBlock('p');
    }
  }, []);

  const handleInput = useCallback(() => {
    if (editorRef.current && !isComposing) {
      const content = editorRef.current.innerHTML;
      onChange(content);
      updateActiveStyles();
    }
  }, [onChange, isComposing, updateActiveStyles]);

  const handleCompositionStart = useCallback(() => {
    setIsComposing(true);
  }, []);

  const handleCompositionEnd = useCallback(() => {
    setIsComposing(false);
    // Trigger onChange after composition ends
    if (editorRef.current) {
      const content = editorRef.current.innerHTML;
      onChange(content);
      updateActiveStyles();
    }
  }, [onChange, updateActiveStyles]);

  const handleKeyDown = useCallback(
    (e: React.KeyboardEvent) => {
      // Handle Enter key for new lines
      if (e.key === 'Enter' && !e.shiftKey) {
        e.preventDefault();
        const selection = window.getSelection();
        if (selection && selection.rangeCount > 0) {
          const range = selection.getRangeAt(0);
          const br = document.createElement('br');
          range.insertNode(br);
          range.setStartAfter(br);
          range.setEndAfter(br);
          selection.removeAllRanges();
          selection.addRange(range);
        }
        handleInput();
      }
    },
    [handleInput]
  );

  const handleSelectionChange = useCallback(() => {
    updateActiveStyles();
  }, [updateActiveStyles]);

  const handleStyleToggle = useCallback(
    (style: string) => {
      if (editorRef.current) {
        editorRef.current.focus();
        applyStyle(style);
        handleInput();
      }
    },
    [applyStyle, handleInput]
  );

  const handleBlockToggle = useCallback(
    (blockType: string) => {
      if (editorRef.current) {
        editorRef.current.focus();
        applyBlockFormat(blockType);
        handleInput();
      }
    },
    [applyBlockFormat, handleInput]
  );

  const focusEditor = useCallback(() => {
    if (editorRef.current) {
      editorRef.current.focus();
    }
  }, []);

  // Add selection change listener
  useEffect(() => {
    document.addEventListener('selectionchange', handleSelectionChange);
    return () => {
      document.removeEventListener('selectionchange', handleSelectionChange);
    };
  }, [handleSelectionChange]);

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
          <Toolbar
            onToggle={handleStyleToggle}
            onBlockToggle={handleBlockToggle}
            activeStyles={activeStyles}
            activeBlock={activeBlock}
          />
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

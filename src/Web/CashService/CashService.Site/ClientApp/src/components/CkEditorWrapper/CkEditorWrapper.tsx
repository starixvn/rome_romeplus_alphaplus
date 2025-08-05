import React, { useEffect, useRef, useState, useCallback } from 'react';
import { TOOLBAR, EXTRA_TOOLBAR, REMOVE_PLUGINS } from './constants';
import { CKEditor } from 'ckeditor4-react';

declare global {
  interface Window {
    CKEDITOR: any;
    currentUsername: string;
  }
}

export interface CkEditorWrapperProps {
  data: string;
  onChange: (data: string) => void;
  className?: string;
  isInvalid?: boolean;
  isRemoveAllToolbar?: boolean;
  isPaddingContent?: boolean;
}

export const CkEditorWrapper: React.FC<CkEditorWrapperProps> = ({
  data,
  onChange,
  className = '',
  isInvalid = false,
  isRemoveAllToolbar = false,
  isPaddingContent = false,
  ...rest
}) => {
  const editorRef = useRef<HTMLDivElement>(null);
  const [editor, setEditor] = useState<any>(null);
  const [isEditorReady, setIsEditorReady] = useState(false);

  // Handle click outside
  const handleClickOutside = useCallback(
    (event: MouseEvent) => {
      const target = event.target as Node;
      if (editorRef.current && !editorRef.current.contains(target)) {
        try {
          const editorData = editor?.editor?.getData();
          if (editorData) {
            console.log('Click outside - editor data:', editorData);
            onChange(editorData);
          }
        } catch (error) {
          console.error('Error getting editor data on click outside:', error);
        }
      }
    },
    [editor]
  );

  const onEditorChange = useCallback((event: any) => {
    try {
      const data = event.editor.getData();
      onChange(data);
    } catch (error) {
      console.error('Error getting editor data:', error);
    }
  }, []);

  const onKeyPress = useCallback((event: any) => {
    if (event.data.keyCode === 1114195) {
      const keyDownEvent = new KeyboardEvent('keydown', { key: 's', ctrlKey: true });
      document.dispatchEvent(keyDownEvent);
      event.cancel();
    }
  }, []);

  const onLoaded = useCallback((editorInstance: any) => {
    setEditor(editorInstance);
    editorInstance.editor.on('instanceReady', function (this: any) {
      this.dataProcessor.writer.lineBreakChars = '\n';
      this.dataProcessor.writer.setRules('p', {
        indent: true,
        breakBeforeOpen: true,
        breakAfterOpen: true,
        breakBeforeClose: false,
        breakAfterClose: true,
      });
      setIsEditorReady(true);
    });
  }, []);

  const onLoadPlugin = useCallback(
    (CKEDITOR: any) => {
      CKEDITOR.plugins.add('editblock', {
        init: function (editor: any) {
          editor?.addCommand('insertEditBlock', {});
          editor?.ui?.addButton('EditBlock', {
            label: 'Insert Edit Block',
            command: 'insertEditBlock',
            toolbar: 'insert',
          });
        },
      });
      CKEDITOR.addCss('body {margin: 2px !important;}');
      CKEDITOR.addCss('p {margin: 0 !important;}');
      if (isPaddingContent) {
        CKEDITOR.addCss('body {padding: 12px !important;}');
      }
    },
    [isPaddingContent]
  );

  useEffect(() => {
    const modal = document.getElementsByClassName('modal');
    if (modal && modal.length > 0 && modal[0].getAttribute('tabindex')) {
      modal[0].removeAttribute('tabindex');
    }
    document.addEventListener('click', handleClickOutside);

    return () => {
      document.removeEventListener('click', handleClickOutside);
    };
  }, [handleClickOutside]);

  useEffect(() => {
    if (editor && isEditorReady && !editor.editor.focusManager.hasFocus) {
      editor.editor.setData(data);
    }
  }, [data, editor, isEditorReady]);

  useEffect(() => {
    return () => {
      try {
        const editorData = editor?.editor?.getData();
        if (editorData) {
          console.log('Component unmount - editor data:', editorData);
          onChange(editorData);
        }
      } catch (error) {
        console.error('Error getting editor data on unmount:', error);
      }
    };
  }, [editor]);

  return (
    <div
      ref={editorRef}
      className={`ckeditor-container ${className} ${isInvalid ? 'is-invalid' : ''} ${isRemoveAllToolbar ? 'remove-toolbar' : ''}`}
    >
      <CKEditor
        {...rest}
        initData={data}
        onChange={onEditorChange}
        config={{
          removeButtons: '',
          colorButton_enableMore: true,
          extraPlugins: EXTRA_TOOLBAR,
          toolbar: TOOLBAR,
          allowedContent: true,
          removePlugins: REMOVE_PLUGINS,
          entities: false,
          basicEntities: false,
          language: 'en',
          disableNativeSpellChecker: false,
          enterMode: 2,
          versionCheck: false,
        }}
        style={{ body: 'margin: 0' }}
        onLoaded={onLoaded}
        onNamespaceLoaded={onLoadPlugin}
        onKey={onKeyPress}
      />
    </div>
  );
};

export default CkEditorWrapper;

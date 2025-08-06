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

      // Disable drag & drop functionality
      this.on('contentDom', function (this: any) {
        const editable = this.document.getById('cke_' + this.name + '_editable_1');
        if (editable) {
          editable.$.ondragover = function (e: DragEvent) {
            e.preventDefault();
            e.stopPropagation();
            return false;
          };
          editable.$.ondrop = function (e: DragEvent) {
            e.preventDefault();
            e.stopPropagation();
            return false;
          };
          editable.$.ondragenter = function (e: DragEvent) {
            e.preventDefault();
            e.stopPropagation();
            return false;
          };
        }
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
          // Configure allowedContent to prevent JavaScript injection
          allowedContent: {
            // Allow basic text formatting
            $1: {
              elements: {
                // Basic text elements
                p: { attributes: '!class' },
                br: {},
                strong: {},
                b: {},
                em: {},
                i: {},
                u: {},
                s: {},
                strike: {},
                sub: {},
                sup: {},

                // Headings
                h1: { attributes: '!class' },
                h2: { attributes: '!class' },
                h3: { attributes: '!class' },
                h4: { attributes: '!class' },
                h5: { attributes: '!class' },
                h6: { attributes: '!class' },

                // Lists
                ul: { attributes: '!class' },
                ol: { attributes: '!class' },
                li: { attributes: '!class' },

                // Links (but no javascript: protocol)
                a: {
                  attributes: '!href,!target,!class',
                  protocols: 'http,https,mailto,ftp',
                },

                // Tables
                table: { attributes: '!class,!width,!height' },
                tbody: { attributes: '!class' },
                thead: { attributes: '!class' },
                tfoot: { attributes: '!class' },
                tr: { attributes: '!class' },
                td: { attributes: '!class,!width,!height' },
                th: { attributes: '!class,!width,!height' },

                // Blockquotes
                blockquote: { attributes: '!class' },

                // Code
                code: { attributes: '!class' },
                pre: { attributes: '!class' },

                // Horizontal rule
                hr: { attributes: '!class' },

                // Images (but no javascript: protocol)
                img: {
                  attributes: '!src,!alt,!title,!width,!height,!class',
                  protocols: 'http,https',
                },
              },
            },
          },
          // Disable plugins that might allow file uploads
          removePlugins: REMOVE_PLUGINS + ',filebrowser,uploadfile,uploadimage',
          entities: false,
          basicEntities: false,
          language: 'en',
          disableNativeSpellChecker: false,
          enterMode: 2,
          versionCheck: false,
          // Disable paste from Word/Excel to prevent unwanted content
          pasteFromWordRemoveFontStyles: true,
          pasteFromWordRemoveStyles: true,
          // Disable auto-embedding of media
          embed_provider: '',
          // Disable file browser
          filebrowserBrowseUrl: '',
          filebrowserUploadUrl: '',
          filebrowserImageBrowseUrl: '',
          filebrowserImageUploadUrl: '',
          filebrowserFlashBrowseUrl: '',
          filebrowserFlashUploadUrl: '',
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

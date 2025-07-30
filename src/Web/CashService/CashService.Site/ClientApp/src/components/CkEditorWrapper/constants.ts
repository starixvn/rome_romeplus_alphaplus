export const TOOLBAR = [
  { name: 'colors', items: ['TextColor', 'BGColor'] },
  {
    name: 'basic styles',
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
      'JustifyLeft',
      'JustifyCenter',
      'JustifyRight',
      'JustifyBlock',
    ],
  },
  { name: 'links', items: ['Link', 'Unlink'] },
  { name: 'clipboard', items: ['Cut', 'Copy', 'Paste', '-', 'Undo', 'Redo'] },
  { name: 'editblock', items: ['EditBlock'] },
];

export const REMOVE_PLUGINS =
  'magicline, elementspath, image, contextmenu, tabletools, liststyle, tableselection';

export const EXTRA_TOOLBAR = 'justify, font, colorbutton, colordialog, editblock';

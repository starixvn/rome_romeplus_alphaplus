

module.exports = () => ({
  'duplicate-button': true,
  'admin-tools': {
    enabled: true,
    resolve: './src/plugins/admin-tools'
  },
	"content-versioning": {
		enabled:  true,
	},
  "tinymce": {
    enabled: true,
    config: {
        editor: {
            outputFormat: "html",
            editorConfig: {
                language: "en",
                height: 500,
                menu: {
                  file: { title: 'File', items: 'newdocument restoredraft | preview | export print | deleteallconversations' },
                  edit: { title: 'Edit', items: 'undo redo | cut copy paste pastetext | selectall | searchreplace' },
                  view: { title: 'View', items: 'code | visualaid visualchars visualblocks | spellchecker | preview fullscreen | showcomments' },
                  insert: { title: 'Insert', items: 'image link media addcomment pageembed template codesample inserttable | charmap emoticons hr | pagebreak nonbreaking anchor tableofcontents | insertdatetime' },
                  format: { title: 'Format', items: 'bold italic underline strikethrough superscript subscript codeformat | styles blocks fontfamily fontsize align lineheight | forecolor backcolor | language | removeformat' },
                  tools: { title: 'Tools', items: 'spellchecker spellcheckerlanguage | a11ycheck code wordcount' },
                  table: { title: 'Table', items: 'inserttable | cell row column | advtablesort | tableprops deletetable' },
                  help: { title: 'Help', items: 'help' }
                },
                
                menubar: 'file edit insert view format table tools help',
                extended_valid_elements: "span, img, small, strong, p",
                forced_root_block: "",
                convert_urls: false,
                entity_encoding: "raw",
                plugins:
                  'print preview casechange \
                  importcss tinydrive searchreplace autolink \
                  autosave save directionality advcode \
                  visualblocks visualchars fullscreen image \
                  link media mediaembed template \
                  codesample table charmap hr \
                  pagebreak nonbreaking anchor \
                  toc insertdatetime advlist lists checklist \
                  wordcount tinymcespellchecker a11ychecker imagetools \
                  textpattern noneditable help formatpainter permanentpen \
                  pageembed charmap mentions quickbars linkchecker \
                  emoticons advtable export hr',                 
                toolbar:
                    "fullscreen preview code | \
                    undo redo | bold italic underline superscript subscript strikethrough | \
                    blocks | fontfamily fontsizeinput | \
                    alignleft aligncenter alignright alignjustify | \
                    link image table pageembed media | \
                    lineheight numlist bullist checklist | \
                    outdent indent | \
                    backcolor forecolor formatpainter removeformat | \
                    emoticons visualblocks | \
                    save print export | \
                    pagebreak anchor codesample footnotes mergetags | \
                    addcomment showcomments | \
                    template | ltr rtl | \
                    casechange typopgraphy spellchecker a11ycheck | \
                    nonbreaking hr | help",
                toolbar_mode: 'wrap',

                quickbars_insert_toolbar: 'quicktable | hr pagebreak',
                quickbars_image_toolbar: 'alignleft aligncenter alignright | rotateleft rotateright | imageoptions',
                contextmenu: "link image imagetools lists table",
              
                advlist_bullet_styles: [{
                    title: 'Default 2',
                    styles: {
                      listStyleType: 'disc',
                      listStyleImage: ""
                    }
                }],
                style_formats: [
                  { title: 'Headings', items: [
                    { title: 'Heading 1', format: 'h1' },
                    { title: 'Heading 2', format: 'h2' },
                    { title: 'Heading 3', format: 'h3' },
                    { title: 'Heading 4', format: 'h4' },
                    { title: 'Heading 5', format: 'h5' },
                    { title: 'Heading 6', format: 'h6' }
                  ]},
                  { title: 'Inline', items: [
                    { title: 'Bold', format: 'bold' },
                    { title: 'Italic', format: 'italic' },
                    { title: 'Underline', format: 'underline' },
                    { title: 'Strikethrough', format: 'strikethrough' },
                    { title: 'Superscript', format: 'superscript' },
                    { title: 'Subscript', format: 'subscript' },
                    { title: 'Code', format: 'code' }
                  ]},
                  { title: 'Blocks', items: [
                    { title: 'Paragraph', format: 'p' },
                    { title: 'Blockquote', format: 'blockquote' },
                    { title: 'Div', format: 'div' },
                    { title: 'Pre', format: 'pre' }
                  ]},
                  { title: 'Align', items: [
                    { title: 'Left', format: 'alignleft' },
                    { title: 'Center', format: 'aligncenter' },
                    { title: 'Right', format: 'alignright' },
                    { title: 'Justify', format: 'alignjustify' }
                  ]}
                ],
                style_formats_merge: true,
                content_style: '.haft-screen { width: 49%; display: inline-block; vertical-align: top; padding: 5px 0; } ' +
                  '.col-1st { border-right: solid 1px #eee; } '+
                  'ul.ul-marked { list-style-type: "✓"; } '+
                  'ul.ul-x { list-style-type: "✗"; } '+
                  'ul.ul-arr { list-style-type: "➣"; } '+
                  'ul.ul-em-dash { list-style-type: "—"; } '+
                  'ul.ul-plus { list-style-type: "+"; } '+
                  'ul.ul-minus { list-style-type: "-"; } '+
                  'ul>li>ul>li>ul>li>ul { list-style-type: "+"; } '+
                  'ul>li>ul>li>ul>li>ul>li>ul { list-style-type: "-"; } '+
                  '',
             
                formats: {
                  removeformat: [
                    {selector: 'b,strong,em,i,font,u,strike,h1,h2,h3,h4,h5,h6', remove : 'all', split : true, expand : false, block_expand: true, deep : true},
                    {selector: '*', attributes : ['style', 'class'], split : false, expand : false, deep : true},
                  ],
                },
                templates : [
                  {
                    title: 'Popup',
                    description: 'Popup',
                    content: '<div>This is Popup</div>'
                  },
                  {
                    title: 'Tooltip',
                    description: 'Tooltip',
                    content: '<div>This is Tooltip</div>'
                  },
                  {
                    title: 'Split 2 columns',
                    description: 'Content with 2 columns',
                    content: '<div class="haft-screen col-1st">This is Column 1</div>\
                      <div class="haft-screen">This is Column 2</div>'
                  },
                  {
                    title: 'Bullest: Marked',
                    description: 'Bullest: Marked',
                    content: '<ul class="ul-marked">\
                        <li>List Item 1</li>\
                        <li>List Item 2</li>\
                      </ul>'
                  },
                  {
                    title: 'Bullest: X Marked',
                    description: 'Bullest: X Marked',
                    content: '<ul class="ul-x">\
                        <li>List Item 1</li>\
                        <li>List Item 2</li>\
                      </ul>'
                  },
                  {
                    title: 'Bullest: Arrow',
                    description: 'Bullest: Arrow',
                    content: '<ul class="ul-arr">\
                        <li>List Item 1</li>\
                        <li>List Item 2</li>\
                      </ul>'
                  },
                  {
                    title: 'Bullest: Em Dash',
                    description: 'Bullest: Em Dash',
                    content: '<ul class="ul-em-dash">\
                        <li>List Item 1</li>\
                        <li>List Item 2</li>\
                      </ul>'
                  }
                ]
            },
        },
      }
  }
});

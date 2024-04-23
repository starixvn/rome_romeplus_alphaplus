module.exports = () => ({
  "duplicate-button": false,
  "admin-tools": {
    enabled: true,
    resolve: "./src/plugins/admin-tools",
  },
  "content-versioning": {
    enabled: true,
  },
  "copy-locales": {
    config: {
      contentTypes: [
        "api::category.category",
        "api::market.market",
        "api::glossary.glossary",
        "api::environment.environment",
        "api::user-level.user-level",
        "api::article.article",
      ],
    },
  },
  tinymce: {
    enabled: true,

    config: {
      editor: {
        outputFormat: "html",
        editorConfig: {
          language: "en",
          height: 500,
          menu: {
            file: {
              title: "File",
              items:
                "newdocument restoredraft | preview | export print | deleteallconversations",
            },
            edit: {
              title: "Edit",
              items:
                "undo redo | cut copy paste pastetext | selectall | searchreplace",
            },
            view: {
              title: "View",
              items:
                "code | visualaid visualchars visualblocks | spellchecker | preview fullscreen | showcomments",
            },
            insert: {
              title: "Insert",
              items:
                "image link media addcomment pageembed template codesample inserttable | charmap emoticons hr | pagebreak nonbreaking anchor tableofcontents | insertdatetime",
            },
            format: {
              title: "Format",
              items:
                "bold italic underline strikethrough superscript subscript codeformat | styles blocks fontfamily fontsize align lineheight | forecolor backcolor | language | removeformat",
            },
            tools: {
              title: "Tools",
              items:
                "spellchecker spellcheckerlanguage | a11ycheck code wordcount",
            },
            table: {
              title: "Table",
              items:
                "inserttable | cell row column | advtablesort | tableprops deletetable",
            },
            help: { title: "Help", items: "help" },
          },

          menubar: "file edit insert view format table tools help",
          extended_valid_elements: "small, strong, p, div",
          valid_elements: "*[*]",
          valid_attributes: "*[*]",
          forced_root_block: "",
          convert_urls: false,
          entity_encoding: "raw",
          plugins:
            "print preview code \
                  importcss tinydrive searchreplace autolink \
                  autosave save directionality \
                  visualblocks visualchars fullscreen image \
                  link media template \
                  codesample table charmap hr \
                  pagebreak nonbreaking anchor \
                  toc insertdatetime advlist lists \
                  wordcount imagetools \
                  textpattern noneditable help \
                  charmap quickbars linkchecker \
                  emoticons hr",
          toolbar:
            "fullscreen preview code | \
                    undo redo | bold italic underline superscript subscript strikethrough | \
                    blocks | fontfamily fontsizeinput | \
                    alignleft aligncenter alignright alignjustify | \
                    link table pageembed | \
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
          toolbar_mode: "wrap",

          quickbars_insert_toolbar: "quicktable | hr pagebreak",
          quickbars_image_toolbar:
            "alignleft aligncenter alignright | rotateleft rotateright | imageoptions",
          contextmenu: "link image imagetools lists table",

          advlist_bullet_styles: [
            {
              title: "Default 2",
              styles: {
                listStyleType: "disc",
                listStyleImage: "",
              },
            },
          ],
          style_formats: [
            {
              title: "Headings",
              items: [
                { title: "Heading 1", format: "h1" },
                { title: "Heading 2", format: "h2" },
                { title: "Heading 3", format: "h3" },
                { title: "Heading 4", format: "h4" },
                { title: "Heading 5", format: "h5" },
                { title: "Heading 6", format: "h6" },
              ],
            },
            {
              title: "Inline",
              items: [
                { title: "Bold", format: "bold" },
                { title: "Italic", format: "italic" },
                { title: "Underline", format: "underline" },
                { title: "Strikethrough", format: "strikethrough" },
                { title: "Superscript", format: "superscript" },
                { title: "Subscript", format: "subscript" },
                { title: "Code", format: "code" },
              ],
            },
            {
              title: "Blocks",
              items: [
                { title: "Paragraph", format: "p" },
                { title: "Blockquote", format: "blockquote" },
                { title: "Div", format: "div" },
                { title: "Pre", format: "pre" },
              ],
            },
            {
              title: "Align",
              items: [
                { title: "Left", format: "alignleft" },
                { title: "Center", format: "aligncenter" },
                { title: "Right", format: "alignright" },
                { title: "Justify", format: "alignjustify" },
              ],
            },
          ],
          style_formats_merge: true,
          content_style:
            "img{display: inline-block;vertical-align: middle;}ul li ul {list-style: none;}ul li ul li{position:relative}ul li ul li:before{position:absolute;content:'';top: 5px;left: -18px;width:12px;height:12px;background-repeat:no-repeat;background-position:center center;background-image: url(data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMTIiIGhlaWdodD0iMTIiIHZpZXdCb3g9IjAgMCAxMiAxMiIgZmlsbD0ibm9uZSIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj4KPHJlY3QgeD0iMS4xOTk5NSIgeT0iNC43OTk4IiB3aWR0aD0iOS42IiBoZWlnaHQ9IjIuNCIgZmlsbD0iIzMzMzMzMyIvPgo8L3N2Zz4K)}ul li ul li ul li:before{position:absolute;content:'';top: 5px;left: -18px;width:12px;height:12px;background-repeat:no-repeat;background-position:center center;background-image: url(data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMTIiIGhlaWdodD0iMTIiIHZpZXdCb3g9IjAgMCAxMiAxMiIgZmlsbD0ibm9uZSIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj4KPHBhdGggZD0iTTEwLjggNi4wMDAyTDEuMjAwMDUgMTAuODAwMkwzLjg2NjcyIDYuMDAwMkwxLjIwMDA1IDEuMjAwMTlMMTAuOCA2LjAwMDJaIiBmaWxsPSIjMzMzMzMzIi8+Cjwvc3ZnPgo=)}.box-note .box-title,.box-note b,.box-note strong{color:#1c62a3}.box-caution .box-title,.box-note .box-title,.box-tip .box-title{position:relative;padding:0 0 0 34px;margin-bottom:8px}.box-caution .box-title:before,.box-note .box-title:before,.box-tip .box-title:before{position:absolute;content:'';top:0;left:0;width:24px;height:24px;background-repeat:no-repeat;background-position:center center}.box{border:1px solid #d1d1d1;border-radius:5px;padding:15px;margin-bottom:12px}.box .box-title{font-size:18px;font-weight:500}.box .box-content p{font-size:16px;line-height:1.625;margin:0}.box .box-content ul{padding:0 0 0 24px;margin:0;list-style:disc}.box .box-content ul li{font-size:16px;line-height:1.625;margin:0!important}.box-note{border-color:#90bdea;background:#e7f3ff}.box-note .box-title:before{background-image:url(data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMjQiIGhlaWdodD0iMjQiIHZpZXdCb3g9IjAgMCAyNCAyNCIgZmlsbD0ibm9uZSIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj4KPHBhdGggZD0iTTEyLjQ3NjMgNS42ODg0OEwxNS44NTg4IDcuNTg5NzNMMTcuNzc5NSAxMC45OTI1TDcuMDI3NTEgMjEuNzQ0NUwzLjM4MTAxIDE5LjgxN0wxLjcyMzUxIDE2LjQ0MkwxMi40NzYzIDUuNjg4NDhaIiBmaWxsPSIjRkY4MjJEIi8+CjxwYXRoIGQ9Ik0xLjA0MjQ4IDIxLjA0ODdMMS40Nzg5OCAyMi4wMzI3TDIuNDIwMjMgMjIuNDI2NEw3LjAxNzczIDIxLjczMjdMMS43MzYyMyAxNi40NTEyTDEuMDQyNDggMjEuMDQ4N1oiIGZpbGw9IiNGRkNFN0MiLz4KPHBhdGggZD0iTTAuNzk3MjQxIDIyLjY3MThMMS4wNDE3NCAyMS4wNDg4TDIuNDIwMjQgMjIuNDI2NkwwLjc5NzI0MSAyMi42NzE4WiIgZmlsbD0iIzQwMkEzMiIvPgo8cGF0aCBkPSJNMTYuNzA2OSAxLjQ1ODExQzE2Ljg0NjIgMS4zMTg3NiAxNy4wMTE2IDEuMjA4MjEgMTcuMTkzNiAxLjEzMjc5QzE3LjM3NTcgMS4wNTczNyAxNy41NzA4IDEuMDE4NTUgMTcuNzY3OCAxLjAxODU1QzE3Ljk2NDggMS4wMTg1NSAxOC4xNTk5IDEuMDU3MzcgMTguMzQyIDEuMTMyNzlDMTguNTI0IDEuMjA4MjEgMTguNjg5NCAxLjMxODc2IDE4LjgyODcgMS40NTgxMUwyMi4wMTAyIDQuNjQwMzZDMjIuMjkxNCA0LjkyMTY1IDIyLjQ0OTQgNS4zMDMxMSAyMi40NDk0IDUuNzAwODZDMjIuNDQ5NCA2LjA5ODYxIDIyLjI5MTQgNi40ODAwNyAyMi4wMTAyIDYuNzYxMzZMMTkuMzU4OSA5LjQxMzM2TDE1Ljk2MzcgNy43MTUzNkwxNC4wNTQ5IDQuMTEwMTFMMTYuNzA2OSAxLjQ1ODExWiIgZmlsbD0iI0Y5MkY2MCIvPgo8cGF0aCBkPSJNMTQuMDU1IDQuMTEwMzVMMTkuMzU5IDkuNDEzNkwxNy43Njc1IDExLjAwNDRMMTIuNDY1IDUuNzAxODVMMTQuMDU1IDQuMTEwMzVaIiBmaWxsPSIjRDNEM0QzIi8+Cjwvc3ZnPgo=)}.box-tip{border-color:#a3c696;background:#eefbe9}.box-tip .box-title{color:#1e661e}.box-tip .box-title:before{background-image:url(data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMjQiIGhlaWdodD0iMjQiIHZpZXdCb3g9IjAgMCAyNCAyNCIgZmlsbD0ibm9uZSIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj4KPHBhdGggZD0iTTE5LjI3MDkgOC42MjExM0MxOS4yNzA5IDQuNTA1OTIgMTYuMDQwNyAxLjE2OTkyIDEyLjA1NjEgMS4xNjk5MkM4LjA3MTU5IDEuMTY5OTIgNC44NDE0MyA0LjUwNTk3IDQuODQxNDMgOC42MjExM0M0Ljg0MTQzIDEwLjQ4MjEgNS41MDIwNCAxMi4xODM2IDYuNTk0MTggMTMuNDg5NEg2LjU5NDEzTDguMTcyMjMgMTYuMzVDOC41ODQ0IDE3LjExMzEgOC43Mzc0IDE3LjYwNTkgOS40MjgxIDE3LjYwNTlIMTQuNTczOEMxNS4yNjQ1IDE3LjYwNTkgMTUuNDY3NyAxNy4wODcgMTUuODI5NiAxNi4zNUwxNy41MTgyIDEzLjQ4OTRIMTcuNTE4MUMxOC42MTAyIDEyLjE4MzYgMTkuMjcwOSAxMC40ODIxIDE5LjI3MDkgOC42MjExM1oiIGZpbGw9IiNGRkU0NkEiLz4KPHBhdGggZD0iTTUuMjg3NzcgOS41Mjg1OEM1LjI4Nzc3IDUuMjc2MDMgOC40OTI5OSAxLjc2MTM0IDEyLjY1NCAxLjE5OEMxMi40NDY0IDEuMTc5NDQgMTIuMjM4MSAxLjE3MDA4IDEyLjAyOTcgMS4xNjk5MkM4LjA0NTEgMS4xNjk5MiA0LjgxNDk0IDQuNTA1OTcgNC44MTQ5NCA4LjYyMTEzQzQuODE0OTQgOS45NzUzNCA1LjE2NTA1IDExLjI0NDkgNS43NzY0NCAxMi4zMzkyQzUuNDYwMDQgMTEuNDYwMyA1LjI4Nzc3IDEwLjUxNDMgNS4yODc3NyA5LjUyODU4WiIgZmlsbD0iI0ZGRjBCNyIvPgo8cGF0aCBkPSJNMTkuMjcwOSA4LjYzNzI1QzE5LjI3MDkgNy4xNjMyNyAxOC44NTYzIDUuNzg5MzYgMTguMTQxMyA0LjYzMjgxQzE4LjI5NDQgNS4yMzY3NiAxOC4zNzE4IDUuODU3MzkgMTguMzcxOCA2LjQ4MDQ0QzE4LjM3MTggMTEuMzc2NyAxMy42MzA5IDE1LjM2NDMgNy43MDg2MiAxNS41MjU4TDguMTcyMjEgMTYuMzY2MkM4LjU4NDM4IDE3LjEyOTMgOC43MzczOCAxNy42MjIxIDkuNDI4MDkgMTcuNjIyMUgxNC41NzM3QzE1LjI2NDUgMTcuNjIyMSAxNS40Njc3IDE3LjEwMzIgMTUuODI5NiAxNi4zNjYyTDE3LjUxODEgMTMuNTA1NUgxNy41MTgxQzE4LjYxMDIgMTIuMTk5NyAxOS4yNzA5IDEwLjQ5ODIgMTkuMjcwOSA4LjYzNzI1WiIgZmlsbD0iI0ZGREEwMCIvPgo8cGF0aCBkPSJNMTUuMDg5MyA5LjkwMDA1QzE1LjA5NjMgOS44OTM5MSAxNS4xMDMyIDkuODg4MjggMTUuMTEwMiA5Ljg4MTUzQzE1LjI0MDkgOS43NTY1MiAxNS4zNDg0IDkuNTU3MTEgMTUuMjU4NCA5LjM3MjE0QzE0Ljg4OTEgOC42MTM4OSAxMy43NTk4IDguMzczMzMgMTIuOTM3IDguNjI0NDRDMTIuODk1MyA4LjYzNzE0IDEyLjg1NDkgOC42NTA5NyAxMi44MTU2IDguNjY1OTJDMTIuMzY3NCA4LjQwNDIyIDExLjgxMDkgOC4zNDEzMSAxMS4zMTU2IDguNDk4NjNDMTEuMjU3IDguNTE3MTkgMTEuMTk5NCA4LjUzOTAxIDExLjE0MzIgOC41NjM5N0MxMC44NzIgOC40MDU1MyAxMC41NTQ4IDguMzI2NSAxMC4yMzg4IDguMzU3MDZDOS41NzcyOCA4LjQyMTA1IDkuMDAyNDYgOC45NTc1MyA4Ljg4ODU1IDkuNjExMDZMOC44MzY0MyA5LjY0MzMxQzkuMjg3MjIgMTAuMzcxOCAxMC40NzM4IDEyLjQ1OTEgMTAuNTQ5OCAxMy4zNTc0QzEwLjY0NTcgMTQuNDkwNSAxMC41NDk5IDE3LjU2NjQgMTAuNTQ4OSAxNy41OTczTDExLjExMTEgMTcuNjE1MUMxMS4xMTUyIDE3LjQ4NzUgMTEuMjA4OSAxNC40NzU1IDExLjExMDIgMTMuMzA5OUMxMS4wNjYzIDEyLjc5MDIgMTAuNzQ4MyAxMS45NjE5IDEwLjE2NTEgMTAuODQ4QzkuODkyODUgMTAuMzI4MSA5LjYyODU3IDkuODcxNTkgOS40Njg1OCA5LjYwMjI1QzkuNTc5MjEgOS4yNDM4NCA5LjkxOTA1IDguOTUzMTMgMTAuMjkyOSA4LjkxNjk4QzEwLjQwNjggOC45MDU5NyAxMC41MjIzIDguOTE4NzIgMTAuNjMyOCA4Ljk1MTE2QzEwLjU5MTIgOS4wMDQ0MiAxMC41NTQ4IDkuMDYxNTIgMTAuNTI0MSA5LjEyMTY5QzEwLjMzNDQgOS40OTM2NCAxMC40MTM2IDkuOTYwNjYgMTAuNzA4NCAxMC4yMDgxQzEwLjg4MyAxMC4zNTQ1IDExLjEzNjcgMTAuNDA4NSAxMS4zNzA2IDEwLjM0ODdDMTEuNTg4NSAxMC4yOTMgMTEuNzU2OCAxMC4xNDk4IDExLjgzMjUgOS45NTU5MkMxMS45MDA2IDkuNzgxNjQgMTEuODk0OCA5LjU4MDQ1IDExLjgxNTMgOS4zNTc5OUMxMS43NjkzIDkuMjMgMTEuNzA1NCA5LjEwOTE4IDExLjYyNTUgOC45OTkxNkMxMS44Mzc4IDguOTU4MzEgMTIuMDU2OSA4Ljk3MDI5IDEyLjI2MzUgOS4wMzQwM0MxMi4xODMxIDkuMTI2NjEgMTIuMTIxNiA5LjIyOTY0IDEyLjA4MTEgOS4zNDEzNEMxMS45OTQ3IDkuNTgwNDUgMTIuMDMyOSA5Ljg2NDYxIDEyLjE4MDkgMTAuMDgzQzEyLjMwODQgMTAuMjcxMyAxMi41MDIyIDEwLjM4MzEgMTIuNzEyOCAxMC4zOTAxQzEyLjcxOTggMTAuMzkwMyAxMi43MjY5IDEwLjM5MDUgMTIuNzMzOSAxMC4zOTA1QzEyLjkxODMgMTAuMzkwNSAxMy4wOTM0IDEwLjMxMTEgMTMuMjI5IDEwLjE2NTZDMTMuMzg4MiA5Ljk5NDY0IDEzLjQ2OTMgOS43NTczMSAxMy40NDU3IDkuNTMwNzJDMTMuNDMwOCA5LjM4NjY3IDEzLjM3OTYgOS4yNDU3NyAxMy4yOTcxIDkuMTE1MDhDMTMuODcwNiA5LjAxMDg4IDE0LjQzMTcgOS4yMDQ2NiAxNC42NzQ0IDkuNDk4OEMxNC41Mjc5IDkuNzQyNTUgMTQuMjI3MSAxMC4yNTQgMTMuOTE2IDEwLjg0ODFDMTMuMzMyOSAxMS45NjIgMTMuMDE0OCAxMi43OTA0IDEyLjk3MDggMTMuMzFDMTIuODcyMiAxNC40NzU2IDEyLjk2NiAxNy40ODc2IDEyLjk3IDE3LjYxNTJMMTMuNTMyMiAxNy41OTc0QzEzLjUzMTIgMTcuNTY2NSAxMy40MzU0IDE0LjQ5MDggMTMuNTMxMyAxMy4zNTc1QzEzLjU5OTcgMTIuNTQ4OCAxNC41Njc4IDEwLjc3NzIgMTUuMDg5MyA5LjkwMDA1Wk0xMS4zMDg1IDkuNzUxNDFDMTEuMjk1NCA5Ljc4NDk3IDExLjI0NjEgOS43OTk5MiAxMS4yMzEzIDkuODAzNzJDMTEuMTcyNyA5LjgxODY3IDExLjEwNjMgOS44MDc3NSAxMS4wNjk5IDkuNzc3MjNDMTAuOTc1NSA5LjY5Nzk3IDEwLjk1NDkgOS41MTQ3OCAxMS4wMjUxIDkuMzc3M0MxMS4wNDgxIDkuMzMzMDMgMTEuMDc3MyA5LjI5MjI2IDExLjExMTggOS4yNTYyMkMxMS4xODczIDkuMzQxOTEgMTEuMjQ3MyA5LjQ0MDI1IDExLjI4NTYgOS41NDcyMkMxMS4zMTY3IDkuNjM0NDEgMTEuMzI1MSA5LjcwODg5IDExLjMwODUgOS43NTE0MVpNMTIuODE3NCA5Ljc4MjA2QzEyLjgwMDkgOS43OTk2NCAxMi43Njc1IDkuODI4NTYgMTIuNzMxMiA5LjgyNzgxQzEyLjY5MjIgOS44MjY1NSAxMi42NjE0IDkuNzg5NDIgMTIuNjQ2NCA5Ljc2NzM5QzEyLjU5OTkgOS42OTg2MyAxMi41ODQ5IDkuNjAyMDYgMTIuNjEwMSA5LjUzMjU5QzEyLjYzOSA5LjQ1MjQ0IDEyLjY5NDkgOS4zODY4NiAxMi43NjAyIDkuMzM0MDhDMTIuODMyOCA5LjQxNjE2IDEyLjg3NzQgOS41MDQ0MiAxMi44ODYyIDkuNTg4OEMxMi44OTI5IDkuNjUzMDYgMTIuODY1MiA5LjczMDY5IDEyLjgxNzQgOS43ODIwNloiIGZpbGw9IiNGQUFGNjMiLz4KPHBhdGggZD0iTTE0LjgwOTMgMTguODc3SDkuMjc2MzdDOC45NjY5OSAxOC44NzcgOC43MTM4NyAxOS4xMzAxIDguNzEzODcgMTkuNDM5NVYxOS43NjU2QzguNzEzODcgMjAuMDc0OSA4Ljk2Njk5IDIwLjMyODEgOS4yNzYzNyAyMC4zMjgxSDExLjA2MzNDMTAuNTMwNSAyMC41MjYzIDkuOTY3OTEgMjAuOTk3NyA5Ljk2NzkxIDIxLjU0NDNWMjEuNzYzN0MxMC4zOTU2IDIyLjI0NDMgMTAuNzkzOCAyMy4wOCAxMS41NTI0IDIzLjA4SDEyLjU3ODVDMTMuMTc2IDIzLjA4IDEzLjYwMDUgMjIuNDM0OSAxNC4xMTc3IDIxLjc2MzdWMjEuNTQ0M0MxNC4xMTc3IDIwLjk5NzcgMTMuNTgwOSAyMC41MjYzIDEzLjA2MDcgMjAuMzI4MUgxNC44MDkzQzE1LjExODYgMjAuMzI4MSAxNS4zNzE4IDIwLjA3NDkgMTUuMzcxOCAxOS43NjU2VjE5LjQzOTVDMTUuMzcxOCAxOS4xMzAxIDE1LjExODYgMTguODc3IDE0LjgwOTMgMTguODc3WiIgZmlsbD0iIzZCODNBNSIvPgo8cGF0aCBkPSJNMTUuMjA2NSAxOC44NzY5SDguODc5MTVDOC41Njk3OCAxOC44NzY5IDguMzE2NjUgMTguNjIzOCA4LjMxNjY1IDE4LjMxNDRWMTcuOTg4M0M4LjMxNjY1IDE3LjY3ODkgOC41Njk3OCAxNy40MjU4IDguODc5MTUgMTcuNDI1OEgxNS4yMDY1QzE1LjUxNTkgMTcuNDI1OCAxNS43NjkgMTcuNjc4OSAxNS43NjkgMTcuOTg4M1YxOC4zMTQ0QzE1Ljc2OSAxOC42MjM4IDE1LjUxNTkgMTguODc2OSAxNS4yMDY1IDE4Ljg3NjlaTTE0Ljg3OCAyMS4yMTY3VjIwLjg5MDVDMTQuODc4IDIwLjU4MTIgMTQuNjI0OSAyMC4zMjggMTQuMzE1NSAyMC4zMjhIOS43NzAxQzkuNDYwNzMgMjAuMzI4IDkuMjA3NiAyMC41ODEyIDkuMjA3NiAyMC44OTA1VjIxLjIxNjdDOS4yMDc2IDIxLjUyNiA5LjQ2MDczIDIxLjc3OTIgOS43NzAxIDIxLjc3OTJIMTQuMzE1NUMxNC42MjQ5IDIxLjc3OTIgMTQuODc4IDIxLjUyNiAxNC44NzggMjEuMjE2N1oiIGZpbGw9IiNBQkJEREIiLz4KPC9zdmc+Cg==)}.box-tip b,.box-tip strong{color:#1e661e}.box-caution{border-color:#b8a491;background:#fffffa}.box-caution .box-title{color:#a75000}.box-caution .box-title:before{background-image:url(data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMjQiIGhlaWdodD0iMjQiIHZpZXdCb3g9IjAgMCAyNCAyNCIgZmlsbD0ibm9uZSIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj4KPHBhdGggZD0iTTUuMjYyNzEgNS40Njc4N0M2LjIzNTQ2IDUuNDY3ODcgNi41MjcyMSA2LjAwMDM3IDYuNTI3MjEgNi4wMDAzN0w2LjU5NzcxIDQuNjI5MzdDNi42NTYyMSAzLjkwOTM3IDcuNDk3NzEgMy4xOTQ2MiA4LjI4NTk2IDMuMTk0NjJDOS4zNjI5NiAzLjE5NDYyIDkuNzczMjEgMy45NzMxMiA5Ljc3MzIxIDMuOTczMTJDOS43MDI3MSAyLjMyMDg3IDEwLjMwMDUgMS40OTUxMiAxMS43ODMyIDEuNDk1MTJDMTIuOTg0IDEuNTcwMTIgMTMuNDc4MiAyLjcxMzEyIDEzLjM4MyAzLjk3MzEyQzEzLjM4MyAzLjk3MzEyIDEzLjg4NyAyLjk4ODM3IDE1LjI4MDUgMy4xMTI4N0MxNi4wNjU3IDMuMTgyNjIgMTYuNjk5NSAzLjk3MzEyIDE2LjY5OTUgNS4yOTY4N0wxNi41NTg1IDExLjA2MjlDMTYuNTU4NSAxMS4wNjI5IDE3LjQ1MSA5Ljc3MDYyIDE4LjExNyA5LjM4NzM3QzE4Ljc4MyA5LjAwMzM3IDE5Ljg4MjUgOS4wMjczNyAyMC40MjYyIDkuNjA0ODdDMjEuMDY0NSAxMC4yODM2IDIxLjE4MDcgMTEuMzQ3MSAyMC43MjI1IDExLjk5NTFDMTkuMDA4IDE0LjIwMzEgMTcuMzY3IDE2LjY2NDYgMTYuODYzIDE3LjY4MzlDMTUuNTE5IDIwLjQwNDEgMTMuNDY1NSAyMi41MDU2IDEwLjcxNjcgMjIuNTA1NkM3LjM4Mjk2IDIyLjUwNTYgNC4zMDE5NiAyMC4wNjI5IDMuOTM3NDYgMTUuNjA5NEMzLjU3Mjk2IDExLjE1NjYgMy41MjY0NiA3LjUwNzEyIDMuNTI2NDYgNy41MDcxMkMzLjQ2NjQ2IDYuNzIxODcgNC4wODM3MSA1LjQ2Nzg3IDUuMjYyNzEgNS40Njc4N1oiIGZpbGw9IiNGRkM4M0QiLz4KPHBhdGggZD0iTTEwLjQ2NCAzLjM3NDE4TDEwLjg2MjIgOS4yMDM5M0MxMC44NjkxIDkuMzAzMTkgMTAuODM2MiA5LjQwMTExIDEwLjc3MDkgOS40NzYxNEMxMC43MDU2IDkuNTUxMTggMTAuNjEzMSA5LjU5NzE5IDEwLjUxMzggOS42MDQwNkMxMC40MTQ2IDkuNjEwOTIgMTAuMzE2NyA5LjU3ODA3IDEwLjI0MTYgOS41MTI3NEMxMC4xNjY2IDkuNDQ3NCAxMC4xMjA2IDkuMzU0OTQgMTAuMTEzNyA5LjI1NTY4TDkuNzUwNyAzLjkzNDQzTDkuNzczMiAzLjk3MTkzQzkuNzA3MiAyLjQxMzQzIDEwLjIzNDUgMS41OTA2OCAxMS41NDAyIDEuNTAyOTNWMS41MTE5M0MxMS4xODI1IDEuNTA4OTMgMTAuOTE0NyAxLjY3NjkzIDEwLjcyNzIgMS45ODgxOEMxMC41MzA3IDIuMzEzNjggMTAuNDI2NSAyLjc5ODE4IDEwLjQ2NCAzLjM3NDE4Wk0xMy4zODMgMy45NzI2OEMxMy4zODc1IDMuOTA3NDMgMTMuMzkwNSAzLjg0MjE4IDEzLjM5MjcgMy43Nzc2OEMxMy4yNzE2IDQuMTU1MDMgMTMuMjA1MiA0LjU0Nzc2IDEzLjE5NTUgNC45NDM5M1Y0Ljk0OTkzTDEzLjEwMTcgOS41NTQ5M0MxMy4wOTk2IDkuNjU0MzkgMTMuMTM3MSA5Ljc1MDYgMTMuMjA2IDkuODIyNEMxMy4yNzQ4IDkuODk0MjEgMTMuMzY5NCA5LjkzNTcyIDEzLjQ2ODggOS45Mzc4MUMxMy41NjgzIDkuOTM5ODkgMTMuNjY0NSA5LjkwMjM5IDEzLjczNjMgOS44MzM1NEMxMy44MDgxIDkuNzY0NjkgMTMuODQ5NiA5LjY3MDE0IDEzLjg1MTcgOS41NzA2OEwxMy45NDU1IDQuOTY3OTNDMTMuOTY1IDQuMzg4OTMgMTQuMDk3NyAzLjkwMjkzIDE0LjMxMjIgMy41NzM2OEMxNC40OSAzLjMwMDY4IDE0LjcxOTUgMy4xMzg2OCAxNS4wMTEyIDMuMTAxOTNDMTMuODIxIDMuMTE2OTMgMTMuMzgzIDMuOTcyNjggMTMuMzgzIDMuOTcyNjhaTTcuNTI2OTUgMTMuMjE3OUM4LjM5OTIgMTMuMTU1NyA5Ljc0Mzk1IDEzLjA3MzkgMTEuMTA0NSAxMy4xMzY5QzEyLjAyODUgMTMuMTc5NyAxMi45MzIyIDEzLjI4ODQgMTMuNjkyIDEzLjUwMTRDMTMuMjM0IDEzLjY1NSAxMi43ODQ5IDEzLjgzMzUgMTIuMzQ2NSAxNC4wMzYyQzEwLjk4MTUgMTQuNjY3NyA5LjI5MDk1IDE1LjczNDIgOC4wNDUyIDE3LjQ2ODlDOC4wMTQ4NiAxNy41MDg4IDcuOTkyODEgMTcuNTU0NCA3Ljk4MDM4IDE3LjYwMjlDNy45Njc5NCAxNy42NTE1IDcuOTY1MzYgMTcuNzAyIDcuOTcyNzkgMTcuNzUxNkM3Ljk4MDIzIDE3LjgwMTIgNy45OTc1MiAxNy44NDg3IDguMDIzNjUgMTcuODkxNUM4LjA0OTc4IDE3LjkzNDMgOC4wODQyMiAxNy45NzE0IDguMTI0OTQgMTguMDAwNkM4LjE2NTY1IDE4LjAyOTggOC4yMTE4MSAxOC4wNTA2IDguMjYwNjkgMTguMDYxN0M4LjMwOTU3IDE4LjA3MjggOC4zNjAxNyAxOC4wNzQgOC40MDk1MSAxOC4wNjUyQzguNDU4ODUgMTguMDU2NCA4LjUwNTkzIDE4LjAzNzggOC41NDc5NiAxOC4wMTA0QzguNTg5OTggMTcuOTgzMSA4LjYyNjExIDE3Ljk0NzcgOC42NTQyIDE3LjkwNjJDOS43OTg3IDE2LjMxMjQgMTEuMzY2MiAxNS4zMTY0IDEyLjY2MTUgMTQuNzE3MkMxMy4xODkyIDE0LjQ3MjggMTMuNzMzNSAxNC4yNjYgMTQuMjkwNSAxNC4wOTg0QzE0LjQ5MTEgMTQuMDM4MyAxNC42OTM3IDEzLjk4NTEgMTQuODk4IDEzLjkzODdMMTQuOTI4IDEzLjkzMTlMMTQuOTM1NSAxMy45MzEySDE0LjkzNjJDMTUuMDEyMSAxMy45MTY3IDE1LjA4MTcgMTMuODc5MSAxNS4xMzU0IDEzLjgyMzZDMTUuMTg5MiAxMy43NjgxIDE1LjIyNDUgMTMuNjk3MyAxNS4yMzY1IDEzLjYyMUMxNS4yNDg0IDEzLjU0NDYgMTUuMjM2NSAxMy40NjY0IDE1LjIwMjQgMTMuMzk3MUMxNS4xNjgzIDEzLjMyNzggMTUuMTEzNSAxMy4yNzA3IDE1LjA0NTcgMTMuMjMzN0MxNC4wMDkyIDEyLjY2NTIgMTIuNTMxNyAxMi40NTIyIDExLjEzOSAxMi4zODc3QzkuNzMxOTUgMTIuMzIyNCA4LjM1MTIgMTIuNDA3MiA3LjQ3Mjk1IDEyLjQ3MDJDNy4zNzU4MyAxMi40OCA3LjI4NjM2IDEyLjUyNzMgNy4yMjM1NSAxMi42MDJDNy4xNjA3MyAxMi42NzY3IDcuMTI5NTIgMTIuNzcyOSA3LjEzNjU1IDEyLjg3MDNDNy4xNDM1OCAxMi45Njc3IDcuMTg4MyAxMy4wNTg0IDcuMjYxMiAxMy4xMjM0QzcuMzM0MSAxMy4xODgzIDcuNDI5NDQgMTMuMjIyMiA3LjUyNjk1IDEzLjIxNzlaTTYuNTI3OTUgNS45ODQ5M0w2LjU5NzcgNC42Mjg5M0M2LjY1NTQ1IDMuOTE2NDMgNy40ODE5NSAzLjIwODQzIDguMjYyNyAzLjE5NDE4QzcuOTI1MiAzLjI0ODkzIDcuNzAwMiAzLjM0NzE4IDcuNTQ4NyAzLjQ2MTE4QzcuNDA2MzUgMy41NjY4NCA3LjI5ODM0IDMuNzEyMTYgNy4yMzgyIDMuODc4OTNDNy4xMDI0NSA0LjIzNjY4IDcuMTIxMiA0LjcxNzQzIDcuMTkyNDUgNS4yOTY0M0w3Ljg0MTIgMTAuMTIxOUM3Ljg0ODU2IDEwLjE3MTEgNy44NDYwNyAxMC4yMjEzIDcuODMzODcgMTAuMjY5NUM3LjgyMTY3IDEwLjMxNzcgNy44IDEwLjM2MyA3Ljc3MDEzIDEwLjQwMjdDNy43NDAyNiAxMC40NDI1IDcuNzAyNzggMTAuNDc1OSA3LjY1OTg4IDEwLjUwMUM3LjYxNjk3IDEwLjUyNjEgNy41Njk1IDEwLjU0MjUgNy41MjAyMiAxMC41NDkxQzcuNDcwOTMgMTAuNTU1NyA3LjQyMDgzIDEwLjU1MjUgNy4zNzI4MSAxMC41Mzk1QzcuMzI0OCAxMC41MjY2IDcuMjc5ODQgMTAuNTA0MyA3LjI0MDU0IDEwLjQ3MzhDNy4yMDEyNSAxMC40NDMzIDcuMTY4NDEgMTAuNDA1MyA3LjE0MzkzIDEwLjM2MjFDNy4xMTk0NSAxMC4zMTg4IDcuMTAzODIgMTAuMjcxMSA3LjA5Nzk1IDEwLjIyMTdMNi41Mjc5NSA1Ljk4NDkzWiIgZmlsbD0iI0Q2N0QwMCIvPgo8cGF0aCBkPSJNMTAuNDY0IDMuMzc0MThMMTAuODYyMiA5LjIwMzkzQzEwLjg2OTEgOS4zMDMxOSAxMC44MzYyIDkuNDAxMTEgMTAuNzcwOSA5LjQ3NjE0QzEwLjcwNTYgOS41NTExOCAxMC42MTMxIDkuNTk3MTkgMTAuNTEzOCA5LjYwNDA2QzEwLjQxNDYgOS42MTA5MiAxMC4zMTY3IDkuNTc4MDcgMTAuMjQxNiA5LjUxMjc0QzEwLjE2NjYgOS40NDc0IDEwLjEyMDYgOS4zNTQ5NCAxMC4xMTM3IDkuMjU1NjhMOS43NTA3IDMuOTM0NDNMOS43NzMyIDMuOTcxOTNDOS43MDcyIDIuNDEzNDMgMTAuMjM0NSAxLjU5MDY4IDExLjU0MDIgMS41MDI5M1YxLjUxMTkzQzExLjE4MjUgMS41MDg5MyAxMC45MTQ3IDEuNjc2OTMgMTAuNzI3MiAxLjk4ODE4QzEwLjUzMDcgMi4zMTM2OCAxMC40MjY1IDIuNzk4MTggMTAuNDY0IDMuMzc0MThaTTEzLjM4MyAzLjk3MjY4QzEzLjM4NzUgMy45MDc0MyAxMy4zOTA1IDMuODQyMTggMTMuMzkyNyAzLjc3NzY4QzEzLjI3MTYgNC4xNTUwMyAxMy4yMDUyIDQuNTQ3NzYgMTMuMTk1NSA0Ljk0MzkzVjQuOTQ5OTNMMTMuMTAxNyA5LjU1NDkzQzEzLjA5OTYgOS42NTQzOSAxMy4xMzcxIDkuNzUwNiAxMy4yMDYgOS44MjI0QzEzLjI3NDggOS44OTQyMSAxMy4zNjk0IDkuOTM1NzIgMTMuNDY4OCA5LjkzNzgxQzEzLjU2ODMgOS45Mzk4OSAxMy42NjQ1IDkuOTAyMzkgMTMuNzM2MyA5LjgzMzU0QzEzLjgwODEgOS43NjQ2OSAxMy44NDk2IDkuNjcwMTQgMTMuODUxNyA5LjU3MDY4TDEzLjk0NTUgNC45Njc5M0MxMy45NjUgNC4zODg5MyAxNC4wOTc3IDMuOTAyOTMgMTQuMzEyMiAzLjU3MzY4QzE0LjQ5IDMuMzAwNjggMTQuNzE5NSAzLjEzODY4IDE1LjAxMTIgMy4xMDE5M0MxMy44MjEgMy4xMTY5MyAxMy4zODMgMy45NzI2OCAxMy4zODMgMy45NzI2OFpNNy41MjY5NSAxMy4yMTc5QzguMzk5MiAxMy4xNTU3IDkuNzQzOTUgMTMuMDczOSAxMS4xMDQ1IDEzLjEzNjlDMTIuMDI4NSAxMy4xNzk3IDEyLjkzMjIgMTMuMjg4NCAxMy42OTIgMTMuNTAxNEMxMy4yMzQgMTMuNjU1IDEyLjc4NDkgMTMuODMzNSAxMi4zNDY1IDE0LjAzNjJDMTAuOTgxNSAxNC42Njc3IDkuMjkwOTUgMTUuNzM0MiA4LjA0NTIgMTcuNDY4OUM4LjAxNDg2IDE3LjUwODggNy45OTI4MSAxNy41NTQ0IDcuOTgwMzggMTcuNjAyOUM3Ljk2Nzk0IDE3LjY1MTUgNy45NjUzNiAxNy43MDIgNy45NzI3OSAxNy43NTE2QzcuOTgwMjMgMTcuODAxMiA3Ljk5NzUyIDE3Ljg0ODcgOC4wMjM2NSAxNy44OTE1QzguMDQ5NzggMTcuOTM0MyA4LjA4NDIyIDE3Ljk3MTQgOC4xMjQ5NCAxOC4wMDA2QzguMTY1NjUgMTguMDI5OCA4LjIxMTgxIDE4LjA1MDYgOC4yNjA2OSAxOC4wNjE3QzguMzA5NTcgMTguMDcyOCA4LjM2MDE3IDE4LjA3NCA4LjQwOTUxIDE4LjA2NTJDOC40NTg4NSAxOC4wNTY0IDguNTA1OTMgMTguMDM3OCA4LjU0Nzk2IDE4LjAxMDRDOC41ODk5OCAxNy45ODMxIDguNjI2MTEgMTcuOTQ3NyA4LjY1NDIgMTcuOTA2MkM5Ljc5ODcgMTYuMzEyNCAxMS4zNjYyIDE1LjMxNjQgMTIuNjYxNSAxNC43MTcyQzEzLjE4OTIgMTQuNDcyOCAxMy43MzM1IDE0LjI2NiAxNC4yOTA1IDE0LjA5ODRDMTQuNDkxMSAxNC4wMzgzIDE0LjY5MzcgMTMuOTg1MSAxNC44OTggMTMuOTM4N0wxNC45MjggMTMuOTMxOUwxNC45MzU1IDEzLjkzMTJIMTQuOTM2MkMxNS4wMTIxIDEzLjkxNjcgMTUuMDgxNyAxMy44NzkxIDE1LjEzNTQgMTMuODIzNkMxNS4xODkyIDEzLjc2ODEgMTUuMjI0NSAxMy42OTczIDE1LjIzNjUgMTMuNjIxQzE1LjI0ODQgMTMuNTQ0NiAxNS4yMzY1IDEzLjQ2NjQgMTUuMjAyNCAxMy4zOTcxQzE1LjE2ODMgMTMuMzI3OCAxNS4xMTM1IDEzLjI3MDcgMTUuMDQ1NyAxMy4yMzM3QzE0LjAwOTIgMTIuNjY1MiAxMi41MzE3IDEyLjQ1MjIgMTEuMTM5IDEyLjM4NzdDOS43MzE5NSAxMi4zMjI0IDguMzUxMiAxMi40MDcyIDcuNDcyOTUgMTIuNDcwMkM3LjM3NTgzIDEyLjQ4IDcuMjg2MzYgMTIuNTI3MyA3LjIyMzU1IDEyLjYwMkM3LjE2MDczIDEyLjY3NjcgNy4xMjk1MiAxMi43NzI5IDcuMTM2NTUgMTIuODcwM0M3LjE0MzU4IDEyLjk2NzcgNy4xODgzIDEzLjA1ODQgNy4yNjEyIDEzLjEyMzRDNy4zMzQxIDEzLjE4ODMgNy40Mjk0NCAxMy4yMjIyIDcuNTI2OTUgMTMuMjE3OVpNNi41Mjc5NSA1Ljk4NDkzTDYuNTk3NyA0LjYyODkzQzYuNjU1NDUgMy45MTY0MyA3LjQ4MTk1IDMuMjA4NDMgOC4yNjI3IDMuMTk0MThDNy45MjUyIDMuMjQ4OTMgNy43MDAyIDMuMzQ3MTggNy41NDg3IDMuNDYxMThDNy40MDYzNSAzLjU2Njg0IDcuMjk4MzQgMy43MTIxNiA3LjIzODIgMy44Nzg5M0M3LjEwMjQ1IDQuMjM2NjggNy4xMjEyIDQuNzE3NDMgNy4xOTI0NSA1LjI5NjQzTDcuODQxMiAxMC4xMjE5QzcuODQ4NTYgMTAuMTcxMSA3Ljg0NjA3IDEwLjIyMTMgNy44MzM4NyAxMC4yNjk1QzcuODIxNjcgMTAuMzE3NyA3LjggMTAuMzYzIDcuNzcwMTMgMTAuNDAyN0M3Ljc0MDI2IDEwLjQ0MjUgNy43MDI3OCAxMC40NzU5IDcuNjU5ODggMTAuNTAxQzcuNjE2OTcgMTAuNTI2MSA3LjU2OTUgMTAuNTQyNSA3LjUyMDIyIDEwLjU0OTFDNy40NzA5MyAxMC41NTU3IDcuNDIwODMgMTAuNTUyNSA3LjM3MjgxIDEwLjUzOTVDNy4zMjQ4IDEwLjUyNjYgNy4yNzk4NCAxMC41MDQzIDcuMjQwNTQgMTAuNDczOEM3LjIwMTI1IDEwLjQ0MzMgNy4xNjg0MSAxMC40MDUzIDcuMTQzOTMgMTAuMzYyMUM3LjExOTQ1IDEwLjMxODggNy4xMDM4MiAxMC4yNzExIDcuMDk3OTUgMTAuMjIxN0w2LjUyNzk1IDUuOTg0OTNaIiBzdHJva2U9IiNENjdEMDAiLz4KPC9zdmc+Cg==)}.box-caution b,.box-caution strong{color:#a75000}" +
            ".tooltip-handbook{position:relative;z-index:2;display:inline-block;vertical-align: text-top;}.tooltip-handbook:hover{z-index:5}.tooltip-handbook:hover .data-tooltip-handbook,.tooltip-handbook:hover:after{opacity:1;visibility:visible}.tooltip-handbook .data-tooltip-handbook{content:attr(data-tooltip-handbook);background:#333;border-radius:5px;color:#fff;font-size:13px!important;font-weight:400;line-height:15.23px!important;padding:12px;max-width: 400px;}" +
            "hr {border-top: 1px solid #E6E6E6; margin: 10px 0;}.popup-handbook{display:inline-block;position:relative}.popup-handbook>span{color:#3873e9;text-decoration:underline;cursor:pointer}.popup-handbook.active .overlay{display:flex;background:rgba(0,0,0,.5);z-index:10}.popup-handbook .overlay{display:flex;justify-content:center;align-items:center}.popup-handbook .overlay .content-popup-handbook{margin:0;position:relative;background:#fff;border:1px solid #d1d1d1;padding:11px;min-width:400px}.popup-handbook .overlay .content-popup-handbook p{font-size:13px;line-height:1.17;margin:0 0 5px}.popup-handbook .overlay .content-popup-handbook ul{margin:0;list-style:disc;padding:0 0 0 24px}.popup-handbook .overlay .content-popup-handbook ul li{font-size:13px;line-height:1.17;margin:0}.popup-handbook .overlay .content-popup-handbook ul li{margin-bottom:5px!important}" +
            "table.table-no-border{	width: auto!important;	height: auto!important;	margin: 0!important;}table.table-no-border tr th, table.table-no-border tr td{	text-align: left;	vertical-align: top;	padding: 0;	font-weight: normal;width: auto!important;height: uto!important;font-size: 16px;line-height: 26px;}" +
            "",

          formats: {
            removeformat: [
              {
                selector: "u,strike,h1,h2,h3,h4,h5,h6",
                remove: "all",
                split: true,
                expand: false,
                block_expand: true,
                deep: true,
              },
              {
                selector: "*",
                attributes: ["style", "class"],
                split: false,
                expand: false,
                deep: true,
              },
            ],
          },
          templates: [
            {
              title: "Message note",
              description: "Note",
              content:
                '<div class="box box-note">\
                  <div class="box-title">Note</div>\
                  <div class="box-content">\
                    <p>Input note</p>\
                  </div>\
                </div>\
               <p></p>',
            },
            {
              title: "Message Tip",
              description: "Tip",
              content:
                '<div class="box box-tip">\
                  <div class="box-title">Note</div>\
                  <div class="box-content">\
                      <p>Auto Transfer and <b>All Day Balance Transfer</b> cannot be used at the same time.</p>\
                      <ul>\
                          <li>If All Day Balance Transfer is enabled, Auto Transfer will be unavailable.</li>\
                          <li>Conversely, if All Day Balance Transfer is disabled, Auto Transfer will be available</li>\
                      </ul>\
                      <p>and now you can either turn on or turn off Auto Transfer for your downlines.</p>\
                  </div>\
              </div>\
              <p></p>',
            },
            {
              title: "Message Caution",
              description: "Caution",
              content:
                '<div class="box box-caution">\
                  <div class="box-title">Note</div>\
                  <div class="box-content">\
                      <p>Auto Transfer and <b>All Day Balance Transfer</b> cannot be used at the same time.</p>\
                      <ul>\
                          <li>If All Day Balance Transfer is enabled, Auto Transfer will be unavailable.</li>\
                          <li>Conversely, if All Day Balance Transfer is disabled, Auto Transfer will be available</li>\
                      </ul>\
                      <p>and now you can either turn on or turn off Auto Transfer for your downlines.</p>\
                  </div>\
              </div>\
              <p></p>',
            },
            {
              title: "Tooltip",
              description: "Tooltip",
              content:
                '<div class="tooltip-handbook">Text display<div class="data-tooltip-handbook">Text show in tooltip</div></div><p></p>',
            },
            {
              title: "Popup",
              description: "Popup",
              content:
                '<div class="popup-handbook" onclick=this.classList.toggle("active")><span>text popup</span>\
                                <div class="overlay">\
                                    <div class="content-popup-handbook">\
                                        <p>\
                                            <b>and now you can either turn on or turn off Auto Transfer for your downlines.</b>\
                                        </p>\
                                        <hr />\
                                        <p>\
                                            and now you can either turn on or turn off Auto Transfer for your downlines.\
                                        </p>\
                                        <ul>\
                                            <li>If All Day Balance Transfer is enabled, Auto Transfer will be unavailable.</li>\
                                            <li>Conversely, if All Day Balance Transfer is disabled, Auto Transfer will be available</li>\
                                        </ul>\
                                    </div>\
                                </div>\
                            </div><p></p>',
            },
            {
              title: "table no border",
              description: "table no border",
              content:
                '<table class="table-no-border">\
                  <tr>\
                    <th>col 1</th>\
                    <th>col 2</th>\
                  </tr>\
                  <tr>\
                    <th>col 1</th>\
                    <th>col 2</th>\
                  </tr>\
                  <tr>\
                    <th>col 1</th>\
                    <th>col 2</th>\
                  </tr>\
                </table><p></p>',
            },
          ],
          paste_data_images: true,
          charmap_append: [
            [0x2600, "sun"],
            [0x2601, "cloud"],
            [0x2713, "check"],
            [0x2192, "rarrow"],
            [0x2265, "greaterorequal"],
            [0x2264, "lessorequal"],
            [0x2260, "notequal"],
            [0x2b9a, "bwarrow"],
          ],
        },
      },
    },
  },
});

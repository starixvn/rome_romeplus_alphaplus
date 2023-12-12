import React from 'react';
import { useCMEditViewDataManager } from '@strapi/helper-plugin';
import { More  } from '@strapi/icons';
import { Button } from '@strapi/design-system';
import configs from '../../configs';

function toDataUrl(url, callback) {
  var xhr = new XMLHttpRequest();
  xhr.onload = function() {
      var reader = new FileReader();
      reader.onloadend = function() {
          callback(reader.result);
      }
      reader.readAsDataURL(xhr.response);
  };
  xhr.open('GET', url);
  xhr.responseType = 'blob';
  xhr.send();
}

function formatArticle() {
  var editor = document.getElementsByClassName("tox-edit-area")[0];
  var iframe = editor.getElementsByTagName('iframe')[0];

  var imgs = iframe.contentWindow.document.getElementsByTagName("img");
  for(var i=0, len = imgs.length; i<len; i++) {
    var img = imgs[i];
    var src = img.src;

    if(src.indexOf('/uploads/') === -1) {
      continue;
    }

    img.setAttribute('crossOrigin', 'anonymous');
    //var base64 = getBase64Image(img);

    var count = 0;

    toDataUrl(src, function(myBase64) {
      // img.setAttribute("data-mce-src", myBase64); // myBase64 is the base64 string

      var div = iframe.contentWindow.document.createElement('div');
      div.className = "div-img";
      div.style = "background-image: url('" + myBase64 +"'); width: " + img.width +"px; height: " + img.height + "px;";
      img.replaceWith(div, img);
      count++;

      if(count === len) {
        saveArticle(iframe.contentWindow.document.getElementsByTagName("body")[0], imgs);
      }
    });
  }
}

function saveArticle(editor, imgs) {
  try{
    for(var i=0; i<imgs.length; i++) {
      var img = imgs[i];
      img.remove();
    }

    tinymce.activeEditor.setDirty(true);

    editor.focus();

    var n = document.getElementById('name');
    n.focus();

    setTimeout(() => {
      var bs = document.getElementsByTagName('button');
      for(var i=0; i<bs.length; i++) {
        var b = bs[i];
        var t = b.innerText;
        if(t === 'Save') {
          b.focus();
          b.click();
          location.reload(true);
        }
      }
    }, 1000);
  } catch(e) {
    console.log(e);
  }
} 

const ArticleFormat = () => {
  const {slug} = useCMEditViewDataManager();
  var a = useCMEditViewDataManager();

  if (!configs.autoSaveModels.some(c => c===slug)) {
    return null;
  }

  return (
    <Button 
      size="S"
      startIcon={<More  />}
      onClick={() => formatArticle()}
    >
      Article Format
    </Button>
  );
};

export default ArticleFormat;

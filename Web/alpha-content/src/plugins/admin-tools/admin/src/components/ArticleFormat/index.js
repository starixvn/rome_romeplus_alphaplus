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
  for(var i=0; i<imgs.length; i++) {
    var img = imgs[i];
    var src = img.src;

    if(src.indexOf('/uploads/') === -1) {
      continue;
    }

    img.setAttribute('crossOrigin', 'anonymous');
    //var base64 = getBase64Image(img);

    toDataUrl(src, function(myBase64) {
      // img.setAttribute("data-mce-src", myBase64); // myBase64 is the base64 string

      var div = iframe.contentWindow.document.createElement('div');
      div.style = "background-image: url('" + myBase64 +"');";
      img.replaceWith(div, img);
    });
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

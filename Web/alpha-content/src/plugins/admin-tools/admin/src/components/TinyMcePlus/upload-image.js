import {getEditorToken} from './security';

function getUploadImageButton() {
  return '<button id="btn-upload-image" aria-label="Upload Image" title="Upload Image" type="button" tabindex="-1" \
            class="tox-tbtn" aria-disabled="false" style="width: 34px;" >\
            <span class="tox-icon tox-tbtn__icon-wrap icon-upload-image"></span>\
          </button>';
}

function uploadImage(adminUrl) {
  var uploadContainer = document.getElementById('upload-image-container');
  if(uploadContainer) {
    uploadContainer.remove();
  }

  var c = document.createElement('div');
  c.innerHTML  = '<div id="upload-image-container">\
                    <div id="form-upload-image">\
                      <input type="file" id="file-upload" accept="image/*" />\
                      <button id="btn-cancel-upload-image">Cancel</button>\
                      <button id="btn-submit-upload-image">Submit</button>\
                    </div>\
                  </div>';
  document.getElementById('main-content').append(c.firstChild);

  document.getElementById('file-upload').onchange = (e) => { selectFile(e); };
  document.getElementById('btn-cancel-upload-image').onclick = (e) => { cancelUpload(e); };
  document.getElementById('btn-submit-upload-image').onclick = (e) => { submitUpload(e, adminUrl); };
}

var selectedFile = null;

function selectFile(e) {
  const files = event.srcElement.files;
  selectedFile = files[0];
}

function cancelUpload(e) {
  document.getElementById('upload-image-container').remove();
  e.preventDefault();
  return false;
}

function appendDivImage(img, base64) {
  var width = img.width;
  var height = img.height;
    
  var html = '<div id="' + selectedFile.name + '" data-image-name="' + selectedFile.name + '"><div class="div-img" style="background-image: url(\'' + base64 + '\');  width:' + width + 'px; height:' + height + 'px;"></div></div>';
  tinyMCE.execCommand('mceInsertContent', false, html);
      
  var editor = document.getElementsByClassName("tox-edit-area")[0];
  var iframe = editor.getElementsByTagName('iframe')[0];
  
  var d = iframe.contentWindow.document.getElementById(selectedFile.name);

  var prev = d.previousSibling;
  var nx = d.nextSibling;

  if(prev) {
    var t1 = iframe.contentWindow.document.createElement('div');
    t1.innerHTML = prev.innerHTML;
    d.prepend(t1.firstChild);
    prev.remove();
  }

  if(nx) {
    var t2 = iframe.contentWindow.document.createElement('div');
    t2.innerHTML = nx.innerHTML;
    d.append(t2.firstChild);
    nx.remove();
  }
}


function appendImage(img, adminUrl) {
      // var p = iframe.contentWindow.document.createElement('p');
      // p.innerHTML = html;
      // iframe.contentWindow.document.body.append(p);

      // tinyMCE.init({paste_data_images : true});
      var width = img.width;
      var height = img.height;
      var name = selectedFile.name;

      var html = '<image src="' + adminUrl + '/api/images/get/' + name + '" style="width:' + width + 'px; height:' + height + 'px;" />';
      tinyMCE.execCommand('mceInsertContent', false, html);
}

function uploadImageToServer(img, base64, adminUrl) {
  var xhttp = new XMLHttpRequest();
  xhttp.onreadystatechange = function() {
      if (this.readyState != 4 || this.status != 200) {
        return;
      }

      if(this.responseText) {
        alert(this.responseText);
        //return;
      }

      appendImage(img, adminUrl);
  };

  var width = img.width;
  var height = img.height;
  xhttp.open("PUT", `${strapi.backendURL}/api/images/upload`, true);

  var data = JSON.stringify({
    width, 
    height, 
    base64, 
    name: selectedFile.name,
    token: getEditorToken()
  });

  xhttp.send(data);
}

function submitUpload(e, adminUrl) {
  e.preventDefault();

  if(!selectedFile) {
    return false;
  }

  const reader = new FileReader();
  reader.readAsDataURL(selectedFile);
  reader.onload = function (evt) {
    var base64 = evt.target.result;
    var image = new Image();
    image.src = base64;
    image.onload = function () {
      // appendDivImage(this, base64);
      uploadImageToServer(this, base64, adminUrl);

      document.getElementById('upload-image-container').remove();
    }
  }

  return false;
}

export { getUploadImageButton, uploadImage };
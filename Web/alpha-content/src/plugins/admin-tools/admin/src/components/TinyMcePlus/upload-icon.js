import {getEditorToken} from './security';

function getUploadIconButton() {
  return '<button id="btn-upload-icon" aria-label="Upload Icon" title="Upload Icon" type="button" tabindex="-1" \
            class="tox-tbtn" aria-disabled="false" style="width: 34px;" >\
            <span class="tox-icon tox-tbtn__icon-wrap icon-upload-icon"></span>\
          </button>';
}

function uploadIcon(adminUrl) {
  var uploadContainer = document.getElementById('upload-icon-container');
  if(uploadContainer) {
    uploadContainer.remove();
  }

  var c = document.createElement('div');
  c.innerHTML  = '<div id="upload-icon-container">\
                    <div id="form-upload-icon">\
                      <input type="file" id="file-upload" accept="image/*" />\
                      <button id="btn-cancel-upload-icon">Cancel</button>\
                      <button id="btn-submit-upload-icon">Submit</button>\
                    </div>\
                  </div>';
  document.getElementById('main-content').append(c.firstChild);

  document.getElementById('file-upload').onchange = (e) => { selectFile(e); };
  document.getElementById('btn-cancel-upload-icon').onclick = (e) => { cancelUpload(e); };
  document.getElementById('btn-submit-upload-icon').onclick = (e) => { submitUpload(e, adminUrl); };
}

var selectedFile = null;

function selectFile(e) {
  const files = event.srcElement.files;
  selectedFile = files[0];
}

function cancelUpload(e) {
  document.getElementById('upload-icon-container').remove();
  e.preventDefault();
  return false;
}

function appendIcon(img, adminUrl) {
      var width = img.width;
      var height = img.height;
      var name = selectedFile.name;

      var html = '<image src="' + adminUrl + '/api/images/get/' + name + '" style="display: inline-block; vertical-align: baseline; width:' + width + 'px; height:' + height + 'px;" />';
      tinyMCE.execCommand('mceInsertContent', false, html);
}

function uploadIconToServer(img, base64, adminUrl) {
  var xhttp = new XMLHttpRequest();
  xhttp.onreadystatechange = function() {
      if (this.readyState != 4 || this.status != 200) {
        return;
      }

      if(this.responseText) {
        alert(this.responseText);
        //return;
      }

      appendIcon(img, adminUrl);
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
      uploadIconToServer(this, base64, adminUrl);

      document.getElementById('upload-icon-container').remove();
    }
  }

  return false;
}

export { getUploadIconButton, uploadIcon };
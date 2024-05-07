import {getEditorToken} from './security';

function getUpdateImageButton() {
  return '<button id="btn-update-image" aria-label="Update Image" title="Update Image" type="button" tabindex="-1" \
            class="tox-tbtn" aria-disabled="false" style="width: 34px;" >\
            <span class="tox-icon tox-tbtn__icon-wrap icon-update-image"></span>\
          </button>';
}

function updateImage(adminUrl) {
  var uploadContainer = document.getElementById('update-image-container');
  if(uploadContainer) {
    uploadContainer.remove();
  }

  var c = document.createElement('div');
  c.innerHTML  = '<div id="update-image-container">\
                    <div id="form-update-image">\
                      <input type="file" id="file-update" accept="image/*" />\
                      <button id="btn-cancel-update-image">Cancel</button>\
                      <button id="btn-submit-update-image">Submit</button>\
                    </div>\
                  </div>';
  document.getElementById('main-content').append(c.firstChild);

  document.getElementById('file-update').onchange = (e) => { selectFile(e); };
  document.getElementById('btn-cancel-update-image').onclick = (e) => { cancelUpdate(e); };
  document.getElementById('btn-submit-update-image').onclick = (e) => { submitUpdate(e, adminUrl); };
}

var selectedFile = null;

function selectFile(e) {
  const files = event.srcElement.files;
  selectedFile = files[0];
}

function cancelUpdate(e) {
  document.getElementById('update-image-container').remove();
  e.preventDefault();
  return false;
}

function updateImageToServer(img, base64, adminUrl) {
  var xhttp = new XMLHttpRequest();
  xhttp.onreadystatechange = function() {
      if (this.readyState != 4 || this.status != 200) {
        return;
      }

      if(this.responseText) {
        alert(this.responseText);
      }
  };

  var width = img.width;
  var height = img.height;
  xhttp.open("PUT", `${strapi.backendURL}/api/images/update`, true);

  var data = JSON.stringify({
    width, 
    height, 
    base64, 
    name: selectedFile.name,
    token: getEditorToken()
  });

  xhttp.send(data);
}

function submitUpdate(e, adminUrl) {
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
      updateImageToServer(this, base64, adminUrl);

      document.getElementById('update-image-container').remove();
    }
  }

  return false;
}

export { getUpdateImageButton, updateImage };
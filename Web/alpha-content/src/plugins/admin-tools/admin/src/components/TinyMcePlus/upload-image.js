function getUploadImageButton() {
  return '<button id="btn-upload-image" aria-label="Upload Image" title="Upload Image" type="button" tabindex="-1" \
            class="tox-tbtn" aria-disabled="false" style="width: 34px;" >\
            <span class="tox-icon tox-tbtn__icon-wrap icon-upload-image"\
            style="width: 24px; height: 24px; background-image: url(\'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAMAAADXqc3KAAAAKlBMVEUAAABMaXEAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAB41qtLAAAADXRSTlOAACBBYHdTv/MM3p7S/Nts0gAAAAlwSFlzAAAOwwAADsMBx2+oZAAAAJJJREFUeJyNkVkSxCAIRGlcY6bvf90pR0lckqrpHykfgjSCFwmgQUYF7UAzZ+UfEQS2FJMyNCCsZ+wVAFAG4MnyCE7yqsgRuMz0+ALqgOOMOwBwJDIuwNX8VIeIE3BkbPekG4GQ/PS5ZQUmge+WbMDc3UDxd/PB9bv56u4Fxn14FAvmDSq8CFMN7BMPpf4HNuCLvprACMjqmk8UAAAAAElFTkSuQmCC\');"></span>\
          </button>';
}

function uploadImage() {
  var uploadContainer = document.getElementById('upload-image-container');
  if(uploadContainer) {
    uploadContainer.remove();
  }

  var c = document.createElement('div');
  c.innerHTML  = '<div></div>';
  document.getElementById('main-content').append(c.firstChild);
}

export { getUploadImageButton, uploadImage };
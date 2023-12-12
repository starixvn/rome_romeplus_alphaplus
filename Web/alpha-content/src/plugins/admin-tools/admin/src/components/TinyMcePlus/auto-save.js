function getAutoSaveButton() {
  return '<button id="btn-auto-save" aria-label="Auto Save" title="Auto Save" type="button" tabindex="-1" \
            class="tox-tbtn" aria-disabled="false" style="width: 34px;" >\
            <span class="tox-icon tox-tbtn__icon-wrap icon-auto-save"></span>\
          </button>';
}

var saveTimeInMinutes = 5;
var autoSaveInterval = 0;
function autoSave() {
  if(autoSaveInterval) {
    clearInterval(autoSaveInterval);
    alert("Auto Save is Off");
    return;
  }

  alert("Auto Save for every " + saveTimeInMinutes + " mins.");
  autoSaveInterval = setInterval(() =>{
    try{
      var bs = document.getElementsByTagName('button');
      for(var i=0; i<bs.length; i++) {
        var b = bs[i];
        var t = b.innerText;
        if(t === 'Save') {
          b.click();
          location.reload(true);
        }
      }
    } catch(e) {
      console.log(e);
    }
  }, saveTimeInMinutes * 60 * 1000);
}

export { getAutoSaveButton, autoSave };
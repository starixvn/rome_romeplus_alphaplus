function getAutoSaveButton() {
  return '<button id="btn-auto-save" aria-label="Auto Save" title="Auto Save" type="button" tabindex="-1" \
            class="tox-tbtn" aria-disabled="false" style="width: 34px;" >\
            <span class="tox-icon tox-tbtn__icon-wrap icon-auto-save"\
            style="width: 24px; height: 24px; background-image: url(\'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAMAAADXqc3KAAAAgVBMVEVMaXELCwsRDxAAAAAJCAgAAAAAAAAAAAAKCgoODg4LCgoODQ4AAAAAAAAAAAAcGRohHR4LCwsiHiAiHyAAAAAeHB0AAAAUEhIfGhshHR4iHh8AAAAiHh9xbm8oJCX8/PxYVVY3NDXs7OyXlpaDgIFKRkeurK27uruGhIVwbm58ensT0pdNAAAAG3RSTlMADl+kM9tA+CQFdYm/7qCY9hW1QLapxFLgvdHvvCFnAAAACXBIWXMAAA7DAAAOwwHHb6hkAAABGElEQVR4nHWS6XKDMAyEFzD4CneuVgInQCG8/wt2TFwmmWn25641kj8JCMqKVMq0yPAudaCgg3r1s5xkEgsRJ5LylyIh94fqQFL8+VFKiYYyTWMFdEJpFAJLZ60tbyojbUyojqRUOD195i+9dzBUIOZ5WpbxMXVs9uBImb7yMDu3jLPjKgJ8EyFyKWLmYRnHcZxdzzGyI4CSiOSJeRinaZoerudSpxRvs5IpmYe7F7ueG0uU18CFUr0F67quvev5KomoAJAI+B73tes6H9g6JVUD0IDyQReCVst8H7nh4fbj5fpvZOSnCuyqW1AlUFD4pLEKbfUkUl2gpAwUPUSojdaXgD6TfcOOqG1rvGH/uKjPq/18DP+czy8NBBzW5MgNZAAAAABJRU5ErkJggg==\');"></span>\
          </button>';
}

var autoSaveInterval = 0;
function autoSave() {
  if(autoSaveInterval) {
    clearInterval(autoSaveInterval);
    alert("Auto Save is Off");
    return;
  }

  alert("Auto Save is On");
  autoSaveInterval = setInterval(() =>{
    try{
      var bs = document.getElementsByTagName('button');
      for(var i=0; i<bs.length; i++) {
        var b = bs[i];
        var t = b.innerText;
        if(t === 'Save') {
          b.click();
          location.reload();
        }
      }
    } catch(e) {
      console.log(e);
    }
  }, 30 * 60 * 1000);
}

export { getAutoSaveButton, autoSave };
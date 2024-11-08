import {getEditorToken} from './security';

function getStopSupportButton() {
  return '<button id="btn-stop-support" aria-label="Stop Support" title="Stop Support" type="button" tabindex="-1" \
            class="tox-tbtn" aria-disabled="false" style="width: 34px;" >\
            <span class="tox-icon tox-tbtn__icon-wrap icon-stop-support"></span>\
          </button>';
}

async function stopSupport(initialData) {
  const { vuid } = initialData;
  var xhttp = new XMLHttpRequest();
  xhttp.onreadystatechange = function() {
      if (this.readyState == 4 && this.status == 200) {
        alert(this.responseText);
        location.reload(true);
      }
  };

  xhttp.open("PUT", `${strapi.backendURL}/api/articles/stop-support/${vuid}`, true);

  var data = JSON.stringify({ token: getEditorToken() });
  xhttp.send(data);
}

export { getStopSupportButton, stopSupport };
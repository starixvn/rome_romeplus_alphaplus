import {getEditorToken} from './security';

function getFixDataButton() {
  return '<button id="btn-fix-data" aria-label="Fix Data" title="Fix Data" type="button" tabindex="-1" \
            class="tox-tbtn" aria-disabled="false" style="width: 34px;" >\
            <span class="tox-icon tox-tbtn__icon-wrap icon-fix-data"></span>\
          </button>';
}

async function fixData() {
  var xhttp = new XMLHttpRequest();
  xhttp.onreadystatechange = function() {
      if (this.readyState == 4 && this.status == 200) {
        console.log("Done");
        location.reload(true);
      }
  };

  xhttp.open("PUT", `${strapi.backendURL}/api/articles/fix-data/all`, true);

  var data = JSON.stringify({ token: getEditorToken() });
  xhttp.send(data);
}

export { getFixDataButton, fixData };
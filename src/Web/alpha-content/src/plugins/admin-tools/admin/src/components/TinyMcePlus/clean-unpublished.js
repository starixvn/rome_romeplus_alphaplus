import {getEditorToken} from './security';

function getCleanUnpublishedButton() {
  return '<button id="btn-clean-unpublished" aria-label="Keep 10 latest unpublished, remove others unpublished" title="Keep 10 latest unpublished, remove others unpublished" type="button" tabindex="-1" \
            class="tox-tbtn" aria-disabled="false" style="width: 34px;" >\
            <span class="tox-icon tox-tbtn__icon-wrap icon-clean-unpublished"></span>\
          </button>';
}

async function cleanUnpublished(initialData) {
  const { vuid } = initialData;
  var xhttp = new XMLHttpRequest();
  xhttp.onreadystatechange = function() {
      if (this.readyState == 4 && this.status == 200) {
        console.log("Done");
        location.reload(true);
      }
  };

  xhttp.open("PUT", `${strapi.backendURL}/api/articles/clean-unpublished/${vuid}`, true);

  var data = JSON.stringify({ token: getEditorToken() });
  xhttp.send(data);
}

export { getCleanUnpublishedButton, cleanUnpublished };
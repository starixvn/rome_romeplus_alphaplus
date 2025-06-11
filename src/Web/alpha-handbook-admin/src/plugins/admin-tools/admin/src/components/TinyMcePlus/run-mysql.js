import {getEditorToken} from './security';

function getRunMySQLButton() {
  return '<button id="btn-run-mysql" aria-label="Run MyQL" title="Run MyQL" type="button" tabindex="-1" \
            class="tox-tbtn" aria-disabled="false" style="width: 34px;" >\
            <span class="tox-icon tox-tbtn__icon-wrap icon-run-mysql"></span>\
          </button>';
}

async function showRunMySQL() {
  var container = document.getElementById('container-run-mysql');
  if(container) {
    container.remove();
  }

  var c = document.createElement('div');
  c.innerHTML  = '<div id="container-run-mysql">\
                    <div id="form-run-mysql">\
                      <textarea type="text" id="txt-run-mysql" value=""></textarea>\
                      <button id="btn-cancel-run-mysql" class="btn-cancel">Cancel</button>\
                      <button id="btn-submit-run-mysql" class="btn-submit">Submit</button>\
                    </div>\
                  </div>';
  document.getElementById('main-content').append(c.firstChild);
  document.getElementById('btn-cancel-run-mysql').onclick = (e) => { cancelUpdate(e); };
  document.getElementById('btn-submit-run-mysql').onclick = (e) => { runMySQL(e); };
}

function cancelUpdate(e) {
  document.getElementById('container-run-mysql').remove();
  e.preventDefault();
  return false;
}

async function runMySQL() {
  var xhttp = new XMLHttpRequest();
  xhttp.onreadystatechange = function() {
      if (this.readyState == 4 && this.status == 200) {
        alert(this.responseText);
      }
  };

  xhttp.open("PUT", `${strapi.backendURL}/api/articles/run-mysql/post`, true);

  var data = JSON.stringify({ token: getEditorToken(), mysql:  document.getElementById('txt-run-mysql').value });
  xhttp.send(data);
}

export { getRunMySQLButton, showRunMySQL };
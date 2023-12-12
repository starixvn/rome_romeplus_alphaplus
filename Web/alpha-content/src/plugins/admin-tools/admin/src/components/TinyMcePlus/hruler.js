function getHRulerButton() {
  return '<button id="btn-hruler" aria-label="HRuler" title="HRuler" type="button" tabindex="-1" \
            class="tox-tbtn" aria-disabled="false" style="width: 34px;" >\
            <span class="tox-icon tox-tbtn__icon-wrap icon-hruler"></span>\
          </button>';
}

var hruler = null;
var mPos = { x:0, y:0 };

var getBoxPos = function() {
  return {
    x: hruler.getBoundingClientRect().x,
    y: hruler.getBoundingClientRect().y
  };
};

function ondragover(ev) {
  ev.preventDefault();
}


function ondragstart(ev) {
  mPos = { x: ev.x, y: ev.y };
}

function ondrop(ev) {
  ev.preventDefault();

  var box = getBoxPos();
  var y = box.y + ev.y - mPos.y + window.scrollY;
  hruler.style.top = y + "px";

  var x = box.x + ev.x - mPos.x + window.scrollX;
  hruler.style.left = x + "px";
}

function showHRuler() {
  var ruler = document.getElementById('hruler-container');
  if(!ruler) {
    var r =    document.createElement('div');
    r.innerHTML  = '<div draggable="true" id="hruler-container">\
                      <div id="hruler"></div>\
                    </div>';
    document.getElementById('main-content').append(r.firstChild);
  }

  ruler = document.getElementById('hruler-container');
  var d = ruler.style.display;

  hruler = ruler;
  hruler.style.left = "0px";
  hruler.style.top = "50%";
  
  ruler.style.display = d === '' || d === 'none' ? "block" : "none";

  hruler.ondragstart = ondragstart;
  document.body.ondrop = ondrop;
  document.body.ondragover = ondragover;
} 


export { getHRulerButton, showHRuler };
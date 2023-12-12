function getVRulerButton() {
  return '<button id="btn-vruler" aria-label="VRuler" title="VRuler" type="button" tabindex="-1" \
            class="tox-tbtn" aria-disabled="false" style="width: 34px;" >\
            <span class="tox-icon tox-tbtn__icon-wrap icon-vruler"></span>\
          </button>';
}

var vruler = null;
var mPos = { x:0, y:0 };

var getBoxPos = function() {
  return {
    x: vruler.getBoundingClientRect().x,
    y: vruler.getBoundingClientRect().y
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
  var x = box.x + ev.x - mPos.x;
  vruler.style.left = x + "px";
}

function showVRuler() {
  var ruler = document.getElementById('vruler-container');
  if(!ruler) {
    var r =    document.createElement('div');
    r.innerHTML  = '<div draggable="true" id="vruler-container">\
                      <div id="vruler"></div>\
                    </div>';
    document.getElementById('main-content').append(r.firstChild);
  }

  ruler = document.getElementById('vruler-container');
  var d = ruler.style.display;

  vruler = ruler;
  vruler.style.left = "50%";
  vruler.style.top = "0px";
  
  debugger
  ruler.style.display =  d === '' || d === 'none' ? "block" : "none";

  vruler.ondragstart = ondragstart;
  document.body.ondrop = ondrop;
  document.body.ondragover = ondragover;
} 


export { getVRulerButton, showVRuler };
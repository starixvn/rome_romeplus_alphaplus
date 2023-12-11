function getHRulerButton() {
  return '<button id="btn-hruler" aria-label="HRuler" title="HRuler" type="button" tabindex="-1" \
            class="tox-tbtn" aria-disabled="false" style="width: 34px;" >\
            <span class="tox-icon tox-tbtn__icon-wrap icon-hruler"\
            style="width: 24px; height: 24px; background-image: url(\'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAMAAADXqc3KAAAALVBMVEUBAQFMaXEBAQEDAwMAAAAFBQUMDAwAAAABAQECAgIAAAABAQEAAAAICAgAAACMbeWUAAAADnRSTlObANpw1Dkj5++3TmqIFcjXR98AAAAJcEhZcwAADsIAAA7CARUoSoAAAABlSURBVHicxdBJCoBADETRDJ3Bbq37H1faaaWIIPrJ7m2KEF9Ev4LRaUYZAol+JbMgBSGISIIarMIbeBhYRoY6quIGJmVvHapx8x1cfB0ybhOn4gucjdVXIUOOylHk9Uuef/cDmAE+SQWzKiMBmQAAAABJRU5ErkJggg==\');"></span>\
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
    r.innerHTML  = '<div draggable="true" id="hruler-container" style="display: none; width: 100%; height: 30px; position: absolute; opacity: 0.5; background-color: #e3e3e3; z-index: 2000; bottom: 0; left: 0; top: 50%; cursor: move;"><div id="hruler" style="width: 100%; height: 15px; margin: auto; text-align: center; background-image: url(\'data:image/png;base64,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\'); background-repeat: repeat-x; background-size: 50% 100%;"></div></div>';
    document.getElementById('main-content').append(r.firstChild);
  }

  ruler = document.getElementById('hruler-container');
  var d = ruler.style.display;

  hruler = ruler;
  hruler.style.left = "0px";
  hruler.style.top = "50%";
  
  ruler.style.display =  d === 'none' ? "block" : "none";

  hruler.ondragstart = ondragstart;
  document.body.ondrop = ondrop;
  document.body.ondragover = ondragover;
} 


export { getHRulerButton, showHRuler };
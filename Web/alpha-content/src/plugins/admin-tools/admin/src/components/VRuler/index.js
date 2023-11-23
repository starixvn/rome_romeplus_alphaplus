import React from 'react';
import { useCMEditViewDataManager } from '@strapi/helper-plugin';
import { Grid  } from '@strapi/icons';
import { Button } from '@strapi/design-system';
import configs from '../../configs';

var vruler = null;
var mPos = { x:0, y:0 };
var getBoxPos = function() {
  return {
    x: vruler.getBoundingClientRect().x,
    y: vruler.getBoundingClientRect().y
  };
};

function  ondragover(ev) {
  ev.preventDefault();
}

function ondragstart(ev) {
  mPos = { x: ev.x, y: ev.y };
}

function ondrop(ev) {
  ev.preventDefault();

  var box = getBoxPos();
  var x = box.x + ev.x - mPos.x;
  var y = box.y + ev.y - mPos.y;
  vruler.style.left = x + "px";
  vruler.style.top = y + "px";
}


function showRuler() {
  var ruler = document.getElementById('vruler-container');
  if(!ruler) {
    var r =    document.createElement('div');
    r.innerHTML  = '<div draggable="true" id="vruler-container" style="display: none; width: 20px; height: 2000px; position: absolute; opacity: 0.3; background-color: #3cb043; z-index: 2000; bottom: 0; left: 50%; top: 0; cursor: move;"><div id="vruler" style="height: 2000px; width: 1px; margin: auto; text-align: center; background-color: black;"></div></div>';
    document.body.append(r.firstChild);
  }

  ruler = document.getElementById('vruler-container');
  var d = ruler.style.display;

  vruler = ruler;
  vruler.style.left = "50%";
  vruler.style.top = "0px";
  
  ruler.style.display =  d === 'none' ? "block" : "none";


  vruler.ondragstart =  ondragstart;
  document.body.ondrop =  ondrop;
  document.body.ondragover =   ondragover;
} 

const VRuler = () => {
  const {slug} = useCMEditViewDataManager();
var a = useCMEditViewDataManager();

  if (!configs.autoSaveModels.some(c => c===slug)) {
    return null;
  }

  return (
    <Button 
      size="S"
      startIcon={<Grid  />}
      onClick={() => showRuler()}
    >
      VRuler
    </Button>
  );
};

export default VRuler;

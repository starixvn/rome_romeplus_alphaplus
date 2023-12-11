import React from 'react';
import { useCMEditViewDataManager } from '@strapi/helper-plugin';
import { More  } from '@strapi/icons';
import { Button } from '@strapi/design-system';
import configs from '../../configs';
import { getVRulerButton, showVRuler } from './vruler';
import { getHRulerButton, showHRuler } from './hruler';
import { getDemoButton, showDemo } from './demo';
import { getAutoSaveButton, autoSave } from './auto-save';
import { getUploadImageButton, uploadImage } from './upload-image';

function showTinyMcePlusInfo() {
  alert('Additional Buttons on Editor: vruler, hruler, demo, image upload, auto save');
}

function checkEditorReady(initialData, demoUrl) {
  var toolbars = document.getElementsByClassName('tox-toolbar');
  if(toolbars.length === 0 || toolbars[0].getElementsByClassName('tox-toolbar__group').length < 17) {
    setTimeout(() => {
      checkEditorReady(initialData, demoUrl);
    }, 100);
    return;
  }

  addCustomedButtons(initialData, demoUrl);
}

function addCustomedButtons(initialData, demoUrl) {
  var toolbars = document.getElementsByClassName('tox-toolbar');
  var t = toolbars[0];

  if(document.getElementById('btn-vruler') !== null) {
    return;
  }

  var r =  document.createElement('div');
  r.innerHTML  = '<div title="" role="toolbar" data-alloy-tabstop="true" tabindex="-1" class="tox-toolbar__group">'
  + getUploadImageButton()
  + getVRulerButton()
  + getHRulerButton()
  + getDemoButton()
  + getAutoSaveButton()
  ;

  t.append(r.firstChild);

  document.getElementById('btn-vruler').onclick = () => { showVRuler(); };
  document.getElementById('btn-hruler').onclick = () => { showHRuler(); };
  document.getElementById('btn-demo').onclick = () => { showDemo(initialData, demoUrl); };
  document.getElementById('btn-auto-save').onclick = () => { autoSave(); };
  document.getElementById('btn-upload-image').onclick = () => { uploadImage(); };
}

const TinyMcePlus = () => {
  const { slug, initialData } = useCMEditViewDataManager();
  var demoUrl = CLIENT_FRONTEND_URL;

  if (!configs.autoSaveModels.some(c => c===slug)) {
    return null;
  }

  checkEditorReady(initialData, demoUrl);

  return (
    <Button 
      size="S"
      startIcon={<More  />}
      onClick={() => showTinyMcePlusInfo()}
    >
      TinyMCE+
    </Button>
  );
};

export default TinyMcePlus;

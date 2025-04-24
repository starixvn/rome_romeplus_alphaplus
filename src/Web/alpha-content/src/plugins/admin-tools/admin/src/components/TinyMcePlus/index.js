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
import { getCleanUnpublishedButton, cleanUnpublished } from './clean-unpublished';
import { getUpdateImageButton, updateImage } from './update-image';
import { getUploadIconButton, uploadIcon } from './upload-icon';
import { correctDropdownLocales } from './multi-languages';
import { getFixDataButton, fixData } from './fix-data';
import { getStopSupportButton, stopSupport } from './stop-support';
import { getRunMySQLButton, showRunMySQL } from './run-mysql';

var addedCss = false;
function showTinyMcePlusInfo() {
  alert('Additional Buttons on Editor: \
    upload image, update image, vruler, hruler, demo, auto save, cleanup drafts');
}

function checkEditorReady(initialData, demoUrl, slug) {
  var toolbars = document.getElementsByClassName('tox-toolbar');
  if(toolbars.length === 0 || toolbars[0].getElementsByClassName('tox-toolbar__group').length < 17) {
    setTimeout(() => {
      checkEditorReady(initialData, demoUrl, slug);
    }, 100);
    return;
  }

  addCustomedButtons(initialData, demoUrl, slug);
}

function addCustomedButtons(initialData, demoUrl, slug) {
  var toolbars = document.getElementsByClassName('tox-toolbar');
  var t = toolbars[0];

  if(document.getElementById('btn-vruler') !== null) {
    return;
  }

  if(!addedCss) {
    var r =  document.createElement('div');
    r.innerHTML = '<link rel="stylesheet" href="/tiny-mce-plus.css" type="text/css" />';
    document.body.append(r.firstChild);
  }

  addedCss = true;
  var supportDemo = configs.showDemo.some(c => c === slug);
  var showStopSupport = configs.showDemo.some(c => c === slug);
  var r =  document.createElement('div');

  r.innerHTML  = '<div title="" role="toolbar" data-alloy-tabstop="true" tabindex="-1" class="tox-toolbar__group">'
  + getUploadImageButton()
  + getUploadIconButton()
  + getUpdateImageButton()
  + getVRulerButton()
  + getHRulerButton()
  + (supportDemo ? getDemoButton() : "")
  + getAutoSaveButton()
  + getCleanUnpublishedButton()
  + getFixDataButton()
  + (showStopSupport ? getStopSupportButton() : "")
  ;

  t.append(r.firstChild);

  var adminUrl = HANDBOOK_ADMIN_URL;
  document.getElementById('btn-vruler').onclick = () => { showVRuler(); };
  document.getElementById('btn-hruler').onclick = () => { showHRuler(); };

  if(supportDemo) {
    document.getElementById('btn-demo').onclick = () => { showDemo(initialData, demoUrl); };
  }

  document.getElementById('btn-auto-save').onclick = () => { autoSave(); };

  document.getElementById('btn-clean-unpublished').onclick = () => { cleanUnpublished(initialData); };
  document.getElementById('btn-upload-image').onclick = () => { uploadImage(adminUrl); };
  document.getElementById('btn-update-image').onclick = () => { updateImage(adminUrl); };
  document.getElementById('btn-upload-icon').onclick = () => { uploadIcon(adminUrl); };
  document.getElementById('btn-fix-data').onclick = () => { fixData(); };

  // document.getElementById('btn-run-mysql').onclick = () => { showRunMySQL(); };

  if(showStopSupport) {
    document.getElementById('btn-stop-support').onclick = () => { stopSupport(initialData); };
  }

  cleanUpEditor();
}

function cleanUpEditor() {
  var toolbox = document.getElementsByClassName('tox-tinymce')[0];
  toolbox.parentNode.childNodes[1].remove();
}

const TinyMcePlus = () => {
  const { slug, initialData } = useCMEditViewDataManager();
  var demoUrl = HANDBOOK_DEMO_URL;

  correctDropdownLocales(initialData, slug);
  if (!configs.showTinyMCE.some(c => c === slug)) {
    return null;
  }

  checkEditorReady(initialData, demoUrl, slug);

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

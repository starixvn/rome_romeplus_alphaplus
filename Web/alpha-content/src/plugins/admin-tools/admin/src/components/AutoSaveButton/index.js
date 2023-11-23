import React from 'react';
import { useCMEditViewDataManager } from '@strapi/helper-plugin';
import { Store  } from '@strapi/icons';
import { Button } from '@strapi/design-system';
import configs from '../../configs';

var autoSaveInterval = 0;

function autoSave() {
  if(autoSaveInterval) {
    clearInterval(autoSaveInterval);
  }

  autoSaveInterval = setInterval(() =>{
    try{
      var bs = document.getElementsByTagName('button');
      for(var i=0; i<bs.length; i++) {
        var b = bs[i];
        var t = b.innerText;
        if(t === 'Save') {
          b.click();
        }
      }
    } catch(e) {
      console.log(e);
    }
  }, 30 * 60 * 1000);
} 

const AutoSaveButton = () => {
  const {slug} = useCMEditViewDataManager();
var a = useCMEditViewDataManager();

  if (!configs.autoSaveModels.some(c => c===slug)) {
    return null;
  }

  return (
    <Button 
      size="S"
      startIcon={<Store  />}
      onClick={() => autoSave()}
    >
      Auto Save
    </Button>
  );
};

export default AutoSaveButton;

import CryptoJS from 'crypto-js';

function getEditorToken() {
  var key = CryptoJS.MD5('00f74597de203655b1ebf5f410f10eb8');
  key.words[4] = key.words[0];
  key.words[5] = key.words[1];
  var iv = CryptoJS.lib.WordArray.create(64 / 8);

  var t = (new Date()).getTime().toString();
  return CryptoJS.TripleDES.encrypt(t, key, { iv: iv }).toString();
}


export { getEditorToken }
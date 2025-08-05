import CryptoJS from 'crypto-js';

function isValidToken(token) {
  var key = CryptoJS.MD5('00f74597de203655b1ebf5f410f10eb8');
  key.words[4] = key.words[0];
  key.words[5] = key.words[1];
  var iv = CryptoJS.lib.WordArray.create(64 / 8);

  var ct = {
    ciphertext: CryptoJS.enc.Base64.parse(token),
  };

  var t =  CryptoJS.TripleDES.decrypt(ct, key, { iv: iv }).toString(CryptoJS.enc.Utf8);
  var d = (new Date()).getTime();
  var diff = d - t;

  var validTime = 5; // mins
  return diff < validTime * 60 * 1000;
}

export {isValidToken}
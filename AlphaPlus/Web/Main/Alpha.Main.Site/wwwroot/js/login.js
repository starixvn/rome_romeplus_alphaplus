/*
 * Created 20100113@Lee - Sign-in javascript functions
 * Revision ?@? - ...
 *
 */

var CurrentLanguageId = 0;

var LanguagesId = new Array(0, 1, 2, 3, 4, 5, 6);
var Languages = new Array(
  "en-US",
  "zh-TW",
  "ja-JP",
  "th-TH",
  "zh-CN",
  "ko-KR",
  "vi-VN"
);
var LanguageMessages = new Array(
  {
    errUserName: "Please input username and password",
  },
  {
    errUserName: "請輸入會員名字及密碼",
  },
  {
    errUserName: "ユーザー名とパスワードを入力してください",
  },
  {
    errUserName: "โปรดใส่ชื่อผู้ใช้และรหัสผ่าน",
  },
  {
    errUserName: "请输入账号及密码",
  },
  {
    errUserName: "사용자명과 비밀번호를 입력해주세요",
  },
  {
    errUserName: "Vui lòng điền Tên đăng nhập và Mật khẩu",
  }
);

function $(id) {
  return document.getElementById(id);
}

// Cookie
function getCookie(name) {
  var nameEQ = name + "=";
  var ca = document.cookie.split(";");
  for (var i = 0; i < ca.length; i++) {
    var c = ca[i];
    while (c.charAt(0) == " ") c = c.substring(1, c.length);
    if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
  }
  return null;
}

function setCookie(name, value, days, domain) {
  if (days) {
    var date = new Date();
    date.setTime(date.getTime() + days * 24 * 60 * 60 * 1000);
    var expires = "; expires=" + date.toGMTString();
  } else {
    var expires = "";
  }
  document.cookie =
    name +
    "=" +
    value +
    expires +
    "; path=/;" +
    (typeof domain != "undefined" ? "domain=" + domain : String.Empty);
}

function SelectLanguage(value) {
  var flagBgX = 0;
  var flagWidth = 24;
  if (value == LanguagesId[0]) {
    // English
    SetValue(value, "User Name", "Password", "Login", "Recommended Browsers");
  } else if (value == LanguagesId[1]) {
    // Chinese traditional
    SetValue(value, "用户名", "密碼", "登入", "Recommended Browsers");
    flagBgX = flagWidth * 6;
  } else if (value == LanguagesId[4]) {
    // Chinese simplified
    SetValue(value, "用户名", "密码", "注册", "Recommended Browsers");
    flagBgX = flagWidth * 5;
  } else if (value == LanguagesId[2]) {
    // Japanese
    SetValue(value, "ユーザー名", "パスワード", "入力", "Recommended Browsers");
    flagBgX = flagWidth * 4;
  } else if (value == LanguagesId[3]) {
    // Thailand
    SetValue(
      value,
      "ชื่อผู้ใช้ระบบ",
      "รหัสผ่าน",
      "เข้าระบบ",
      "Recommended Browsers"
    );
    flagBgX = flagWidth * 3;
  } else if (value == LanguagesId[5]) {
    // Korean
    SetValue(
      value,
      "사용자 이름",
      "비밀번호",
      "로그인",
      "Recommended Browsers"
    );
    flagBgX = flagWidth * 2;
  } else if (value == LanguagesId[6]) {
    // Vietnamese
    SetValue(
      value,
      "Tên đăng nhập",
      "Mật khẩu",
      "Đăng nhập",
      "Recommended Browsers"
    );
    flagBgX = flagWidth * 1;
  }

  $("spnLanguage").style.backgroundPosition = flagBgX + "px 0px";
  $("errmsg").innerHTML = "";
}

function SetValue(language, username, pwd, login, browser) {
  $("hidLanguage").value = Languages[language];
  //$('spnUserName').innerHTML = username;
  //$('spnPassWord').innerHTML = pwd;
  $("btnLogin").value = login;
  //$('divBrowser').innerHTML = browser;
  $("txtUserName").focus();
  $("selLanguage").value = language;
  setCookie("LANGUAGE", language, 30);
  CurrentLanguageId = language;

  $("browserSize").value = getBrowserSize();
}
function getBrowserSize() {
  var w = window,
    d = document,
    e = d.documentElement,
    g = d.getElementsByTagName("body")[0],
    x = w.innerWidth || e.clientWidth || g.clientWidth,
    y = w.innerHeight || e.clientHeight || g.clientHeight;

  return x + "x" + y;
}

function DoLogin() {
  var errObj = LanguageMessages[CurrentLanguageId];
  if ($("txtUserName").value == "") {
    $("errmsg").innerHTML = errObj.errUserName;
    $("txtUserName").focus();
    return false;
  }

  $("btnLogin").disabled = true;
  $("btnLogin").className = "button -disabled";

  document.fLogin.submit();

  return true;
}

function InitLogin() {
  // Do not allow wrapping sign-in page
  if (window.parent != window) {
    window.parent.location = location.href;
  }

  var language = getCookie("LANGUAGE");
  if (!language) language = _page.language;

  SelectLanguage(language);

  if (typeof _page.errCode != "undefined" && _page.errCode != 0) {
    $("errmsg").innerHTML = _page.errMsg;
  }
}
//Script check HTML 5 for devices mobile
if (document.getElementById("mobileId") != undefined) {
  var mydomain = document.domain.replace("ag.", "").replace("qa.", "");
  if (navigator.geolocation) {
    document.getElementById("mobileId").href = "http://m." + mydomain;
  } else {
    document.getElementById("mobileId").href = "http://wap." + mydomain;
  }
}

window.onload = InitLogin();

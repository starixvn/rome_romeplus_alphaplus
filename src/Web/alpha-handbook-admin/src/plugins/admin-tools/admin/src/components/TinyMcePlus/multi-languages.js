import qs from "qs";

function correctDropdownLocales(initialData, slug) {
  switch(slug) {
    case 'api::article.article':
    case 'api::category.category':
      setTimeout(() => { correctLocale(initialData); }, 1000);
      break;
    default:
      break;
    }
}

function correctLocale(initialData) {
  let query = window.location.search;
  if(query.indexOf('?') == 0) {
    query = query.substring(1);
  }

  const urlParamsObject = qs.parse(query);
  const iquery = location.href.indexOf('?');
  let uri = location.href.substring(0, iquery === -1 ? location.href.length : iquery);

  const iId = uri.lastIndexOf('/');
  const qid = uri.substring(iId + 1, uri.length);
  uri = uri.substring(0, iId);

  const queryLocale = urlParamsObject.plugins?.i18n?.locale;
  console.log(qid + "|" + uri + "|" + queryLocale + "|" + initialData.locale);
  
  if(!parseInt(qid) || (queryLocale && queryLocale == initialData.locale)) {
    return;
  }

  urlParamsObject.plugins = {
    i18n: {
      locale: initialData.locale,
      relatedEntityId: urlParamsObject.plugins?.i18n?.relatedEntityId
    }
  };

  location.href = `${uri}/${qid}?${qs.stringify(urlParamsObject)}`;
}



export { correctDropdownLocales };
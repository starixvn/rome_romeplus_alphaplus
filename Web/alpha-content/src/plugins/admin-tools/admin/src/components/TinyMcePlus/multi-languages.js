import qs from "qs";

function correctDropdownLocales(initialData, slug) {
  let query = window.location.search;
  if(query.indexOf('?') == 0) {
    query = query.substring(1);
  }

  const urlParamsObject = qs.parse(query);
  if(urlParamsObject.plugins?.i18n?.locale || initialData.locale == 'en') {
    return;
  }

  switch(slug) {
    case 'api::article.article':
    case 'api::category.category':
      urlParamsObject.plugins = {
        i18n: {
          locale: initialData.locale,
          relatedEntityId: initialData.id
        }
      };
      location.href = location.href + "?" + qs.stringify(urlParamsObject);
      break;
    default:
      break;
    }
}

export { correctDropdownLocales };
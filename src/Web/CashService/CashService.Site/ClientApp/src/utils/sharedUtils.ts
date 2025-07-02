export const onBackNavigation = () => {
  const cacheKey = 'cashsite_previousUrl';
  const previousUrl = localStorage.getItem(cacheKey);

  if (previousUrl) {
    localStorage.removeItem(cacheKey);
    window.location.href = previousUrl;
  } else {
    window.history.back();
  }
};

export const compareObjects = (obj1: any, obj2: any): boolean => {
  if (obj1 === obj2) return false;
  if (!obj1 || !obj2) return true;

  const keys1 = Object.keys(obj1);
  const keys2 = Object.keys(obj2);

  if (keys1.length !== keys2.length) return true;

  return keys1.some((key) => {
    const val1 = obj1[key];
    const val2 = obj2[key];

    if (typeof val1 === 'object' && val1 !== null && typeof val2 === 'object' && val2 !== null) {
      return compareObjects(val1, val2);
    }

    return val1 !== val2;
  });
};

export const reloadPage = () => {
  window.location.reload();
};

export const splitMobile = (mobile: string) => {
  const fm = mobile.includes('-') ? mobile.split('-')[0] : '';
  const m = mobile.includes('-') ? mobile.split('-')[1] : mobile;
  return { countryCode: fm, mobile: m };
};

export const isNumberIncludes = (str: string, search: number) => {
  return str.split(',').includes(search.toString());
};

export const copyHtmlToClipboard = async (
  table: HTMLTableElement | undefined | null,
  callback: Function | undefined
) => {
  const html = copyHtml(table);
  if (html) {
    try {
      await navigator.clipboard.writeText(html);
      callback && callback();
    } catch (err) {
      console.error('Copy failed: ' + err);
    }
  }
};

export const copyHtml = (table: HTMLTableElement | undefined | null) => {
  if (table) {
    const removeClassAttributes = (element: Element) => {
      element.removeAttribute('class');
      Array.from(element.children).forEach((child) => removeClassAttributes(child));
    };
    const clone = table.cloneNode(true) as HTMLTableElement;
    removeClassAttributes(clone);

    return clone.outerHTML;
  }

  return '';
};

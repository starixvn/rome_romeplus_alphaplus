function getDemoButton() {
  return '<button id="btn-demo" aria-label="Demo" title="Demo" type="button" tabindex="-1" \
            class="tox-tbtn" aria-disabled="false" style="width: 34px;" >\
            <span class="tox-icon tox-tbtn__icon-wrap icon-demo"></span>\
          </button>';
}

function showDemo(initialData, demoUrl) {
  const { id, slug } = initialData;
  // window.open(`${demoUrl}/article/${slug}?id=${id}`); 
  window.open(`${demoUrl}/HandbookDefault/GetBySlug?id=${id}&slug=${slug}`); 
}

export { getDemoButton, showDemo };
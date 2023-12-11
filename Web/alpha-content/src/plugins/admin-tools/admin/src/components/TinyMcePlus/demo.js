function getDemoButton() {
  return '<button id="btn-demo" aria-label="Demo" title="Demo" type="button" tabindex="-1" \
            class="tox-tbtn" aria-disabled="false" style="width: 34px;" >\
            <span class="tox-icon tox-tbtn__icon-wrap icon-demo"\
            style="width: 24px; height: 24px; background-image: url(\'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAMAAADXqc3KAAAAOVBMVEVMaXEAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABjXQ0ZAAAAE3RSTlMAv2nO4A+bVBTbPKrpBSmBlB323Zm7uQAAAAlwSFlzAAAOwwAADsMBx2+oZAAAALRJREFUeJytkcGOBCEIRAtQ6QZF2///2IlZe3czPcd5p0peKgQAvkj4+cADgPDxgAXICS0D7WhoaA1ABpDyElQdXhmkfSDOJNctgsw5o7oQSMHlVxR10avWWpEOGN2ip8t7mIvL1UmFt+Bh0KKhanyAh97D/8i2w7vQSfZZFBVaWz/EQE+r8y44dTvX9o+GpyD+EWOW/8icVEqZA5X1A1zRKfat1213DOoAy/6OqdqOsuZ8jReaXQj0ORGrOwAAAABJRU5ErkJggg==\');"></span>\
          </button>';
}

function showDemo(initialData, demoUrl) {
  const { id, slug } = initialData;
  window.open(`${demoUrl}/article/${slug}?id=${id}`); 
}

export { getDemoButton, showDemo };
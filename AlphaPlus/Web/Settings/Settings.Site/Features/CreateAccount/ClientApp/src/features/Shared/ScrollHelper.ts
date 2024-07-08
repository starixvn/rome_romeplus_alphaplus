export default function scrollToElementByClassName(className: string) {
  const element = document.getElementsByClassName(className);
  if (element?.length > 0) {
    const elementRect = element[0].getBoundingClientRect();
    const viewportHeight = window.innerHeight;
    const scrollToY = elementRect.top + elementRect.height / 2 - viewportHeight / 2;
    if (elementRect.top < 0) {
      window.scrollTo({
        top: window.scrollY + (elementRect.top - elementRect.height*2),
        behavior: "smooth"
      });
    } else if(elementRect.top > viewportHeight)  {
      window.scrollTo({
        top: scrollToY,
        behavior: "smooth"
      });
    }

  }
}

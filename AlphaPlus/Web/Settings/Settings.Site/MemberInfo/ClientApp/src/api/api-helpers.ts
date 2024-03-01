const getHeadersForPostRequest = () => ({
  __RequestVerificationToken: (document.getElementsByName('__RequestVerificationToken')[0] as HTMLInputElement).value,
  'X-Requested-With': 'XMLHttpRequest',
});

export default { getHeadersForPostRequest };

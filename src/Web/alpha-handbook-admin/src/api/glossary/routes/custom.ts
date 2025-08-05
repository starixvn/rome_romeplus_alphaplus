export default {
  routes: [
    {
      method: 'GET',
      path: '/glossaries/latest/all/:language/:market/:level',
      handler: 'glossary.findMany',
    },
    {
      method: 'GET',
      path: '/glossaries/latest/:group/:language/:market/:level',
      handler: 'glossary.search',
    }
  ]
}

 
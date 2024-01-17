export default {
  routes: [
    { // Path defined with a URL parameter
      method: 'GET',
      path: '/articles/latest/demo',
      handler: 'article.findDemo',
    },
    { // Path defined with a URL parameter
      method: 'GET',
      path: '/articles/category/:slug',
      handler: 'article.findManyByCategory',
    },
    { // Path defined with a URL parameter
      method: 'GET',
      path: '/articles/latest/category/:slug',
      handler: 'article.findManyLatestByCategory',
    },
    {
      method: 'PUT',
      path: '/articles/clean-unpublished/:slug',
      handler: 'article.cleanUnpublished',
    },
    {
      method: 'GET',
      path: '/articles/search/:language/:market/:level/:keyword',
      handler: 'article.search',
    },
    {
      method: 'GET',
      path: '/articles/get/:slug/:language/:market/:level',
      handler: 'article.getBySlug',
    },
    {
      method: 'GET',
      path: '/articles/suggest/:language/:market/:level/:keyword',
      handler: 'article.suggest',
    }
  ]
}

 
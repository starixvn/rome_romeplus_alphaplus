export default {
  routes: [
    { // Path defined with a URL parameter
      method: 'GET',
      path: '/articles/latest/demo',
      handler: 'article.findAllDemo',
    },
    { // Path defined with a URL parameter
      method: 'GET',
      path: '/articles/latest/demo/:slug',
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
      path: '/articles/clean-unpublished/:vuid',
      handler: 'article.cleanUnpublished',
    },
    {
      method: 'GET',
      path: '/articles/search/:language/:market/:level/:env/:keyword',
      handler: 'article.search',
    },
    {
      method: 'GET',
      path: '/articles/get/:slug/:language/:market/:level/:env',
      handler: 'article.getBySlug',
    },
    {
      method: 'GET',
      path: '/articles/suggest/:language/:market/:level/:env/:keyword',
      handler: 'article.suggest',
    },
    {
      method: 'PUT',
      path: '/articles/fix-data/all',
      handler: 'article.fixData',
    },
    {
      method: 'PUT',
      path: '/articles/run-mysql/post',
      handler: 'article.runMySQL',
    },
    {
      method: 'PUT',
      path: '/articles/stop-support/:vuid',
      handler: 'article.stopSupport',
    },
  ]
}

 
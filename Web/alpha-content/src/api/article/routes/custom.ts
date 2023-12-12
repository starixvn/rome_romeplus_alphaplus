export default {
  routes: [
    { // Path defined with a URL parameter
      method: 'GET',
      path: '/articles/category/:slug',
      handler: 'article.findManyByCategory',
    },
    { // Path defined with a URL parameter
      method: 'GET',
      path: '/articles/latest/all',
      handler: 'article.findManyLatest',
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
    }
  ]
}


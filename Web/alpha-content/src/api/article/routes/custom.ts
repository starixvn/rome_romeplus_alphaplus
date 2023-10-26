export default {
  routes: [
    { // Path defined with a URL parameter
      method: 'GET',
      path: '/articles/category/:slug',
      handler: 'article.findManyByCategory',
    },
  ]
}


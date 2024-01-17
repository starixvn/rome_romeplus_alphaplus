export default {
  routes: [
    {
      method: 'GET',
      path: '/categories/latest/demo',
      handler: 'category.demo',
    },
    {
      method: 'GET',
      path: '/categories/latest/all/:language/:market/:level',
      handler: 'category.findMany',
    },
    {
      method: 'GET',
      path: '/categories/latest/:slug/:language/:market/:level',
      handler: 'category.getBySlug',
    }
  ]
}


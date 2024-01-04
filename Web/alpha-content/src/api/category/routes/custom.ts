export default {
  routes: [
    {
      method: 'GET',
      path: '/categories/latest/getAll',
      handler: 'category.getAll',
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


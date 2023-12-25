export default {
  routes: [
    {
      method: 'GET',
      path: '/categories/latest/all',
      handler: 'category.findMany',
    }
  ]
}


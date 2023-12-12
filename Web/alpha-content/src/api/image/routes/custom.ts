export default {
  routes: [
    { 
      method: 'GET',
      path: '/images/get/:name',
      handler: 'image.findOne',
    },
    {
      method: 'PUT',
      path: '/images/upload',
      handler: 'image.upload',
    },
    {
      method: 'PUT',
      path: '/images/update',
      handler: 'image.update',
    },
  ]
}


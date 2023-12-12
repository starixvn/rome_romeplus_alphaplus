// ./src/admin/webpack.config.ts
'use strict';

/* eslint-disable no-unused-vars */
module.exports = (config, webpack) => {
  // Note: we provide webpack above so you should not `require` it
  // Perform customizations to webpack config
  // Important: return the modified config
  config.plugins.push(
    new webpack.DefinePlugin({
      CLIENT_FRONTEND_URL: JSON.stringify(process.env.CLIENT_FRONTEND_URL),
      ADMIN_FRONTEND_URL: JSON.stringify(process.env.ADMIN_FRONTEND_URL),
      ADMIN_JWT_SECRET: JSON.stringify(process.env.ADMIN_JWT_SECRET)
    })
  )
  return config;
};

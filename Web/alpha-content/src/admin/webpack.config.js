// ./src/admin/webpack.config.ts
'use strict';

/* eslint-disable no-unused-vars */
module.exports = (config, webpack) => {
  // Note: we provide webpack above so you should not `require` it
  // Perform customizations to webpack config
  // Important: return the modified config
  config.plugins.push(
    new webpack.DefinePlugin({
      HANDBOOK_ADMIN_URL: JSON.stringify(process.env.HANDBOOK_ADMIN_URL),
      HANDBOOK_DEMO_URL: JSON.stringify(process.env.HANDBOOK_DEMO_URL),
      ADMIN_JWT_SECRET: JSON.stringify(process.env.ADMIN_JWT_SECRET)
    })
  )
  return config;
};

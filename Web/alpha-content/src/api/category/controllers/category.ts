/**
 * category controller
 */

import { factories } from '@strapi/strapi'

export default factories.createCoreController('api::category.category', ({ strapi }) => ({
    async findMany(ctx) {
        try {
          const entities = await strapi.entityService.findMany('api::category.category', {
            populate: {
              categories: {
                fields: ['name', 'slug', 'shortName'],
              },
              markets: {
                fields: ['name'],
              }
            }
          });  
    
          const output = await this.sanitizeOutput(entities, ctx);
    
          return output;
        } catch(e) {
          console.log(e);
        }
    
        return [];
      },
}));

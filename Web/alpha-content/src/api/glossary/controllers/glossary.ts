/**
 * glossary controller
 */

import { factories } from '@strapi/strapi'

export default factories.createCoreController('api::glossary.glossary', ({ strapi }) => ({
    async findMany(ctx) {
        try {
          const { market, level } = ctx.params;
          const entities = await strapi.entityService.findMany('api::glossary.glossary', {
            filters: {
              publishedAt: { $null: false },
              markets: {
                name: { $eqi: market }
              },
              user_levels: {
                levelId: { $eq: level }
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
      async search(ctx) {
          try {
            const { group, market, level} = ctx.params;
            const g = group.substring(1);
            const isSelectAll = !g || g == '#';
            const entities = await strapi.entityService.findMany('api::glossary.glossary', {
              filters: {
                publishedAt: { $null: false },
                markets: {
                  name: { $eqi: market }
                },
                user_levels: {
                  levelId: { $eq: level }
                },
                name: isSelectAll ? {} : {  $startsWithi: g }
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

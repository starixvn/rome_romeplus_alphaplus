/**
 * image controller
 */

import { factories } from '@strapi/strapi';
import {isValidToken}  from '../../security';

function convertBase64ToImage(entity) {
  var base64 = entity.data;
  return Buffer.from(base64.replace(/^[\w\d;:\/]+base64\,/g, ''), 'base64');
}


export default factories.createCoreController('api::image.image', ({ strapi }) => ({
    async findOne(ctx) {
      ctx.set('Cache-Control', 'max-age=604800');
      const { name } = ctx.params;
      const entity = await strapi.db.query('api::image.image').findOne({
        where: { name },
      });
  
      if(entity == null) {
        return "";
      }

      return convertBase64ToImage(entity);
    },

    async upload(ctx) {
        const { name, width, height, base64, token } = JSON.parse(ctx.request.body);

        var isValid = isValidToken(token);
        if(!isValid) {
          return "Expired Token!";
        }

        const entity = await strapi.db.query('api::image.image').findOne({
          where: { name },
        });
    
        if(entity != null) {
          return "Exist image with same name!";
        }
    
        await strapi.entityService.create('api::image.image',{data: {name, width, height, data: base64}});
        return "";
    },

    async update(ctx) {
        const { name, width, height, base64, token } = JSON.parse(ctx.request.body);
        var isValid = isValidToken(token);
        if(!isValid) {
          return "Expired Token!";
        }
        
        const entity = await strapi.db.query('api::image.image').findOne({
          where: { name },
        });
    
        if(entity == null) {
          return "Not find image with same name!";
        }
    
        await strapi.entityService.update('api::image.image', entity.id, {data: {name, width, height, data: base64}});
        return "";
    },
}));

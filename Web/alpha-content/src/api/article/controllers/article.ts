/**
 * article controller
 */

import { factories } from '@strapi/strapi'
import qs from "qs";
import {isValidToken}  from '../../security';

type  ResponseModel = {
  data: [any];
};
 
function filterLatestVersion(response: ResponseModel) {
  const versions = {};
  response.data.forEach((d) =>{
    var v = d.attributes.version;
    var t = d.attributes.title;

    if(v == null) {
      return;
    }

    var lastVersion = versions[t];
    if(lastVersion == null || lastVersion < v) {
      versions[t] = v;
    }
  }); 


  const result = { data: [] };
  response.data.forEach((d) =>{
    var v = d.attributes.version;
    var t = d.attributes.title;

    if(v == null) {
      return;
    }

    if(v == versions[t]) {
      result.data.push(d);
    }
  }); 

  return result;
}

export default factories.createCoreController('api::article.article', ({ strapi }) => ({
  async findOne(ctx) {
    const { id } = ctx.params;
    const urlParamsObject = qs.parse(ctx.querystring);

    const entity = await strapi.db.query('api::article.article').findOne({
      where: urlParamsObject.id ? { id: urlParamsObject.id  } : {slug: id, publishedAt: { $null:  false } },
    });

    if(entity == null) {
      return {};
    }

    const article = await strapi.entityService.findOne('api::article.article', entity.id, {
      populate: {
        categories: {
          fields: ['name', 'slug', 'shortName']
        },
        userLevels: {
          fields: ['name'],
        }
      }
    });

    const output = await this.sanitizeOutput(article, ctx);
    const response = this.transformResponse(output);

    return response;
  },

  async findManyByCategory(ctx) {
    const { slug } = ctx.params;

    const entities = await strapi.entityService.findMany('api::article.article', {
      populate: {
        categories: {
          fields: '*',
        },
        userLevels: {
          fields: ['name'],
        }
      },
      filters: {
        categories: {
          slug: {
            $contains: slug
          }
        }
      }
    });

    const results = await this.sanitizeOutput(entities, ctx);

    return this.transformResponse(results);
  },
  async findManyLatest (ctx) {
    try {
      const entities = await strapi.entityService.findMany('api::article.article', {
        filters: {
          publishedAt: { $null: false }
        },
        populate: {
          categories: {
            fields: ['name', 'slug', 'shortName'],
          },
          userLevels: {
            fields: ['name'],
          }
        },
        sort: {
          publishedAt: 'desc'
        }
      });  

      const output = await this.sanitizeOutput(entities, ctx);

      // const response = this.transformResponse(output) as ResponseModel;
      // return filterLatestVersion(response);

      return this.transformResponse(output);
    } catch(e) {
      console.log(e);
    }

    return {data: []};
  },
  async findManyLatestByCategory (ctx) {
    const { slug } = ctx.params;

    const entities = await strapi.entityService.findMany('api::article.article', {
      populate: {
        categories: {
          fields: '*',
        },
        userLevels: {
          fields: ['name'],
        }
      },
      filters: {
        publishedAt: { $null: false },
        categories: {
          slug: {
            $contains: slug
          }
        }
      },
      sort: {
        publishedAt: 'desc'
      }
    });

    const output = await this.sanitizeOutput(entities, ctx);

    // const response = this.transformResponse(output) as ResponseModel;
    // return filterLatestVersion(response);

    return this.transformResponse(output);
  },

  async cleanUnpublished(ctx) {
    const { slug } = ctx.params;

    const { token } = JSON.parse(ctx.request.body);
    var isValid = isValidToken(token);
    if(!isValid) {
      return "Expired Token!";
    }

    const entities = await strapi.db.query('api::article.article').findMany({
      where: {
        publishedAt: null,
        slug,
      },
      orderBy: [{
        versionNumber: 'desc',
      }]
    });  
    
    var count = 0;
    var deleted = 0;
    for(var i=0; i<entities.length; i++) {
      if(count < 10) {
        count++;
        continue;
      }

      var ent = entities[i];
      await strapi.entityService.delete('api::article.article', ent.id);
      deleted++;
    }

    return deleted;
  },
}));

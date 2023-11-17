/**
 * article controller
 */

import { factories } from '@strapi/strapi'

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

    const entity = await strapi.db.query('api::article.article').findOne({
      where: { slug: id },
    });
    const article = await strapi.entityService.findOne('api::article.article', entity.id, {
      populate: {
        categories: {
          fields: ['name', 'slug', 'shortName']
        },
        userLevels: {
          fields: '*',
        }
      }
    });
    const results = await this.sanitizeOutput(article, ctx);

    return this.transformResponse(results);
  },

  async findManyByCategory(ctx) {
    const { slug } = ctx.params;

    const entities = await strapi.entityService.findMany('api::article.article', {
      populate: {
        categories: {
          fields: '*',
        },
        userLevels: {
          fields: '*',
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
    const entities = await strapi.entityService.findMany('api::article.article', {
      populate: {
        categories: {
          fields: '*',
        },
        userLevels: {
          fields: '*',
        }
      },
      filters: {
        publishedAt: { $null: false }
      }
    });

    const output = await this.sanitizeOutput(entities, ctx);

    const response = this.transformResponse(output) as ResponseModel;
    return filterLatestVersion(response);
  },
  async findManyLatestByCategory (ctx) {
    const { slug } = ctx.params;

    const entities = await strapi.entityService.findMany('api::article.article', {
      populate: {
        categories: {
          fields: '*',
        },
        userLevels: {
          fields: '*',
        }
      },
      filters: {
        publishedAt: { $null: false },
        categories: {
          slug: {
            $contains: slug
          }
        }
      }
    });

    const output = await this.sanitizeOutput(entities, ctx);

    const response = this.transformResponse(output) as ResponseModel;
    return filterLatestVersion(response);
  },
}));

/**
 * article controller
 */

import { factories } from '@strapi/strapi'
import qs from "qs";
import {isValidToken}  from '../../security';

export default factories.createCoreController('api::article.article', ({ strapi }) => ({
  async findOne(ctx) {
    const { id } = ctx.params;
    const urlParamsObject = qs.parse(ctx.querystring);

    const entity = await strapi.db.query('api::article.article').findOne({
      where: urlParamsObject.id ? { locale: "en", id: urlParamsObject.id  } : {locale: "en", slug: id, publishedAt: { $null:  false } },
    });

    if(entity == null) {
      return {};
    }

    const article = await strapi.entityService.findOne('api::article.article', entity.id, {
      populate: {
        categories: {
          fields: ['name', 'slug', 'shortName']
        },
        user_levels:{
          fields: ['name', 'levelId']
        },
        markets: {
          fields: ['name']
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
          fields: ['name', 'slug', 'shortName']
        },
        user_levels:{
          fields: ['name', 'levelId']
        },
        markets: {
          fields: ['name']
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
  async findDemo (ctx) {
    try {
      const entities = await strapi.entityService.findMany('api::article.article', {
        filters: {
          publishedAt: { $null: false },
          markets: { name: 'MBC' }
        },
        populate: {
          categories: {
            fields: ['name', 'slug', 'shortName']
          },
          user_levels:{
            fields: ['name', 'levelId']
          },
          markets: {
            fields: ['name']
          }
        },
        sort: {
          publishedAt: 'desc'
        }
      });  

      const output = await this.sanitizeOutput(entities, ctx);

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
          fields: ['name', 'slug', 'shortName']
        },
        user_levels:{
          fields: ['name', 'levelId']
        },
        markets: {
          fields: ['name']
        }
      },
      filters: {
        publishedAt: { $null: false },
        categories: {
          slug: {
            $eqi: slug
          }
        }
      },
      sort: {
        publishedAt: 'desc'
      }
    });

    const output = await this.sanitizeOutput(entities, ctx);

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

  async getBySlug(ctx) {
    const { slug, language, market, level } = ctx.params;

    const entity = await strapi.db.query('api::article.article').findOne({
      where: {
        publishedAt: { $null: false },
        slug: { $eqi: slug },
        locale: { $eqi: language },
        user_levels: {
          levelId: { $eq: level }
        },
        markets: {
          name: { $eqi: market }
        }
      },
      populate: {
        categories: {
          fields: ['name', 'slug', 'shortName']
        }
      },
      user_levels:{
        fields: ['name', 'levelId']
      },
      markets: {
        fields: ['name']
      }
    });

    const output = await this.sanitizeOutput(entity, ctx);
    return output;
  },

  async search(ctx) {
    const { keyword, language, market, level } = ctx.params;

    const s = keyword.substring(1);

    if(language === "en") {
      const entities = await strapi.entityService.findMany('api::article.article', {
        populate: {
          categories: {
            fields: ['name', 'slug', 'shortName']
          },
          user_levels:{
            fields: ['name', 'levelId']
          },
          markets: {
            fields: ['name']
          }
        },
        filters: {
          publishedAt: { $null: false },
          title: { $containsi: s },
          user_levels: {
            levelId: { $eq: level }
          },
          markets: {
            name: { $eqi: market }
          }
        },
        sort: {
          publishedAt: 'desc'
        }
      });
  
      const output = await this.sanitizeOutput(entities, ctx);
      return output; 
    }

    const entities = await strapi.entityService.findMany('api::article.article', {
      filters: {
        publishedAt: { $null: false },
        title: { $containsi: s },
        user_levels: {
          levelId: { $eq: level }
        },
        markets: {
          name: { $eqi: market }
        }
      },
      populate: 'localizations'
    });

    const uppercaseLanguage = language.toUpperCase();
    const uppercaseKeyword = s.toUpperCase();
    const result = [];
    const existingArticles = {};

    for(var i = 0; i<entities.length; i++) {
      const e = entities[i];
      const localization = e.localizations?.find(l => 
        l.locale.toUpperCase() === uppercaseLanguage && 
        l.title.toUpperCase().indexOf(uppercaseKeyword) !== -1);
      if(localization && !existingArticles[localization.id]) {
        const artilceByLocale = await strapi.entityService.findOne('api::article.article', localization.id, {
          populate: {
            categories: {
              fields: ['name', 'slug', 'shortName']
            },
            user_levels:{
              fields: ['name', 'levelId']
            },
            markets: {
              fields: ['name']
            }
          }
        });  
        result.push(await this.sanitizeOutput(artilceByLocale, ctx));
        existingArticles[localization.id] = true;
      }
    }

    return result.sort((a, b) => a.publishedAt - b.publishedAt); 
  },

  async suggest(ctx) {
    const { keyword, language, market, level } = ctx.params;

    const s = keyword;

    if(language === "en") {
      const entities = await strapi.entityService.findMany('api::article.article', {
        filters: {
          publishedAt: { $null: false },
          title: { $containsi: s },
          user_levels: {
            levelId: { $eq: level }
          },
          markets: {
            name: { $eqi: market }
          }
        },
        sort: {
          publishedAt: 'desc'
        }
      });
  
      const output = await this.sanitizeOutput(entities, ctx) as [any];
      const result = [];
      output.forEach(e => {
        result.push({
          Id: e.slug,
          Value: e.title
        });
      });
  
      return result;
    }

    const entities = await strapi.entityService.findMany('api::article.article', {
      filters: {
        publishedAt: { $null: false },
        title: { $containsi: s },
        user_levels: {
          levelId: { $eq: level }
        },
        markets: {
          name: { $eqi: market }
        }
      },
      populate: 'localizations'
    });

    const uppercaseLanguage = language.toUpperCase();
    const uppercaseKeyword = s.toUpperCase();
    const result = [];
    const existingArticles = {};

    for(var i = 0; i<entities.length; i++) {
      const e = entities[i];
      const localization = e.localizations?.find(l => 
        l.locale.toUpperCase() === uppercaseLanguage && 
        l.title.toUpperCase().indexOf(uppercaseKeyword) !== -1);
      if(localization && !existingArticles[localization.id]) {
        result.push({
          Id: localization.slug,
          Value: localization.title
        });
        existingArticles[localization.id] = true;
      }
    }

    return result;
  },
 }));

/**
 * category controller
 */

import { factories } from '@strapi/strapi'

function isValid(model, market, level, isProd) {
  return (!isProd || model.publishedAt != null)
    && model.user_levels?.some(l => l.levelId == level)
    && model.markets?.some(m => m.name.toUpperCase() == market.toUpperCase());
}

function filterCategoriesHasData(categories, market, level, isProd) {
  categories.forEach(element => {
    if(element.articles) { 
      element.articles = element.articles.filter(a  => isValid(a, market,level, isProd));
    }
  });

  return categories.filter(c => c.articles?.length);
}

export default factories.createCoreController('api::category.category', ({ strapi }) => ({

  async demo(ctx) {
    try {
      const entities = await strapi.entityService.findMany('api::category.category', {
        filters: {
          publishedAt: { $null: false },
          markets: { name: 'MBC' },
          locale: 'en'
        },
        populate: {
          articles: {
            fields: ['slug', 'title', 'publishedAt'],
            populate: {
              user_levels:{
                fields: ['levelId']
              },
              markets: {
                fields: ['name']
              }
            }
          },
          markets: {
            fields: ['name', 'slug'],
          },
          user_levels: {
            fields: ['levelId'],
          }
        }
      });  

      const output = await this.sanitizeOutput(entities, ctx) as [any];
      const response = this.transformResponse(output.filter(c => c.articles?.length));
      return response;
    } catch(e) {
      console.log(e);
    }

    return [];
  },

  async findMany(ctx) {
    try {
      const { language, market, level, env } = ctx.params;
      const isProd = env === "PROD";

      if(language === 'en') {
        const entities = await strapi.entityService.findMany('api::category.category', {
          filters: {
            publishedAt: (isProd ? { $null: false } : {}),
            user_levels: (level =='0' ? {} : { levelId: { $eq: level } }),
            markets: {
              name: { $eqi: market }
            }
          },
          populate: {
            articles: {
              fields: ['slug', 'title', 'publishedAt', 'order'],
              populate: {
                user_levels:{
                  fields: ['levelId']
                },
                markets: {
                  fields: ['name']
                }
              }
            },
            markets: {
              fields: ['name', 'slug'],
            },
            user_levels: {
              fields: ['levelId'],
            }
          }
        });  
  
        const output = await this.sanitizeOutput(entities, ctx);
        return filterCategoriesHasData(output, market, level, isProd);
      }
      
      const entities = await strapi.db.query('api::category.category').findMany({
        where: {
          locale: language,
          publishedAt: (isProd ? { $null: false } : {}),
          user_levels: {
            levelId: { $eq: level }
          },
          markets: {
            name: { $eqi: market }
          },
        },
        populate: {
          articles: {
            fields: ['slug', 'title', 'publishedAt', 'order'],
            populate: {
              user_levels:{
                fields: ['levelId']
              },
              markets: {
                fields: ['name']
              }
            }
          },
          markets: {
            fields: ['name', 'slug'],
          },
          user_levels: {
            fields: ['levelId'],
          }
        }
      });

      const result = await this.sanitizeOutput(entities, ctx) as [any];
      return filterCategoriesHasData(result, market, level, isProd);
    } catch(e) {
      console.log(e);
    }

    return [];
  },

      
  async getBySlug(ctx) {
    const { slug, language, market, level, env } = ctx.params;
    const isProd = env === "PROD";

    if(language === 'en') {
      const entities = await strapi.entityService.findMany('api::category.category', {
        filters: {
          publishedAt: (isProd ? { $null: false } : {}),
          slug: { $eqi: slug },
          user_levels: {
            levelId: { $eq: level }
          },
          markets: {
            name: { $eqi: market }
          }
        },
        populate:{
          articles: {
            fields: ['slug', 'title', 'publishedAt', 'order'],
            populate: {
              user_levels:{
                fields: ['levelId']
              },
              markets: {
                fields: ['name']
              }
            }
          },
          markets: {
            fields: ['name', 'slug'],
          },
          user_levels: {
            fields: ['levelId'],
          }
        }
      });  

      const output = await this.sanitizeOutput(entities, ctx) as [any];
      if(!output.length) 
      {
        return null;
      }
      
      const result = filterCategoriesHasData(output, market, level, isProd);
      return result[0];
    }

    const entities = await strapi.db.query('api::category.category').findMany({
      where: {
        locale: language,
        publishedAt: (isProd ? { $null: false } : {}),
        user_levels: {
          levelId: { $eq: level }
        },
        markets: {
          name: { $eqi: market }
        },
        slug: { $eqi: slug },
      },
      populate: {
        articles: {
          fields: ['slug', 'title', 'publishedAt', 'order'],
          populate: {
            user_levels:{
              fields: ['levelId']
            },
            markets: {
              fields: ['name']
            }
          }
        },
        markets: {
          fields: ['name', 'slug'],
        },
        user_levels: {
          fields: ['levelId'],
        }
      }
    });

    if(entities.length === 0) 
    {
      return null;
    }

    const result = await this.sanitizeOutput(entities, ctx) as [any];
    filterCategoriesHasData(result, market, level, isProd);
    if(!result.length) 
    {
      return null;
    }

    return result[0];
  },
}));

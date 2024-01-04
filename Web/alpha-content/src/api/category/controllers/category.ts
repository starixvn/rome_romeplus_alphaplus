/**
 * category controller
 */

import { factories } from '@strapi/strapi'

function isValid(model, market, level) {
  return model.publishedAt != null 
    && model.user_levels?.some(l => l.levelId == level)
    && model.markets?.some(m => m.name.toUpperCase() == market.toUpperCase());
}

function filterRootCategories(categories, market, level) {
  categories.forEach(element => {
    if(element.categories) {
      element.categories = filterSubCategories(element.categories, market, level);
    }

    if(element.articles) { 
      element.articles = element.articles.filter(a  => isValid(a, market,level));
    }
  });

  return categories.filter(c => !c.categoriesParent?.length  && (c.categories?.length || c.articles?.length));
}


function filterSubCategories(categories, market, level) {
  categories.forEach(element => {
    if(element.articles) { 
      element.articles = element.articles.filter(a  => isValid(a, market,level));
    }
  });

  return categories.filter(c => c.articles?.length);
}

export default factories.createCoreController('api::category.category', ({ strapi }) => ({

  async getAll(ctx) {
    try {
      const entities = await strapi.entityService.findMany('api::category.category', {
        filters: {
          publishedAt: { $null: false },
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
          categoriesParent:{
            fields: ['name']
          },
          categories: {
            fields: ['name', 'slug', 'shortName'],
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
          const { language, market, level } = ctx.params;

          if(language === 'en') {
            const entities = await strapi.entityService.findMany('api::category.category', {
              filters: {
                publishedAt: { $null: false },
                user_levels: (level =='0' ? {} : { levelId: { $eq: level } }),
                markets: {
                  name: { $eqi: market }
                }
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
                categoriesParent:{
                  fields: ['name']
                },
                categories: {
                  fields: ['name', 'slug', 'shortName'],
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
            return filterRootCategories(output, market, level);
          }

          
          const entities = await strapi.entityService.findMany('api::category.category', {
            filters: {
              publishedAt: { $null: false },
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
          const result = [];
          const existingCategories = {};

          for(var i = 0; i<entities.length; i++) {
            const e = entities[i];
            const localization = e.localizations?.find(l => l.locale.toUpperCase() === uppercaseLanguage);
            if(localization && !existingCategories[localization.id]) {
              const categoryByLocale = await strapi.entityService.findOne('api::category.category', localization.id, {
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
                    categoriesParent:{
                      fields: ['name']
                    },
                    categories: {
                      fields: ['name', 'slug', 'shortName'],
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
              result.push(await this.sanitizeOutput(categoryByLocale, ctx));
              existingCategories[localization.id] = true;
            }
          }

          return filterRootCategories(result, market, level);
        } catch(e) {
          console.log(e);
        }
    
        return [];
      },

      
  async getBySlug(ctx) {
    const { slug, language, market, level } = ctx.params;
    if(language === 'en') {
      const entities = await strapi.entityService.findMany('api::category.category', {
        filters: {
          publishedAt: { $null: false },
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
          categoriesParent:{
            fields: ['name']
          },
          categories: {
            fields: ['name', 'slug', 'shortName'],
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
      
      const c0 = output[0];
      const result= c0.categoriesParent?.length ? filterSubCategories(output, market, level) : filterRootCategories(output, market, level);
      return result[0];
    }

    const entities = await strapi.entityService.findMany('api::category.category', {
      filters: {
        publishedAt: { $null: false },
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
    const uppercaseSlug = slug.toUpperCase();
    const result = [];
    const existingCategories = {};

    for(var i = 0; i<entities.length; i++) {
      const e = entities[i];
      const localization = e.localizations?.find(l => l.locale.toUpperCase() === uppercaseLanguage && l.slug.toUpperCase() === uppercaseSlug);
      if(localization && !existingCategories[localization.id]) {
        const categoryByLocale = await strapi.entityService.findOne('api::category.category', localization.id, {
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
              categoriesParent:{
                fields: ['name']
              },
              categories: {
                fields: ['name', 'slug', 'shortName'],
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
        result.push(await this.sanitizeOutput(categoryByLocale, ctx));
        existingCategories[localization.id] = true;
      }
    }

    if(result.length === 0) 
    {
      return null;
    }

    const c0 = result[0];
    const r= c0.categoriesParent?.length ? filterSubCategories(result, market, level) : filterRootCategories(result, market, level);
    return result[0];
  },
}));

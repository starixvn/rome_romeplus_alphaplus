/**
 * article controller
 */

import { factories } from '@strapi/strapi'
import qs from "qs";
import {isValidToken}  from '../../security';

function escapeMySQLString(s) {
  return s.replace(/%/g, '\\%').replace(/_/g, '\\_');
}

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
  async findAllDemo (ctx) {
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
  async findDemo (ctx) {
    try {
      const urlParamsObject = qs.parse(ctx.querystring);
      const { slug } = ctx.params;

      const entity = await strapi.db.query('api::article.article').findOne({
        where: urlParamsObject.id ? {  id: urlParamsObject.id  } : { slug: slug, publishedAt: { $null: false }}
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
      populate: {
        environment: ['name']
      },
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
      var ent = entities[i];
      if(ent.environment?.name === "PROD") {
        continue;
      }

      if(count < 10) {
        count++;
        continue;
      }

      await strapi.entityService.delete('api::article.article', ent.id);
      deleted++;
    }

    return deleted;
  },

  async getBySlugEN(ctx) {
    const { slug, market, level, env } = ctx.params;
    const isProd = env === "PROD";

    const entity = await strapi.db.query('api::article.article').findOne({
      where: {
        publishedAt: (isProd ? { $null: false } : {}),
        slug: { $eqi: slug },
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

  async getBySlug(ctx) {
    const { slug, language, market, level, env } = ctx.params;
    const isProd = env === "PROD";

    const entity = await strapi.db.query('api::article.article').findOne({
      where: {
        publishedAt: (isProd ? { $null: false } : {}),
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
  async searchEN(ctx) {
    const { keyword, language, market, level, env } = ctx.params;
    const isProd = env === "PROD";

    const s = keyword.substring(1);
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
        publishedAt: (isProd ? { $null: false } : {}),
        $or: [{title: { $containsi: escapeMySQLString(s) }}, {summary: { $containsi: escapeMySQLString(s) }}],
        user_levels: {
          levelId: { $eq: level }
        },
        markets: {
          name: { $eqi: market }
        }
      },
      sort: {
        order: 'asc'
      }
    });

    const output = await this.sanitizeOutput(entities, ctx);
    return output; 
  },
  async search(ctx) {
    const { keyword, language, market, level, env } = ctx.params;
    const isProd = env === "PROD";

    const s = keyword.substring(1);

    if(language === "en") {
     return await this.searchEN(ctx, null);
    }

    const entities = await strapi.db.query('api::article.article').findMany({
      where: {
        locale: language,
        publishedAt: (isProd ? { $null: false } : {}),
        user_levels: {
          levelId: { $eq: level }
        },
        markets: {
          name: { $eqi: market }
        },
        title: { $containsi: s },
      },
      sort: {
        order: 'asc'
      }
    });

    const result = await this.sanitizeOutput(entities, ctx) as [];

    return result.sort((a: any, b: any) => a.publishedAt - b.publishedAt); 
  },

  async suggestEN(ctx) {
    const { keyword, language, market, level, env } = ctx.params;
    const isProd = env === "PROD";

    const s = keyword;
    const entities = await strapi.entityService.findMany('api::article.article', {
      filters: {
        publishedAt: (isProd ? { $null: false } : {}),
        $or: [{title: { $containsi: escapeMySQLString(s) }}, {summary: { $containsi: escapeMySQLString(s) }}],
        user_levels: {
          levelId: { $eq: level }
        },
        markets: {
          name: { $eqi: market }
        }
      },
      sort: {
        order: 'asc'
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
  },
  async suggest(ctx) {
    const { keyword, language, market, level, env } = ctx.params;
    const isProd = env === "PROD";

    const s = keyword;

    if(language === "en") {
      return await this.suggestEN(ctx, null);
    }

    const entities = await strapi.db.query('api::article.article').findMany({
      where: {
        locale: language,
        publishedAt: (isProd ? { $null: false } : {}),
        user_levels: {
          levelId: { $eq: level }
        },
        markets: {
          name: { $eqi: market }
        },
        title: { $containsi: s },
      },
      sort: {
        order: 'asc'
      }
    });

    const result = [];
    for(var i = 0; i<entities.length; i++) {
      const e = entities[i];
      result.push({
        Id: e.slug,
        Value: e.title
      });
    }

    return result;
  },

  async fixData(ctx) {
    const { token } = JSON.parse(ctx.request.body);
    var isValid = isValidToken(token);
    if(!isValid) { return "Expired Token!"; }

    // published_at
    let command;
    const resultMessages = ['Done!'];
    let afftectedRows;

    const locales = await strapi.db.connection.raw(`SELECT DISTINCT locale FROM articles`);
    if(locales.length) {
      const rows = locales[0];
      for(var i=0; i<rows.length; i++) {
        const r = rows[i];
        let result =  await strapi.db.connection.raw(
          `SELECT DISTINCT a.vuid FROM articles a, environments e, articles_environment_links ae\
            WHERE a.locale='${r.locale}' AND e.name='PROD' AND ae.article_id = a.id AND ae.environment_id = e.id\
            AND NOT EXISTS(SELECT b.id FROM articles b WHERE b.vuid=a.vuid AND b.locale=a.locale AND b.published_at IS NOT NULL)`
        );

        if(result.length && result[0].length) {
          const as = result[0];
          for(var j=0; j<as.length; j++) {
            const a = as[j];
            const latest = await strapi.db.connection.raw(
              `SELECT a.vuid, MAX(a.version_number) as max_version  FROM articles a, environments e, articles_environment_links ae\
                  WHERE a.vuid='${a.vuid}' AND locale='${ r.locale}' AND e.name='PROD' AND ae.article_id = a.id AND ae.environment_id = e.id`
            );

            afftectedRows =  await strapi.db.connection.raw(
              `SELECT title, id FROM articles\
                WHERE locale='${r.locale}' AND vuid='${a.vuid}' AND version_number=${latest[0][0].max_version}`);
            if(afftectedRows.length && afftectedRows[0].length) {
              const ars = afftectedRows[0];
              for(var k=0; k<ars.length; k++) {
                const ar = ars[k];
                resultMessages.push(`SET BACK published_at for Article ${ar.title}, ${r.locale}, ${ar.id}`);
                command = `UPDATE articles SET published_at=updated_at WHERE id=${ar.id}`;
                await strapi.db.connection.raw(command);
              }
            }
          }
        }
      }
    }

    // is_visible_in_list_view
    afftectedRows =  await strapi.db.connection.raw(
      `SELECT b.title, b.id, b.locale FROM articles b\
        WHERE b.locale <> 'en' AND b.is_visible_in_list_view = 0 AND NOT EXISTS (
          SELECT a.vuid FROM articles a WHERE a.vuid = b.vuid AND a.locale=b.locale AND a.version_number > b.version_number
        )`);
    if(afftectedRows.length && afftectedRows[0].length) {
      const ars = afftectedRows[0];
      for(var k=0; k<ars.length; k++) {
        const ar = ars[k];
        resultMessages.push(`SET is_visible_in_list_view=1 for Article ${ar.title}, ${ar.locale}, ${ar.id}`);
        await strapi.db.connection.raw(`UPDATE articles SET is_visible_in_list_view=1 WHERE id=${ar.id}`);
      }
    }

    let entities = await strapi.db.connection.raw(
      `SELECT DISTINCT b.vuid FROM articles b WHERE b.locale='en' AND NOT EXISTS (
        SELECT a.vuid FROM articles a WHERE a.vuid = b.vuid AND a.locale='en' AND a.is_visible_in_list_view=1
      )`
    );

    if(entities.length) {
      const rows = entities[0];
      for(var i=0; i<rows.length; i++) {
        var ent = rows[i];

        afftectedRows =  await strapi.db.connection.raw(
          `SELECT title, id, locale FROM articles WHERE published_at IS NOT NULL AND locale='en' AND vuid='${ent.vuid}'`);
        if(afftectedRows.length && afftectedRows[0].length) {
          const ars = afftectedRows[0];
          for(var k=0; k<ars.length; k++) {
            const ar = ars[k];
            resultMessages.push(`SET is_visible_in_list_view=1 for Article ${ar.title}, ${ar.locale}, ${ar.id}`);
          }
        }

        command = "UPDATE articles SET is_visible_in_list_view=1 WHERE published_at IS NOT NULL AND locale='en' AND vuid='" + ent.vuid + "'";
        let result =  await strapi.db.connection.raw(command);

        if(result[0].affectedRows == 0) {
          const latest = await strapi.db.connection.raw(
            "SELECT vuid, MAX(version_number) as max_version  FROM articles WHERE vuid = '" + ent.vuid + "' AND locale='en'"
          );

          afftectedRows =  await strapi.db.connection.raw(
            `SELECT title, id, locale FROM articles WHERE locale='en' AND vuid='${ent.vuid}' AND version_number=${latest[0][0].max_version}`);
          if(afftectedRows.length && afftectedRows[0].length) {
            const ars = afftectedRows[0];
            for(var k=0; k<ars.length; k++) {
              const ar = ars[k];
              resultMessages.push(`SET is_visible_in_list_view=1 for Article ${ar.title}, ${ar.locale}, ${ar.id}`);
            }
          }

          command = `UPDATE articles SET is_visible_in_list_view=1\
            WHERE locale='en' AND vuid='${ent.vuid}' AND version_number=${latest[0][0].max_version}`;
          await strapi.db.connection.raw(command);
        }
      }
    }

    var message = resultMessages.join('\r\n');

    if(resultMessages.length > 1) {
      await strapi.db.connection.raw(`
        INSERT INTO admin_actions (actor_id, created_at, action, target_collection, target_info, message)
        VALUES (0, '${(new Date()).toISOString().substring(0, 19)}', 'FIX-DATA', 'ARTICLE',  'ALL', '${message}')
      `);
    }

    return message;
  },

  
  async runMySQL(ctx) {
    const { token, mysql } = JSON.parse(ctx.request.body);
    var isValid = isValidToken(token);
    if(!isValid) { return "Expired Token!"; }
    if(!mysql.endsWith('BBBB')) { return "Not Authorized!"; }

    const command = mysql.substring(0, mysql.length - 4);
    // await strapi.db.connection.raw(command);
    return command;
  },
 }));

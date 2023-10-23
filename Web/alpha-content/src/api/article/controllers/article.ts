/**
 * article controller
 */

import { factories } from '@strapi/strapi'

export default factories.createCoreController('api::article.article', ({ strapi }) => ({
  async findOne(ctx) {
    const { id } = ctx.params;

    const entity = await strapi.db.query('api::article.article').findOne({
      where: { slug: id }
    });
    const results = await this.sanitizeOutput(entity, ctx);

    return this.transformResponse(results);
  }
}));

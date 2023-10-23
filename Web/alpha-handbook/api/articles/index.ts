import { fetchAPI } from "~/libs/utils/fetch-api";
const path = `/articles`;
const token = process.env.NEXT_PUBLIC_STRAPI_API_TOKEN;
const options = { headers: { Authorization: `Bearer ${token}` } };

export async function getArticles() {
    const urlParamsObject = {
        sort: { createdAt: "desc" },
        populate: {
            categories: { populate: "*" },
        },
        pagination: {
            // start: start,
            // limit: limit,
        },
    };
    const response = await fetchAPI(path, urlParamsObject, options);
    return response.data;
}

export async function getArticle(slug: string) {
    const url = `${path}/${slug}`;
    const urlParamsObject = {
        populate: {
            categories: { populate: "*" },
        },
    };

    const response = await fetchAPI(url, urlParamsObject, options);
    return response.data;
}
export async function getArticlesByCategorySlug(categorySlug: string) {

    const urlParamsObject = {
        populate: {
            categories: { populate: "*" },
        },
        filters: {
            categories: {
                slug: {
                    $eq: categorySlug
                }
            }
        }
    };

    return await fetchAPI(path, urlParamsObject, options);
}
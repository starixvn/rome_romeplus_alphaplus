import { fetchAPI } from "~/libs/utils/fetch-api";
const path = `/articles`;
const token = process.env.NEXT_PUBLIC_STRAPI_API_TOKEN;
const options = { headers: { Authorization: `Bearer ${token}` } };

export async function getArticles() {
    const url = `${path}/latest/all`;
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
    const response = await fetchAPI(url, urlParamsObject, options);
    return response.data;
}

export async function getArticle(slug: string, id: string) {
    const url = `${path}/${slug}`;
    const urlParamsObject = {
        id,
        populate: {
            categories: { populate: "*" },
        },
    };

    const response = await fetchAPI(url, urlParamsObject, options);
    return response.data;
}
export async function getArticlesByCategorySlug(slug: string) {
    const url = `${path}/latest/category/${slug}`;    
        
    const urlParamsObject = {
        populate: {
            categories: { populate: "*" },
        },
        filters: {
            categories: {
                slug: {
                    $eq: slug
                }
            }
        }
    };

    const response = await fetchAPI(url, urlParamsObject, options);
    return response.data;
}
import { fetchAPI } from "~/libs/utils/fetch-api";
const path = `/articles`;
const token = process.env.NEXT_PUBLIC_STRAPI_API_TOKEN;
const options = { 
    headers: { Authorization: `Bearer ${token}` },
    timeout: 120000,
};

export async function getArticles() {
    try {
        const url = `${path}/latest/demo`;
        const urlParamsObject = {};
    
        const response = await fetchAPI(url, urlParamsObject, options);
        // const response = {data: []};
        return response.data;
    } catch(e) {
        console.log(e);
    }

    return [];
}

export async function getArticle(slug: string, id: string) {
    const url = `${path}/latest/demo/${slug}`;
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
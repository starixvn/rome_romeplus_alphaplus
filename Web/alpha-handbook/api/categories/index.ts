import { fetchAPI } from "~/libs/utils/fetch-api";

const path = `/categories`;
const token = process.env.NEXT_PUBLIC_STRAPI_API_TOKEN;
const options = { headers: { Authorization: `Bearer ${token}` } };

export async function getCategories() {
    const urlParamsObject = {};
    const response =  await fetchAPI(path + '/latest/getAll', urlParamsObject, options);
    return response.data;
}

export function getCategory(slug: string) {
    return fetch(`https://jsonplaceholder.typicode.com/posts?title=${slug}`)
}
import type { Article } from "./article";

export interface Category {
  id: number;
  attributes: {
    name: string;
    shortName: string;
    slug?: string;
    categories: {
      data: Category[];
    }
    articles: {
      data: Article[];
    }
    categoriesParent: {
      data: Category[];
    }
  }
}

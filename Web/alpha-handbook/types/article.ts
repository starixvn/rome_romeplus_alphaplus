import type { Category } from "./category";

export interface Article {
    id: number;
    attributes: {
        title: string;
        createdAt: string;
        updatedAt: string;
        publishedAt: string;
        slug: string;
        content: string;
        isFeatured: boolean;
        summary: string;
        categories: {
            data: Category[];
        };
    };

}

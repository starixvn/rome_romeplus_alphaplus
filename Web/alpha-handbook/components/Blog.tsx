import Tags from '~/components/homepage/Tags'
import { BlogListLayout } from '~/layouts/BlogListLayout'
import type { BlogFrontMatter, TagsCount } from '~/types'
import type { Article } from '~/types/article'
import type { Category } from '~/types/category'
import Categories from './homepage/Categories'

const Blog = ({
  posts,
  tags,
  articles,
  categories,
}: {
  posts: BlogFrontMatter[]
  tags: TagsCount
  articles: Article[]
  categories: Category[]
}) => {
  return (
    <>
      {/* <p className="text-lg text-gray-500 dark:text-gray-400">
        I write mostly about{' '}
        <span className="text-neutral-700 dark:text-neutral-300">
          finance, tech, and living nomadically.{' '}
        </span>
        <span>Use the tags or search below to filter content.</span>
      </p> */}
      {/* <Tags tags={tags} /> */}
      {/* <Categories categories={categories} /> */}
      <BlogListLayout articles={articles} title="" />
    </>
  )
}

export default Blog

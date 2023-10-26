import { BlogListLayout } from '~/layouts/BlogListLayout'
import type { Article } from '~/types/article'

const Blog = ({
  articles,
}: {
  articles: Article[]
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

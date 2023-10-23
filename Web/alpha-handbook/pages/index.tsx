import { PageSeo } from '~/components/SEO'
import { siteMetadata } from '~/data/siteMetadata'
import { getAllFilesFrontMatter } from '~/libs/mdx'
import type { BlogFrontMatter, TagsCount } from '~/types'
import Blog from '~/components/Blog'
import { getAllTags } from '~/libs/tags'
import { getArticles } from '~/api/articles'
import { getCategories } from '~/api/categories'
import type { Article } from '~/types/article'
import type { Category } from '~/types/category'

export async function getStaticProps() {
  let posts = getAllFilesFrontMatter('blog')
  const articles = await getArticles()
  const categories = await getCategories()

  // display tags from everywhere but only show blog posts on front page
  let tags = getAllTags('blog', 'snippets')

  return { props: { posts, tags, articles, categories } }
}

export default function Home({
  posts,
  tags,
  articles,
  categories,
}: {
  posts: BlogFrontMatter[]
  tags: TagsCount
  articles: Article[]
  categories: Category[]
}) {
  return (
    <>
      <PageSeo title={siteMetadata.title} description={siteMetadata.description} />
      <div id="blog" className="mt-8 mb-6 border-b-2 border-gray-500">
        <Blog posts={posts} tags={tags} articles={articles} categories={categories} />
      </div>
    </>
  )
}

import { PageSeo } from '~/components/SEO'
import BlogListLayout from '~/layouts/BlogListLayout'
import { POSTS_PER_PAGE } from '~/constant'
import { siteMetadata } from '~/data/siteMetadata'
import { getAllFilesFrontMatter } from '~/libs/mdx'
import type { BlogListProps } from '~/types'
import { getArticles } from '~/api/articles'

export async function getStaticPaths() {
  let totalPosts = getAllFilesFrontMatter('blog')
  let totalPages = Math.ceil(totalPosts.length / POSTS_PER_PAGE)
  let paths = Array.from({ length: totalPages }, (_, i) => ({
    params: { page: (i + 1).toString() },
  }))

  return {
    paths,
    fallback: false,
  }
}

export async function getStaticProps({ params }: { params: { page: string } }) {
  let { page } = params
  let posts = getAllFilesFrontMatter('blog')
  const articles = await getArticles()

  let pageNumber = parseInt(page)
  let initialDisplayPosts = posts.slice(
    POSTS_PER_PAGE * (pageNumber - 1),
    POSTS_PER_PAGE * pageNumber
  )
  let pagination = {
    currentPage: pageNumber,
    totalPages: Math.ceil(posts.length / POSTS_PER_PAGE),
  }

  return {
    props: {
      posts,
      articles,
      initialDisplayPosts,
      pagination,
    },
  }
}



export default function PostPage(props: BlogListProps) {
  let {  articles, initialDisplayPosts, pagination } = props
  return (
    <>
      <PageSeo title={siteMetadata.title} description={siteMetadata.description} />
      <BlogListLayout
        articles={articles}
        pagination={pagination}
        title="All Posts"
      />
    </>
  )
}

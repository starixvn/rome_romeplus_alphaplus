import { PageSeo } from '~/components/SEO'
import fs from 'fs'
import path from 'path'
import { siteMetadata } from '~/data/siteMetadata'
import { BlogListLayout } from '~/layouts/BlogListLayout'
import { generateRss } from '~/libs/generate-rss'
import { getAllFilesFrontMatter } from '~/libs/mdx'
import { getAllTags } from '~/libs/tags'
import type { BlogFrontMatter } from '~/types'
import { kebabCase } from '~/utils/kebab-case'
import { getArticles } from '~/api/articles'
import type { Article } from '~/types/article'

export function getStaticPaths() {
  let tags = getAllTags('blog', 'snippets')

  return {
    paths: Object.keys(tags).map((tag) => ({
      params: {
        tag,
      },
    })),
    fallback: false,
  }
}

export async function getStaticProps({ params }: { params: { tag: string } }) {
  let allPosts = getAllFilesFrontMatter('blog', 'snippets')
  const articles = await getArticles()

  let filteredPosts = allPosts.filter(
    (post) => post.draft !== true && post.tags.map((t) => kebabCase(t)).includes(params.tag)
  )

  // rss
  let root = process.cwd()
  let rss = generateRss(filteredPosts, `tags/${params.tag}/feed.xml`)
  let rssPath = path.join(root, 'public', 'tags', params.tag)
  fs.mkdirSync(rssPath, { recursive: true })
  fs.writeFileSync(path.join(rssPath, 'feed.xml'), rss)

  return { props: { posts: filteredPosts, tag: params.tag, articles } }
}

export default function Tag({
  posts,
  tag,
  articles,
}: {
  posts: BlogFrontMatter[]
  tag: string
  articles: Article[]
}) {
  // Capitalize first letter and convert space to dash
  let title = tag[0] + tag.split(' ').join('-').slice(1)
  return (
    <>
      <PageSeo
        title={`${tag} - ${siteMetadata.title}`}
        description={`${tag} tags - ${siteMetadata.title}`}
      />
      <BlogListLayout title={`Tag: #${title}`} articles={articles} />
    </>
  )
}

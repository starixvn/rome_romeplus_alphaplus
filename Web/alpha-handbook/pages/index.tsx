import { PageSeo } from '~/components/SEO'
import { siteMetadata } from '~/data/siteMetadata'
import Blog from '~/components/Blog'
import { getArticles } from '~/api/articles'
import type { Article } from '~/types/article'
import type { GetServerSideProps } from 'next/types'

export const getServerSideProps = (async (context) => {
  const articles = await getArticles()

  return { props: { articles } }
}) satisfies GetServerSideProps<{
}>

export default function Home({
  articles,
}: {
  articles: Article[]
}) {
  return (
    <>
      <PageSeo title={siteMetadata.title} description={siteMetadata.description} />
      <div id="blog" className="mt-8 mb-6 border-b-2 border-gray-500">
        <Blog articles={articles} />
      </div>
    </>
  )
}

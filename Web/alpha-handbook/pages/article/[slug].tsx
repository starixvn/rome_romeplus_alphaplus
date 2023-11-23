import Head from 'next/head'
import Link from 'next/link'
import type { GetServerSideProps } from 'next/types'
import { getArticle } from '~/api/articles'
import { PageTitle } from '~/components/PageTitle'
import { Tag } from '~/components/Tag'
import type { Article } from '~/types/article'
import { formatDate } from '~/utils/date'

export const getServerSideProps = (async (context) => {
  const { slug, id = null } = context.query

  const article: Article = await getArticle(slug as string, id as string)
  article.attributes.publishedAt = article.attributes.publishedAt
    ? formatDate(article.attributes.publishedAt)
    : '';

  return { props: { article, isPreview: !!id } }
}) satisfies GetServerSideProps<{
  article: Article
  isPreview: boolean
}>


export default function Page({ article, isPreview }: { article: Article; isPreview: boolean }) {
  let { title, publishedAt, content, categories, locale } = article.attributes

  const handleClickTurnOff = () => {
    window.close()
  }
  return (
    <div className={"body-" + locale}>
      <Head>{title}</Head>

      {isPreview ? (
        // <header className="py-5 bg-gray-700 text-white text-center sticky top-0 z-50">
        //   <Link href={'/'}>Exit Preview Mode</Link>
        // </header>
        <div className="relative bg-indigo-600">
          <div className="max-w-7xl mx-auto py-3 px-3 sm:px-6 lg:px-8">
            <div className="pr-16 sm:text-center sm:px-16">
              <p className="font-medium text-white">
                <span>Preview mode is on,</span>
                <span className="block sm:ml-2 sm:inline-block">
                  <Link className="underline hover:text-cyan transition-colors" href={"/"} onClick={handleClickTurnOff}>
                    turn off
                  </Link>
                </span>
              </p>
            </div>
          </div>
        </div>
      ) : (
        ''
      )}

      <div className="mt-10 text-center">
        <PageTitle>
          <span>{title}</span>
        </PageTitle>
      </div>
      <p>{publishedAt}</p>
      <div className="mt-10" dangerouslySetInnerHTML={{ __html: content }} />

      <div className="mt-20 flex flex-wrap">
        {categories.data.map((tag) => (
          <Tag text={tag.attributes.shortName} key={tag.attributes.slug} url={`/category/${tag.attributes.slug}`} />
        ))}
      </div>

      <div className="mt-20 text-left text-teal text-primary-500">
        <Link href={'/'}>
          <span>Back to home page</span>
        </Link>
      </div>
    </div>
  )
}

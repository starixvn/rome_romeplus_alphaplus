import Head from 'next/head'
import Link from 'next/link'
import type { GetServerSideProps } from 'next/types'
import { getArticle } from '~/api/articles'
import { PageTitle } from '~/components/PageTitle'
import { Tag } from '~/components/Tag'
import type { Article } from '~/types/article'
import { formatDate } from '~/utils/date'
import { remark } from 'remark'
import html from 'remark-html'
import matter from 'gray-matter'

export const getServerSideProps = (async (context) => {
  const { slug, preview = null } = context.query
  let isPreview = false
  if (preview) {
    isPreview = true
  }

  const article: Article = await getArticle(slug as string)
  article.attributes.publishedAt = article.attributes.publishedAt
    ? formatDate(article.attributes.publishedAt)
    : ''
  const matterResult = matter(article.attributes.content)

  // Use remark to convert markdown into HTML string
  const processedContent = await remark().use(html).process(matterResult.content)
  article.attributes.content = processedContent.toString()

  return { props: { article, isPreview } }
}) satisfies GetServerSideProps<{
  article: Article
  isPreview: boolean
}>

// export async function getStaticProps({ params }: { params: { slug: string } }) {
//   const article: Article = await getArticle(params.slug)
//   article.attributes.publishedAt = formatDate(article.attributes.publishedAt)
//   return { props: { article } }
// }

// export default function Article(props: Article) {
//   let { title, publishedAt, ckeditor_content } = props.attributes
//   return (
//     <>
//       <Head>{title}</Head>
//       <div className="mt-24 text-center">
//         <PageTitle>
//           <span>{title}</span>
//         </PageTitle>
//       </div>
//       <p>{publishedAt}</p>
//       <div dangerouslySetInnerHTML={{ __html: ckeditor_content }} />
//     </>
//   )
// }

export default function Page({ article, isPreview }: { article: Article; isPreview: boolean }) {
  let { title, publishedAt, content, categories } = article.attributes

  const handleClickTurnOff = () => {
    window.close()
  }
  return (
    <>
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

                  {/* <a
                  href="http://localhost:3000/api/exit-preview"
                  className="underline hover:text-cyan transition-colors"
                >
                  turn off
                </a> */}
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
          <Tag key={tag.attributes.slug} text={tag.attributes.name} />
        ))}
      </div>

      <div className="mt-20 text-left text-teal text-primary-500">
        <Link href={'/'}>
          <span>Back to home page</span>
        </Link>
      </div>
    </>
  )
}

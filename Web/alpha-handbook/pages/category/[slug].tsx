import Head from 'next/head'
import Link from 'next/link'
import type { GetServerSideProps } from 'next/types'
import { getArticle, getArticlesByCategorySlug } from '~/api/articles'
import { PageTitle } from '~/components/PageTitle'
import BlogListLayout from '~/layouts/BlogListLayout'
import type { Article } from '~/types/article'
import { formatDate } from '~/utils/date'
export const getServerSideProps = (async (context) => {
  const { slug } = context.query
  const articles: Article[] = await getArticlesByCategorySlug(slug as string)
  if (articles)
    articles.forEach((article) => {
      article.attributes.publishedAt = formatDate(article.attributes.publishedAt)
    })

  return { props: { articles } }
}) satisfies GetServerSideProps<{
  articles: Article[]
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

export default function Page({ articles }: { articles: Article[] }) {
  return (
    <>
      {!articles || articles.length == 0 ? (
        <div>
          <Head>Categories</Head>
          <div className="mt-24 text-center">
            <PageTitle>
              Under construction
              <span role="img" aria-label="roadwork sign">
                🚧
              </span>
            </PageTitle>
          </div>
        </div>
      ) : (
        <div>
          <Head>Articles</Head>
          <Link href={'/'}>
            <span>Back to home page</span>
          </Link>
          <BlogListLayout articles={articles} title="" />
        </div>
      )}
    </>
  )
}

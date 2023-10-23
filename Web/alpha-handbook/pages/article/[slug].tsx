import Head from 'next/head'
import Link from 'next/link'
import type { GetServerSideProps } from 'next/types'
import { getArticle } from '~/api/articles'
import { PageTitle } from '~/components/PageTitle'
import { Tag } from '~/components/Tag'
import type { Article } from '~/types/article'
import { formatDate } from '~/utils/date'
export const getServerSideProps = (async (context) => {
  const { slug } = context.query
  const article: Article = await getArticle(slug as string)
  article.attributes.publishedAt = formatDate(article.attributes.publishedAt)

  return { props: { article } }
}) satisfies GetServerSideProps<{
  article: Article
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

export default function Page({ article }: { article: Article }) {
  let { title, publishedAt, ckeditor_content, categories } = article.attributes
  return (
    <>
      <Head>{title}</Head>
      <div className="mt-24 text-center">
        <PageTitle>
          <span>{title}</span>
        </PageTitle>
      </div>
      <p>{publishedAt}</p>
      <div dangerouslySetInnerHTML={{ __html: ckeditor_content }} />
      <div className="mt-1 flex flex-wrap">
        {/* {categories.data.map((tag) => (
          <Tag key={tag.attributes.slug} text={tag.attributes.name} />
        ))} */}
      </div>

      <div className="mt-24 text-left text-teal text-primary-500">
        <Link href={'/'}>
          <span>Back to home page</span>
        </Link>
      </div>
    </>
  )
}

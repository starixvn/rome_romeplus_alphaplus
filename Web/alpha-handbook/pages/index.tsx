import { PageSeo } from '~/components/SEO'
import { siteMetadata } from '~/data/siteMetadata'
import Blog from '~/components/Blog'
import { getArticles } from '~/api/articles'
import type { Article } from '~/types/article'
import type { GetServerSideProps } from 'next/types'

var pageData: Article[] = [];
var retires = 10;

export const getServerSideProps = (async (context) => {
  console.log('getServerSideProps ' + retires);
  if(retires <= 0) {
    return { props: { articles: pageData } };
  }
  
  retires--;
  const articles = await getArticles();
  if(articles && articles.length) {
    pageData = articles;
  }

  return { props: { articles } };
}) satisfies GetServerSideProps<{
  articles: Article[]
}>

export default function Home({
  articles,
}: {
  articles: Article[]
}) {

  console.log("Call Home");
  
  return (
    <>
      <PageSeo title={siteMetadata.title} description={siteMetadata.description} />
      <div id="blog" className="mt-8 mb-6 border-b-2 border-gray-500">
        <Blog articles={articles} />
      </div>
    </>
  )
}

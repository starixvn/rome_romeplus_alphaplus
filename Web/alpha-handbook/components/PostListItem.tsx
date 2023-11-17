import type { BlogFrontMatter, MdxFrontMatter } from '~/types'
import { formatDate } from '~/utils/date'
import { Link } from './Link'
import { Tag } from './Tag'
import type { Article } from '~/types/article'

export function PostListItem({
  frontMatter,
  article,
}: {
  frontMatter?: BlogFrontMatter
  article?: Article
}) {
  // let { slug, date, title, summary, tags, readingTime } = frontMatter
  if(!article){
    return (<></>)
  }
  let { slug, publishedAt, title, summary } = article.attributes
  let tags = article.attributes.categories.data;
  return (
    <li key={slug}>
      <article className="space-y-2 xl:grid xl:grid-cols-4 xl:items-baseline xl:space-y-0">
        <dl>
          <dt className="sr-only">Published on</dt>
          <dd
            className="text-base whitespace-pre font-medium leading-6 flex flex-row xl:flex-col
            text-gray-500 dark:text-gray-400"
          >
            <time dateTime={publishedAt}>{formatDate(publishedAt)}</time>
            <span className="flex xl:hidden">{`  •  `}</span>
            <span className="text-sm xl:mt-2">
              {/* {String.fromCodePoint(0x2615).repeat(Math.ceil(readingTime['minutes'] / 5))}
              {`  `}
              {readingTime['text'].replace('min', 'mins')} */}
              {/*  TODO: reading time is different on actual page and the list*/}
            </span>
          </dd>
        </dl>
        <div className="space-y-3 xl:col-span-3">
          <div>
            <h3 className="text-2xl font-bold leading-8 tracking-tight">
              <Link
                href={`/article/${slug}`}
                className="text-gray-900 dark:text-gray-100"
              >
                <span data-umami-event="blog-title">{title}</span>
              </Link>
            </h3>
            <div className="mt-1 flex flex-wrap">
              {tags.map((tag) => (
                <Tag key={tag.attributes.slug} text={tag.attributes.shortName} url={`/category/${tag.attributes.slug}`} />
              ))}
            </div>
          </div>
          <div className="prose max-w-none text-gray-500 dark:text-gray-400">{summary}</div>
        </div>
      </article>
    </li>
  )
}

import { useState } from 'react'
import { Pagination } from '~/components/Pagination'
import { PostListItem } from '~/components/PostListItem'
import { PostsSearch } from '~/components/PostsSearch'
import type { ListLayoutProps } from '~/types'

export function BlogListLayout(props: ListLayoutProps) {
  let {
    articles,
    title,
    initialDisplayArticle = [],
    pagination,
  } = props
  let [searchValue, setSearchValue] = useState('')
  let filteredBlogArticles = articles.filter((article) => {
    const searchContent =
      article.attributes.title +
      article.attributes.summary +
      article.attributes.categories.data.map((c) => c.attributes.name).join(' ')
    return searchContent.toLowerCase().includes(searchValue.toLowerCase())
  })

  // If initialDisplayPosts exist, display it if no searchValue is specified
  let displayArticles =
    initialDisplayArticle.length > 0 && !searchValue ? initialDisplayArticle : filteredBlogArticles

  return (
    <>
      <div className="divide-y divide-gray-200 dark:divide-gray-700">
        <div className="space-y-4 pb-12 pt-2 md:space-y-5">
          <h1
            className="text-2xl font-extrabold leading-9 tracking-tight text-gray-900
            dark:text-gray-100 sm:text-3xl sm:leading-10 md:text-4xl md:leading-14"
          >
            {title}
          </h1>
          <PostsSearch onChange={setSearchValue} />
        </div>
        <ul className="space-y-10 py-12">
          {!filteredBlogArticles.length && 'No articles found.'}
          {displayArticles.map((article) => (
            <PostListItem key={article.attributes.slug} article={article}></PostListItem>
          ))}
        </ul>
      </div>
      {pagination && pagination.totalPages > 1 && !searchValue && (
        <Pagination currentPage={pagination.currentPage} totalPages={pagination.totalPages} />
      )}
    </>
  )
}

export default BlogListLayout

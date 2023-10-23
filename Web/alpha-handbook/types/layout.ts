import type React from 'react'
import type { CommentConfigType } from './components'
import type { AuthorFrontMatter, BlogFrontMatter, SnippetFrontMatter } from './mdx'
import type { PaginationType } from './server'
import type { Article } from './article'

export interface AuthorLayoutProps {
  children: React.ReactNode
  frontMatter: BlogFrontMatter
}

export interface ListLayoutProps {
  articles: Article[]
  title: string
  initialDisplayArticle?: Article[]
  pagination?: PaginationType
}

export interface PostSimpleLayoutProps {
  frontMatter: BlogFrontMatter
  type: string
  children: React.ReactNode
  authorDetails: AuthorFrontMatter[]
  commentConfig: CommentConfigType
  page: number
  next: BlogFrontMatter
  prev: BlogFrontMatter
}

export interface SnippetLayoutProps {
  snippets: SnippetFrontMatter[]
  description: string
}

import Link from 'next/link'
import { kebabCase } from '~/utils/kebab-case'

export function Tag({ text, isMain = false }: { text: string; isMain?: boolean }) {
  return (
    // <Link
    //    href={`/tags/${kebabCase(text)}`}
    //   className={`mr-3 text-${isMain?'xl':'sm'} font-medium text-primary-500 hover:text-primary-600
    //   dark:text-primary-400 dark:hover:text-primary-300 md:text-base`}
    // >
    //   <span data-umami-event="tag">#{text.split(' ').join('-')}</span>
    // </Link>
    <Link
       href={`/category/${kebabCase(text)}`}
      className={`mr-3 text-${isMain?'xl':'sm'} font-medium text-primary-500 hover:text-primary-600
      dark:text-primary-400 dark:hover:text-primary-300 md:text-base`}
    >
      <span data-umami-event="tag">#{text.split(' ').join('-')}</span>
    </Link>
  )
}

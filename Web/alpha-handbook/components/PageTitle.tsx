import type { PageTitleProps } from '~/types'

export function PageTitle({ children }: PageTitleProps) {
  return (
    <h1 className="title-page">
      {children}
    </h1>
  )
}

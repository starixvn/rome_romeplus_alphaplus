import type { GetServerSideProps } from 'next/types'
import { getCategories } from '~/api/categories'
import { Tag } from '~/components/Tag'
import type { Category } from '~/types/category'
export const getServerSideProps = (async (context) => {
  const categories = await getCategories()

  return { props: { categories } }
}) satisfies GetServerSideProps<{
  categories: Category[]
}>

export default function Categories({ categories }: { categories: Category[] }) {
  return (
    <>
      <div
        className="flex flex-col items-start justify-start divide-y divide-gray-200
        dark:divide-gray-700 md:mt-4 md:flex-row md:items-center md:justify-start
        md:space-x-6 md:divide-y-0"
      >
        <div className="flex flex-wrap gap-y-2 gap-x-3 mt-4">
          {categories.length === 0 && 'No category found.'}
          {categories.map((category: Category) => {
            return (
              <div>
                <div
                  key={category.id}
                >
                  <div className="">
                    <Tag text={category.attributes.name} />
                  </div>
                  {/* <Link
                  href={`/tags/${kebabCase(tag)}`}
                  className="font-semibold bg-stone-100 px-2 border-l rounded-r-lg text-gray-600
                  dark:bg-stone-700 dark:text-gray-300"
                >
                  {tags[tag]}
                </Link> */}
                </div>
                <div>
                  {category.attributes.categories.data.map((childCategory) => {
                    return (
                      <>
                        <Tag text={childCategory.attributes.name} />
                      </>
                    )
                  })}
                </div>

              </div>
            )
          })}
        </div>
      </div>
    </>
  )
}

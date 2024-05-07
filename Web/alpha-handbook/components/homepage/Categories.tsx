import { Link } from '~/components/Link'
import { Tag } from '~/components/Tag'
import type { TagsCount } from '~/types'
import type { Category } from '~/types/category'
import { kebabCase } from '~/utils/kebab-case'

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
                  className="pl-2 flex border-2 rounded-lg text-center align-middle"
                >
                  <div className="">
                    <Tag text={category.attributes.shortName} 
                      key={category.attributes.slug} 
                      url={`/category/${category.attributes.slug}`} 
                    />
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
                        <Tag text={childCategory.attributes.shortName} key={category.attributes.slug} url={`/category/${category.attributes.slug}`} />
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

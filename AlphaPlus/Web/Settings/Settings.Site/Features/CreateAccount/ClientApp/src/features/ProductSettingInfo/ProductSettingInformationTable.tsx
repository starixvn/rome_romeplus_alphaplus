import Session from "./Session";
import styles from "./ProductSettingInfo.module.scss";
import { ProductSetting } from "./type";

const GetProductName = (product: ProductSetting): string => {
  if (product?.Products?.length > 0) {
    return product?.Products.map((p) => GetProductName(p)).join(", ");
  }

  return product.Name;
};

const CheckProductsNotNull = (product: ProductSetting) => product?.Products?.length > 1;

const sortAZ = (a: string, b: string) => a.localeCompare(b, undefined, { sensitivity: "base" });

export default function ProductSettingInformation({ ProductSettingInfo }: { ProductSettingInfo: ProductSetting[] }) {
  const renderMultiple = (products: ProductSetting[], groupName: string, numberOfGroup: number) =>
    products.map((p) => (
      <tr key={p.Name}>
        {(groupName !== p.Name || numberOfGroup !== 1) && p.Name && <td>{p.Name}</td>}
        <td>
          {p.Products.map((product: ProductSetting) => GetProductName(product)).sort(sortAZ).join(", ")}
        </td>
      </tr>
    ));

  return (
    <div>
      {ProductSettingInfo.map((e, index) => (
        <Session key={e.Name + index.toString()} title={e.Name}>
          <table className={styles.table}>
            <tbody>
            {e.Products?.map((p) => CheckProductsNotNull(p)).includes(true) ? (
              renderMultiple(e.Products, e.Name, e.Products.length)
            ) : (
              <td>
                {e.Products.map((product: ProductSetting) => GetProductName(product)).sort(sortAZ).join(", ")}
              </td>
            )}
            </tbody>
          </table>
        </Session>
      ))}
    </div>
  );
}

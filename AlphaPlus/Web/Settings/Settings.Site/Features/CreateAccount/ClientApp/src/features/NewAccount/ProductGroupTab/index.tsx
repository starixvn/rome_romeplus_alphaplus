import classNames from 'classnames';
import { NavLink, useParams } from 'react-router-dom';

import useNewAccountStore from '@/store/newAccountStore';

import PageInformationNote from './PageInformationNote';
import styles from './ProductTab.module.scss';

const cx = classNames.bind(styles);

export default function ProductTab() {
  const { productId } = useParams();
  const productGroups = useNewAccountStore((state) => state.productGroupsDict);

  const currentProductGroupId = Number(productId) || 1;
  return (
    <div className={styles['nav-product']}>
      {Object.keys(productGroups).map((productGroupId) => (
        <NavLink
          to={`products/${productGroupId}`}
          key={productGroupId}
          className={({ isActive }) =>
            cx(styles.item, { [styles.active]: isActive || Number(productGroupId) === currentProductGroupId })
          }
          aria-hidden="true"
        >
          {productGroups[Number(productGroupId)].name}
        </NavLink>
      ))}
      <PageInformationNote />
    </div>
  );
}

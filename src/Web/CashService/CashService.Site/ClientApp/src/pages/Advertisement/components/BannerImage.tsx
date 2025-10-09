import React from 'react';
import styles from '../cash-service-ads.module.scss';

interface BannerImageProps {
  src: string;
}

const BannerImage: React.FC<BannerImageProps> = ({ src }) => {
  return (
    <img
      src={src}
      alt="AdvCash"
      className={styles['banner-image']}
    />
  );
};

export default BannerImage;

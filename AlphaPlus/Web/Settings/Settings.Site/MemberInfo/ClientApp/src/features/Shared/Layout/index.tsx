import styles from './Layout.module.scss';

interface IProps {
  children: React.ReactNode;
  theme: string;
}

export default function Layout({ children, theme }: IProps) {
  return (
    <div className={styles.layout} data-theme={theme}>
      {children}
    </div>
  );
}

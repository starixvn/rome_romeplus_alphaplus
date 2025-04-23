import { ReactNode } from 'react';

export interface IBlockProps {
  title: string | ReactNode;
  isCollapse?: {
    collapse?: boolean;
    resetCollapse?: number;
  };
  headerChildren?: ReactNode;
  children?: ReactNode;
}

export interface ILabelProps {
  label?: string | ReactNode;
}

export interface IBlockItemProps extends ILabelProps {
  children: string | ReactNode;
}

export interface IBlockRowProps extends IBlockItemProps {
  isHeaderRow?: boolean;
}

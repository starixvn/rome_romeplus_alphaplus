export type ResultType = 'error' | 'success' | 'warning' | 'validate';

export interface ResultMessageProps {
  type?: ResultType;
  message: string | null;
  className?: string;
}

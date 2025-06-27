export type ResultType = 'error' | 'success' | 'warning';

export interface ResultMessageProps {
  type?: ResultType;
  isError?: boolean;
  isSuccess?: boolean;
  isWarning?: boolean;
  message: string | null;
  className?: string;
}

export interface ValidateMessageProps {
  message: string;
  className?: string;
} 
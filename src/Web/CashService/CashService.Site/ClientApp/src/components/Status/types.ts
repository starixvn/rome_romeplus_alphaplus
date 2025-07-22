export type StatusType = 'completed' | 'failed' | 'disabled' | 'processing' | 'maintenance';

export interface StatusComponentProps {
  type?: StatusType;
  text?: string;
  className?: string;
} 
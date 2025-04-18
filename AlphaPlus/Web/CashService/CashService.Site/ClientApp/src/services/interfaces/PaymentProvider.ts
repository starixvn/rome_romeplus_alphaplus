// Payment Provider interface
export interface PaymentProvider {
  id: number;
  name: string;
  fee: number;
  status: number;
  lastUpdatedStr: string;
} 
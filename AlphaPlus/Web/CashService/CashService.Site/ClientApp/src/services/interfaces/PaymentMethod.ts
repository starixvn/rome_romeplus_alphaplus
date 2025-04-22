// Payment Method interface
export interface PaymentMethod {
  id: number;
  name: string;
  providerId: number;
  transactionType: number;
  status: number;
  creator: number;
} 
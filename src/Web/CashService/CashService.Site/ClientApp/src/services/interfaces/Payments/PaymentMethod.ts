// Payment Method interface
export interface PaymentMethod {
  id: number;
  name: string;
  custId?: number;
  providerId: number;
  transactionType: number;
  method: number;
  status: number;
  apiMethodKey?: string;
  merchantID?: string;
  appSecret?: string;
  apiConfig?: string;
} 
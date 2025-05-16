export interface AddWithdrawalModel {
  custId: number;
  paymentFrom: string;
  paymentTo: string;
  amount: number;
  remark: string;
  image: File;
  minLimitAmt: number;
  maxLimitAmt: number;
  providerId: number;
}

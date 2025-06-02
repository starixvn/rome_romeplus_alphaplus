import { PaymentMethod } from "./PaymentMethod";
import { ChannelContactModel } from "./ChannelContactModel";

// AddPaymentMethodViewModel interface
export interface AddPaymentMethodViewModel {
  paymentMethods: PaymentMethod[];
  channelContact: ChannelContactModel;
} 
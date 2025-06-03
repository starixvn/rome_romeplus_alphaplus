import { PaymentMethod } from "./PaymentMethod";
import { ChannelContactModel } from "./ChannelContactModel";

// AddPaymentMethodViewModel interface
export interface AddPaymentMethodViewModel {
  paymentMethods: PaymentMethod[];
  channelContact: ChannelContactModel;
}

// Function to create a default instance of AddPaymentMethodViewModel
export const createDefaultAddPaymentMethodViewModel = (): AddPaymentMethodViewModel => {
  return {
    paymentMethods: [],
    channelContact: {
      tele: ""
    }
  };
};

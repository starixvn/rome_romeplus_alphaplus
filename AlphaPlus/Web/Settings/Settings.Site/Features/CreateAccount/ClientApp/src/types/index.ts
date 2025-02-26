export interface Result {
  code: number;
  message: string;
  isSuccessful: boolean;
  ErrMsg?: string;
}

export interface SubMessagesResult extends Result {
  subMessages: {
    isLive: boolean;
    message: string;
  }[];
}

export interface CreateAccountResult extends SubMessagesResult {
  availableAlphaDomain?: string[];
  newUserName: string;
  userName: string;
}
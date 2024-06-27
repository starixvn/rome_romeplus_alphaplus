export interface Result {
  code: number;
  message: string;
  isSuccessful: boolean;
  ErrMsg?: string;
}

export interface SubMessagesResult extends Result {
  subMessages: string[];
}

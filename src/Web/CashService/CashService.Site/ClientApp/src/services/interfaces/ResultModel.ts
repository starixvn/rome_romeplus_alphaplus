// Result Model interface for standardized API responses
export interface ResultModel {
  isSuccessful: boolean;
  message: string;
  code: number;
} 

export const createErrorResultModel = (code: number, message: string): ResultModel => {
  return {
    isSuccessful: false,
    message,
    code
  };
};



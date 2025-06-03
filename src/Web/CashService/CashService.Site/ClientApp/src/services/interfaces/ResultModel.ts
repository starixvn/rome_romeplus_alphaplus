// Result Model interface for standardized API responses
export interface ResultModel {
  isSuccessful: boolean;
  message: string;
  code: number;
} 

export const createErrorResultModel = (message: string, code: number = 400): ResultModel => {
  return {
    isSuccessful: false,
    message,
    code
  };
};



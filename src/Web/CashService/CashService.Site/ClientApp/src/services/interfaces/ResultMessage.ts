export interface ResultMessage {
    type: 'error' | 'success' | 'warning';
    message: string;
}
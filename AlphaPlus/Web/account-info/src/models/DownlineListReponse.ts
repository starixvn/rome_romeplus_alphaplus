import Downline from './downline';
import TotalDownline from './total-downline';

export default interface DownlineListResponse {
    DownlineList: Downline[];
    TotalDownlines: TotalDownline[];
    TotalPages: number;
}

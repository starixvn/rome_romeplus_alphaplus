import Downline from './downline';

export default interface DownlineListResponse {
    DownlineList: Downline[];
    TotalPages: number;
    StatusId: number;
    PageIndex: number;
}

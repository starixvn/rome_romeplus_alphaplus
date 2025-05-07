export interface Promotion {
    PromotionTitle: string;
    PromotionType: string;
    FromDate: Date;
    ToDate: Date;
    SportsPercentage: number
    VirtualSportsPercentage: number;
    GamesPercentage: number;
    LiveCasinoPercentage: number;
    SportsMaxPayout: number;
    VirtualSportsMaxPayout: number;
    GamesMaxPayout: number;
    LiveCasinoMaxPayout: number;
    Period: string;
    Provider: string;
    Status: string;
    LastUpdated: string;
}

export interface PromotionParams {
    Type: string;
    Status: string;
}
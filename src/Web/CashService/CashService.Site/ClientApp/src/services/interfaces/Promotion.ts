export interface Promotion {
    promotionTitle: string;
    promotionType: string;
    fromDate: Date;
    toDate: Date;
    sportsPercentage: number;
    virtualSportsPercentage: number;
    gamesPercentage: number;
    liveCasinoPercentage: number;
    sportsMaxPayout: number;
    virtualSportsMaxPayout: number;
    gamesMaxPayout: number;
    liveCasinoMaxPayout: number;
    period: string;
    provider: string;
    status: string;
    lastUpdated: string;
}

export interface PromotionParams {
    type: string;
    status: string;
}
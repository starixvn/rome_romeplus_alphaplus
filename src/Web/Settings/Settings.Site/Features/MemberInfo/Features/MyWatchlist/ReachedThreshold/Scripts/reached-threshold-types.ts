'use strict';

interface ReachlistAccount {
	RowNumber: number;
	Username: string;
	LastLoggedString?: string;
	TranslatedStatus: string;
	IsOpen: boolean;
	CustId: number;
	IsPAText: string;
	LastReachedDateString: string;
	TranslatedLastLimitTypeReach: string;
	WinLossAmountString: string;
	WinningThresholdString: string;
	LosingThresholdString: string;
	ReachedCountString: string;
	Status: string;
	ClosedReason: number;
	Locked: number;
}

interface ReachedThresholdPagingData {
	ReachlistAccounts: ReachlistAccount[];
	TotalItems: number;
}



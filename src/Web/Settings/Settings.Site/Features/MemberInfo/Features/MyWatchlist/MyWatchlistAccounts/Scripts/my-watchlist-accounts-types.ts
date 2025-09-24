'use strict';

interface MyWatchlistAccountRow {
	RowNumber: number;
	CustId: number;
	Username: string;
	LastLoggedString?: string;
	AddedDateString?: string;
	TranslatedStatus: string;
	IsPAText: string;
	WinningThresholdString: string;
	LosingThresholdString: string;
	WinningThreshold: number;
	LosingThreshold: number;
	Status: string;
	ClosedReason: number;
	Locked: number;
}

interface MyWatchlistSummaryData {
	ActiveMembers: number;
	MonitoredMembers: number;
	MinAddedDateString?: string;
	ReachedDailyLimitMembers: number;
	UnlimitedThresholdMembers: number;
	UnlimitedThresholdUsers?: string;
	UnlimitedThresholdCustId?: string;
}

interface MyWatchlistAccountPagingData {
	WatchlistAccounts: MyWatchlistAccountRow[];
	TotalItems: number;
	WatchlistSummary: MyWatchlistSummaryData;
}



export interface UnsuspendSingleModel {
    RoleId: number;

    AffectedDownlines: number;

    UnchangedDownlines: number;
}

export interface UnsuspendSingleDataViewModel {
    SearchAccountId: number;

    SearchAccountName: string;

    Levels: UnsuspendSingleModel[];

    AffectedDownlinesTotal: number;

    UnchangedDownlinesTotal: number;
}

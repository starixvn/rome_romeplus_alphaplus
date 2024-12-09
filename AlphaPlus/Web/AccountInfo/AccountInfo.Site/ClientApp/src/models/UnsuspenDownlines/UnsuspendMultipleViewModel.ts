import Result from '../result';

export interface UnsuspendDownlineModel {
    SearchAccountId: number;
    DownlineRoleId: number;
    AffectedDownlines: number;
    UnchangedDownlines: number;
}

export interface UnsuspendMultipleModel {
    SearchAccountId: number;
    Username: string;
    Suspended: boolean;
    Downlines: UnsuspendDownlineModel[];
}

export interface UnsuspendMultipleViewModel {
    SearchAccountRoleId: number;
    Data: UnsuspendMultipleModel[];
}

export interface UnsuspendMultipleResult {
    RoleId: number;
    Results: Result[];
}

export default interface Customer {
    username: string;
    masterName: string;
    showMaster: boolean;
    agentName: string;
    showAgent: boolean;
    memberName: string;
    showMember: boolean;
    roleId: number;
    canViewLog: boolean;
    isShowCreateMemberByQRCode: boolean;
    createMemberByQRCodeUrl: string;
    Enabled: boolean;
    Closed: boolean;
    ClosedReason: number;
    Suspended: boolean;
    Locked: number;
}

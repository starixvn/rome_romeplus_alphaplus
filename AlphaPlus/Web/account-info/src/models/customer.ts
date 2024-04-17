export default interface Customer {
    CustId: number;
    RoleId: number;
    ActorRoleId: number;
    UserName: string;
    NickName: string;
    ShowNickName: boolean;
    FullName: string;
    Phone: string;
    Fax: string;
    Closed: boolean;
    ClosedReason: number;
    Suspended: boolean;
    Enabled: boolean;
    Locked: number;
    MasterName: string;
    ShowMaster: boolean;
    AgentName: string;
    ShowAgent: boolean;
    MemberName: string;
    ShowMember: boolean;
    CanViewLog: boolean;
    IsShowCreateMemberByQRCode: boolean;
    CreateMemberByQRCodeUrl: string;
}

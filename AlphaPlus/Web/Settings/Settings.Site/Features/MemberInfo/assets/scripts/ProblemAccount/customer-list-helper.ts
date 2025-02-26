
class CustomerListHelper {
    viewResources: ViewResources;
    $: JQueryStatic;
    private canUpdateAccount: string;

    constructor($: JQueryStatic, viewResources: ViewResources) {
        this.$ = $;
        this.viewResources = viewResources;
        this.canUpdateAccount = JSON.parse(this.$("#CanUpdateAccount").val());
    }

    private buildCommonAttributesJson(CustId, Username: string, roleId) {
        return [
            { Name: 'cust-id', Value: CustId },
            { Name: 'username', Value: Username },
            { Name: 'cust-role', Value: roleId }
        ];
    }

    private buildNoInfoContent(colspan: number) {
        return `<tr><td colspan='${colspan}' align='center' class='no-info'>${this.viewResources.NoInformationIsAvailable}</td></tr>`;
    }

    private renderCheckboxColumn = (item, isAgentLogin: boolean, levelId: number) => {
        if (isAgentLogin || item.CanEdit) {
            const attrs = {
                'class': 'checkbox',
                'data-custid': item.CustId,
                'data-username': item.UserName,
                'data-custrole': levelId
            };

            return `<input type="checkbox" ${this.buildAttributes(attrs)} />`;
        }

        return "";
    }

    private renderEditColumn = item => {
        const attrs = {
            'class': 'edit icon-table-edit',
            'id': `setting-${item.CustId}`,
            'data-custid': item.CustId,
            'data-username': item.UserName,
            'title': this.viewResources.EditSettingText
        };

        return `<div ${this.buildAttributes(attrs)}/>`;
    }

    private renderUsernameColumn = item => {
        let lockedOrMonitored = "";
        if (item.IsExistedWatchlist) {
            lockedOrMonitored += `<span class="icon-mywatchlist" title="${this.viewResources.MonitoredMemberText}"></span>`;
        }
        if (item.Locked) {
            lockedOrMonitored += `<span class="icon-locked" title="${this.viewResources.LockedText}"></span>`;
        }

        return lockedOrMonitored.length
            ? `<div class="cell-username">${item.UserName}<span class="right-icon">${lockedOrMonitored}</span></div>`
            : `${item.UserName}`;
    }

    private renderStatusColumn = item => {
        const className = item.Status === 1 ? "open-button" : "";
        const attrs = {
            'class': `${className}`,
            'id': `status-${item.CustId}`,
            'data-custid': item.CustId,
            'data-username': item.UserName,
        };

        return `<div ${this.buildAttributes(attrs)}>${item.StatusName}</div>`;
    }

    private renderReviewStatusColumn = (item, levelId: number) => {
        if (item.CanEdit) {
            const attrs = {
                'class': 'not-reviewed',
                'data-custid': item.CustId,
                'data-username': item.UserName,
                'data-custrole': levelId
            };

            return `<div ${this.buildAttributes(attrs)}>${item.ReviewStatus}</div>`;
        }

        return item.ReviewStatus;
    };

    private buildAttributes = attrs => this.$.map(attrs, (value, key) => `${key}='${value}'`).join(' ');

    private buildColumnAttributesJson(customer, levelId: number, actorId: number, customerIndex: number) {
        const memberId = siteInfrastructure.Role.Member.Id, isMember = levelId === memberId,
            isAgentLogin = actorId === siteInfrastructure.Role.Agent.Id,
            isViewNickname = actorId - 1 == levelId,
            { CustId, UserName, StatusName, NickName, FirstName, LastName, CreatedDateStr, ReviewStatus, Count } = customer,
            commonAttributesJson = this.buildCommonAttributesJson(CustId, UserName, levelId);

        return [
            {
                Content: customerIndex.toString(),
                Attributes: [{ Name: 'class', Value: 'text-center' }],
                Visibile: true
            },
            {
                Content: this.renderCheckboxColumn(customer, isAgentLogin, levelId),
                Attributes: [{ Name: 'class', Value: 'center' }],
                Visibile: this.canUpdateAccount
            },
            {
                Content: this.renderEditColumn(customer),
                Attributes: [{ Name: 'class', Value: 'center' }],
                Visibile: isAgentLogin && this.canUpdateAccount
            },
            {
                Content: isAgentLogin ? this.renderUsernameColumn(customer) : UserName,
                Attributes: [{ Name: 'class', Value: isMember ? '' : ' link view-downline' }].concat(commonAttributesJson),
                Visibile: true
            },
            {
                Content: NickName,
                Attributes: [],
                Visibile: isViewNickname
            },
            {
                Content: FirstName,
                Attributes: [],
                Visibile: true
            },
            {
                Content: LastName,
                Attributes: [],
                Visibile: true
            },
            {
                Content: isAgentLogin && this.canUpdateAccount ? this.renderStatusColumn(customer) : StatusName,
                Attributes: [{ Name: 'class', Value: 'text-center' }],
                Visibile: true
            },
            {
                Content: CreatedDateStr,
                Attributes: [{ Name: 'class', Value: 'text-center' }],
                Visibile: true
            },
            {
                Content: Count,
                Attributes: [{ Name: 'class', Value: 'text-right' }],
                Visibile: !isMember
            },
            {
                Content: isAgentLogin || !this.canUpdateAccount ? ReviewStatus : this.renderReviewStatusColumn(customer, levelId),
                Attributes: [{ Name: 'class', Value: 'text-center' }],
                Visibile: true
            }
        ];
    }

    private buildAccountStatusCssClass(customer) {
        let className = "";
        switch (customer.Status) {
            case 2:
            case 4:
                className = customer.StatusNameEN;
                break;
            case 3:
                className = customer.StatusNameEN + customer.ClosedReasonName;
                break;
            default:
                className = customer.Locked === 0 ? customer.StatusNameEN : this.buildLockedAccessCssClass(customer.Locked);
                break;
        }

        return 'row-customer RowBg' + className;
    }

    private buildLockedAccessCssClass(locked: number) {
        switch (locked) {
            case 1: // OTP/Security Code
                return 'LockedFalsedOTPSC';
            case 2: // Password
                return 'LockedFalsePassword';
            default:
                return ''
        }
    }

    public buildBody(Customers, levelId: number, actorId: number, reportParam: ReportParam) {
        if (Customers.length === 0) {
            if (levelId === siteInfrastructure.Role.Member.Id) {
                return this.buildNoInfoContent(this.viewResources.TotalColumnMember);
            } else if (levelId === siteInfrastructure.Role.Agent.Id) {
                return this.buildNoInfoContent(this.viewResources.TotalColumnAgent);
            } else {
                return this.buildNoInfoContent(this.viewResources.TotalColumnMaster);
            }
        }

        let customerIndex = 1, bodyContent = '';

        for (let i = 0; i < Customers.length; i++ , customerIndex++) {
            let rowContent = '';
            this.buildColumnAttributesJson(
                Customers[i],
                levelId,
                actorId,
                customerIndex).forEach(column => {
                    if (column.Visibile) {
                        rowContent += new ColumnWrapper(column.Content,
                            column.Attributes.map(attr => new Attribute(attr.Name, attr.Value))).ToString();
                    }
                });

            const rowAttributes = [
                new Attribute('class', this.buildAccountStatusCssClass(Customers[i])),
                new Attribute('cust-id', Customers[i].CustId)
            ];
            const reviewAttribute = Customers[i].IsReviewed ? new Attribute('is-reviewed', Customers[i].IsReviewed) : null;
            reviewAttribute && rowAttributes.push(reviewAttribute);

            const statusAttribute = Customers[i].StatusNameEN === "Open" ? new Attribute('is-open', 'true') : null;
            statusAttribute && rowAttributes.push(statusAttribute);

            const monitoredAttribute = Customers[i].IsExistedWatchlist ? new Attribute('is-monitored', Customers[i].IsExistedWatchlist) : null;
            monitoredAttribute && rowAttributes.push(monitoredAttribute);

            bodyContent += new RowWrapper(rowContent, rowAttributes).ToString();
        }

        return bodyContent;
    }
}
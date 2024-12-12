'use strict';

declare var encryptor: Encryptor;

interface Window {
    age: {
        SignOut: Function;
    };
    clearPopupSubtitle: Function;
    ShowSecCodePopup: Function;
    popupManager: {
        open: Function;
        openWithHanlder: Function;
        close: Function;
        closeAllPopup: Function;
    };
    passwordEmptyMsg: string;
    editInfoText: string;
    passwordFormatMsg: string;
    hasnotPerm: boolean;
}

class SubAccount {
    topWindow: Window;
    $: JQueryStatic;

    constructor($: JQueryStatic, topWindow: Window) {
        this.$ = $;
        this.topWindow = topWindow;
    }

    Init = () => {
        if (!window.hasnotPerm) {
            this.$('#btn-add').click((item) => {
                this.Add(this.$(item.currentTarget).text());
            });
            this.$('.info').click((item) => {
                var subAccId = this.$(item.currentTarget).data('subaccid');
                this.Edit(subAccId, window.editInfoText);
            });
            this.$('.pwd').click((item) => {
                var subAccId = this.$(item.currentTarget).data('subaccid');
                var username = this.$(item.currentTarget).data('username');
                this.ResetPassword(subAccId, username);
            });
            this.$('.sc').click((item) => {
                var subAccId = this.$(item.currentTarget).data('subaccid');
                var username = this.$(item.currentTarget).data('username');
                this.ResetSecurityCode(subAccId, username);
            });
            this.$('.otp').click((item) => {
                var subAccId = this.$(item.currentTarget).data('subaccid');
                var username = this.$(item.currentTarget).data('username');
                this.RemoveOtp(subAccId, username);
            });
            this.$('.pageaccess').click((item) => {
                var subAccId = this.$(item.currentTarget).data('subaccid');
                this.UpdatePageAccess(subAccId, item);
            });
            this.$('.lMemInfo').click((item) => {
                this.OpenUpdatePopup(item);
            });
            this.$('#btnEdit').click(() => {
                this.UpdateInfo();
            });
            this.$('#btn-reset').click(() => {
                this.Reset();
            });
            this.$('#MemInfo').click((item) => {
                this.OpenAddPopup(item);
            });
            this.$('#btnAdd').click(() => {
                if (this.ValidatePassword(this.$('#txtPwd').val())) {
                    this.AddSubAccount(0);
                } else {
                    this.$('#errMsg')
                        .html(
                            this.$.trim(this.$('#txtPwd').val()) === ''
                                ? window.passwordEmptyMsg
                                : window.passwordFormatMsg
                        )
                        .css({
                            border: '1px solid red',
                            padding: '10px',
                            color: 'red'
                        })
                        .show();
                }
            });
            this.$('#None').change((item) => {
                this.$('#MemInfo').text(this.$(item.currentTarget).next().text());
            });
            this.$('#View').change((item) => {
                this.$('#MemInfo').text(this.$(item.currentTarget).next().text());
            });
            this.$('#UpdateView').change((item) => {
                this.$('#MemInfo').text(this.$(item.currentTarget).next().text());
            });
            this.$('#FullControl').change((item) => {
                this.$('#MemInfo').text(this.$(item.currentTarget).next().text());
            });
            this.$('#txtPwd').keyup((item) => {
                var pwd = this.$(item.currentTarget).val();
                this.ValidatePassword(pwd);
            });
        }
    };

    Add = (title: string) => {
        this.topWindow.popupManager.open('/settings/SubAccount/Add', title, 734, 536);
    };

    Edit = (subAccId: number, title: string) => {
        this.topWindow.popupManager.open('/settings/SubAccount/Get?subAccId=' + subAccId, title, 468, 368);
    };

    ResetPassword = (subAccId: number, username: string) => {
        this.topWindow.popupManager.open(
            '/main/Password/ResetSubAccountPassword?subAccountId=' + subAccId + '&Username=' + username,
            '',
            734,
            536
        );
    };

    ResetSecurityCode = (subAccId: number, username: string) => {
        this.topWindow.popupManager.open(
            '/main/SecurityCode/ResetSecurityCode?subAccId=' + subAccId + '&Username=' + username,
            '',
            734,
            536
        );
        this.topWindow.clearPopupSubtitle(true);
    };

    RemoveOtp = (subAccId: number, username: string) => {
        this.topWindow.popupManager.open(
            '/main/Otp/ResetOtp?subAccountId=' + subAccId + '&Username=' + username,
            '',
            734,
            536
        );
        this.topWindow.clearPopupSubtitle(true);
    };

    UpdatePageAccess = (subAccId: number, item: any) => {
        this.$.blockUI();

        var pageAccess = this.$(item.currentTarget).data('pageaccess');

        if (this.$('#' + subAccId + '-D').is(':checked')) {
            pageAccess += '[D]';
        }

        if (this.$('#' + subAccId + '-E').is(':checked')) {
            pageAccess += '[E]';
        }

        if (this.$('#' + subAccId + '-L').is(':checked')) {
            pageAccess += '[L]';
        }

        if (this.$('#' + subAccId + '-C').is(':checked')) {
            pageAccess += '[C]';
        }

        if (this.$('#' + subAccId + '-G').is(':checked')) {
            pageAccess += '[G]';
        }

        var data = {
            subAccId: subAccId,
            pageAccess: pageAccess
        };

        var updateUrl = '/settings/SubAccount/UpdatePageAccess';

        this.$.ajax({
            headers: {
                RequestVerificationToken: this.$("input[name='__RequestVerificationToken']").val(),
                'X-Requested-With': 'XMLHttpRequest'
            },
            url: updateUrl,
            data: data,
            type: 'POST',
            success: (result) => {
                if (result && result.code === 999991) {
                    var $top = this;
                    this.topWindow[updateUrl] = function () {
                        $top.UpdatePageAccess(subAccId, item);
                    };

                    this.topWindow.ShowSecCodePopup(updateUrl, this.topWindow.age);
                } else {
                    if (result && result.Code !== 0) {
                        alert(
                            (result && result.Message) ||
                                'Sorry, there was an unexpected issue on the server. Please seek help from our customer support. [5000]!'
                        );
                    }

                    (<HTMLIFrameElement>this.topWindow.$('#main')[0]).src = (<HTMLIFrameElement>(
                        this.topWindow.$('#main')[0]
                    )).src;
                }
            },
            complete: () => {
                this.$.unblockUI();
            }
        });
    };

    AddSubAccount = (subAccId: number) => {
        this.$.blockUI();

        var pageAccess = this.$('#View').is(':checked')
            ? '[H]'
            : this.$('#UpdateView').is(':checked')
            ? '[B][H]'
            : this.$('#FullControl').is(':checked')
            ? '[A][B][H]'
            : '';

        if (this.$('#' + subAccId + '-D').is(':checked')) {
            pageAccess += '[D]';
        }

        if (this.$('#' + subAccId + '-E').is(':checked')) {
            pageAccess += '[E]';
        }

        if (this.$('#' + subAccId + '-L').is(':checked')) {
            pageAccess += '[L]';
        }

        if (this.$('#' + subAccId + '-C').is(':checked')) {
            pageAccess += '[C]';
        }

        if (this.$('#' + subAccId + '-G').is(':checked')) {
            pageAccess += '[G]';
        }

        var data = {
            first: this.$('#FirstOptions_Value').val(),
            next: this.$('#NextOptions_Value').val(),
            password: encryptor.encrypt(this.$('#txtPwd').val(), this.$('#Code').val()),
            firstName: this.$('#txtFname').val() || '-',
            lastName: this.$('#txtLname').val() || '-',
            pageAccess: pageAccess || ''
        };

        var updateUrl = '/settings/SubAccount/Add';

        this.$.ajax({
            headers: {
                RequestVerificationToken: this.$("input[name='__RequestVerificationToken']").val(),
                'X-Requested-With': 'XMLHttpRequest'
            },
            url: updateUrl,
            data: data,
            type: 'POST',
            success: (result) => {
                if (result && result.IsSuccessful) {
                    (<HTMLIFrameElement>this.topWindow.$('#main')[0]).src = (<HTMLIFrameElement>(
                        this.topWindow.$('#main')[0]
                    )).src;
                    this.topWindow.popupManager.close();
                } else {
                    this.$('#errMsg')
                        .html(
                            (result && result.Message) ||
                                'Sorry, there was an unexpected issue on the server. Please seek help from our customer support. [5000]!'
                        )
                        .css({
                            border: '1px solid red',
                            padding: '10px',
                            color: 'red'
                        })
                        .show();
                }
            },
            complete: () => {
                this.$.unblockUI();
            }
        });
    };

    OpenUpdatePopup = (evt: any) => {
        var currentPerm = this.$(evt.currentTarget).data('pageaccess');
        switch (currentPerm) {
            case '[H]':
                this.$('#pView').prop('checked', true);
                break;

            case '[B][H]':
                this.$('#pUpdateView').prop('checked', true);
                break;

            case '[A][B][H]':
                this.$('#pFullControl').prop('checked', true);
                break;

            default:
                this.$('#pNone').prop('checked', true);
                break;
        }

        this.$('input[type=radio]').change((item) => {
            var subAccId = this.$(evt.currentTarget).data('subaccid');
            this.UpdatePageAccess(subAccId, item);
        });

        var popup = this.$('#Popup');
        popup.css({
            top: evt.clientY + 'px',
            left: evt.clientX + 'px',
            display: 'block',
            position: 'absolute'
        });
        var timerId = setTimeout(() => {
            popup.hide(100);
        }, 2000);

        popup.on('mouseover', () => {
            clearTimeout(timerId);
        });

        popup.on('mouseout', () => {
            timerId = setTimeout(() => {
                popup.hide(100);
            }, 100);
        });
    };

    OpenAddPopup = (evt: any) => {
        var popup = this.$('#nPopup');
        popup.css({
            top: evt.clientY + 'px',
            left: evt.clientX + 'px',
            display: 'block',
            position: 'absolute'
        });
        var timerId = setTimeout(() => {
            popup.hide(100);
        }, 2000);

        popup.on('mouseover', () => {
            clearTimeout(timerId);
        });

        popup.on('mouseout', () => {
            timerId = setTimeout(() => {
                popup.hide(100);
            }, 100);
        });
    };

    UpdateInfo = () => {
        this.$.blockUI();

        var subAccId = this.$('#SubAccId').val();
        var firstName = this.$('#txtFirstName').val();
        var lastName = this.$('#txtLastName').val();
        var closed = this.$('#chkClosed').is(':checked');

        this.$.ajax({
            headers: {
                RequestVerificationToken: this.$("input[name='__RequestVerificationToken']").val(),
                'X-Requested-With': 'XMLHttpRequest'
            },
            url: '/settings/SubAccount/UpdateInfo',
            data: {
                subAccId: subAccId,
                firstName: firstName || '-',
                lastName: lastName || '-',
                closed: closed
            },
            type: 'POST',
            success: (result) => {
                if (result && result.IsSuccessful) {
                    (<HTMLIFrameElement>this.topWindow.$('#main')[0]).src = (<HTMLIFrameElement>(
                        this.topWindow.$('#main')[0]
                    )).src;
                    this.topWindow.popupManager.close();
                } else {
                    this.$('#errMsg')
                        .html(
                            (result && result.Message) ||
                                'Sorry, there was an unexpected issue on the server. Please seek help from our customer support. [5000]!'
                        )
                        .css({
                            border: '1px solid red',
                            padding: '10px',
                            color: 'red'
                        })
                        .show();
                }
            },
            complete: () => {
                this.$.unblockUI();
            }
        });
    };

    Reset = () => {
        this.$('#txtFirstName').val(this.$('#Firstname').val());
        this.$('#txtLastName').val(this.$('#Lastname').val());
        this.$('#chkClosed').prop('checked', this.$('#Closed').val() === 'True' ? true : false);
    };

    ValidatePassword = (password: string): boolean => {
        var validPwd = this.IsPassword(password);
        if (validPwd) {
            this.$('#spnPwdIcon').removeClass('PwdError').removeClass('PwdSuccess').addClass('PwdSuccess');
            return true;
        } else {
            this.$('#spnPwdIcon').removeClass('PwdError').removeClass('PwdSuccess').addClass('PwdError');
            return false;
        }
    };

    IsPassword = (str: string): boolean => {
        var regex1 = /^(?=.{8,100}$).*\d/i;
        var regex2 = /^(?=.{8,100}$).*[a-z]/;
        var regex3 = /^(?=.{8,100}$).*[A-Z]/;
        var regex4 = /^(?=.{8,100}$).*[\[\]\^\$\.\|\?\*\+\(\)\\~`\!@#%&\-_+={}'""<>:;,]/i;

        var whiteSpace = /^\S*$/;

        var case1 = str.search(regex1) != -1 ? 1 : 0;
        var case2 = str.search(regex2) != -1 ? 1 : 0;
        var case3 = str.search(regex3) != -1 ? 1 : 0;
        var case4 = str.search(regex4) != -1 ? 1 : 0;

        var case5 = str.search(whiteSpace) != -1;
        return case1 + case2 + case3 + case4 >= 3 && case5;
    };
}

const subAcc = new SubAccount($, top);

$(() => {
    subAcc.Init();
});

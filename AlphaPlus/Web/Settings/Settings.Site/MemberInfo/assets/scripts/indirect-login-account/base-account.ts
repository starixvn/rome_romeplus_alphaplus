﻿"use strict";

abstract class BaseAccount {
    $: JQueryStatic;
    topWindow: any;
    encryptor: Encryptor;
    alert: Alert;
    txtCustName: JQuery;
    txtPassword: JQuery;
    hashKey: string;
    requestVerificationToken: string;
    resources: any = {};

    protected constructor(
        $: JQueryStatic,
        topWindow: any,
        encryptor: Encryptor,
        alert: Alert) {
        this.$ = $;
        this.topWindow = topWindow;
        this.encryptor = encryptor;
        this.alert = alert;
    }

    public init(): void {
        this.cacheDOMElements();
        this.registerEvents();
    }

    public cacheDOMElements(): void {
        this.txtCustName = this.$("#txt-custname");
        this.txtPassword = this.$("#txt-password");
        this.hashKey = this.$("#txt-hashkey").val();
        this.requestVerificationToken = this.$('input[name="__RequestVerificationToken"]').val();

        this.resources.requiredPasswordError = this.$("#txt-requiredPasswordError").val();
        this.resources.requiredYourDownlineUsernameOrNicknameError = this.$("#txt-requiredYourDownlineUsernameOrNicknameError").val();
    }

    public registerEvents(): void {
        const self = this;

        this.$("#chk-showPassword").change(function () {
            self.txtPassword.attr("type", this.checked ? "text" : "password");
        });

        this.$("#form-account").submit(function (e) {
            e.preventDefault();
            self.onSubmitForm(self.$(this).attr("action"));
        });
    }

    public onSubmitForm(url: string): void {
        this.$.blockUI();
        const formData = this.getFormData();

        if (!formData) {
            this.$.unblockUI();
            return;
        }

        this.$.ajax({
            type: "POST",
            url: url,
            beforeSend: xhr => xhr.setRequestHeader("RequestVerificationToken", this.requestVerificationToken),
            data: formData,
            dataType: "json",
            success: data => {
                if (data.ErrCode) {
                    this.alert.showError(data.ErrMsg);
                } else {
                    this.alert.show(data.Code, data.Message);
                    data.IsOk && this.onSubmitFormSuccess();
                }
            },
            complete: this.$.unblockUI
        });
    }

    public getFormData() {
        const custname = this.$.trim(this.txtCustName.val());

        if (!custname) {
            this.alert.showError(this.resources.requiredYourDownlineUsernameOrNicknameError);
            return;
        }

        const password = this.txtPassword.val();

        if (!password) {
            this.alert.showError(this.resources.requiredPasswordError);
            return;
        }

        this.alert.hide();
        return {
            custname: custname,
            password: this.encryptor.encrypt(password, this.hashKey),
            browsersize: this.getBrowserSize()
        };
    }

    protected getBrowserSize(): string {
        var w = this.topWindow.window,
            d = this.topWindow.document,
            e = d.documentElement,
            g = d.getElementsByTagName("body")[0],
            x = w.innerWidth || e.clientWidth || g.clientWidth,
            y = w.innerHeight || e.clientHeight || g.clientHeight;

        return x + "x" + y;
    }

    abstract onSubmitFormSuccess(): void;
}
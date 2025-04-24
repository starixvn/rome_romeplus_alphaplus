describe("Disconnect", () => {
    let topWindow: any;
    let alert: Alert;
    let disconnect: Disconnect;
    const template =
        `<div id="alert-message" class="msg-content hidden">
            <div id="alert-message-content" class=""></div>
        </div>
        <form action="/site-memberinfo/IndirectLoginAccount/Disconnect" class="form-validation" id="form-disconnect" method="post">
            <input name="__RequestVerificationToken" type="hidden" value="EdbdDOVeOLzAhgerZOsV6ARFk7zjoip2ruAqRYHikA6l51PNZzpbLevmi0rNgRjqlLEHrlUBlqO1zMFeOmmYnx_ZZ0U1">
            <input type="hidden" value="1176083" id="txt-custId">
            <input type="hidden" value="0" id="txt-subAccId">
            <input type="hidden" value="master1" id="txt-custname">
            <button type="submit" class="btn-remove">Disconnect</button>
        </form>`;

    beforeEach(() => {
        setFixtures(template);
        topWindow = {
            closePopupAndReloadMainFrameAfterUpdateSuccess: jasmine.any(Function)
        };

        alert = new Alert($);
        disconnect = new Disconnect($, topWindow, alert);
    });

    describe("_onDisconnect", () => {
        it("_Always call 'blockUI'", () => {
            // arrange
            const url = "";
            spyOn($, "blockUI");

            // act
            disconnect.onDisconnect(url);

            // assert
            expect($.blockUI).toHaveBeenCalled();
        });
        it("_Always call 'ajax'", () => {
            // arrange
            const url = "";
            spyOn($, "ajax");

            // act
            disconnect.onDisconnect(url);

            // assert
            expect($.ajax).toHaveBeenCalled();
        });
    });

    describe("_onDisconnectSuccess", () => {
        it("_ErrCode show error message", () => {
            // arrange
            const data = { ErrCode: 1, ErrMsg: "Error!!!" };
            spyOn(alert, "showError");

            // act
            disconnect.onDisconnectSuccess(data);

            // assert
            expect(alert.showError).toHaveBeenCalled();
        });
        it("_Show message", () => {
            // arrange
            const data = { Code: 0, Message: "Success!!!" };
            spyOn(alert, "show");

            // act
            disconnect.onDisconnectSuccess(data);

            // assert
            expect(alert.show).toHaveBeenCalled();
        });
        it("_IsOk call closePopupAndReloadMainFrameAfterUpdateSuccess", () => {
            // arrange
            const data = { Code: 0, Message: "Success!!!", IsOk: true };
            spyOn(topWindow, "closePopupAndReloadMainFrameAfterUpdateSuccess");

            // act
            disconnect.onDisconnectSuccess(data);

            // assert
            expect(topWindow.closePopupAndReloadMainFrameAfterUpdateSuccess).toHaveBeenCalled();
        });
    });
});
describe("NewAccount", () => {
    let topWindow: any;
    let encryptor: Encryptor;
    let alert: Alert;
    let newAccount: NewAccount;
    const template =
        `<form action="/site-memberinfo/IndirectLoginAccount/NewAccount" id="form-account" method="post">
            <input name="__RequestVerificationToken" type="hidden" value="v-Q43BPhkajksEnOAeRiDRepyfhBeP02nCLmqJrdu5vgev1_0EuYL6_mP3SVPTofZ2rXq4aEiUMsVJMpAEyXD0teQdk1">
            <div class="form-group">
                <label for="txt-custname">Your Downline Username Or Nickname</label>
                <input type="text" id="txt-custname" data-enpassusermodified="yes">
            </div>
            <div class="form-group">
                <label for="txt-password">Password</label>
                <input type="password" id="txt-password" data-enpassusermodified="yes">
            </div>
            <div class="form-group">
                <input type="checkbox" id="chk-showPassword">
                <label for="chk-showPassword">Show Password</label>
            </div>
            <div class="form-group">
                <button type="submit" class="btn-submit">Connect</button>
            </div>
        </form>
        <input type="hidden" value="2dc6" id="txt-hashkey">`;

    beforeEach(() => {
        topWindow = {
            closePopupAndReloadMainFrameAfterUpdateSuccess: jasmine.any(Function)
        };

        encryptor = new Encryptor();
        alert = new Alert($);
        newAccount = new NewAccount($, topWindow, encryptor, alert);
    });

    describe("_init", () => {
        beforeEach(() => {
            // arrange
            spyOn(newAccount, "cacheDOMElements");
            spyOn(newAccount, "registerEvents");

            // act
            newAccount.init();
        });

        it("_Always_Calls cacheDOMElements", () => {
            expect(newAccount.cacheDOMElements).toHaveBeenCalled();
        });
        it("_Always_Calls registerEvents", () => {
            expect(newAccount.registerEvents).toHaveBeenCalled();
        });
    });

    describe("_cacheDOMElements", () => {
        beforeEach(() => {
            setFixtures(template);
        });
        it("_Always 'txtCustName' is cached", () => {
            // act
            newAccount.cacheDOMElements();

            // assert
            expect(newAccount.txtCustName).toBeDefined();
        });
        it("_Always 'txtPassword' is cached", () => {
            // act
            newAccount.cacheDOMElements();

            // assert
            expect(newAccount.txtPassword).toBeDefined();
        });
        it("_Always 'hashKey' is cached", () => {
            // act
            newAccount.cacheDOMElements();

            // assert
            expect(newAccount.hashKey).toBeDefined();
        });
        it("_Always 'requestVerificationToken' is cached", () => {
            // act
            newAccount.cacheDOMElements();

            // assert
            expect(newAccount.requestVerificationToken).toBeDefined();
        });
    });

    describe("_registerEvents", () => {
        it("_Always bind form submit event", () => {
            // arrange
            setFixtures(template);
            spyOn(newAccount, "onSubmitForm");

            // act
            newAccount.registerEvents();
            $("#form-account").submit();

            // assert
            expect(newAccount.onSubmitForm).toHaveBeenCalled();
        });
    });
});
describe("ManageAccount", () => {
    let topWindow: any;
    let manageAccount: ManageAccount;
    const template =
        `<input type="button" class="btn-submit" value="Connect To New Account" id="btn-connectToNewAccount" />
        <table>
            <tbody>
                <tr data-custid="1176083" data-custname="master1">
                    <td class="text-center">
                        <span class="icon icon-indirect-login"></span>
                    </td>
                    <td class="text-center">
                        <span class="icon icon-disconnect"></span>
                    </td>
                </tr>
            </tbody>
        </table>
        <input id="txt-rootURL" type="hidden" value="/site-memberinfo/" />`;

    beforeEach(() => {
        setFixtures(template);
        topWindow = {
            popupManager: {
                open: jasmine.any(Function)
            },
            alpha: {
                detectResponsiveInformation: jasmine.any(Function)
            }
        };

        manageAccount = new ManageAccount($, topWindow);
    });

    describe("_init", () => {
        beforeEach(() => {
            // arrange
            spyOn(manageAccount, "cacheDOMElements");
            spyOn(manageAccount, "registerEvents");

            // act
            manageAccount.init();
        });

        it("_Always_Calls cacheDOMElements", () => {
            expect(manageAccount.cacheDOMElements).toHaveBeenCalled();
        });
        it("_Always_Calls registerEvents", () => {
            expect(manageAccount.registerEvents).toHaveBeenCalled();
        });
    });

    describe("_cacheDOMElements", () => {
        it("_Always 'rootURL' is cached", () => {
            // act
            manageAccount.cacheDOMElements();

            // assert
            expect(manageAccount.rootURL).toBeDefined();
        });
    });

    describe("_registerEvents", () => {
        it("_Always bind Connect To New Account button event", () => {
            // arrange
            manageAccount.cacheDOMElements();
            spyOn(topWindow.popupManager, "open");
            spyOn(topWindow.alpha, "detectResponsiveInformation");

            // act
            manageAccount.registerEvents();
            $("#btn-connectToNewAccount").click();

            // assert
            expect(topWindow.popupManager.open).toHaveBeenCalled();
        });
        it("_Always bind Disconnect event", () => {
            // arrange
            manageAccount.cacheDOMElements();
            spyOn(topWindow.popupManager, "open");

            // act
            manageAccount.registerEvents();
            $(".icon-disconnect").click();

            // assert
            expect(topWindow.popupManager.open).toHaveBeenCalled();
        });
    });
});
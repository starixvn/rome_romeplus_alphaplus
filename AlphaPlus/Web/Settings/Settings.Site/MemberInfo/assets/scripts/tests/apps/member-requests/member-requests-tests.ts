describe("MemberRequests", () => {
    const jQueryFake = $;
    const singleSelectionDropdownComponent = new SingleSelectionDropdownComponent(jQueryFake);
    let topWindow: any;

    let memberRequests: MemberRequests;

    let template = {
        normal: `<body>
                    <iframe id="accountinfo-content">
                    </iframe>
                    <select id="dropdown-product">
                        <option value="6"
                                data-productname="Casino"
                                data-disabled="false"
                                data-has-setting="true"></option>
                    </select>
                    <button id="icon-refresh"></button>
                    <button id="icon-information"></button>
                    <div id="msg-warning"></div>
                    <div id="msg-info"></div>
                    <input id="disabledProductWarningMessage" value="disabledProductWarningMessage" />
                    <input id="noSettingWarning" value="noSettingWarning" />
                    <input id="memberRequestUrl" value="memberRequestUrl" />
                    <div id="box-content">
                        <table>
                            <tr>
                                  <td id="view-member-info-link" class="view-member-info-link">ViewedCustName</td>
                                  <td id="edit-setting" class="edit-setting"
                                        data-custid="1"
                                        data-username="member01"></td>
                            </tr>
                        </table>
                    </div>
                    <div id="account-info-header"></div>
                    <div class="sidebar"></div>
                    <input id="viewerId" value="1" />
                    <input id="editSettingUrl" value="editSettingUrl" />
                 </body>`,
        collapsedSidebar: `<body>
                                <div class="sidebar collapsed"></div>
                                <select id="dropdown-product">
                                    <option value="6"
                                            data-productname="Casino"
                                            data-disabled="false"
                                            data-has-setting="true"></option>
                                </select>
                                <button id="icon-refresh"></button>
                                <div id="msg-warning"></div>
                                <div id="msg-info"></div>
                                <input id="disabledProductWarningMessage" value="disabledProductWarningMessage" />
                                <input id="noSettingWarning" value="noSettingWarning" />
                                <input id="memberRequestUrl" value="memberRequestUrl" />
                                <div id="box-content">
                                    <table>
                                        <tr>
                                              <td id="view-member-info-link" class="view-member-info-link"></td>
                                              <td id="edit-setting" class="edit-setting"
                                                    data-custid="1"
                                                    data-username="member01"></td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="account-info-header"></div>
                                <div class="sidebar"></div>
                                <input id="viewerId" value="1" />
                                <input id="editSettingUrl" value="editSettingUrl" />
                             </body>`,
        hiddenMobileSidebar: `<body>
                                <div class="sidebar hidden-mobile"></div>
                                <select id="dropdown-product">
                                    <option value="6"
                                            data-productname="Casino"
                                            data-disabled="false"
                                            data-has-setting="true"></option>
                                </select>
                                <button id="icon-refresh"></button>
                                <div id="msg-warning"></div>
                                <div id="msg-info"></div>
                                <input id="disabledProductWarningMessage" value="disabledProductWarningMessage" />
                                <input id="noSettingWarning" value="noSettingWarning" />
                                <input id="memberRequestUrl" value="memberRequestUrl" />
                                <div id="box-content">
                                    <table>
                                        <tr>
                                              <td id="view-member-info-link" class="view-member-info-link"></td>
                                              <td id="edit-setting" class="edit-setting"
                                                    data-custid="1"
                                                    data-username="member01"></td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="account-info-header"></div>
                                <div class="sidebar"></div>
                                <input id="viewerId" value="1" />
                                <input id="editSettingUrl" value="editSettingUrl" />
                             </body>`,
        expandedSidebar: `<body>
                            <div class="sidebar"></div>
                            <select id="dropdown-product">
                                <option value="6"
                                        data-productname="Casino"
                                        data-disabled="false"
                                        data-has-setting="true"></option>
                            </select>
                            <button id="icon-refresh"></button>
                            <div id="msg-warning"></div>
                            <div id="msg-info"></div>
                            <input id="disabledProductWarningMessage" value="disabledProductWarningMessage" />
                            <input id="noSettingWarning" value="noSettingWarning" />
                            <input id="memberRequestUrl" value="memberRequestUrl" />
                            <div id="box-content">
                                <table>
                                    <tr>
                                            <td id="view-member-info-link" class="view-member-info-link"></td>
                                            <td id="edit-setting" class="edit-setting"
                                                data-custid="1"
                                                data-username="member01"></td>
                                    </tr>
                                </table>
                            </div>
                            <div id="account-info-header"></div>
                            <div class="sidebar"></div>
                            <input id="viewerId" value="1" />
                            <input id="editSettingUrl" value="editSettingUrl" />
                            </body>`
    };

    beforeEach(() => {
        topWindow = {
            $: jQueryFake,
            menuBar: {
                collapseLeftPanel: jasmine.any(Function)
            },
            popupManager: {
                openByRelativeUrl: jasmine.any(Function),
                open: jasmine.any(Function)
            },
            clearPopupSubtitle: jasmine.any(Function)
        };

        memberRequests = new MemberRequests(jQueryFake, topWindow, singleSelectionDropdownComponent);

        spyOn(singleSelectionDropdownComponent, "render");
    });

    describe("_init", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.normal);
            spyOn(memberRequests, "cacheDOMElements");
            spyOn(memberRequests, "registerEvents");
            spyOn(memberRequests, "changeProduct");

            // act
            memberRequests.init();
        });

        it("_Always_Calls cacheDOMElements", () => {
            expect(memberRequests.cacheDOMElements).toHaveBeenCalled();
        });
        it("_Always_Calls registerEvents", () => {
            expect(memberRequests.registerEvents).toHaveBeenCalled();
        });
        it("_Always_Calls changeProduct", () => {
            expect(memberRequests.changeProduct).toHaveBeenCalled();
        });
        it("_Always_Calls singleSelectionDropdownComponent.render", () => {
            expect(singleSelectionDropdownComponent.render).toHaveBeenCalledWith(
                new SingleSelectionDropdownOption("dropdown-product", "UserSelectedProductId", memberRequests.changeProduct));
        });
    });
    describe("_registerEvents", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.normal);
            spyOn(memberRequests, "refreshPage");
            spyOn(memberRequests, "showGuidelines");

            // act
            memberRequests.registerEvents();
        });
        it("_Always_Bind refresh event", () => {
            // act
            memberRequests.$("#icon-refresh").click();

            // assert
            expect(memberRequests.refreshPage).toHaveBeenCalled();
        });
        it("_Always_Bind show Guidelines event", () => {
            // act
            memberRequests.$("#icon-information").click();

            // assert
            expect(memberRequests.showGuidelines).toHaveBeenCalled();
        });
    });
    describe("_cacheDOMElements", () => {
        it("_Always_All neccessary elements are cached", () => {
            // arrange
            setFixtures(template.normal);

            // act
            memberRequests.cacheDOMElements();

            // assert
            expect(memberRequests.$dropdownProduct).toBeDefined();
            expect(memberRequests.$msgWarning).toBeDefined();
            expect(memberRequests.$msgInfo).toBeDefined();

            expect(memberRequests.disabledProductWarningmessage, CancellationToken cancellation).toBe("disabledProductWarningMessage");
            expect(memberRequests.noSettingWarning).toBe("noSettingWarning");
            expect(memberRequests.memberRequestUrl).toBe("memberRequestUrl");

            expect(memberRequests.$boxContent).toBeDefined();
            expect(memberRequests.$accountInfoHeader).toBeDefined();
            expect(memberRequests.$sideBar).toBeDefined();

            expect(memberRequests.viewerId).toBe(1);
        });
    });

    describe("_changeProduct", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.normal);
            spyOn(memberRequests, "updateWarningMessage");
            spyOn(memberRequests, "loadMemberRequests");

            // act
            memberRequests.changeProduct();
        });
        it("_Always_Calls loadMemberRequests", () => {
            // assert
            expect(memberRequests.loadMemberRequests).toHaveBeenCalled();
        });
    });
    describe("_convertAttributeToBool", () => {
        let convertAttributeToBoolTestCase = (attributeValue: string, expectedValue: boolean) => {
            // act
            let actualValue = memberRequests.convertAttributeToBool(attributeValue);

            // assert
            expect(actualValue).toEqual(expectedValue);
        };

        it("_Null Attribute -> Returns False", () => {
            convertAttributeToBoolTestCase(null, false);
        });
        it("_Empty Value -> Returns False", () => {
            convertAttributeToBoolTestCase("", false);
        });
        it("_Not True Value -> Returns False", () => {
            convertAttributeToBoolTestCase("False", false);
            convertAttributeToBoolTestCase(" ", false);
            convertAttributeToBoolTestCase("  ", false);
        });
        it("_True Value -> Returns True", () => {
            convertAttributeToBoolTestCase("true", true);
            convertAttributeToBoolTestCase("True", true);
        });
    });   
    describe("_loadMemberRequests", () => {
        it("_Always_Block UI and Call Ajax", () => {
            // arrange
            setFixtures(template.normal);
            spyOn(jQueryFake, "blockUI");
            spyOn(jQueryFake, "ajax");
            memberRequests.cacheDOMElements();

            // act
            memberRequests.loadMemberRequests();

            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(jQueryFake.ajax).toHaveBeenCalledWith({
                method: "GET",
                data: {
                    UserSelectedProductId: "6",
                    DisabledUserSelectedProduct: false
                },
                url: memberRequests.memberRequestUrl,
                cache: false,
                success: memberRequests.renderMemberRequests
            });
        });
    });
    describe("_renderMemberRequests", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.normal);
            spyOn(memberRequests, "registerEventsForTable");
            spyOn(jQueryFake, "unblockUI");
            memberRequests.cacheDOMElements();

            // act
            memberRequests.renderMemberRequests("Member Request Content");
        });
        it("_Always_Sets Content to box Content", () => {
            // assert
            expect(memberRequests.$boxContent.html()).toBe("Member Request Content");
        });
        it("_Always_Calls registerEventsForTable", () => {
            // assert
            expect(memberRequests.registerEventsForTable).toHaveBeenCalled();
        });
        it("_Always_Calls $.unblockUI", () => {
            // assert
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
    });
    describe("_getSelectedProduct", () => {
        it("Always_Returns correct value", () => {
            // arrange
            setFixtures(template.normal);
            memberRequests.cacheDOMElements();
            let expectedValue = "6";

            // act
            let actualValue = memberRequests.getSelectedProduct();

            // assert
            expect(actualValue.val()).toBe(expectedValue);
        });
    });  
    describe("_viewMemberInfo", () => {
        let event = { stopPropagation: jasmine.createSpy("stopPropagation", () => null) };

        it("_Side Bar Collapsed_Expand Side Bar", () => {
            // arrange
            setFixtures(template.collapsedSidebar);
            memberRequests.cacheDOMElements();
            spyOn(topWindow.menuBar, "collapseLeftPanel");

            // act
            memberRequests.viewMemberInfo(event, $("#view-member-info-link"));

            // assert
            expect(topWindow.menuBar.collapseLeftPanel).toHaveBeenCalled();
        });
        it("_Side Bar hidden-mobile_Expand Side Bar", () => {
            // arrange
            setFixtures(template.hiddenMobileSidebar);
            memberRequests.cacheDOMElements();
            spyOn(topWindow.menuBar, "collapseLeftPanel");

            // act
            memberRequests.viewMemberInfo(event, $("#view-member-info-link"));

            // assert
            expect(topWindow.menuBar.collapseLeftPanel).toHaveBeenCalled();
        });
        it("_Side Bar Expanded_Not Call collapseLeftPanel", () => {
            // arrange
            setFixtures(template.expandedSidebar);
            memberRequests.cacheDOMElements();
            spyOn(topWindow.menuBar, "collapseLeftPanel");

            // act
            memberRequests.viewMemberInfo(event, $("#view-member-info-link"));

            // assert
            expect(topWindow.menuBar.collapseLeftPanel).not.toHaveBeenCalled();
        });
        it("_Always_Click Account Info", () => {
            // arrange
            setFixtures(template.normal);
            memberRequests.cacheDOMElements();
            spyOn(memberRequests.$accountInfoHeader, "click");

            // act
            memberRequests.viewMemberInfo(event, $("#view-member-info-link"));

            // assert
            expect(memberRequests.$accountInfoHeader.click).toHaveBeenCalled();
        });
        it("_Always_stopPropagation", () => {
            // arrange
            setFixtures(template.normal);
            memberRequests.cacheDOMElements();

            // act
            memberRequests.viewMemberInfo(event, $("#view-member-info-link"));

            // assert
            expect(event.stopPropagation).toHaveBeenCalled();
        });

        it("_Account Info Is Loaded_Search Cust Info", () => {
            // arrange
            jasmine.clock().install();

            setFixtures(template.normal);
            memberRequests.cacheDOMElements();
            spyOn(memberRequests, "isLoadedAccountInfo").and.returnValue(true);
            spyOn(memberRequests, "searchCustInfo");

            // act
            memberRequests.viewMemberInfo(event, $("#view-member-info-link"));
            jasmine.clock().tick(300);

            // assert
            expect(memberRequests.searchCustInfo).toHaveBeenCalledWith("ViewedCustName");
            jasmine.clock().uninstall();
        });
    });
    describe("_isLoadedAccountInfo", () => {
        it("_Could not find txtUsername_Returns false", () => {
            // arrange
            setFixtures(template.normal);
            memberRequests.cacheDOMElements();
            memberRequests.$accountInfoHeader.click();
            memberRequests.$accountInfoContent = memberRequests.topWindow.$("#accountinfo-content");

            // act
            var actualValue = memberRequests.isLoadedAccountInfo();

            // assert
            expect(actualValue).toBeFalsy();
        });
    });   
    describe("_refreshPage", () => {
        it("_Always_Reload location", () => {
            // arrange
            spyOn(location, "reload");

            // act
            memberRequests.refreshPage();

            // assert
            expect(location.reload).toHaveBeenCalled();
        });
    });
    describe("_showGuidelines", () => {
        it("_Always_Show guidelines", () => {
            // arrange
            spyOn(topWindow.popupManager, "open");
            memberRequests.guidelinesUrl = "guidelinesUrl";
            memberRequests.guidelinesTitle = "guidelinesTitle";

            // act
            memberRequests.showGuidelines();

            // assert
            expect(topWindow.popupManager.open).toHaveBeenCalledWith(
                "guidelinesUrl",
                "guidelinesTitle",
                900,
                600,
                "popup-guidelines");
        });
    });
});

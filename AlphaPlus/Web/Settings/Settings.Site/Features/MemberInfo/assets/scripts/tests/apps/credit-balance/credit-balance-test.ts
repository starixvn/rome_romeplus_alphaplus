describe("CreditBalance", () => {
    const jQueryFake: any = $;
    let topWindow: any;
    let pager: PagerComponent;
    let columnsControl: ColumnsControlComponent;
    let statusComponent: SingleSelectionDropdownComponent;
    let creditBalance: CreditBalance;

    let template: any = {
        normal: `<body>
                    <div class="page-title">
                        <span>Credit/Balance
                            <span id="icon-information" class="icon icon-information-outline"></span>
                        </span>
                    </div>
                    <form action="/site-memberinfo/creditbalance" id="creditBalanceForm">
                        <div class="form-group">
                            <label>Username</label>
                            <input type="text"
                                   name="UserName"
                                   class="typeahead"
                                   data-provide="typeahead"
                                   autocomplete="off"
                                   placeholder="Username or First/Last Name" />
                        </div>
                        <div class="form-group">
                            <label>Status</label>
                            <select id="dropdown-status" name="dropdown-status">
                                <option value="0">All</option>
                                <option selected="selected" value="1">Open</option>
                                <option value="2">Suspended</option>
                                <option value="3">Closed</option>
                                <option value="4">Disabled</option>
                            </select>
                        </div>
                        <div class="form-group">
                            <button type="submit" class="btn-submit">Submit</button>
                        </div>
                        <input type="hidden" value="1176070" name="CustId" id="autoSuggestionCustId" />
                        <input type="hidden" value="4" name="CustLevelId" />
                    </form>
                    <div id="tbl-container">
                        <div class="columns-control-container">
                            <div class="columns-control-hint hide" id="columns-control-hint">
                                <p>Right-click on the header to show/hide columns</p>
                                <div>
                                    <input type="checkbox" id="chkDontShowAgain" />
                                    <label for="chkDontShowAgain">Do not show again</label>
                                </div>
                            </div>
                            <div class="columns-control hide" id="columns-control"></div>
                        </div>
                        <table id="tbl-report" class="hide">
                            <thead>
                                <tr>
                                    <th id="column-no">No.</th>
                                    <th class="column-st">Status</th>
                                    <th>Username</th>
                                    <th class="column-fn">First Name</th>
                                    <th class="column-ln">Last Name</th>
                                    <th class="column-cd">Credit</th>
                                    <th class="column-bl">Balance</th>
                                    <th class="column-yb">Yesterday Balance</th>
                                    <th class="column-bc">Available Credit</th>
                                    <th class="column-ot">Outstanding</th>
                                    <th class="column-crd">Created Date</th>
                                    <th class="column-ll">Last Login</th>
                                    <th class="column-li">Login IP</th>
                                    <th class="column-un">Username</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr class="">
                                    <td class="text-right">1</td>
                                    <td class="text-center column-st">Open</td>
                                    <td>
                                        <a href="?custId=1176083&custLevelId=3" class="link">master1</a>
                                    </td>
                                    <td class="column-fn">---</td>
                                    <td class="column-ln">---</td>
                                    <td class="text-right column-cd">
                                        <a href="javascript:;" class="link creditlink" data-custid="1176083" data-username="master1" data-levelid="3">900,000,002</a>
                                    </td>
                                    <td class="text-right column-bl">
                                        <a href="javascript:;" class="link yesterdaybalancelink" data-custid="14244423">
                                            <span class="negative-value">-11,664.31</span>
                                        </a>
                                    </td>
                                    <td class="text-right column-yb">
                                        <a href="javascript:;" class="link balancelink" data-custid="14244423">
                                            <span class="negative-value">-11,664.31</span>
                                        </a>
                                    </td>
                                    <td class="text-right column-bc">826,715,185</td>
                                    <td class="text-right column-ot">371,662</td>
                                    <td class="text-right column-to">2,468,058</td>
                                    <td class="text-center text-sm column-crd">08/07/2018 04:47:00 AM</td>
                                    <td class="text-center text-sm column-ll">08/07/2018 04:47:00 AM</td>
                                    <td class="text-center text-sm column-li">
                                        <a href="javascript:;" class="link iplink column-loginip">::1</a>
                                    </td>
                                    <td class="column-un">
                                        <a href="?custId=1176083&custLevelId=3" class="link">master1</a>
                                    </td>
                                </tr>
                                <tr class="even-row">
                                    <td class="text-right">2</td>
                                    <td class="text-center column-st">Open</td>
                                    <td>
                                        <a href="?custId=27851214&custLevelId=3" class="link">MasterBL</a>
                                    </td>
                                    <td class="column-fn">---</td>
                                    <td class="column-ln">---</td>
                                    <td class="text-right column-cd">
                                        <a href="javascript:;" class="link creditlink" data-custid="27851214" data-username="MasterBL" data-levelid="3">0</a>
                                    </td>
                                    <td class="text-right column-bl">
                                        <a href="javascript:;" class="link balancelink" data-custid="1584729">
                                            <span class="negative-value">-187.95</span>
                                        </a>
                                    </td>
                                    <td class="text-right column-yb">
                                        <a href="javascript:;" class="link yesterdaybalancelink" data-custid="1584729">
                                            <span class="negative-value">-187.95</span>
                                        </a>
                                    </td>
                                    <td class="text-right column-bc">0</td>
                                    <td class="text-right column-ot">0</td>
                                    <td class="text-right column-to">0</td>
                                    <td class="text-center text-sm column-crd">-</td>
                                    <td class="text-center text-sm column-ll">-</td>
                                    <td class="text-center text-sm column-li">
                                        <a href="javascript:;" class="link iplink column-loginip">192.168.1.1</a>
                                    </td>
                                    <td class="column-un">
                                        <a href="?custId=27851214&custLevelId=3" class="link">MasterBL</a>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <input type="hidden" value="/ex-main/" id="OldMainRootPath" />
                    <input type="hidden" value="138086459" id="CookiePrefix" />
                </body>`
    };

    beforeEach(() => {
        topWindow = {
            popupManager: {
				openByRelativeUrl: jasmine.any(Function),
				openWithHanlder: jasmine.any(Function)
			},
			ageWnd: {
				open: jasmine.any(Function),
				close: jasmine.any(Function),
				createInstance: jasmine.any(Function)
			},
			updateGivenCreditPopup: {
				onClosing: jasmine.any(Function)
			}
        };
        pager = new PagerComponent($);
        columnsControl = new ColumnsControlComponent($, window.document);
        statusComponent = new SingleSelectionDropdownComponent($);
        creditBalance = new CreditBalance(jQueryFake, topWindow, pager, columnsControl, statusComponent);
    });

    describe("_init", () => {
        beforeEach(() => {
            // arrange
            spyOn(creditBalance, "cacheDOMElements");
            spyOn(creditBalance, "registerEvents");
            spyOn(creditBalance, "registerAutoSuggestion");
            spyOn(pager, "render");
            spyOn(columnsControl, "render");
            spyOn(jQueryFake.fn, "stickyTableHeaders");

            setFixtures(template.normal);

            // act
            creditBalance.init();
        });

        it("_Always_Calls cacheDOMElements", () => {
            expect(creditBalance.cacheDOMElements).toHaveBeenCalled();
        });

        it("_Always_Calls registerEvents", () => {
            expect(creditBalance.registerEvents).toHaveBeenCalled();
        });

        it("_Always_Calls registerAutoSuggestion", () => {
            expect(creditBalance.registerAutoSuggestion).toHaveBeenCalled();
        });

        it("_Always_Calls pager.render", () => {
            expect(pager.render).toHaveBeenCalled();
        });

        it("_Always_Calls columnsControl.render", () => {
            expect(columnsControl.render).toHaveBeenCalled();
        });

        it("_Always_Calls stickyTableHeaders", () => {
            expect(jQueryFake.fn.stickyTableHeaders).toHaveBeenCalled();
        });
    });

    describe("_cacheDOMElements", () => {
        it("_Always_All neccessary elements are cached", () => {
            // arrange
            setFixtures(template.normal);

            // act
            creditBalance.cacheDOMElements();

            // assert
            expect(creditBalance.creditBalanceForm).toBeDefined();
        });
    });

    describe("_registerEvents", () => {
        beforeEach(() => {
            // arrange
			setFixtures(template.normal);

			spyOn(topWindow.ageWnd, "createInstance");
			spyOn(topWindow.ageWnd, "open");
			spyOn(topWindow.ageWnd, "close"); 

			spyOn(topWindow.popupManager, "openByRelativeUrl");
			spyOn(topWindow.popupManager, "openWithHanlder");

			spyOn(topWindow.updateGivenCreditPopup, "onClosing");

            creditBalance.cacheDOMElements();

            // act
            creditBalance.registerEvents();
        });

        it("_Always_Bind credit event", () => {
            // act
            creditBalance.$(".creditlink").click();

            // assert
			expect(topWindow.popupManager.openWithHanlder).toHaveBeenCalled();
        });

        it("_Always_Bind balance event", () => {
            // act
            creditBalance.$(".balancelink").click();

            // assert
            expect(topWindow.popupManager.openByRelativeUrl).toHaveBeenCalled();
        });

        it("_Always_Bind yesterday balance event", () => {
            // act
            creditBalance.$(".yesterdaybalancelink").click();

            // assert
            expect(topWindow.popupManager.openByRelativeUrl).toHaveBeenCalled();
        });

        it("_Always_Bind ip event", () => {
            // act
            creditBalance.$(".iplink").click();

            // assert
            expect(topWindow.popupManager.openByRelativeUrl).toHaveBeenCalled();
        });
    })
});
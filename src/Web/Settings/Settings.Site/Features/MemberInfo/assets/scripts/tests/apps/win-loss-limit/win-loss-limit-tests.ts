describe("WinLossLimit", () => {
    const jQueryFake = $;
    let winLossLimit: WinLossLimit;
    const productComponent = new SingleSelectionDropdownComponent(jQueryFake);
    const masterListComponent = new SingleSelectionDropdownComponent(jQueryFake);
    const agentListComponent = new SingleSelectionDropdownComponent(jQueryFake);
    const pagerComponent = new PagerComponent(jQueryFake);

    let template = {
        normal: `<body>
                    <select id="dropdown-product">
                        <option selected data-sport-type="1" data-bet-type="2"></option>
                    </select>
                    <div id="tbl-report"></div>
                    <div id="dropdown-master"></div>
                    <div id="dropdown-agent"></div>
                    <div id="txt-username"></div>
                    <div id="txt-player-id"></div>
                    <div id="SportType"></div>
                    <div id="BetType"></div>
                    <div id="UserSelectedProductId"></div>
                 </body>`,
        superHtml: `<body>
                        <div id="dropdown-product"></div>
                        <div id="tbl-report"></div>
                        <div id="dropdown-master"></div>
                        <div id="dropdown-agent"></div>
                     </body>`,
        masterHtml: `<body>
                        <div id="dropdown-product"></div>
                        <div id="tbl-report"></div>
                            <div id="dropdown-agent"></div>
                     </body>`,
        agentHtml: `<body>
                        <div id="dropdown-product"></div>
                        <div id="tbl-report"></div>
                     </body>`,
    };

    beforeEach(() => {
        winLossLimit = new WinLossLimit(jQueryFake, productComponent, masterListComponent, agentListComponent, pagerComponent);

        spyOn(productComponent, "render");
        spyOn(masterListComponent, "render");
        spyOn(agentListComponent, "render");
        spyOn(pagerComponent, "render");
    });

    describe("_init", () => {
        it("_Always_Calls cacheDOMElements", () => {
            // arrange
            setFixtures(template.normal);
            spyOn(winLossLimit, "cacheDOMElements").and.callThrough();

            // act
            winLossLimit.init();

            // assert
            expect(winLossLimit.cacheDOMElements).toHaveBeenCalled();
        });

        it("_Always_Calls bindEvents", () => {
            // arrange
            setFixtures(template.normal);
            spyOn(winLossLimit, "bindEvents");

            // act
            winLossLimit.init();

            // assert
            expect(winLossLimit.bindEvents).toHaveBeenCalled();
        });

        it("_Always_Render pager", () => {
            // arrange
            setFixtures(template.normal);

            // act
            winLossLimit.init();

            // assert
            expect(pagerComponent.render).toHaveBeenCalled();
        });

        it("_Always_Render sticky header", () => {
            // arrange
            setFixtures(template.normal);
            spyOn($.fn, "stickyTableHeaders");

            // act
            winLossLimit.init();

            // assert
            expect($.fn.stickyTableHeaders).toHaveBeenCalled();
        });

        it("_SuperLevel_Calls render master list", () => {
            // arrange
            setFixtures(template.superHtml);
            spyOn(winLossLimit, "cacheDOMElements").and.callThrough();

            // act
            winLossLimit.init();

            // assert
            expect(masterListComponent.render).toHaveBeenCalled();
            expect(agentListComponent.render).toHaveBeenCalled();
        });

        it("_MasterLevel_Calls render master list", () => {
            // arrange
            setFixtures(template.masterHtml);
            spyOn(winLossLimit, "cacheDOMElements").and.callThrough();

            // act
            winLossLimit.init();

            // assert
            expect(masterListComponent.render).not.toHaveBeenCalled();
            expect(agentListComponent.render).toHaveBeenCalled();
        });

        it("_AgentLevel_Calls render master list", () => {
            // arrange
            setFixtures(template.agentHtml);
            spyOn(winLossLimit, "cacheDOMElements").and.callThrough();

            // act
            winLossLimit.init();

            // assert
            expect(masterListComponent.render).not.toHaveBeenCalled();
            expect(agentListComponent.render).not.toHaveBeenCalled();
        });
    });

    describe("_cacheDOMElements", () => {
        it("_Always_All neccessary elements are cached", () => {
            // arrange
            setFixtures(template.normal);

            // act
            winLossLimit.cacheDOMElements();

            // assert
            expect(winLossLimit.$productList).toBeDefined();
            expect(winLossLimit.$sportType).toBeDefined();
            expect(winLossLimit.$betType).toBeDefined();
            expect(winLossLimit.$productId).toBeDefined();
            expect(winLossLimit.$winLossLimitForm).toBeDefined();
            expect(winLossLimit.$masterList).toBeDefined();
            expect(winLossLimit.$agentList).toBeDefined();
            expect(winLossLimit.$masterId).toBeDefined();
            expect(winLossLimit.$agentId).toBeDefined();
            expect(winLossLimit.$username).toBeDefined();
            expect(winLossLimit.$playerId).toBeDefined();
            expect(winLossLimit.$tableReport).toBeDefined();
        });
    });

    describe("_bindEvents", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.normal);
            winLossLimit.cacheDOMElements();
            spyOn(winLossLimit, "doneInputTextbox");

            // act
            winLossLimit.bindEvents();
        });

        it("_Always_Bind username keyup events", () => {
            // act
            winLossLimit.$username.keyup();

            // assert
            expect(winLossLimit.doneInputTextbox).toHaveBeenCalled();
        });
        it("_Always_Bind playerId keyup events", () => {
            // act
            winLossLimit.$playerId.keyup();

            // assert
            expect(winLossLimit.doneInputTextbox).toHaveBeenCalled();
        });
    });
    describe("_changeProduct", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.normal);
            winLossLimit.cacheDOMElements();
            spyOn(winLossLimit, "submitForm");

            // act
            winLossLimit.changeProduct();
        });

        it("_Always_Submit form", () => {
            // assert
            expect(winLossLimit.submitForm).toHaveBeenCalled();
        });
        it("_Always_Set value before submit form", () => {
            // assert
            expect(winLossLimit.$sportType.val()).toEqual('1');
            expect(winLossLimit.$betType.val()).toEqual('2');
        });
    });
    describe("_changeMaster", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.normal);
            winLossLimit.cacheDOMElements();
            spyOn(winLossLimit, "submitForm");

            // act
            winLossLimit.changeMaster();
        });

        it("_Always_Submit form", () => {
            // assert
            expect(winLossLimit.submitForm).toHaveBeenCalled();
        });
    });
    describe("_changeAgent", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.normal);
            winLossLimit.cacheDOMElements();
            spyOn(winLossLimit, "submitForm");

            // act
            winLossLimit.changeAgent();
        });

        it("_Always_Submit form", () => {
            // assert
            expect(winLossLimit.submitForm).toHaveBeenCalled();
        });
    });
    describe("_submitForm", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.normal);
            winLossLimit.cacheDOMElements();
            spyOn($.fn, "submit");
            spyOn(jQueryFake, "blockUI");

            // act
            winLossLimit.submitForm();
        });

        it("_Always_Submit form", () => {
            // assert
            expect($.fn.submit).toHaveBeenCalled();
        });
        it("_Always_blockUI", () => {
            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
        });
    });
    describe("_doneInputTextbox", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.normal);
            winLossLimit.cacheDOMElements();
            spyOn(winLossLimit, "submitForm");
        });

        it("_Not enter_Not Submit form", () => {
            // act
            winLossLimit.doneInputTextbox({ keyCode: 1, which: false });

            // assert
            expect(winLossLimit.submitForm).not.toHaveBeenCalled();
        });
        it("_Enter_Submit form", () => {
            // act
            winLossLimit.doneInputTextbox({ keyCode: 13, which: false });

            // assert
            expect(winLossLimit.submitForm).toHaveBeenCalled();
        });
    });
});
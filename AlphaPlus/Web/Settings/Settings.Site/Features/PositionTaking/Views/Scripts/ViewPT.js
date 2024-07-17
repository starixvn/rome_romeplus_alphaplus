$(document).ready(function () {
    viewPTManagement.initPage();
});

var ViewPTManagement = function () {
    var self = this;
    self.domElements = {
        boxHeader: null,
        pagingControl: null
    };

    self.cachedData = {
        getPositionTakingInfoCriteria: {
            PageSize: 0,
            PageIndex: 0
        },

        PositionTakingInfo: null,
        lastParam: null,
        ShowPTIcon: false
    };
    self.Enums = {
        RoleId: {
            Member: 1,
            Agent: 2,
            Master: 3,
            Super: 4
        }
    };

    self.PositionTakingInfo = bootstrapData;
    self.PageSizeList = bootstrapData.PageSizeList;
    self.ShowPTIcon = bootstrapData.Product.ShowPTIcon;

    self.initPage = function () {
        $("#arrayCustID").val(self.PositionTakingInfo.CustomerInfo.CustId);

        self.cacheElement();
        self.renderPagingControl();
        self.cachedData.getPositionTakingInfoCriteria = {
            PageSize: self.PositionTakingInfo.PageSize,
            PageIndex: self.PositionTakingInfo.PageIndex
        };

        self.cachedData.PositionTakingInfo = self.PositionTakingInfo;
        self.cachedData.ShowPTIcon = self.ShowPTIcon;

        self.renderPositionTakingInfo(self.PositionTakingInfo);
    };
    self.cacheElement = function () {
        self.domElements.pagingControl = $("#tblHeader .pagingcontrol");
        self.domElements.mainTable = $("#gridContainer");
        self.domElements.viewPageContainer = $(".viewpage-container");
    };
    self.renderPagingControl = function () {
        if (!self.PositionTakingInfo || !self.PositionTakingInfo.Product) {
            self.domElements.pagingControl.empty();
            return;
        }

        var controlHtml = '';
        controlHtml += viewResources.Page_Size;
        controlHtml += '<select id="sel_PagingTop" name="sel_PagingTop">';
        var pageSizeLength = self.PageSizeList.length;
        for (var i = 0; i < pageSizeLength; i++) {
            var pageSize = self.PageSizeList[i];
            var isSelected = self.PositionTakingInfo.PageSize === pageSize ? 'selected="selected" ' : '';
            controlHtml += '<option value="' + pageSize + '"' + isSelected + '>' + pageSize + '</option>';
        }

        controlHtml += '</select>';
        self.domElements.pagingControl.empty().append(controlHtml);

        self.domElements.pagingDropdown = $("#tblHeader .pagingcontrol #sel_PagingTop");
        self.domElements.pagingDropdown.unbind("change");
        self.domElements.pagingDropdown.change(function () {
            self.getPositionTakingInfo(parseInt($(this).val()), 1);
        });
    };
    self.getPositionTakingInfo = function (pageSize, pageIndex) {
        var criteria = {
            ProductId: self.cachedData.PositionTakingInfo.Product.Id,
            MainProductId: self.cachedData.PositionTakingInfo.Product.MainProductId,
            PageSize: pageSize,
            PageIndex: pageIndex,
            CustId: self.cachedData.PositionTakingInfo ?
                self.cachedData.PositionTakingInfo.ViewedCustomerInfo.CustId :
                0,
            RoleId: self.cachedData.PositionTakingInfo ?
                self.cachedData.PositionTakingInfo.ViewedCustomerInfo.RoleId :
                0
        };

        self.cachedData.lastParam = criteria;
        $.ajax({
            type: "POST",
            url: window.rootUrl + "SportsbookPT/GetPositionTaking",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(criteria),
            success: function (rs) {
                self.cachedData.PositionTakingInfo = rs;
                self.renderPositionTakingInfo(rs);
            }
        });
    };
    self.renderPositionTakingInfo = function (positionTakingInfo) {
        if (!self.PositionTakingInfo || !self.PositionTakingInfo.Product) {
            return;
        }

        self.cachedData.getPositionTakingInfoCriteria.PageSize = positionTakingInfo.PageSize;
        self.cachedData.getPositionTakingInfoCriteria.PageIndex = positionTakingInfo.PageIndex;

        var gridOptions = self.getGridOptions(positionTakingInfo);
        self.domElements.mainTable.removeClass("hidden").fanexGrid(gridOptions);
        self.renderPositionTakingInfoFooter(positionTakingInfo);

        $("#box_header, #tblFooter, #tblHeader").width($("#gridContainer").width() + 2);
    };

    self.renderPositionTakingInfoFooter = function (positionTakingInfo) {
        if (positionTakingInfo.TotalPage <= 1) {
            self.domElements.viewPageContainer.addClass("display-none");
            return;
        }

        var pagingControlContent = self.renderFooterPaging(positionTakingInfo.PageIndex, positionTakingInfo.TotalPage);
        self.domElements.viewPageContainer.empty().append(pagingControlContent).removeClass("display-none");
        self.cacheFooterPagingElements();
    };
    self.renderFooterPaging = function (pageIndex, totalPage) {
        var pagingControlContent = '';
        pagingControlContent += '<div class="pagingContainer">';
        pagingControlContent += '<span class="icon pagingFirst" type="button"' +
            (pageIndex === 1 ? ' disabled="disabled"' : ' onclick="viewPTManagement.firstPaging();"') + '></span>';
        pagingControlContent += '<span class="icon pagingPrev " type="button"' +
            (pageIndex === 1 ? ' disabled="disabled"' : ' onclick="viewPTManagement.previousPaging();"') + ' ></span>';

        pagingControlContent += '<span class="pagingSeperator"></span>';
        pagingControlContent += viewResources.Page;
        pagingControlContent += '<input class="pagingCurrent" type="text"' +
            ' onkeyup="viewPTManagement.doEnterPaging(this, event);"' +
            ' value="' + pageIndex + '"' +
            ' maxlength="' + totalPage.toString().length + '"/>';
        pagingControlContent += viewResources.of + ' ' + totalPage;
        pagingControlContent += '<span class="pagingSeperator"></span>';

        pagingControlContent += '<span class="icon pagingNext " type="button"' +
            (pageIndex === totalPage ? ' disabled="disabled"' : ' onclick="viewPTManagement.nextPaging()"') + '></span>';
        pagingControlContent += '<span class="icon pagingLast " type="button"' +
            (pageIndex === totalPage ? ' disabled="disabled"' : ' onclick="viewPTManagement.lastPaging()"') + '> </span>';

        pagingControlContent += '</div>';
        return pagingControlContent;
    };
    self.cacheFooterPagingElements = function () {
        self.domElements.pagingCurrent = self.domElements.viewPageContainer.find(".pagingCurrent");
        self.domElements.pagingFirst = self.domElements.viewPageContainer.find(".pagingFirst");
        self.domElements.pagingPrev = self.domElements.viewPageContainer.find(".pagingPrev");
        self.domElements.pagingNext = self.domElements.viewPageContainer.find(".pagingNext");
        self.domElements.pagingLast = self.domElements.viewPageContainer.find(".pagingLast");
    };
    self.firstPaging = function () {
        var selectedPageIndex = 1;
        self.domElements.pagingCurrent.val(selectedPageIndex);
        self.getPositionTakingInfo(self.cachedData.getPositionTakingInfoCriteria.PageSize, selectedPageIndex);
    };
    self.nextPaging = function () {
        var selectedPageIndex = parseInt(self.cachedData.PositionTakingInfo.PageIndex) + 1;
        self.domElements.pagingCurrent.val(selectedPageIndex);
        self.getPositionTakingInfo(self.cachedData.getPositionTakingInfoCriteria.PageSize, selectedPageIndex);
    };
    self.doEnterPaging = function (element, event) {
        var keyCode = event.which;
        if (keyCode !== 13) {
            return;
        }

        var selectedPageIndex = parseInt(element.value);
        if (selectedPageIndex < 1) {
            selectedPageIndex = 1;
        }
        else if (self.cachedData.PositionTakingInfo &&
            self.cachedData.PositionTakingInfo.TotalPage < selectedPageIndex) {
            selectedPageIndex = self.cachedData.PositionTakingInfo.TotalPage;
        }

        self.domElements.pagingCurrent.val(selectedPageIndex);
        self.getPositionTakingInfo(self.cachedData.getPositionTakingInfoCriteria.PageSize, selectedPageIndex);
    };
    self.previousPaging = function () {
        var selectedPageIndex = parseInt(self.cachedData.PositionTakingInfo.PageIndex) - 1;
        self.domElements.pagingCurrent.val(selectedPageIndex);
        self.getPositionTakingInfo(self.cachedData.getPositionTakingInfoCriteria.PageSize, selectedPageIndex);
    };
    self.lastPaging = function () {
        var selectedPageIndex = self.cachedData.PositionTakingInfo.TotalPage;
        self.domElements.pagingCurrent.val(selectedPageIndex);
        self.getPositionTakingInfo(self.cachedData.getPositionTakingInfoCriteria.PageSize, selectedPageIndex);
    };

    self.getGridOptions = function (positionTakingInfo) {
        var headerInfo = self.renderPositionTakingInfoHeader(positionTakingInfo);
        var headerColumns = headerInfo.headerColumns;
        var bodyData = self.renderPositionTakingInfoBody(positionTakingInfo);
        var gridOptions = {
            customizeHeader: true,
            groupHeaderRows: headerInfo.groupHeaderRows,
            columns: headerColumns,
            bodyRows: bodyData,
            headerColumnHeight: 24,
            bodyRowHeight: 24,
            noDataText: viewResources.noInfo,
            numberOfFrozenColumn: 0,
            gridExpandHeight: positionTakingInfo.TotalPage <= 1 ? 140 : 180,
            showHighlightRow: true,
            showPager: false,
            showDistinctRow: false,
            minWidth: 600
        };

        return gridOptions;
    };

    self.renderPositionTakingInfoHeader = function (positionTakingInfo) {
        var headerColumns = [];
        var groupHeaderColumns = [];

        headerColumns.push({
            HeaderText: viewResources.Symbol_No,
            Width: 40,
            Name: "ColNo",
            HeaderAlign: "center",
            CellAlign: "center"
        });

        groupHeaderColumns.push({
            Width: 40,
            Name: "ColNo",
            HeaderAlign: "center",
            CellAlign: "center",
            MergeColNumber: positionTakingInfo.IsShowUplinePT ? 3 : 2
        });

        headerColumns.push({
            HeaderText: viewResources.Username,
            Width: positionTakingInfo.CustomerList.length ? 180 : 447,
            Name: "ColUsername",
            HeaderAlign: "center",
            CellAlign: "left"
        });

        if (positionTakingInfo.IsShowUplinePT && positionTakingInfo.CustomerList.length) {
            headerColumns.push({
                HeaderText: '',
                Width: 20,
                Name: "ColUserLevelName",
                HeaderAlign: "center",
                CellAlign: "center",
                ThClass: 'column-level-name'
            });
        }

        var sportLength = positionTakingInfo.Product.ActivationGroups.length;
        for (var i = 0; i < sportLength; i++) {
            var sport = positionTakingInfo.Product.ActivationGroups[i];
            if (!sport.Active) {
                continue;
            }

            var liveTypeLength = sport.ActivationGroups.length;
            for (var j = 0; j < liveTypeLength; j++) {
                var liveType = sport.ActivationGroups[j];
                if (!liveType.Active) {
                    continue;
                }

                var betTypeLength = liveType.ActivationGroups.length;
                var activeBetTypeLength = 0;
                for (var k = 0; k < betTypeLength; k++) {
                    var betType = liveType.ActivationGroups[k];
                    if (!betType.Active) {
                        continue;
                    }
                    var width = betTypeLength < 3 ? 300 : 150;
                    if (betType.Name.length > 35) {
                        width = 500;
                    }
                    headerColumns.push({
                        HeaderText: betType.Name,
                        Width: width,
                        Name: "ColSportType" + sport.Id + "-LiveType" + liveType.Id + "-BetType" + betType.Id,
                        HeaderAlign: "center",
                        CellAlign: "center",
                        ThClass: 'column-bettype-name'
                    });
                    activeBetTypeLength++;
                }

                if (activeBetTypeLength) {
                    groupHeaderColumns.push({
                        HeaderText: sport.Name + " " + liveType.Name,
                        Name: "ColSportType" + sport.Id + "-LiveType" + liveType.Id + "-BetType" + liveType.ActivationGroups[0].Id,
                        HeaderAlign: "center",
                        CellAlign: "center",
                        MergeColNumber: activeBetTypeLength
                    });
                }
            }
        }

        return {
            groupHeaderRows: [{
                Columns: groupHeaderColumns
            }],
            headerColumns: headerColumns
        };
    };
    self.renderPositionTakingInfoBody = function (positionTakingInfo) {
        var bodyData = [];
        var custLength = positionTakingInfo.CustomerList.length;
        if (positionTakingInfo.IsShowUplinePT) {
            bodyData = self.renderPositionTakingInfoBody_WithUplineInfo(positionTakingInfo);
        } else {
            bodyData = self.renderPositionTakingInfoBody_WithoutUplineInfo(positionTakingInfo);
        }

        return bodyData;
    };

    self.renderPositionTakingInfoBody_WithUplineInfo = function (positionTakingInfo) {
        var bodyData = [];
        var custLength = positionTakingInfo.CustomerList.length;
        var uplineLength = positionTakingInfo.ViewedLevels.length - 1;
        for (var i = 0; i < custLength; i++) {
            var custPt = positionTakingInfo.CustomerList[i];
            var columns = [];
            columns.push({
                Name: "ColNo",
                Value: custPt.GroupIndex,
                MergeRowNumber: uplineLength + 1
            });

            columns.push({
                Name: "ColUsername",
                Value: self.drilldownLink(positionTakingInfo, custPt.CustomerInfo),
                MergeRowNumber: uplineLength + 1
            });

            for (var j = 0; j < uplineLength; j++) {
                var uplinePT = custPt.UplinePT[j];
                columns.push({
                    Name: "ColUserLevelName",
                    Value: self.getLevelName(positionTakingInfo.ViewedLevels[j]),
                    TdClass: 'column-levelname'
                });

                columns = columns.concat(self.renderPTBySport(uplinePT));
                bodyData.push({
                    Columns: columns,
                    CssClass: self.customerStatusRowCss(uplinePT.CustomerInfo)
                });

                columns = [];
            }

            columns.push({
                Name: "ColUserLevelName",
                Value: self.getLevelName(positionTakingInfo.ViewedLevels[uplineLength]),
                TdClass: 'column-levelname'
            });

            columns = columns.concat(self.renderPTBySport(custPt));
            bodyData.push({
                Columns: columns,
                CssClass: self.customerStatusRowCss(custPt.CustomerInfo)
            });
        }

        return bodyData;
    };
    self.renderPositionTakingInfoBody_WithoutUplineInfo = function (positionTakingInfo) {
        var bodyData = [];
        var custLength = positionTakingInfo.CustomerList.length;
        for (var i = 0; i < custLength; i++) {
            var custPt = positionTakingInfo.CustomerList[i];
            var columns = [];
            columns.push({
                Name: "ColNo",
                Value: custPt.GroupIndex
            });

            columns.push({
                Name: "ColUsername",
                Value: self.drilldownLink(positionTakingInfo, custPt.CustomerInfo)
            });

            columns = columns.concat(self.renderPTBySport(custPt));
            bodyData.push({
                Columns: columns,
                CssClass: self.customerStatusRowCss(custPt.CustomerInfo)
            });
        }

        return bodyData;
    };

    self.drilldownLink = function (positionTakingInfo, customerInfo) {
        var link = '';
        switch (positionTakingInfo.ViewedCustomerInfo.RoleId) {
            case self.Enums.RoleId.Super:
            case self.Enums.RoleId.Master:
                link = '<a onclick="viewPTManagement.viewDownlinePTList(\'' + customerInfo.CustId + '\');" href="#">' +
                    customerInfo.CustName +
                    '</a>';
                break;
            case self.Enums.RoleId.Agent:
                link = '<a  onclick="viewPTManagement.editMemberPTList(' +
                    '\'' + customerInfo.CustId + '\',' +
                    '\'' + customerInfo.CustName + '\'' + ');" href="javascript:;">' +
                    customerInfo.CustName +
                    '</a>';
                break;
            default:
                break;
        }

        return link;
    };

    self.renderPTBySport = function (custPt) {
        var sportLength = custPt.PTByGroups.length;
        var columns = [];
        for (var i = 0; i < sportLength; i++) {
            var sport = custPt.PTByGroups[i];
            columns = columns.concat(self.renderPTByLiveType(custPt, sport));
        }

        return columns;
    };
    self.renderPTByLiveType = function (custPt, sport) {
        var liveTypeLength = sport.PTByGroups.length;
        var columns = [];
        for (var i = 0; i < liveTypeLength; i++) {
            var liveType = sport.PTByGroups[i];
            columns = columns.concat(self.renderPTByBetType(custPt, sport, liveType));
        }

        return columns;
    };
    self.renderPTByBetType = function (custPt, sport, liveType) {
        var betTypeLength = liveType.PTByGroups.length;
        var columns = [];
        for (var i = 0; i < betTypeLength; i++) {
            var betType = liveType.PTByGroups[i];
            var ptValue = numeral(betType.PTValue).value();
            columns.push({
                Name: "ColSportType" + sport.Id + "-LiveType" + liveType.Id + "-BetType" + betType.Id,
                Value: ptValue
            });
        }

        return columns;
    };

    self.viewDownlinePTList = function (custId) {
        var downlineId = self.getDirectDownlineId(self.cachedData.PositionTakingInfo.ViewedCustomerInfo.RoleId);
        var criteria = {
            ProductId: self.cachedData.PositionTakingInfo.Product.Id,
            PageSize: self.cachedData.PositionTakingInfo ?
                self.cachedData.PositionTakingInfo.PageSize :
                0,
            PageIndex: 1,
            CustId: custId,
            RoleId: downlineId,
            ShowPTIcon: self.cachedData.ShowPTIcon,
            MainProductId: self.cachedData.PositionTakingInfo.Product.MainProductId,
            ProductGroupId: self.cachedData.PositionTakingInfo.Product.GroupId
        };
        self.cachedData.lastParam = criteria;
        window.location.href = window.rootUrl + "SportsbookPT/DownlinePT?" + $.param(criteria);
    };
    self.getDirectDownlineId = function (roleId) {
        switch (roleId) {
            case self.Enums.RoleId.Super:
                return self.Enums.RoleId.Master;
            case self.Enums.RoleId.Master:
                return self.Enums.RoleId.Agent;
            case self.Enums.RoleId.Agent:
                return self.Enums.RoleId.Member;
            default:
                return self.Enums.RoleId.Member;
        }
    };
    self.customerStatusRowCss = function (customerInfo) {
        if (!customerInfo.Enabled) {
            return 'disabled-row'
        }

        if (customerInfo.Closed) {
            return customerInfo.ClosedReason.ClassName;
        }

        if (customerInfo.Suspended) {
            return 'suspended-row';
        }

        if (customerInfo.LockedAccess.IsLocked) {
            return customerInfo.LockedAccess.ClassName
        }

        return '';
    };

    self.editMemberPTList = function (custId, custName) {
        var params = $.param({
            productId: self.PositionTakingInfo.Product.Id,
            customerLevel: 1,
            uplineLevel: self.PositionTakingInfo.CustomerInfo.RoleId,
            customerId: custId,

            isMultiple: false,
            username: custName,
            userId: self.PositionTakingInfo.CustomerInfo.CustId,
            subAccid: 0,
            isViewPt: true,
            isDarkMode: window.isDarkMode
        });

        var title = viewResources.editPTTitle + " - " + custName;
        var popupWidth = 1250;

        if (self.PositionTakingInfo.Product.IsNewSetting == true) {
            var payload = $.param({
                productGroupId: self.PositionTakingInfo.Product.GroupId,
                custId: custId,
                roleId: 1,
                uplineRoleId: self.PositionTakingInfo.CustomerInfo.RoleId,
                lng: self.PositionTakingInfo.Language,
                isDarkMode: window.isDarkMode
            });
            top.popupManager.open(top.alpha.urls.memberInfoRootPath + "Customer/EditCustomer?" + payload, title, popupWidth);
        } else {
            var url = self.PositionTakingInfo.Product.CustomerSettingLink;

            if (url) {
                url += "?" + params;
                top.popupManager.open(url, title, popupWidth);
            }
            else {
                url = "DCS/ViewCustomerSetting?" + params;
                top.popupManager.openByRelativeUrl(url, title, popupWidth);
            }
        }
    };
    self.getRoleName = function (roleId) {
        switch (roleId) {
            case self.Enums.RoleId.Super:
                return "Super";
            case self.Enums.RoleId.Master:
                return "Master";
            case self.Enums.RoleId.Agent:
                return "Agent";
            default:
                return "";
        }
    };
    self.getLevelName = function (levelId) {
        switch (levelId) {
            case self.Enums.RoleId.Super:
                return "S";
            case self.Enums.RoleId.Master:
                return "M";
            case self.Enums.RoleId.Agent:
                return "A";
            default:
                return "";
        }
    };
};

var viewPTManagement = new ViewPTManagement();

function getEditSettingPopUpLeft() {
    var accountInfoWidth = top.isNewSite ? top.$('.sidebar').width() : top.menu.innerWidth;
    return accountInfoWidth + 8;
}

window.isProductOptionClicked = false;
function ptView(url) {
    if (window.isProductOptionClicked) {
        window.location.href = window.rootUrl + url;
    }

    window.isProductOptionClicked = !window.isProductOptionClicked;
}

function ptProductDropdownBlur() {
    window.isProductOptionClicked = false;
}

function ptProductChanged(element) {
    var isRedirect = navigator.userAgent.match(/iPad/i) != null || navigator.userAgent.indexOf("Safari") > -1;
    if (isRedirect) {
        window.isProductOptionClicked = false;
        var url = $(element).val();
        location.href = window.rootUrl + url;
    }
}

function correctCheckValueOnDropdown(dropdownId) {
    var dropdown = $("#" + dropdownId)[0];
    if (!dropdown) {
        return;
    }

    var findSelected = false;
    for (var i = 0; i < dropdown.length; i++) {
        var option = dropdown[i];
        var attributeLength = option.attributes.length;
        for (var j = 0; j < attributeLength; j++) {
            var attr = option.attributes[j];
            if (attr.name == "selected") {
                if (attr.value == "selected") {
                    $("#" + dropdownId).val(option.value);
                    findSelected = true;
                }

                break;
            }
        }

        if (findSelected) {
            break;
        }
    }
}

window.top.successfulUpdatingCallback = function (isPartialUpdate, isMultipleUpdate) {
    window.top.reloadPopUpSetting();
};

$(document).ready(function () {
    correctCheckValueOnDropdown("dropdown-product");
});

var ViewPTManagement = function () {
    var self = this;
    self.domElements = {
        boxHeader: null,
        pagingControl: null
    };

    self.cachedData = {
        getPositionTakingInfoCriteria: {
            pageSize: 0,
            pageIndex: 0
        },

        positionTakingInfo: null,
        lastParam: null
    };
    self.Enums = {
        roleId: {
            Member: 1,
            Agent: 2,
            Master: 3,
            Super: 4
        }
    };

    self.positionTakingInfo = bootstrapData;
    self.pageSizeList = bootstrapData.pageSizeList;

    self.initPage = function () {
        $('#arrayCustID').val(self.positionTakingInfo.customerInfo.custId);

        self.cacheElement();
        self.renderPagingControl();
        self.cachedData.getPositionTakingInfoCriteria = {
            pageSize: self.positionTakingInfo.pageSize,
            pageIndex: self.positionTakingInfo.pageIndex
        };

        self.cachedData.positionTakingInfo = self.positionTakingInfo;

        self.renderPositionTakingInfo(self.positionTakingInfo);

        $('.view-downline')
            .unbind()
            .bind('click', function (event) {
                self.viewDownlinePTList(event);
            });
    };
    self.cacheElement = function () {
        self.domElements.pagingControl = $('#tblHeader .pagingcontrol');
        self.domElements.mainTable = $('#gridContainer');
        self.domElements.viewPageContainer = $('.viewpage-container');
    };
    self.renderPagingControl = function () {
        if (!self.positionTakingInfo || !self.positionTakingInfo.product) {
            self.domElements.pagingControl.empty();
            return;
        }

        var controlHtml = '';
        controlHtml += viewResources.Page_Size;
        controlHtml += '<select id="sel_PagingTop" name="sel_PagingTop">';
        var pageSizeLength = self.pageSizeList.length;
        for (var i = 0; i < pageSizeLength; i++) {
            var pageSize = self.pageSizeList[i];
            var isSelected = self.positionTakingInfo.pageSize === pageSize ? 'selected="selected" ' : '';
            controlHtml += '<option value="' + pageSize + '"' + isSelected + '>' + pageSize + '</option>';
        }

        controlHtml += '</select>';
        self.domElements.pagingControl.empty().append(controlHtml);

        self.domElements.pagingDropdown = $('#tblHeader .pagingcontrol #sel_PagingTop');
        self.domElements.pagingDropdown.unbind('change');
        self.domElements.pagingDropdown.change(function () {
            self.getPositionTakingInfo(parseInt($(this).val()), 1);
        });
    };
    self.getPositionTakingInfo = function (pageSize, pageIndex) {
        var criteria = {
            ProductId: self.cachedData.positionTakingInfo.product.id,
            PageSize: pageSize,
            PageIndex: pageIndex,
            CustId: self.cachedData.positionTakingInfo
                ? self.cachedData.positionTakingInfo.viewedCustomerInfo.custId
                : 0,
            RoleId: self.cachedData.positionTakingInfo
                ? self.cachedData.positionTakingInfo.viewedCustomerInfo.roleId
                : 0
        };

        self.cachedData.lastParam = criteria;
        $.ajax({
            type: 'POST',
            url: window.rootUrl + 'PositionTaking',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(criteria),
            success: function (rs) {
                self.cachedData.positionTakingInfo = rs;
                self.renderPositionTakingInfo(rs);
                $(".view-downline").unbind().bind('click', function (event) { self.viewDownlinePTList(event); });
            }
        });
    };
    self.renderPositionTakingInfo = function (positionTakingInfo) {
        if (!self.positionTakingInfo || !self.positionTakingInfo.product) {
            return;
        }

        self.cachedData.getPositionTakingInfoCriteria.pageSize = positionTakingInfo.pageSize;
        self.cachedData.getPositionTakingInfoCriteria.pageIndex = positionTakingInfo.pageIndex;

        var gridOptions = self.getGridOptions(positionTakingInfo);
        self.domElements.mainTable.removeClass('hidden').fanexGrid(gridOptions);
        self.renderPositionTakingInfoFooter(positionTakingInfo);

        $('#box_header, #tblFooter, #tblHeader').width($('#gridContainer').width() + 2);
    };

    self.renderPositionTakingInfoFooter = function (positionTakingInfo) {
        if (positionTakingInfo.totalPage <= 1) {
            self.domElements.viewPageContainer.addClass('display-none');
            return;
        }

        var pagingControlContent = self.renderFooterPaging(positionTakingInfo.pageIndex, positionTakingInfo.totalPage);
        self.domElements.viewPageContainer.empty().append(pagingControlContent).removeClass('display-none');
        self.cacheFooterPagingElements();
    };
    self.renderFooterPaging = function (pageIndex, totalPage) {
        var pagingControlContent = '';
        pagingControlContent += '<div class="pagingContainer">';
        pagingControlContent +=
            '<span class="icon pagingFirst" type="button"' +
            (pageIndex === 1 ? ' disabled="disabled"' : ' onclick="viewPTManagement.firstPaging();"') +
            '></span>';
        pagingControlContent +=
            '<span class="icon pagingPrev " type="button"' +
            (pageIndex === 1 ? ' disabled="disabled"' : ' onclick="viewPTManagement.previousPaging();"') +
            ' ></span>';

        pagingControlContent += '<span class="pagingSeperator"></span>';
        pagingControlContent += viewResources.Page;
        pagingControlContent +=
            '<input class="pagingCurrent" type="text"' +
            ' onkeyup="viewPTManagement.doEnterPaging(this, event);"' +
            ' value="' +
            pageIndex +
            '"' +
            ' maxlength="' +
            totalPage.toString().length +
            '"/>';
        pagingControlContent += viewResources.of + ' ' + totalPage;
        pagingControlContent += '<span class="pagingSeperator"></span>';

        pagingControlContent +=
            '<span class="icon pagingNext " type="button"' +
            (pageIndex === totalPage ? ' disabled="disabled"' : ' onclick="viewPTManagement.nextPaging()"') +
            '></span>';
        pagingControlContent +=
            '<span class="icon pagingLast " type="button"' +
            (pageIndex === totalPage ? ' disabled="disabled"' : ' onclick="viewPTManagement.lastPaging()"') +
            '> </span>';

        pagingControlContent += '</div>';
        return pagingControlContent;
    };
    self.cacheFooterPagingElements = function () {
        self.domElements.pagingCurrent = self.domElements.viewPageContainer.find('.pagingCurrent');
        self.domElements.pagingFirst = self.domElements.viewPageContainer.find('.pagingFirst');
        self.domElements.pagingPrev = self.domElements.viewPageContainer.find('.pagingPrev');
        self.domElements.pagingNext = self.domElements.viewPageContainer.find('.pagingNext');
        self.domElements.pagingLast = self.domElements.viewPageContainer.find('.pagingLast');
    };
    self.firstPaging = function () {
        var selectedPageIndex = 1;
        self.domElements.pagingCurrent.val(selectedPageIndex);
        self.getPositionTakingInfo(self.cachedData.getPositionTakingInfoCriteria.pageSize, selectedPageIndex);
    };
    self.nextPaging = function () {
        var selectedPageIndex = parseInt(self.cachedData.positionTakingInfo.pageIndex) + 1;
        self.domElements.pagingCurrent.val(selectedPageIndex);
        self.getPositionTakingInfo(self.cachedData.getPositionTakingInfoCriteria.pageSize, selectedPageIndex);
    };
    self.doEnterPaging = function (element, event) {
        var keyCode = event.which;
        if (keyCode !== 13) {
            return;
        }

        var selectedPageIndex = parseInt(element.value);
        if (selectedPageIndex < 1) {
            selectedPageIndex = 1;
        } else if (
            self.cachedData.positionTakingInfo &&
            self.cachedData.positionTakingInfo.totalPage < selectedPageIndex
        ) {
            selectedPageIndex = self.cachedData.positionTakingInfo.totalPage;
        }

        self.domElements.pagingCurrent.val(selectedPageIndex);
        self.getPositionTakingInfo(self.cachedData.getPositionTakingInfoCriteria.pageSize, selectedPageIndex);
    };
    self.previousPaging = function () {
        var selectedPageIndex = parseInt(self.cachedData.positionTakingInfo.pageIndex) - 1;
        self.domElements.pagingCurrent.val(selectedPageIndex);
        self.getPositionTakingInfo(self.cachedData.getPositionTakingInfoCriteria.pageSize, selectedPageIndex);
    };
    self.lastPaging = function () {
        var selectedPageIndex = self.cachedData.positionTakingInfo.totalPage;
        self.domElements.pagingCurrent.val(selectedPageIndex);
        self.getPositionTakingInfo(self.cachedData.getPositionTakingInfoCriteria.pageSize, selectedPageIndex);
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
            gridExpandHeight: positionTakingInfo.totalPage <= 1 ? 140 : 180,
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
            Name: 'ColNo',
            HeaderAlign: 'center',
            CellAlign: 'center'
        });

        groupHeaderColumns.push({
            Width: 40,
            Name: 'ColNo',
            HeaderAlign: 'center',
            CellAlign: 'center',
            MergeColNumber: positionTakingInfo.isShowUplinePT ? 3 : 2
        });

        headerColumns.push({
            HeaderText: viewResources.Username,
            Width: positionTakingInfo.customerList.length ? 180 : 447,
            Name: 'ColUsername',
            HeaderAlign: 'center',
            CellAlign: 'left'
        });

        if (positionTakingInfo.isShowUplinePT && positionTakingInfo.customerList.length) {
            headerColumns.push({
                HeaderText: '',
                Width: 20,
                Name: 'ColUserLevelName',
                HeaderAlign: 'center',
                CellAlign: 'center',
                ThClass: 'column-level-name'
            });
        }

        var sportLength = positionTakingInfo.product.activationGroups.length;
        for (var i = 0; i < sportLength; i++) {
            var sport = positionTakingInfo.product.activationGroups[i];
            if (!sport.active) {
                continue;
            }

            var liveTypeLength = sport.activationGroups.length;
            for (var j = 0; j < liveTypeLength; j++) {
                var liveType = sport.activationGroups[j];
                if (!liveType.active) {
                    continue;
                }

                var betTypeLength = liveType.activationGroups.length;
                var activeBetTypeLength = 0;
                for (var k = 0; k < betTypeLength; k++) {
                    var betType = liveType.activationGroups[k];
                    if (!betType.active) {
                        continue;
                    }
                    var width = betTypeLength < 3 ? 300 : 150;
                    if (betType.name.length > 35) {
                        width = 500;
                    }
                    headerColumns.push({
                        HeaderText: betType.name,
                        Width: width,
                        Name: 'ColSportType' + sport.id + '-LiveType' + liveType.id + '-BetType' + betType.id,
                        HeaderAlign: 'center',
                        CellAlign: 'center',
                        ThClass: 'column-bettype-name'
                    });
                    activeBetTypeLength++;
                }

                if (activeBetTypeLength) {
                    groupHeaderColumns.push({
                        HeaderText: sport.name + ' ' + liveType.name,
                        Name:
                            'ColSportType' +
                            sport.id +
                            '-LiveType' +
                            liveType.id +
                            '-BetType' +
                            liveType.activationGroups[0].id,
                        HeaderAlign: 'center',
                        CellAlign: 'center',
                        MergeColNumber: activeBetTypeLength
                    });
                }
            }
        }

        return {
            groupHeaderRows: [
                {
                    Columns: groupHeaderColumns
                }
            ],
            headerColumns: headerColumns
        };
    };
    self.renderPositionTakingInfoBody = function (positionTakingInfo) {
        var bodyData = [];
        var custLength = positionTakingInfo.customerList.length;
        if (positionTakingInfo.isShowUplinePT) {
            bodyData = self.renderPositionTakingInfoBody_WithUplineInfo(positionTakingInfo);
        } else {
            bodyData = self.renderPositionTakingInfoBody_WithoutUplineInfo(positionTakingInfo);
        }

        return bodyData;
    };

    self.renderPositionTakingInfoBody_WithUplineInfo = function (positionTakingInfo) {
        var bodyData = [];
        var custLength = positionTakingInfo.customerList.length;
        var uplineLength = positionTakingInfo.viewedLevels.length - 1;
        for (var i = 0; i < custLength; i++) {
            var custPt = positionTakingInfo.customerList[i];
            var columns = [];
            columns.push({
                Name: 'ColNo',
                Value: custPt.groupIndex,
                MergeRowNumber: uplineLength + 1
            });

            columns.push({
                Name: 'ColUsername',
                Value: self.drilldownLink(positionTakingInfo, custPt.customerInfo),
                MergeRowNumber: uplineLength + 1
            });

            for (var j = 0; j < uplineLength; j++) {
                var uplinePT = custPt.uplinePT[j];
                columns.push({
                    Name: 'ColUserLevelName',
                    Value: self.getLevelName(positionTakingInfo.viewedLevels[j]),
                    TdClass: 'column-levelname'
                });

                columns = columns.concat(self.renderPTBySport(uplinePT));
                bodyData.push({
                    Columns: columns,
                    CssClass: self.customerStatusRowCss(uplinePT.customerInfo)
                });

                columns = [];
            }

            columns.push({
                Name: 'ColUserLevelName',
                Value: self.getLevelName(positionTakingInfo.viewedLevels[uplineLength]),
                TdClass: 'column-levelname'
            });

            columns = columns.concat(self.renderPTBySport(custPt));
            bodyData.push({
                Columns: columns,
                CssClass: self.customerStatusRowCss(custPt.customerInfo)
            });
        }

        return bodyData;
    };
    self.renderPositionTakingInfoBody_WithoutUplineInfo = function (positionTakingInfo) {
        var bodyData = [];
        var custLength = positionTakingInfo.customerList.length;
        for (var i = 0; i < custLength; i++) {
            var custPt = positionTakingInfo.customerList[i];
            var columns = [];
            columns.push({
                Name: 'ColNo',
                Value: custPt.groupIndex
            });

            columns.push({
                Name: 'ColUsername',
                Value: self.drilldownLink(positionTakingInfo, custPt.customerInfo)
            });

            columns = columns.concat(self.renderPTBySport(custPt));
            bodyData.push({
                Columns: columns,
                CssClass: self.customerStatusRowCss(custPt.customerInfo)
            });
        }

        return bodyData;
    };

    self.drilldownLink = function (positionTakingInfo, customerInfo) {
        if (positionTakingInfo.levelType == '2') {
            var agentId = 0;
            switch (positionTakingInfo.viewedCustomerInfo.roleId) {
                case self.Enums.RoleId.Super:
                    agentId = -4;
                    break;
                case self.Enums.RoleId.Master:
                    agentId = -3;
                    break;
                default:
                    agentId = 0;
                    break;
            }
            return (
                '<a  onclick="viewPTManagement.editMemberPTList(' +
                "'" +
                customerInfo.CustId +
                "'," +
                "'" +
                customerInfo.CustName +
                "'," +
                "'" +
                agentId +
                "'" +
                ');" href="javascript:;">' +
                customerInfo.CustName +
                '</a>'
            );
        }

        var link = '';
        switch (positionTakingInfo.viewedCustomerInfo.roleId) {
            case self.Enums.roleId.Super:
            case self.Enums.roleId.Master:
                link =
                    '<a class="view-downline" data-custid="' +
                    customerInfo.custId +
                    '" href="#">' +
                    customerInfo.custName +
                    '</a>';
                break;
            case self.Enums.roleId.Agent:
                link =
                    '<a  onclick="viewPTManagement.editMemberPTList(' +
                    "'" +
                    customerInfo.custId +
                    "'," +
                    "'" +
                    "'" +
                    customerInfo.CustName +
                    "'," +
                    "'" +
                    0 +
                    "'" +
                    ');" href="javascript:;">' +
                    customerInfo.custName +
                    '</a>';
                break;
            default:
                break;
        }

        return link;
    };

    self.renderPTBySport = function (custPt) {
        var sportLength = custPt.positionTakingByGroups.length;
        var columns = [];
        for (var i = 0; i < sportLength; i++) {
            var sport = custPt.positionTakingByGroups[i];
            columns = columns.concat(self.renderPTByLiveType(custPt, sport));
        }

        return columns;
    };
    self.renderPTByLiveType = function (custPt, sport) {
        var liveTypeLength = sport.positionTakingByGroups.length;
        var columns = [];
        for (var i = 0; i < liveTypeLength; i++) {
            var liveType = sport.positionTakingByGroups[i];
            columns = columns.concat(self.renderPTByBetType(custPt, sport, liveType));
        }

        return columns;
    };
    self.renderPTByBetType = function (custPt, sport, liveType) {
        var betTypeLength = liveType.positionTakingByGroups.length;
        var columns = [];
        for (var i = 0; i < betTypeLength; i++) {
            var betType = liveType.positionTakingByGroups[i];
            var ptValue = numeral(betType.value).value();
            columns.push({
                Name: 'ColSportType' + sport.id + '-LiveType' + liveType.id + '-BetType' + betType.id,
                Value: ptValue
            });
        }

        return columns;
    };

    self.viewDownlinePTList = function (event) {
        var custId = $(event.currentTarget).data('custid');
        var downlineId = self.getDirectDownlineId(self.cachedData.positionTakingInfo.viewedCustomerInfo.roleId);
        var criteria = {
            ProductId: self.cachedData.positionTakingInfo.product.id,
            PageSize: self.cachedData.positionTakingInfo ? self.cachedData.positionTakingInfo.pageSize : 0,
            PageIndex: 1,
            CustId: custId,
            RoleId: downlineId,
            ProductGroupId: self.cachedData.positionTakingInfo.product.groupId
        };
        self.cachedData.lastParam = criteria;
        self.viewDataURL(event, '/settings/PositionTaking?' + $.param(criteria));
    };
    self.viewDataURL = function (event, url) {
        event.preventDefault();

        const mainFrame = top.$('#main');
        if (!top.alpha.core.isIe()) {
            mainFrame.contents().find('head').append(top.$('#AssetCommonCssLink').val());
            mainFrame.contents().find('body').empty().block();
        }

        mainFrame.attr('src', url);
    };
    self.getDirectDownlineId = function (roleId) {
        switch (roleId) {
            case self.Enums.roleId.Super:
                return self.Enums.roleId.Master;
            case self.Enums.roleId.Master:
                return self.Enums.roleId.Agent;
            case self.Enums.roleId.Agent:
                return self.Enums.roleId.Member;
            default:
                return self.Enums.roleId.Member;
        }
    };
    self.customerStatusRowCss = function (customerInfo) {
        if (!customerInfo.enabled) {
            return 'disabled-row';
        }

        if (customerInfo.closed) {
            return customerInfo.closedReason.className;
        }

        if (customerInfo.suspended) {
            return 'suspended-row';
        }

        if (customerInfo.lockedAccess.IsLocked) {
            return customerInfo.lockedAccess.className;
        }

        return '';
    };

    self.editMemberPTList = function (custId, custName, agentId) {
        var title = viewResources.editPTTitle + ' - ' + custName;
        var popupWidth = 1250;

        var payload = $.param({
            productGroupId: self.PositionTakingInfo.Product.GroupId,
            custId: custId,
            roleId: 1,
            agentId: agentId,
            uplineRoleId: self.PositionTakingInfo.CustomerInfo.RoleId,
            lng: self.PositionTakingInfo.Language,
            isDarkMode: window.isDarkMode
        });
        top.popupManager.open(
            top.alpha.urls.memberInfoRootPath + 'Customer/EditCustomer?' + payload,
            title,
            popupWidth
        );
    };
    self.getRoleName = function (roleId) {
        switch (roleId) {
            case self.Enums.roleId.Super:
                return 'Super';
            case self.Enums.roleId.Master:
                return 'Master';
            case self.Enums.roleId.Agent:
                return 'Agent';
            default:
                return '';
        }
    };
    self.getLevelName = function (levelId) {
        switch (levelId) {
            case self.Enums.roleId.Super:
                return 'S';
            case self.Enums.roleId.Master:
                return 'M';
            case self.Enums.roleId.Agent:
                return 'A';
            default:
                return '';
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
    var isRedirect = navigator.userAgent.match(/iPad/i) != null || navigator.userAgent.indexOf('Safari') > -1;
    if (isRedirect) {
        window.isProductOptionClicked = false;
        var url = $(element).val();
        location.href = window.rootUrl + url;
    }
}

function levelTypeChanged(element) {
    var isRedirect = navigator.userAgent.match(/iPad/i) != null || navigator.userAgent.indexOf('Safari') > -1;
    if (isRedirect) {
        window.isProductOptionClicked = false;
        var levelType = $(element).val();
        const mainFrame = top.$('#main');
        var currentUrl = mainFrame.attr('src');
        mainFrame.attr('src', this.replaceQueryStringValue(currentUrl, 'levelType', levelType));
    }
}

function replaceQueryStringValue(queryString, paramName, paramValue) {
    if (typeof queryString === 'undefined' || queryString === null) {
        return '';
    }

    var regex = new RegExp('([?&])' + paramName + '=[^&]*', 'i');

    var newQueryString = regex.test(queryString)
        ? queryString.replace(regex, '$1' + paramName + '=' + paramValue)
        : queryString + (queryString.indexOf('?') >= 0 ? '&' : '?') + paramName + '=' + paramValue;

    return newQueryString;
}

function correctCheckValueOnDropdown(dropdownId) {
    var dropdown = $('#' + dropdownId)[0];
    if (!dropdown) {
        return;
    }

    var findSelected = false;
    for (var i = 0; i < dropdown.length; i++) {
        var option = dropdown[i];
        var attributeLength = option.attributes.length;
        for (var j = 0; j < attributeLength; j++) {
            var attr = option.attributes[j];
            if (attr.name == 'selected') {
                if (attr.value == 'selected') {
                    $('#' + dropdownId).val(option.value);
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

window.top.successfulUpdatingCallback = function () {
    window.top.reloadPopUpSetting();
};

$(document).ready(function () {
    viewPTManagement.initPage();
    correctCheckValueOnDropdown('dropdown-product');
});

class WinLossColumns {
    private readonly visibleColumnComponent: VisibleColumnComponent;
    private readonly $visibleColumnContainer: JQuery;
    private readonly tblReportId: string = '#tbl-report';
    private sortComponent: SortComponent;
    private getSortedColumnNameCallback: () => string;
    
    private readonly downlineTotalColumn = 'downlinetotal';

    private readonly commonColumns = ['turnover', 'grosscomm', 'company', 'reward'];
    private readonly memberColumns = ['pwinloss', 'pcomm', 'ptotal'];
    private readonly agentColumns = ['awinloss', 'acomm', 'atotal'];
    private readonly masterColumns = ['winloss', 'comm', 'total'];
    private readonly superColumns = ['winloss', 'comm', 'total'];
    
    // Level-specific columns
    private readonly agentLevelColumns = ['turnover', 'grosscomm', 'pwinloss', 'pcomm', 'ptotal', 'awinloss', 'acomm', 'atotal', 'company', 'reward'];
    private readonly masterLevelColumns = ['turnover', 'grosscomm', 'downlinetotal', 'winloss', 'comm', 'total', 'company', 'reward'];
    private readonly superLevelColumns = ['turnover', 'grosscomm', 'downlinetotal', 'winloss', 'comm', 'total', 'company', 'reward'];

    private readonly subAccId = $('#SubAccId').val() || 0;
    private readonly custId = $('#UserId').val() || 0;  
    private readonly agentColumnsFromStorage = localStorage.getItem('agentColumns_' + this.custId + '_' + this.subAccId) || this.agentLevelColumns.join(',');
    private readonly masterColumnsFromStorage = localStorage.getItem('masterColumns_' + this.custId + '_' + this.subAccId) || this.masterLevelColumns.join(',');
    private readonly superColumnsFromStorage = localStorage.getItem('superColumns_' + this.custId + '_' + this.subAccId) || this.superLevelColumns.join(',');

    constructor(visibleColumnComponent: VisibleColumnComponent) {
        this.visibleColumnComponent = visibleColumnComponent;
        this.$visibleColumnContainer = $('#dropdown-visible-column').closest('.form-group');
    }

    setSortComponent(sortComponent: SortComponent, getSortedColumnNameCallback: () => string) {
        this.sortComponent = sortComponent;
        this.getSortedColumnNameCallback = getSortedColumnNameCallback;
    }

    init() {
        // Only initialize if the dropdown exists (not in historic mode)
        if ($('#dropdown-visible-column').length === 0) {
            return;
        }

        // Determine user level and set default visible columns
        const userLevel = this.getUserLevel();
        const visibleColumnsInput = userLevel === 'agent' ? this.agentColumnsFromStorage : userLevel === 'master' ? this.masterColumnsFromStorage : this.superColumnsFromStorage;
        
        const finalVisibleColumns = visibleColumnsInput;
        
        this.visibleColumnComponent.render({
            listColumn: columnList,
            onDropdownHidden: () => {
                this.showHideColumns($(this.tblReportId));
                // Update hidden input with current visible columns
                const visibleColumns = this.visibleColumnComponent.getShowColumns().join(',');
                const key = userLevel + 'Columns_' + this.custId + '_' + this.subAccId;
                localStorage.setItem(key, visibleColumns);
                
                // Re-bind sort events after showing/hiding columns (without re-sorting)
                if (this.sortComponent) {
                    this.sortComponent.rebindSortEvents();
                }
            },
            visibleColumns: finalVisibleColumns
        });
        this.showHideColumns($(this.tblReportId));
    }

    showHideColumns($tblReport: JQuery) {
        // Only apply column visibility in normal mode (not stack mode)
        if ($tblReport.hasClass('stack-mode')) {
            return;
        }

        const visibleColumn = this.visibleColumnComponent.getColumns();
        const userLevel = this.getUserLevel();
        
        // Handle columns based on user level
        this.showHideColumnsByLevel($tblReport, visibleColumn, userLevel);
        
        // Update column indices after showing/hiding columns
        this.updateColumnIndices($tblReport);
    }
    
    private updateColumnIndices($tblReport: JQuery) {
        // Update data-colindex based on data-original-colindex for visible columns only
        // This ensures correct column ordering regardless of which columns are hidden
        
        const $thead = $tblReport.find('thead');
        
        // Get all sortable columns that have data-original-colindex (excluding stack-mode columns)
        const allColumns: Array<{$th: JQuery, originalIndex: number}> = [];
        
        $thead.find('th.sorting[data-original-colindex]').not('.visible-stack-mode').each(function() {
            const $th = $(this);
            const originalIndex = parseInt($th.attr('data-original-colindex') || '0');
            allColumns.push({ $th: $th, originalIndex: originalIndex });
        });
        
        // Sort by original index to maintain correct order
        allColumns.sort((a, b) => a.originalIndex - b.originalIndex);
        
        // Assign new colindex only to visible columns
        let newColIndex = 0;
        allColumns.forEach(col => {
            if (col.$th.is(':visible')) {
                col.$th.attr('data-colindex', newColIndex);
                newColIndex++;
            } else {
                // Clear colindex from hidden columns
                col.$th.removeAttr('data-colindex');
            }
        });
    }
    
    public getUserLevel(): string {
        const isAgentLevel = $('#IsAgentLevel').val() === 'True';
        if (isAgentLevel)
        {
            return 'agent';
        }

        const isMasterLevel = $('#IsMasterLevel').val() === 'True';
        if (isMasterLevel)
        {
            return 'master';
        }

        const isSuperLevel = $('#IsSuperLevel').val() === 'True';
        if (isSuperLevel)
        {
            return 'super';
        }

        return 'unknown';
    }    
    
    private showHideColumnsByLevel($tblReport: JQuery, visibleColumn: string[], userLevel: string) {
        switch (userLevel) {
            case 'agent':
                this.showHideAgentLevelColumns($tblReport, visibleColumn);
                break;
            case 'master':
                this.showHideMasterLevelColumns($tblReport, visibleColumn);
                break;
            case 'super':
                this.showHideSuperLevelColumns($tblReport, visibleColumn);
                break;
            default:
                this.showHideCommonColumns($tblReport, visibleColumn);
                break;
        }
    }
    
    private showHideAgentLevelColumns($tblReport: JQuery, visibleColumn: string[]) {
        // Handle common columns
        this.commonColumns.forEach(columnName => {
            this.showHideSpecificColumn($tblReport, visibleColumn, columnName);
        });
        
        // Handle member group columns
        this.showHideGroupColumnsByNames($tblReport, visibleColumn, this.memberColumns, 0);
        
        // Handle agent group columns
        this.showHideGroupColumnsByNames($tblReport, visibleColumn, this.agentColumns, 1);
    }
    
    private showHideMasterLevelColumns($tblReport: JQuery, visibleColumn: string[]) {
        // Handle common columns including downlinetotal
        this.commonColumns.forEach(columnName => {
            this.showHideSpecificColumn($tblReport, visibleColumn, columnName);
        });
        
        // Handle downlinetotal column
        this.showHideSpecificColumn($tblReport, visibleColumn, this.downlineTotalColumn);
        
        // Handle master/super group columns
        this.showHideGroupColumnsByNames($tblReport, visibleColumn, this.masterColumns, 0);
    }

    private showHideSuperLevelColumns($tblReport: JQuery, visibleColumn: string[]) {
        // Handle common columns including downlinetotal
        this.commonColumns.forEach(columnName => {
            this.showHideSpecificColumn($tblReport, visibleColumn, columnName);
        });
        
        // Handle downlinetotal column
        this.showHideSpecificColumn($tblReport, visibleColumn, this.downlineTotalColumn);
        
        // Handle master/super group columns
        this.showHideGroupColumnsByNames($tblReport, visibleColumn, this.superColumns, 0);
    }
    
    private showHideCommonColumns($tblReport: JQuery, visibleColumn: string[]) {
        this.commonColumns.forEach(columnName => {
            this.showHideSpecificColumn($tblReport, visibleColumn, columnName);
        });
        
        // Handle downlinetotal column separately - only show for Super/Master level
        this.showHideDownlineTotalColumn($tblReport, visibleColumn);
    }
    
    private showHideDownlineTotalColumn($tblReport: JQuery, visibleColumn: string[]) {
        // Check if this is Agent level by looking for member columns
        const isAgentLevel = $tblReport.find('[data-colname="awinloss"]').length > 0;
        
        if (!isAgentLevel) {
            // Super/Master level - show downlinetotal if it's checked
            this.showHideSpecificColumn($tblReport, visibleColumn, this.downlineTotalColumn);
        } else {
            // Agent level - always hide downlinetotal
            const matchedColumns = $tblReport.find(`[data-colname=${this.downlineTotalColumn}]`);
            matchedColumns.addClass('hide');
        }
    }
    


    private showHideGroupColumnsByNames($tblReport: JQuery, visibleColumn: string[], columnNames: string[], groupIndex: number) {
        const groupColumns = $tblReport.find(`th.col-level`);
        const $groupColumn = groupColumns.eq(groupIndex);
        
        if ($groupColumn.length > 0) {
            let colspan = 0;
            
            // Count visible columns for this group
            columnNames.forEach(columnName => {
                if (this.showHideSpecificColumn($tblReport, visibleColumn, columnName)) {
                    colspan++;
                }
            });

            // Update colspan and visibility
            if (colspan > 0) {
                $groupColumn.attr('colspan', colspan).removeClass('hide');
                // Ensure group header is visible in normal mode
                if (!$tblReport.hasClass('stack-mode')) {
                    $groupColumn.removeClass('hidden-stack-mode');
                }
            } else {
                $groupColumn.addClass('hide');
            }
        }
    }

    private showHideSpecificColumn($tblReport: JQuery, visibleColumn: string[], key: string) {
        const matchedColumns = $tblReport.find(`[data-colname=${key}]`);
        const visible = visibleColumn[key];
        // If visible is undefined, treat as true (show by default)
        const shouldShow = visible !== false;
        shouldShow ? matchedColumns.removeClass('hide') : matchedColumns.addClass('hide');

        return shouldShow;
    }

    showInNormalMode() {
        if (this.$visibleColumnContainer.length > 0) {
            this.$visibleColumnContainer.removeClass('hide');
        }
        
        // Reset group headers for normal mode
        this.resetGroupHeadersForNormalMode();
    }

    hideInStackMode() {
        if (this.$visibleColumnContainer.length > 0) {
            this.$visibleColumnContainer.addClass('hide');
        }
    }
    
    private resetGroupHeadersForNormalMode() {
        const $tblReport = $(this.tblReportId);
        
        // Show normal mode group headers and hide stack mode headers
        $tblReport.find('th.col-level').removeClass('hide');
        $tblReport.find('th.visible-stack-mode').addClass('hide');
        
        // Remove hidden-stack-mode class from group headers
        $tblReport.find('th.col-level').removeClass('hidden-stack-mode');
    }

    getShowColumns(): string[] {
        return this.visibleColumnComponent.getShowColumns();
    }
}


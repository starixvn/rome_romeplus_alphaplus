class VisibleColumnContextMenuComponent {
    private cookieNameColumn: string;
    private cookieNameHint: string;

    init(cookiePrefix: string, $table: JQuery, defaultCookieValue: string = undefined) {
        this.cookieNameColumn = cookiePrefix + '_columns';
        this.cookieNameHint = this.cookieNameColumn + 'X';
        this.setDefaultCookieColumns(defaultCookieValue);

        const $contextMenu = $('#columns-contextmenu');
        const $hint = $contextMenu.find('.hint');
        const $columnDropdown = $contextMenu.find('.column-dropdown');
        const $tableHeaders = this.buildColumnDropdown($columnDropdown, $table);

        const cookieHint = this.getCookieHint();
        if (cookieHint < 5) {
            $hint.show();
            setTimeout(() => $hint.hide(), 1000 * 10);
            this.setCookieHint(cookieHint + 1);
        }

        $contextMenu.find('#chk-dontShowAgain').click(() => {
            $hint.hide();
            this.setCookieHint(5);
        });

        $columnDropdown.mouseleave(function () {
            $columnDropdown.hide();
        });

        const $columns = $columnDropdown.find('.column').change((e) => {
            this.handleShowHideColumn($(e.target), $table);
            this.setCookieColumns($columns);
        });

        $tableHeaders
            .mouseup((event) => {
                if (event.which === 3) {
                    $hint.hide();
                    this.positionMenu(event, $columnDropdown[0]);
                    $columnDropdown.show();
                }
            })
            .on('contextmenu', (event) => event.preventDefault());
    }

    private buildColumnDropdown($columnDropdown: JQuery, $table: JQuery) {
        const cookieColumnsObject = this.getCookieColumnsObject();
        const html = [];
        const $tableHeaders = $table.find('thead th');
        $tableHeaders.each(function () {
            const $element = $(this);
            const colName = $element.data('colname');

            if (colName) {
                const id = 'chk-column-' + colName;
                html.push('<li><input type="checkbox" class="column" id="', id, '" data-colname="', colName, '"');
                if (cookieColumnsObject.hasOwnProperty(colName)) {
                    html.push(' checked="checked"');
                } else {
                    $table.find(`[data-colname="${colName}"]`).addClass('hide');
                }
                html.push(' /><label for="', id, '">', $element.text(), '</label></li>');
            }
        });

        $columnDropdown.html(html.join(''));

        return $tableHeaders;
    }

    private handleShowHideColumn($checkbox: JQuery, $table: JQuery) {
        const $tableColumns = $table.find(`[data-colname="${$checkbox.data('colname')}"]`);
        if ($checkbox.is(':checked')) {
            $tableColumns.removeClass('hide');
        } else {
            $tableColumns.addClass('hide');
        }
    }

    private setDefaultCookieColumns(defaultValue: string) {
        const cookieColumns = this.getCookie(this.cookieNameColumn);
        if (!cookieColumns && defaultValue) {
            this.setCookie(this.cookieNameColumn, defaultValue);
        }
    }

    private getCookieColumnsObject() {
        const cookieColumnsObject = {};
        const cookieColumns = this.getCookie(this.cookieNameColumn);

        if (cookieColumns.length) {
            const cookieColumnsArray = cookieColumns.split(',');
            for (const item of cookieColumnsArray) {
                cookieColumnsObject[item] = true;
            }
        }

        return cookieColumnsObject;
    }

    private setCookieColumns($columns) {
        const visibleColumns = [];
        $columns.filter(':checked').each(function () {
            visibleColumns.push($(this).data('colname'));
        });

        this.setCookie(this.cookieNameColumn, visibleColumns.length ? visibleColumns.join(',') : '_');
    }

    private getCookieHint() {
        const cookieHint = this.getCookie(this.cookieNameHint);

        return cookieHint.length ? +cookieHint : 0;
    }

    private setCookieHint(value: number) {
        this.setCookie(this.cookieNameHint, value.toString());
    }

    private getCookie(name: string): string {
        return $.cookie(name) || '';
    }

    private setCookie(name: string, value: string, expiredDays: number = 30) {
        const cookieExpire = alphaTransfer.isIe()
            ? { expires: expiredDays }
            : { expires: expiredDays, path: window.location.pathname };

        $.cookie(name, value, cookieExpire);
    }

    private positionMenu(event, columnDropdown: HTMLElement) {
        const clickCoords = this.getPosition(event);
        const clickCoordsX = clickCoords.x;
        const clickCoordsY = clickCoords.y;
        const menuWidth = columnDropdown.offsetWidth + 4;
        const menuHeight = columnDropdown.offsetHeight + 4;
        const windowWidth = window.innerWidth;
        const windowHeight = window.innerHeight;
        if (windowWidth - clickCoordsX < menuWidth) {
            columnDropdown.style.left = windowWidth - menuWidth + 'px';
        } else {
            columnDropdown.style.left = clickCoordsX + 'px';
        }

        if (windowHeight - clickCoordsY < menuHeight) {
            columnDropdown.style.top = windowHeight - menuHeight + 'px';
        } else {
            columnDropdown.style.top = clickCoordsY + 'px';
        }
    }

    private getPosition(event) {
        let posx = 0;
        let posy = 0;
        event = event || window.event;
        if (event.pageX || event.pageY) {
            posx = event.pageX;
            posy = event.pageY;
        } else if (event.clientX || event.clientY) {
            posx = event.clientX + document.body.scrollLeft + document.documentElement.scrollLeft;
            posy = event.clientY + document.body.scrollTop + document.documentElement.scrollTop;
        }

        return {
            x: posx,
            y: posy + 15
        };
    }
}

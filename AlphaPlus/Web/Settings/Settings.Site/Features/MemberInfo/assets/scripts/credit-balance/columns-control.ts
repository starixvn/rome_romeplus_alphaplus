﻿'use strict';

class ColumnsControlComponent {
    $: JQueryStatic;
    document: Document;
    timerId: number = null;
    tableHeader: JQuery;
    columnsShowHide: any;
    columnsControlHint: JQuery;
    checkboxDontShowAgent: JQuery;
    columnsControl: JQuery;

    constructor($: JQueryStatic, document: Document) {
        this.$ = $;
        this.document = document;
    }

    render = (cookiePrefix: string) => {
        this.cacheDOMElements();

        this.initColumnsControlHint(cookiePrefix);

        this.columnsControl.html(this.buildHtmlColumnsShowHide(cookiePrefix));

        this.registerEvents(cookiePrefix);
    };

    cacheDOMElements = () => {
        this.tableHeader = this.$('#tbl-report thead th');
        this.columnsShowHide = this.tableHeader.filter((index: number, item: any) => item.className);

        this.columnsControlHint = this.$('#columns-control-hint');
        this.checkboxDontShowAgent = this.$('#chkDontShowAgain');
        this.columnsControl = this.$('#columns-control');
    };

    registerEvents = (cookiePrefix) => {
        const self: any = this;

        this.tableHeader
            .mouseup(function (event: any) {
                if (event.which === 3) {
                    self.autoHideColumnsControlHint();
                    self.columnsControl.css({ left: event.clientX + 20 + 'px' }).removeClass('hide');
                }
            })
            .on('contextmenu', () => false);

        this.checkboxDontShowAgent.click(function () {
            self.setCookieHint(cookiePrefix, 5);
            self.autoHideColumnsControlHint();
        });

        this.columnsControl.mouseleave(function () {
            self.columnsControl.addClass('hide');
        });
        this.columnsControl.find('.columns-control-checkbox').change(function () {
            self.showHideCheckboxChange(self.$(this), cookiePrefix);
        });
    };

    initColumnsControlHint = (cookiePrefix: string) => {
        let cookieValue: number = this.getCookieHint(cookiePrefix);

        if (cookieValue < 5) {
            this.columnsControlHint.removeClass('hide');

            this.timerId = setTimeout(this.autoHideColumnsControlHint, 1000 * 10);
            this.setCookieHint(cookiePrefix, ++cookieValue);
        }
    };

    autoHideColumnsControlHint = () => {
        if (this.timerId !== null) {
            clearTimeout(this.timerId);
            this.timerId = null;
        }

        this.columnsControlHint.addClass('hide');
    };

    buildHtmlColumnsShowHide = (cookiePrefix: string) => {
        const self: any = this;
        let cookieColumnsShowHide: string[] = this.getCookieColumnsShowHide(cookiePrefix);

        if (cookieColumnsShowHide.length === 0) {
            cookieColumnsShowHide = this.setCookieInitializeColumnsShowHide(cookiePrefix, this.columnsShowHide.length);
        }

        let html: string[] = ['<ul>'];
        this.columnsShowHide.each((index: number, th: any) => {
            const element: JQuery = this.$(th);
            const id: string = 'chk-' + index;
            const className = element.data('target') || element.attr('class');

            html.push('<li>');
            html.push('<input type="checkbox" class="columns-control-checkbox" id="');
            html.push(id);
            html.push('" data-index="');
            html.push(index.toString());
            html.push('" data-target="');
            html.push(className);
            html.push('"');
            if (cookieColumnsShowHide[index] === '1') {
                html.push(' checked="checked"');
            } else {
                self.$('.' + className).addClass('hide');
            }
            html.push(' />');
            html.push('<label for="');
            html.push(id);
            html.push('">');
            html.push(element.text());
            html.push('</label>');
            html.push('</li>');
        });

        html.push('</ul>');

        return html.join('');
    };

    showHideCheckboxChange = (checkbox: JQuery, cookiePrefix: string) => {
        const indexArray: string[] = this.getCookieColumnsShowHide(cookiePrefix);
        const targetClass: string = checkbox.data('target');

        if (checkbox.is(':checked')) {
            this.$('.' + targetClass).removeClass('hide');
            indexArray[checkbox.data('index')] = '1';
        } else {
            this.$('.' + targetClass).addClass('hide');
            indexArray[checkbox.data('index')] = '0';
        }

        this.setCookieColumnsShowHide(cookiePrefix, indexArray);
    };

    setCookieHint = (cookiePrefix: string, value: number) => this.setCookie(cookiePrefix + 'X', value.toString(), 30);

    getCookieHint = (cookiePrefix: string) => {
        const cookieValue: string = this.getCookie(cookiePrefix + 'X');

        return cookieValue.length ? parseInt(cookieValue) : 0;
    };

    setCookieColumnsShowHide = (cookiePrefix: string, indexArray: string[]) =>
        this.setCookie(cookiePrefix, indexArray.join(','), 30);

    setCookieInitializeColumnsShowHide = (cookiePrefix: string, arrayLength: number) => {
        let columnsShowHide: string[] = [];

        for (let i: number = 0; i < arrayLength; i++) {
            columnsShowHide.push('1');
        }

        this.setCookieColumnsShowHide(cookiePrefix, columnsShowHide);

        return columnsShowHide;
    };

    getCookieColumnsShowHide = (cookiePrefix: string) => {
        const cookieValue: string = this.getCookie(cookiePrefix);

        return cookieValue.length ? cookieValue.split(',') : [];
    };

    setCookie = (name: string, value: string, expiredDays: number, domain: string = undefined) => {
        let expires: string = '';

        if (expiredDays) {
            let date: any = new Date();
            date.setTime(date.getTime() + expiredDays * 24 * 60 * 60 * 1000);
            expires = ';expires=' + date.toUTCString();
        }

        this.document.cookie =
            name + '=' + value + expires + ';path=/;' + (typeof domain !== 'undefined' ? 'domain=' + domain : '');
    };

    getCookie = (name: string) => {
        const nameEQ: string = name + '=';
        const ca: string[] = this.document.cookie.split(';');

        for (let i: number = 0, max: number = ca.length; i < max; i++) {
            let c: any = ca[i];
            while (c.charAt(0) === ' ') {
                c = c.substring(1);
            }

            if (c.indexOf(nameEQ) === 0) {
                return c.substring(nameEQ.length, c.length);
            }
        }

        return '';
    };
}

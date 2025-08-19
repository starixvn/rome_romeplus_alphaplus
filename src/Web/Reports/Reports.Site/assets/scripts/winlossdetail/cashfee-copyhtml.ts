class CashFeeCopyHtml {
    private clipboard: any;
    private iconCopyClipboard: any;

    constructor(clipboard: any) {
        this.clipboard = clipboard;
    }

    bindEvent($tblReport: JQuery) {
        $('#icon-copy').tooltip('destroy').tooltip();
        this.iconCopyClipboard && this.iconCopyClipboard.destroy();
        this.iconCopyClipboard = new this.clipboard('#icon-copy', {
            text: () => this.copyHtmlTable($tblReport)
        });
        if (window.document.queryCommandSupported && window.document.queryCommandSupported('copy')) {
            this.iconCopyClipboard
                .on('success', (e) => {
                    const $this = $(e.trigger);
                    $this.attr('data-original-title', $('#hidden-copiedText').val());
                    $this.tooltip('show');
                    setTimeout(() => {
                        $this.tooltip('hide');
                        $this.attr('data-original-title', $('#hidden-copyText').val());
                    }, 3000);
                })
                .on('error', function (e) {
                    $(e.trigger).remove();
                    console.error('Action:', e.action);
                    console.error('Trigger:', e.trigger);
                });
        }
    }

    private copyHtmlTable($tblReport: JQuery) {
        const html = ['<table>'];
        this.buildHtmlHeader($tblReport, html);
        this.buildHtmlBody($tblReport, html);
        this.buildHtmlFooter($tblReport, html);
        html.push('</table>');

        return html.join('');
    }

    private buildHtmlHeader($tblReport: JQuery, html: string[]) {
        html.push('<thead><tr>');
        $tblReport.find('thead tr:first th:not(.hide)').each(function () {
            html.push('<th>', this.innerText, '</th>');
        });

        html.push('</tr></thead>');
    }

    private buildHtmlBody($tblReport: JQuery, html: string[]) {
        const getColumnText = function (td) {
            let title;
            const $td = $(td);
            if ($td.data('colname') === 'fullname') {
                title = $td.find('div:first').data('original-title');
            }

            return title || td.innerText;
        };
        html.push('<tbody>');

        $tblReport.find('tbody tr').each(function () {
            html.push('<tr>');
            $(this)
                .find('td:not(.hide)')
                .each(function () {
                    html.push('<td>', getColumnText(this), '</td>');
                });
            html.push('</tr>');
        });

        html.push('</tbody>');
    }

    private buildHtmlFooter($tblReport: JQuery, html: string[]) {
        const footerRows = $tblReport.find('tfoot tr td:not(.hide)');

        if (footerRows.length) {
            html.push('<tfoot><tr>');
            footerRows.each(function () {
                const $this = $(this);

                html.push('<td');

                const colspan = $this.attr('colspan');

                if (colspan) {
                    html.push(` colspan="${colspan}"`);
                }

                html.push('>', this.innerText, '</td>');
            });

            html.push('</tr></tfoot>');
        }
    }
}

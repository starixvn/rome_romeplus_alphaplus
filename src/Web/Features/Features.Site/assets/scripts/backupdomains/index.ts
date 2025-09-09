'use strict';

declare const Clipboard: any;
class BackupDomainsView {
    $: JQueryStatic;
    clipboard: any;
    compactFeedbackAgent: CompactFeedbackComponent;
    compactFeedbackMember: CompactFeedbackComponent;

    constructor(
        $: JQueryStatic,
        clipboard,
        compactFeedbackAgent: CompactFeedbackComponent,
        compactFeedbackMember: CompactFeedbackComponent
    ) {
        this.$ = $;
        this.clipboard = clipboard;
        this.compactFeedbackAgent = compactFeedbackAgent;
        this.compactFeedbackMember = compactFeedbackMember;
    }

    public init() {
        this.bindCopyDomain('agency');
        this.bindCopyDomain('member');
        this.$('.domain-url').click(function (e) {
            e.preventDefault();
            window.open('//' + this.innerText);
        });
        this.compactFeedbackAgent.init();
        this.compactFeedbackMember.init();
    }

    private bindCopyDomain(id) {
        this.$(`#tbl-${id} .icon-content-copy`).each((i, item) => {
            const iconCopyId = item.id;
            const $iconCopy = this.$(item).tooltip();

            if (iconCopyId === `icon-copy-all-${id}`) {
                this.copyAllDomains(id, `#${iconCopyId}`);
            } else {
                this.copyDomain(`#${iconCopyId}`);
                this.showInUse($iconCopy);
            }
        });
    }

    private copyAllDomains(id: string, iconCopyId: string) {
        const clipboard = new this.clipboard(iconCopyId, {
            text: (trigger) => {
                const urls = [];
                this.$(`#tbl-${id} .domain-url`).each(function () {
                    urls.push(this.innerText);
                });

                return urls.join('\r\n');
            }
        });
        this.completeCopyDomain(clipboard, this.$('#copyAll').val());
    }

    private copyDomain(iconCopyId: string) {
        const clipboard = new this.clipboard(iconCopyId, {
            text: (trigger) => this.$(trigger).parent().data('domain')
        });
        this.completeCopyDomain(clipboard, this.$('#copyDomain').val());
    }

    private completeCopyDomain(clipboard, copyText: string) {
        if (window.document.queryCommandSupported && window.document.queryCommandSupported('copy')) {
            clipboard
                .on('success', (e) => {
                    const $element = this.$(e.trigger);
                    $element.attr('data-original-title', this.$('#copied').val());
                    $element.tooltip('show');
                    setTimeout(() => {
                        $element.tooltip('hide');
                        $element.attr('data-original-title', copyText);
                    }, 3000);
                })
                .on('error', (e) => {
                    this.$(e.trigger).remove();
                    window.console.error('Action:', e.action);
                    window.console.error('Trigger:', e.trigger);
                });
        }
    }

    private showInUse($iconCopy: JQuery) {
        const $copyColumn = $iconCopy.parent();

        if (this.isInUse($copyColumn.data('domain'))) {
            const $domainColumn = $copyColumn.prev();
            $domainColumn.append(`<span class="icon icon-check pull-right" title="${this.$('#inUse').val()}"></span>`);
            $domainColumn.find('.icon-check').tooltip();
        }
    }

    private isInUse(domain: string) {
        return window.location.href.indexOf(domain.replace('www.', '')) !== -1;
    }
}

$(function () {
    new BackupDomainsView(
        $,
        Clipboard,
        new CompactFeedbackComponent('#box-icon-title-agency'),
        new CompactFeedbackComponent('#box-icon-title-member')
    ).init();
});

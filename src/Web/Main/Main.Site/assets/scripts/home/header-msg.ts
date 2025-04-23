class HeaderMessage {
    private publicMessageId: number = 0;
    private privateMessageId: number = 0;
    private scrollMessageTimeout: number;
    private scrollerLeft: number = 0;
    private scrollerWidth: number = 0;
    private maskWidth: number = 0;
    private scrollerPaused: boolean;
    private privateMsg: HTMLElement;
    private publicMsg: HTMLElement;
    private scroller: HTMLElement;
    private domElements = {
        privateMessage: null,
        publicMessage: null,
        marqueeMessage: null,
        scroller: null,
        contentMessage: null
    };
    protected urls = {
        mainAppPrivateMessageForm: alpha.urls.messageRootPath + 'MessageHistory/Index?messageType=1',
        mainAppPublicMessageForm: alpha.urls.messageRootPath + 'MessageHistory/Index?messageType=0',
        mainAppSpecialMessageForm: alpha.urls.messageRootPath + 'MessageHistory/Index?messageType=2',
        getMessages: alpha.urls.messageRootPath + 'MarqueeMessage/Index'
    };
    protected $: JQueryStatic;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    init = () => {
        this.cacheElements();
        this.registerEvents();
        this.getMessages();
    };

    viewLiveMessage = (event: JQueryEventObject) => {
        const messageType = parseInt(this.$(event.currentTarget).attr('messageType'));

        headerNav.buildIconNewMessageByType(messageType);
        if (messageType == alpha.messageType.Normal) {
            indexManager.changeMenu(this.urls.mainAppPublicMessageForm);
        } else if (messageType == alpha.messageType.Personal) {
            indexManager.changeMenu(this.urls.mainAppPrivateMessageForm);
        } else if (messageType == alpha.messageType.Special) {
            indexManager.changeMenu(this.urls.mainAppSpecialMessageForm);
        }
    };

    getMessages = () => {
        this.$.ajax({
            type: 'POST',
            url: this.urls.getMessages,
            success: this.onSuccessGetMessages
        });
    };

    onSuccessGetMessages = (rawResults) => {
        const results = rawResults;

        this.showPublicMessage(results);

        this.showPrivateMessage(results);

        this.applyScroller();

        setTimeout(this.getMessages, 5 * 60 * 1000);
    };

    showPublicMessage = (results: any) => {
        const publicMessage = results.PublicMessage ? results.PublicMessage : 0;

        // Process Public Message: show new public message and its link to message form
        const url = this.urls.mainAppPublicMessageForm;

        if (publicMessage.Id && publicMessage.Id !== this.publicMessageId) {
            this.domElements.publicMessage
                .html(publicMessage.Content + '. ')
                .attr('messageType', publicMessage.MessageType);

            this.publicMessageId = publicMessage.Id;
            if (this.publicMessageId !== 0) {
                this.appendMessageToPopup(publicMessage.Content);
            }
        }
    };

    showPrivateMessage = (results: any) => {
        const privateMessage = results.PrivateMessage ? results.PrivateMessage : 0;

        // Process Private Message: show new private message
        const url = this.urls.mainAppPrivateMessageForm;

        if (privateMessage.Id && this.privateMessageId !== privateMessage.Id) {
            //// Append Private Message to Marquee with green color format.
            this.domElements.privateMessage
                .html(
                    '<font id=\'private-message-content\' color="green">Private message: ' +
                        privateMessage.Content +
                        '</font>'
                )
                .attr('messageType', privateMessage.MessageType);

            if (this.privateMessageId !== 0) {
                this.appendMessageToPopup(privateMessage.Content);
            }
        }
    };

    appendMessageToPopup = (message) => {
        this.domElements.contentMessage.append('<p class="not-shown-message"></p>');

        this.domElements.contentMessage.find('p:last').html(message);
    };

    applyScroller = () => {
        if (this.scrollerLeft) {
            return;
        }

        this.scrollerLeft = 100;
        this.scrollerWidth = this.domElements.scroller.width();

        this.scroller = this.domElements.scroller[0];
        this.privateMsg = this.domElements.privateMessage[0];
        this.publicMsg = this.domElements.publicMessage[0];

        this.privateMsg.onmouseover = this.publicMsg.onmouseover = () => {
            this.pauseMarquee();
        };
        this.privateMsg.onmouseout = this.publicMsg.onmouseout = () => {
            this.playMarquee();
        };

        const textNews = this.domElements.marqueeMessage[0];
        this.maskWidth = textNews.offsetWidth;

        this.scrollMessage();
    };

    playMarquee = () => {
        if (this.scrollerPaused) {
            this.scrollerPaused = false;
            this.scrollMessageTimeout = setTimeout(this.scrollMessage, 20);
        } else {
            this.scrollerPaused = false;
        }
    };

    pauseMarquee = () => {
        clearTimeout(this.scrollMessageTimeout);
        this.scrollMessageTimeout = 0;
        this.scrollerPaused = true;
    };

    scrollMessage = () => {
        if (this.scrollerPaused) {
            return;
        }

        if (this.scrollerLeft + this.scrollerWidth < -5) {
            this.scrollerLeft = this.maskWidth + 5;
            this.scrollerWidth = this.domElements.scroller.width();
        }

        this.scroller.style.left = this.scrollerLeft-- + 'px';
        this.scrollMessageTimeout = setTimeout(this.scrollMessage, 20);
    };

    protected cacheElements = () => {
        this.domElements.privateMessage = this.$('#private-message');
        this.domElements.publicMessage = this.$('#public-message');
        this.domElements.marqueeMessage = this.$('#marqueeMessage');
        this.domElements.contentMessage = this.$('#msg-content');
        this.domElements.scroller = this.$('#scroller');
    };

    protected registerEvents = () => {
        this.domElements.privateMessage.on('click', this.viewLiveMessage);
        this.domElements.publicMessage.on('click', this.viewLiveMessage);
    };
}

$(() => {
    if (alpha.isScreenLower768) {
        return;
    }

    const headerMessage = new HeaderMessage($);
    headerMessage.init();
});

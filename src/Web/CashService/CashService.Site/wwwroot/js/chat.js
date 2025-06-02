"use strict";

class ChatClient {
    constructor() {
        this.connection = new signalR.HubConnectionBuilder()
            .withUrl("/cash-service/chathub")
            .withAutomaticReconnect()
            .build();

        this.messageHandlers = new Map();
        this.setupEventHandlers();
    }

    setupEventHandlers() {
        // Connection events
        this.connection.onreconnecting(error => {
            console.log('Reconnecting to chat hub...', error);
            this.notify('Connection lost. Reconnecting...', 'warning');
        });

        this.connection.onreconnected(connectionId => {
            console.log('Reconnected to chat hub');
            this.notify('Reconnected to chat server', 'success');
        });

        this.connection.onclose(error => {
            console.log('Connection closed', error);
            this.notify('Connection closed', 'error');
        });

        // Chat events
        this.connection.on("ReceiveMessage", (user, message) => {
            this.handleMessage("ReceiveMessage", { user, message });
        });

        this.connection.on("ReceivePrivateMessage", (fromUser, message) => {
            this.handleMessage("ReceivePrivateMessage", { fromUser, message });
        });

        this.connection.on("ReceiveGroupMessage", (user, message) => {
            this.handleMessage("ReceiveGroupMessage", { user, message });
        });

        this.connection.on("UserConnected", (user) => {
            this.handleMessage("UserConnected", { user });
        });

        this.connection.on("UserDisconnected", (user) => {
            this.handleMessage("UserDisconnected", { user });
        });

        this.connection.on("UserTyping", (user, isTyping) => {
            this.handleMessage("UserTyping", { user, isTyping });
        });

        this.connection.on("OnlineUsers", (users) => {
            this.handleMessage("OnlineUsers", { users });
        });

        this.connection.on("UserJoinedGroup", (userId, groupName) => {
            this.handleMessage("UserJoinedGroup", { userId, groupName });
        });

        this.connection.on("UserLeftGroup", (userId, groupName) => {
            this.handleMessage("UserLeftGroup", { userId, groupName });
        });
    }

    async start() {
        try {
            await this.connection.start();
            console.log("Connected to chat hub");
            this.notify('Connected to chat server', 'success');
        } catch (err) {
            console.error("Error connecting to chat hub:", err);
            this.notify('Failed to connect to chat server', 'error');
            setTimeout(() => this.start(), 5000);
        }
    }

    on(eventName, handler) {
        if (!this.messageHandlers.has(eventName)) {
            this.messageHandlers.set(eventName, new Set());
        }
        this.messageHandlers.get(eventName).add(handler);
    }

    off(eventName, handler) {
        if (this.messageHandlers.has(eventName)) {
            this.messageHandlers.get(eventName).delete(handler);
        }
    }

    handleMessage(eventName, data) {
        if (this.messageHandlers.has(eventName)) {
            this.messageHandlers.get(eventName).forEach(handler => handler(data));
        }
    }

    async sendMessage(user, message) {
        try {
            await this.connection.invoke("SendMessage", user, message);
        } catch (err) {
            console.error("Error sending message:", err);
            this.notify('Failed to send message', 'error');
        }
    }

    async sendPrivateMessage(fromUser, toUser, message) {
        try {
            await this.connection.invoke("SendPrivateMessage", fromUser, toUser, message);
        } catch (err) {
            console.error("Error sending private message:", err);
            this.notify('Failed to send private message', 'error');
        }
    }

    async joinGroup(groupName) {
        try {
            await this.connection.invoke("JoinGroup", groupName);
        } catch (err) {
            console.error("Error joining group:", err);
            this.notify('Failed to join group', 'error');
        }
    }

    async leaveGroup(groupName) {
        try {
            await this.connection.invoke("LeaveGroup", groupName);
        } catch (err) {
            console.error("Error leaving group:", err);
            this.notify('Failed to leave group', 'error');
        }
    }

    async sendGroupMessage(groupName, user, message) {
        try {
            await this.connection.invoke("SendGroupMessage", groupName, user, message);
        } catch (err) {
            console.error("Error sending group message:", err);
            this.notify('Failed to send group message', 'error');
        }
    }

    async sendTypingIndicator(user, isTyping) {
        try {
            await this.connection.invoke("SendTypingIndicator", user, isTyping);
        } catch (err) {
            console.error("Error sending typing indicator:", err);
        }
    }

    async getOnlineUsers() {
        try {
            await this.connection.invoke("GetOnlineUsers");
        } catch (err) {
            console.error("Error getting online users:", err);
            this.notify('Failed to get online users', 'error');
        }
    }

    notify(message, type = 'info') {
        // You can implement your own notification system here
        console.log(`[${type.toUpperCase()}] ${message}`);
    }
}

// Export the ChatClient class
window.ChatClient = ChatClient;

// Initialize chat when the page loads
document.addEventListener("DOMContentLoaded", () => {
    new ChatClient();
}); 
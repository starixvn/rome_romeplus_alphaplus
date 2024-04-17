export const UserLevel = {
    Super: 4,
    Master: 3,
    Agent: 2,
    Member: 1
};

declare global {
    interface Window {
        alpha: {
            urls: { [key: string]: string };
        };
    }
}

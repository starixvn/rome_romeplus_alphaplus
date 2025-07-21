import { useState } from "react";
import { ResultType } from "../components/Message/types";
import { MessageComponent } from "../components/Message";

export const useResultMessage = () => {
    const [result, setResult] = useState<{ type: ResultType; text: string } | null>(null);

    const showResult = (type: ResultType, text: string, duration: number = 3000, onClose?: () => void) => {
        setResult({
            type: type,
            text: text,
        });
        setTimeout(() => {
            setResult(null);
            onClose?.();
        }, duration);
    };

    const clearResult = () => {
        setResult(null);
    };

    const ResultMessage = (): React.ReactNode => {
        if (result) {
            return <MessageComponent type={result.type} message={result.text} />;
        }
        return null;
    }

    return { result, showResult, clearResult, ResultMessage };
}

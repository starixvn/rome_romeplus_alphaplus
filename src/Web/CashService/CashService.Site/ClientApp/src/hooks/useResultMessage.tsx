import { useState } from "react";
import { ResultType } from "../components/Message/types";
import { ResultMessageComponent } from "../components/Message/ResultMessageComponent";

export const useResultMessage = () => {
    const [result, setResult] = useState<{ type: ResultType; text: string } | null>(null);

    const showResult = (type: ResultType, text: string) => {
        setResult({
            type: type,
            text: text,
        });
        setTimeout(() => {
            setResult(null);
        }, 3000);
    };

    const clearResult = () => {
        setResult(null);
    };

    const ResultMessage = (): React.ReactNode => {
        if (result) {
            return <ResultMessageComponent type={result.type} message={result.text} />;
        }
        return null;
    }

    return { result, showResult, clearResult, ResultMessage };
}

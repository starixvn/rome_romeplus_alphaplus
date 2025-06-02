import { useRef, useEffect, useCallback } from "react";

type CallbackFn<T extends any[]> = (...args: T) => void;

export function useDebouncedCallback<T extends any[]>(
  callback: CallbackFn<T>,
  delay: number,
  immediate: boolean = false
): CallbackFn<T> {
  const timeoutRef = useRef<ReturnType<typeof setTimeout> | null>(null);

  const debouncedFn = useCallback((...args: T) => {
    const callNow = immediate && !timeoutRef.current;

    if (timeoutRef.current) {
      clearTimeout(timeoutRef.current);
    }

    timeoutRef.current = setTimeout(() => {
      timeoutRef.current = null;
      if (!immediate) {
        callback(...args);
      }
    }, delay);

    if (callNow) {
      callback(...args);
    }
  }, [callback, delay, immediate]);

  useEffect(() => {
    return () => {
      if (timeoutRef.current) {
        clearTimeout(timeoutRef.current);
      }
    };
  }, []);

  return debouncedFn;
}
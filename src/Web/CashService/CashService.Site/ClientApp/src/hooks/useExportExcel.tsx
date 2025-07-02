import { useCallback } from 'react';
import { axiosInstance } from '../services/axios';

export default function useExportExcel(apiUrl: string, deps: React.DependencyList = [apiUrl]) {
  const download = useCallback(async () => {
    const response = await axiosInstance.get(apiUrl, { responseType: 'blob' });

    const fileName = response.headers['content-disposition']
      .split('filename=')[1]
      .split(';')[0]
      .replace(/"/g, '');

    const blob = response.data;
    const url = window.URL.createObjectURL(blob);

    const a = document.createElement('a');
    a.href = url;
    a.download = fileName;
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
    setTimeout(() => window.URL.revokeObjectURL(url), 100);
  }, deps);

  return download;
}

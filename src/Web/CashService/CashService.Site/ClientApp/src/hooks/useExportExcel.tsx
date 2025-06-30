import { useCallback } from 'react';
import { api } from '../services/axios';

export function useExportExcel(
  apiUrl: string,
  formData = undefined,
  deps: React.DependencyList = [apiUrl]
) {
  const download = useCallback(async () => {
    const { data, error, response }: any = await api.post(apiUrl, formData, {
      responseType: 'blob',
    });

    if (error) {
      console.error('Error exporting excel:', error);
      return;
    }

    const fileName = response.headers['content-disposition']
      .split('filename=')[1]
      .split(';')[0]
      .replace(/"/g, '');
    const url = window.URL.createObjectURL(new Blob([data]));
    const link = document.createElement('a');
    link.href = url;
    link.setAttribute('download', fileName);
    document.body.appendChild(link);
    link.click();
    link.parentNode?.removeChild(link);
    window.URL.revokeObjectURL(url);
  }, deps);

  return download;
}

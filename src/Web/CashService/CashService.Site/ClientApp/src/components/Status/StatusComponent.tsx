import React, { useMemo } from 'react';
import { useTranslation } from 'react-i18next';
import { StatusComponentProps } from './types';
import { validateStatusType, safeTranslate, isValidText, getStatusClasses } from './utils';

interface StatusStyles {
  container: string;
  text: string;
}

export const StatusComponent: React.FC<StatusComponentProps> = ({
  type = 'completed',
  text,
  className = '',
}) => {
  const { t } = useTranslation('global');

  // Validate and get safe type
  const safeType = validateStatusType(type);

  const styles = useMemo((): StatusStyles => {
    const containerClasses = getStatusClasses(safeType);
    
    // Get translated text with fallback
    const getDefaultText = () => safeTranslate(t, safeType);
    
    return {
      container: containerClasses,
      text: text ?? getDefaultText()
    };
  }, [safeType, text, t]);

  // Handle empty or null text gracefully
  if (!isValidText(styles.text)) {
    console.warn('StatusComponent: Empty text provided, using default text');
    return null;
  }

  return (
    <div className={className} role="alert">
      <span className={styles.container}>
        {styles.text}
      </span>
    </div>
  );
}; 
import { useState } from 'react';
import { useTranslation } from 'react-i18next';

import useNewAccountStore from '@/store/newAccountStore';

import styles from './PTTemplateSelect.module.scss';

export default function PTTemplateSelect() {
  const { t } = useTranslation();
  const ptTemplates = useNewAccountStore((state) => state.ptTemplates);
  const selectedTemplate = useNewAccountStore((state) => state.selectedPTTemplate);
  const setIsSelectingTemplate = useNewAccountStore((state) => state.setIsSelectingPTTemplate);
  const handleSelectPTTemplate = useNewAccountStore((state) => state.handleSelectPTTemplate);
  const [isOpen, setIsOpen] = useState(false);

  const handleOptionClick = (templateName: string) => {
    handleSelectPTTemplate(templateName);
    setIsSelectingTemplate(true);
    setTimeout(() => {
      setIsSelectingTemplate(false);
    }, 1500);
    setIsOpen(false);
  };

  return (
    <div className={styles.dropdown}>
      <button type="button" className={styles['dropdown-toggle']} onClick={() => setIsOpen(!isOpen)}>
        {selectedTemplate === '' ? t('SelectTemplate') : selectedTemplate}
        <span className="caret" />
      </button>
      {isOpen && (
        <ul className={styles['dropdown-menu']}>
          <li>
            <button
              type="button"
              onClick={(e) => {
                e.preventDefault();
                handleOptionClick('');
              }}
            >
              {t('SelectTemplate')}
            </button>
          </li>
          {ptTemplates.map((template) => {
            return (
              <li key={template.id}>
                <button
                  type="button"
                  onClick={(e) => {
                    e.preventDefault();
                    handleOptionClick(template.name);
                  }}
                  disabled={!template.isValid}
                >
                  {template.name}
                </button>
              </li>
            );
          })}
        </ul>
      )}
    </div>
  );
}

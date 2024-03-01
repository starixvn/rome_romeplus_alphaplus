import { renderToStaticMarkup } from 'react-dom/server';
import { useTranslation } from 'react-i18next';

export default function MixSportParlayNote() {
  const { t } = useTranslation();

  const tooltipContent = () => (
    <>
      <div>
        <b>{`${t('MixSportParlay')}:`} </b>
        {t('MixSportParlayHint1')}
      </div>
      <div>{t('MixSportParlayHint2')}</div>
    </>
  );

  return (
    <span
      className="icon icon-information-outline"
      style={{ float: 'right', marginTop: '3px' }}
      data-tooltip-id="global-tooltip"
      data-tooltip-html={renderToStaticMarkup(tooltipContent())}
    />
  );
}

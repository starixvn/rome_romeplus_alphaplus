import StatusItem from '../../models/status-item';
import BettypeLevelStatus from './BettypeLevelStatus';
import ConfirmStatus from './ConfirmStatus';
import ProductLevelStatus from './ProductLevelStatus';
import styles from './Status.module.scss';

type Props = {
    statusItems: StatusItem[];
};

export default function Status({ statusItems }: Props) {
    return (
        <div className={styles['status-setting']}>
            {statusItems.map((status, index) => {
                if (status.ConfirmMessage) {
                    return <ConfirmStatus key={index} status={status} />;
                }

                return status.IsEditOnProductLevel ? (
                    <ProductLevelStatus status={status} />
                ) : (
                    <BettypeLevelStatus status={status} />
                );
            })}
        </div>
    );
}

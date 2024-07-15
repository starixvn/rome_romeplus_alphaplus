import { useGlobalContext } from '../../context/global-context';
import StatusItem from '../../models/status-item';
import NoInformation from '../common/NoInformation';
import BettypeLevelStatus from './BettypeLevelStatus';
import ConfirmStatus from './ConfirmStatus';
import ProductLevelStatus from './ProductLevelStatus';
import styles from './Status.module.scss';

type Props = {
    statusItems: StatusItem[] | undefined;
    getCustomerStatus: (requestId: string | undefined) => void;
};

export default function Status({ statusItems, getCustomerStatus }: Props) {
    const { mobileView } = useGlobalContext();

    if (!statusItems) {
        return mobileView ? <NoInformation /> : null;
    }

    return (
        <div className={styles['status-setting']}>
            {statusItems.map((status, index) => {
                if (status.ConfirmMessage) {
                    return <ConfirmStatus key={index} status={status} getCustomerStatus={getCustomerStatus} />;
                }

                return status.IsEditOnProductLevel ? (
                    <ProductLevelStatus status={status} getCustomerStatus={getCustomerStatus} />
                ) : (
                    <BettypeLevelStatus status={status} />
                );
            })}
        </div>
    );
}

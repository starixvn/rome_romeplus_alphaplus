import { UserLevel } from '../../constants';
import { useGlobalContext } from '../../context/global-context';
import TotalDownline from '../../models/total-downline';
import TotalDirectDownlines from './TotalDirectDownlines';
import TotalIndirectDownlines from './TotalIndirectDownlines';

export default function TotalDownlines({ totalDownlines }: { totalDownlines: TotalDownline[] }) {
    const { customer } = useGlobalContext();
    const totalIndirectDownlines = totalDownlines[1];
    const totalDirectDownlines = totalDownlines[0];
    return (
        <div>
            {customer?.RoleId === UserLevel.Master && totalIndirectDownlines && (
                <TotalIndirectDownlines {...totalIndirectDownlines} />
            )}
            {totalDirectDownlines && <TotalDirectDownlines {...totalDirectDownlines} />}
        </div>
    );
}

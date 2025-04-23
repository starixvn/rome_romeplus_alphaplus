import styles from './DownlineList.module.scss';

type Props = {
    isDirectMember?: boolean;
    statusId: number;
    title: string;
    checked: boolean;
    value: number;
    className: string;
    handleChange: (statusId: number) => void;
};

export default function RadioButtonStatus({
    isDirectMember = false,
    statusId,
    title,
    checked,
    className,
    value,
    handleChange
}: Props) {
    const id = `${title}-${statusId}-${isDirectMember}`;
    return (
        <div className={`radio ${styles['radio']}`}>
            <input id={id} type="radio" onChange={() => handleChange(statusId)} checked={checked} />
            <label className="label-text" htmlFor={id}>
                {title}
                <span className={className}>{`(${value})`}</span>
            </label>
        </div>
    );
}

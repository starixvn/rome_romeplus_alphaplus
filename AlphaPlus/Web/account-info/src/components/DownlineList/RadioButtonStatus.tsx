import styles from './DownlineList.module.scss';

type Props = {
    statusId: number;
    title: string;
    checked: boolean;
    value: number;
    className: string;
    handleChange: (statusId: number) => void;
};

export default function RadioButtonStatus({ statusId, title, checked, className, value, handleChange }: Props) {
    const id = `${title}-${statusId}`;
    return (
        <div className={styles['radio']}>
            <input id={id} type="radio" onChange={() => handleChange(statusId)} checked={checked} />
            <label className="label-text" htmlFor={id}>
                {title}
                <span className={className}>{`(${value})`}</span>
            </label>
        </div>
    );
}

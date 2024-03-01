import classNames from 'classnames';

import { IconCorrect, IconWrong } from '@/features/Shared/Icons';

import CharSelector from './CharSelect';
import styles from './Username.module.scss';

interface IProps {
  newUserName: string;
  uplineUserName: string;
  onChangeUserName: (newValue: string) => void;
  onBlur: () => void;
  error?: string;
}

export default function Username({ newUserName = '', uplineUserName = '', onChangeUserName, onBlur, error }: IProps) {
  const buildUserName = (newChar: string, index: number) => {
    const newValue = newUserName.split('');
    newValue[index] = newChar;
    onChangeUserName(`${uplineUserName}${newValue.join('')}`);
  };

  return (
    <div id="userName" className={classNames(styles.username, { [styles.error]: error })} onBlur={onBlur}>
      <div className={styles.name}>{uplineUserName.toUpperCase()}</div>
      {newUserName?.split('')?.map((value, index) => (
        <CharSelector
          key={value + index.toString()}
          className={classNames({ [styles.error]: error })}
          onSelect={(e) => buildUserName(e, index)}
          value={value}
        />
      ))}
      <span>{error ? IconWrong : IconCorrect}</span>
    </div>
  );
}

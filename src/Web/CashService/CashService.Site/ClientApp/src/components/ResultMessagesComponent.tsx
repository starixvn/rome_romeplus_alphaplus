import React from 'react';
import { IconCheck, IconError, IconWarning } from './Icons';

export type ResultType = 'error' | 'success' | 'warning';

interface ResultMessage {
  type?: ResultType;
  message: string;
}

interface ResultMessagesProps {
  messages: ResultMessage[];
  className?: string;
}

export const ResultMessagesComponent: React.FC<ResultMessagesProps> = ({
  messages,
  className = ''
}) => {
  if (!messages || messages.length === 0) {
    return null;
  }

  return (
    <div className={className}>
      {messages.map((msg, index) => (
        <MessageItem 
          key={index} 
          type={msg.type || 'error'} 
          message={msg.message} 
          className={index > 0 ? 'mt-2' : ''} 
        />
      ))}
    </div>
  );
};

interface MessageItemProps {
  type: ResultType;
  message: string;
  className?: string;
}

const MessageItem: React.FC<MessageItemProps> = ({ type, message, className = '' }) => {
  const getMessageStyles = () => {
    switch (type) {
      case 'error':
        return {
          bg: 'bg-[#FFF2F2]',
          border: 'border-[#FE0000]',
          icon: <IconError className="w-[20px] h-[20px] fill-[#D42020] absolute top-[7px] left-[10px] max-[768px]:top-[10px] max-[768px]:left-[12px]" />
        };
      case 'success':
        return {
          bg: 'bg-[#E2FFF2]',
          border: 'border-[#5CB774]',
          icon: <IconCheck className="w-[20px] h-[20px] fill-[#5CB774] absolute top-[7px] left-[10px] max-[768px]:top-[10px] max-[768px]:left-[12px]" />
        };
      case 'warning':
        return {
          bg: 'bg-[#F6F2D7]',
          border: 'border-[#FFDD9C]',
          icon: <IconWarning className="w-[20px] h-[20px] fill-[#7F3D1D] absolute top-[7px] left-[10px] max-[768px]:top-[10px] max-[768px]:left-[12px]" />
        };
    }
  };

  const styles = getMessageStyles();

  return message ? (
    <div className={`${className} max-[768px]:py-[12px]`} role="alert">
      <div className={`${styles.bg} ${styles.border} relative font-bold text-[12px] text-[#333] border-[1px] rounded-[10px] pl-[36px] pr-[12px] py-[8px] leading-[18px] block max-[768px]:text-[16px] max-[768px]:leading-[16px] max-[768px]:pl-[40px] max-[768px]:pr-[12px] max-[768px]:py-[12px]`}>
        {styles.icon}
        <div dangerouslySetInnerHTML={{ __html: message }}></div>
      </div>
    </div>
  ) : null;
};

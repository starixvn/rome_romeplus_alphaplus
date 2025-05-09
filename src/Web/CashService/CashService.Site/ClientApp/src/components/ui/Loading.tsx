import React from 'react';

interface LoadingProps {
  size?: 'sm' | 'md' | 'lg';
  className?: string;
}

export const Loading: React.FC<LoadingProps> = ({ 
  size = 'md', 
  className = '' 
}) => {
  const sizeClasses = {
    sm: 'scale-75',
    md: 'scale-100',
    lg: 'scale-125'
  };

  return (
    <div className='flex justify-center items-center h-[100px]'>
        <span className={`loader ${className} ${sizeClasses[size]}`}></span>    
    </div>
  );
};

export default Loading;

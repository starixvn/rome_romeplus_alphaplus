import React from 'react';

const sizeClasses = {
  sm: 'scale-75',
  md: 'scale-100',
  lg: 'scale-125'
};

interface LoadingProps {
  size?: 'sm' | 'md' | 'lg';
  className?: string;
}

export const LoadingComponent: React.FC<LoadingProps> = ({ 
  size = 'md', 
  className = '' 
}) => {
  return (
    <div className='flex justify-center items-center h-[100px]'>
        <span className={`loader ${className} ${sizeClasses[size]}`}></span>    
    </div>
  );
};

export default LoadingComponent;

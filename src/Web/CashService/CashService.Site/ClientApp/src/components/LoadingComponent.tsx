import React from 'react';

interface LoadingProps {
  className?: string;
}

export default function LoadingComponent() {
  return (
    <div className='flex justify-center items-center h-[100px]'>
        <span className="loader"></span>    
    </div>
  );
};


export const LoadingFullComponent: React.FC<LoadingProps> = () => {
  return (    
    <div className='flex justify-center items-center h-full'>
        <span className={`loader`}></span>    
    </div>
  );
};

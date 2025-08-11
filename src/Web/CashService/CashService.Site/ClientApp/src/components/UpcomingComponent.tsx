export default function UpcomingComponent({ hasTab = false }: { hasTab?: boolean }) {
  return (
    <div className={`${hasTab ? 'top-[81px]' : 'top-[40px]'} absolute z-[100] left-0 bottom-0 right-0`}>      
      <div className={`bg-[#00000080]  flex items-start justify-center h-full select-none`}> 
        <div className="p-[12px] bg-[var(--modal-background)] border-[1px] border-[var(--modal-border-color)] shadow-[var(--modal-box-shadow)] mt-[100px] w-[400px] max-[768px]:max-w-[90%] rounded-[3px] min-h-[100px] flex flex-col items-center justify-center" >
          <h3 className="w-[100%] font-semibold text-[16px] text-[#094980] dark:text-[#459be5] capitalize text-center">A new feature is coming soon — stay tuned!</h3>
        </div>
      </div>
    </div>
  );
};
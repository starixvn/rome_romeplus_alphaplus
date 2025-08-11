import { IconCash, IconCash2, IconCash3, IconOClock } from "./Icons";

export default function UpcomingComponent({ hasTab = false }: { hasTab?: boolean }) {
  return (
    <div className={`${hasTab ? 'top-[81px]' : 'top-[40px]'} absolute z-[100] left-0 bottom-0 right-0`}>      
      <div className={` flex items-start justify-center h-full select-none`}> 
        <div className="p-[12px] upcoming-bg w-full rounded-[3px] h-full flex flex-col items-center justify-center relative" >
          <h3 className="w-[100%] font-semibold text-[20px] text-[#459be5] capitalize text-center"><span className="block text-[150%]">Get ready!<br />Coming Soon!</span></h3>
          <IconCash className="animate-rotate block w-[30px] h-[30px] absolute top-[30px] left-[20px] fill-[#FDA578] dark:fill-[#FDA578]!"/>
          <IconCash2 className="animate-flip block w-[30px] h-[30px] absolute bottom-[30px] right-[40px] fill-[#FFE845] dark:fill-[#FFE845]!"/>
          <IconCash3 className="animate-rotate block w-[40px] h-[40px] absolute top-[10px] right-[11px] fill-[#46FDBF]! dark:fill-[#46FDBF]!"/>
          <IconOClock className="animate-flip block w-[30px] h-[30px] absolute bottom-[30px] left-[50px] fill-[#E7D8FB] dark:fill-[#E7D8FB]!"/>      
          <span className="rotate-49 block w-[30px] h-[30px] absolute top-[30px] left-[200px] after:content-[''] after:w-[8px] after:h-full after:bg-[#FCD9DB] after:absolute after:left-[11px]"></span>
          <span className="block absolute text-[60px] text-[#FC5746] font-bold top-[20px] right-[200px]">*</span>
        </div>
      </div>
    </div>
  );
};
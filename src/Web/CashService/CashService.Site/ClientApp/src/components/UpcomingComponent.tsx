import { IconCash, IconCash2, IconCash3, IconOClock } from "./Icons";

 

export default function UpcomingComponent({ hasTab = false }: { hasTab?: boolean }) {
  return (
    <div className={`${hasTab ? 'top-[81px]' : 'top-[40px]'} absolute z-[100] left-0 bottom-0 right-0 overflow-hidden`}>      
      <div className={`bg-[#00000080] flex items-start justify-center h-full select-none`}> 
        <div className="p-[12px] bg-[rgba(0,0,76,1)] bg-[linear-gradient(158deg,rgba(0,0,76,1)_0%,rgba(43,62,105,1)_50%,rgba(0,0,6,1)_100%)] w-full rounded-[3px] h-full flex items-center justify-center relative" >
          <h3 className="font-bold text-[40px] text-[#fff] capitalize text-center relative">
            <span className="block text-[150%]">Get ready!<br />Coming Soon!</span>
            <IconCash className="animate-rotate w-[30px] h-[30px] absolute top-[-100px] left-[47%] fill-[#FDA578] dark:fill-[#FDA578]!"/>
            <IconCash className="animate-rotate w-[30px] h-[30px] absolute top-[-10px] left-[-230px] fill-[#FFD580]! dark:fill-[#FFD580]!"/>
            <IconCash className="animate-rotate w-[30px] h-[30px] absolute bottom-[-10px] right-[-180px] fill-[#B5EAD7]! dark:fill-[#B5EAD7]!"/>
            <IconCash className="animate-rotate w-[30px] h-[30px] absolute bottom-[-120px] left-[70px] fill-[#FFDAC1]! dark:fill-[#FFDAC1]!"/>

            <IconCash2 className="animate-flip w-[30px] h-[30px] absolute bottom-[-30px] left-[-130px] fill-[#4169E1] dark:fill-[#4169E1]!"/>
            <IconCash2 className="animate-flip w-[30px] h-[30px] absolute top-[-170px] left-[-40px] fill-[#FF4F81]! dark:fill-[#FF4F81]!"/>
            <IconCash2 className="animate-flip w-[30px] h-[30px] absolute bottom-[-110px] right-[10px] fill-[#00E5FF]! dark:fill-[#00E5FF]!"/>
            <IconCash2 className="animate-flip w-[30px] h-[30px] absolute top-[-110px] right-[-110px] fill-[#FFDD00]! dark:fill-[#FFDD00]!"/>


            <IconCash3 className="animate-rotate block w-[40px] h-[40px] absolute top-[0] right-[-100%] fill-[#46FDBF]! dark:fill-[#46FDBF]!"/>
            <IconCash3 className="animate-rotate block w-[40px] h-[40px] absolute top-[-220px] left-[-280px] fill-[#F50057]! dark:fill-[#F50057]!"/>
            <IconCash3 className="animate-rotate block w-[40px] h-[40px] absolute top-[-270px] right-[50px] fill-[#FF7E5F]! dark:fill-[#FF7E5F]!"/>
            <IconCash3 className="animate-rotate block w-[40px] h-[40px] absolute bottom-[-230px] left-[-230px] fill-[#45B649]! dark:fill-[#45B649]!"/>
            <IconCash3 className="animate-rotate block w-[40px] h-[40px] absolute bottom-[-230px] right-[-230px] fill-[#A8E6CF]! dark:fill-[#A8E6CF]!"/>


            <IconOClock className="animate-flip block w-[30px] h-[30px] absolute bottom-[-50px] left-[-430px] fill-[#E7D8FB] dark:fill-[#E7D8FB]!"/>
            <IconOClock className="animate-flip block w-[30px] h-[30px] absolute top-[-240px] right-[-300px] fill-[#9D4EDD]! dark:fill-[#9D4EDD]!"/>
            <IconOClock className="animate-flip block w-[30px] h-[30px] absolute bottom-[-110px] right-[-460px] fill-[#C0FFEE] dark:fill-[#C0FFEE]!"/>
            <IconOClock className="animate-flip block w-[30px] h-[30px] absolute bottom-[-290px] left-[220px] fill-[#E7D8FB] dark:fill-[#E7D8FB]!"/>
            <span className="animate-rotate block w-[30px] h-[30px] absolute top-[-140px] left-[-530px] after:content-[''] after:w-[8px] after:h-full after:bg-[#FFE845] after:absolute after:left-[11px] before:content-[''] before:w-full before:h-[8px] before:bg-[#FFE845] before:absolute before:top-[11px] before:left-[0] "></span>
            <span className="animate-rotate block w-[30px] h-[30px] absolute bottom-[-280px] left-[-580px] after:content-[''] after:w-[8px] after:h-full after:bg-[#FF9B48] after:absolute after:left-[11px]"></span>
            </h3>
          
          {/* <IconCash className="block w-[30px] h-[30px] absolute top-[30px] left-[920px] fill-[#4169E1] dark:fill-[#4169E1]!"/>
          <IconCash className="block w-[30px] h-[30px] absolute top-[630px] left-[150px] fill-[#DC143C] dark:fill-[#DC143C]!"/>
          <IconCash className="block w-[30px] h-[30px] absolute top-[630px] left-[750px] fill-[#2E8B57]! dark:fill-[#2E8B57]!"/>
          <IconCash className="block w-[30px] h-[30px] absolute top-[230px] left-[350px] fill-[#FF8C00]! dark:fill-[#FF8C00]!"/>
          <IconCash2 className="block w-[30px] h-[30px] absolute top-[700px] left-[140px] fill-[#FFE845] dark:fill-[#FFE845]!"/>
          <IconCash2 className="block w-[30px] h-[30px] absolute bottom-[30px] right-[40px] fill-[#C71585] dark:fill-[#C71585]!"/>
          <IconCash2 className="animate-flip block w-[30px] h-[30px] absolute bottom-[30px] right-[40px] fill-[#FFE845] dark:fill-[#FFE845]!"/>
          <IconCash3 className="animate-rotate block w-[40px] h-[40px] absolute top-[10px] right-[11px] fill-[#46FDBF]! dark:fill-[#46FDBF]!"/>
          <IconOClock className="animate-flip block w-[30px] h-[30px] absolute bottom-[30px] left-[50px] fill-[#E7D8FB] dark:fill-[#E7D8FB]!"/>      
          <span className="rotate-49 block w-[30px] h-[30px] absolute top-[30px] left-[200px] after:content-[''] after:w-[8px] after:h-full after:bg-[#FCD9DB] after:absolute after:left-[11px]"></span>
          <span className="block absolute text-[60px] text-[#FC5746] font-bold top-[20px] right-[200px]">*</span> */}
        </div>
      </div>
    </div>
  );
};
import '../../styles/common.css';
import '../../styles/global.scss';

import { StrictMode } from 'react';
import { createRoot } from "react-dom/client";
import './TopBanner.scss';
import { axiosInstance, createHeaderXMLHttp } from '../../services/axios';

function TopBannerComponent() {
    const handleActivate = async () => {
        const response = await axiosInstance.post(`/main/Dashboard/UpdateCashServiceStatus?IsEnableCash=true`, {}, { headers: createHeaderXMLHttp(), });
        if (response.data.isSuccessful) {
          window.location.reload();
        }
    };


    const handleFindOutMore = () => {
        window.postMessage({
            type: 'openCashServicePopup'
        }, '*');
        window.postMessage({
            type: 'triggerFindOutMore'
        }, '*');
    };

    return (
        <div className="cash-service-banner">
            <span className="exclusively">        
                <img src="assets/images/lightning.png" />
                <span>EXCLUSIVELY FOR YOU!</span>
            </span>
            <div className="cash-service-banner-content">
                <span className="message">Enjoy Cash Service for free! Until 31st Mar 2025</span>
                <button 
                    className="banner-btn"
                    onClick={handleActivate}
                >
                    Activate now
                </button>
                <button 
                    className="banner-btn secondary"
                    onClick={handleFindOutMore}
                >
                    Find out more
                </button>
            </div>
        </div>
    );
}
export default TopBannerComponent;
const rootElement = document.getElementById('top-banner-root');
if (rootElement) {
    createRoot(rootElement).render(
        <StrictMode>
          <TopBannerComponent />
        </StrictMode>
    );
}
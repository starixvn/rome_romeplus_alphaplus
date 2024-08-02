import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import Modal from 'react-modal';

import SettingsService, { handleError } from '../../../services/settings-service';
import { ProfilePictureModel } from '../type';
import styles from './PictureLibraryModal.module.scss';

const customStyles = {
    content: window.top?.alpha.isScreenLower768
        ? {
              top: '0',
              left: '0',
              right: '0',
              bottom: '0',
              border: 'none',
              padding: '0',
              overflow: 'hidden'
          }
        : {
              top: '50%',
              left: '50%',
              right: 'auto',
              bottom: 'auto',
              padding: '0',
              marginRight: '-50%',
              transform: 'translate(-50%, -50%)',
              width: '600px',
              height: '710px'
          }
};
type Props = {
    isOpen: boolean;
    closeModal: () => void;
    setCroppedImage: (image?: ProfilePictureModel) => void;
};

export default function PictureLibraryModal({ isOpen, closeModal, setCroppedImage }: Props) {
    const { t } = useTranslation();
    const [isLoading, setIsLoading] = useState(false);
    const [selectedImage, setSelectedImage] = useState<ProfilePictureModel | undefined>(undefined);
    const [pictureLibrary, setPictureLibrary] = useState<ProfilePictureModel[]>([]);
    const [currentPage, setCurrentPage] = useState(1);
    let tapped: boolean | null = false;

    useEffect(() => {
        const getPictureLibrary = () => {
            SettingsService.GetPictureLibrary(currentPage)
                .then((data) => {
                    if (data && data.Images && data.Images.length > 0) {
                        const images = [...pictureLibrary, ...data.Images];
                        setPictureLibrary([...images]);
                        setSelectedImage(images[0]);
                        setIsLoading(true);

                        if (currentPage < data.Pager.TotalPages) {
                            setCurrentPage(currentPage + 1);
                        } else {
                            setIsLoading(false);
                            localStorage.setItem('profilePictureLibrary', JSON.stringify(images));
                        }
                    }
                })
                .catch(handleError);
        };
        const profilePictureLibrary = localStorage.getItem('profilePictureLibrary');
        if (profilePictureLibrary) {
            const images = JSON.parse(profilePictureLibrary);
            setPictureLibrary(images);
            setSelectedImage(images[0]);
        } else {
            getPictureLibrary();
        }
    }, [currentPage]);

    const handleOnSelect = () => {
        setCroppedImage(selectedImage);
        closeModal();
    };

    const handleOnClick = (image: ProfilePictureModel) => {
        let timeOut: number = 0;
        if (!tapped) {
            setSelectedImage(image);
            timeOut = setTimeout(function () {
                tapped = null;
            }, 300);
        } else {
            clearTimeout(timeOut);
            tapped = null;
            handleOnSelect();
        }
    };

    return (
        <Modal isOpen={isOpen} onRequestClose={closeModal} style={customStyles} contentLabel="" ariaHideApp={false}>
            <div className={`${styles['picture-library']} select-modal`}>
                <div className="header">
                    <span className="title">{t('PictureLibrary')}</span>
                    <span className={`icon-close`} onClick={closeModal}></span>
                </div>
                <div className="btn-group">
                    <button className="btn-select" onClick={handleOnSelect}>
                        {t('Select')}
                    </button>
                    <button className={`btn-cancel `} onClick={closeModal}>
                        {t('Cancel')}
                    </button>
                </div>
                <div className={styles['library']}>
                    {pictureLibrary.length > 0 &&
                        selectedImage !== undefined &&
                        pictureLibrary.map((profilePicture, index) => (
                            <img
                                className={`${styles['library-image-item']} ${
                                    selectedImage?.ImageId === profilePicture.ImageId ? styles['selected-image'] : ''
                                }`}
                                alt="Crop"
                                src={profilePicture.Image}
                                onClick={() => handleOnClick(profilePicture)}
                                onTouchStart={() => handleOnClick(profilePicture)}
                                key={index}
                            />
                        ))}
                    {isLoading && <img className="blockMsg library-image-item" />}
                </div>
            </div>
        </Modal>
    );
}

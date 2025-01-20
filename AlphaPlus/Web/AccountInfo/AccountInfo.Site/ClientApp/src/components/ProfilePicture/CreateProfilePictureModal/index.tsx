import { useEffect, useState, useRef } from 'react';
import { useTranslation } from 'react-i18next';
import ReactCrop, { Crop } from 'react-image-crop';
import Modal from 'react-modal';

import { ProfilePictureModel } from '../type';

const isDesktop = !window.top?.alpha.isScreenLower768,
    totalHeightOfTitleAndButtonOnPopup = 118,
    profilePictureWidthDefault = 240,
    popupStyleMobile = {
        top: '0',
        left: '0',
        right: '0',
        bottom: '0',
        border: 'none',
        padding: '0',
        overflow: 'hidden'
    },
    popupStyleDesktop = {
        top: '50%',
        left: '50%',
        right: 'auto',
        bottom: 'auto',
        padding: '10',
        marginRight: '-50%',
        transform: 'translate(-50%, -50%)',
        width: '600px',
        height: '710px',
        overflow: 'hidden'
    },
    cropDefault = {
        x: 0,
        y: 0,
        aspect: 1,
        width: 240,
        height: 240
    },
    defaultPopupStyles = { content: isDesktop ? popupStyleDesktop : popupStyleMobile };

type Props = {
    modalIsOpen: boolean;
    closeModal: () => void;
    imageSrc?: string;
    resetMessageContainer: () => void;
    setCroppedImage: (image?: ProfilePictureModel) => void;
};

export default function CreateProfilePictureModal({
    modalIsOpen,
    closeModal,
    imageSrc,
    resetMessageContainer,
    setCroppedImage
}: Props) {
    const { t } = useTranslation();
    const croppedImageId = -1;
    const [croppedImageUrl, setCroppedImageUrl] = useState<string>('');
    const [crop, setCrop] = useState<Crop>(cropDefault);
    const [imageWrapperStyle, setImageWrapperStyle] = useState({});
    const [popupStyles, setPopupStyles] = useState(defaultPopupStyles);
    const imgRef = useRef<HTMLImageElement>(null);

    useEffect(() => {
        setCrop(cropDefault);
        setImageWrapperStyle({});
        setPopupStyles(defaultPopupStyles);
    }, [imageSrc]);

    const handleOnSelect = () => {
        setCroppedImage({
            Image: croppedImageUrl,
            ImageId: croppedImageId
        });

        closeModal();
        resetMessageContainer();
    };

    const makeClientCrop = (crop: any) => {
        if (imgRef.current && crop.width && crop.height) {
            const croppedImageUrl = getCroppedImg(imgRef.current, crop);
            setCroppedImageUrl(croppedImageUrl);
        }
    };

    const getCroppedImg = (image: any, crop: any) => {
        const canvas = document.createElement('canvas');
        const scaleX = image.naturalWidth / image.width;
        const scaleY = image.naturalHeight / image.height;
        canvas.width = crop.width;
        canvas.height = crop.height;
        const ctx = canvas.getContext('2d');

        ctx?.drawImage(
            image,
            crop.x * scaleX,
            crop.y * scaleY,
            crop.width * scaleX,
            crop.height * scaleY,
            0,
            0,
            crop.width,
            crop.height
        );

        return canvas.toDataURL();
    };

    const getContainedSize = (img: any) => {
        const ratio = img.naturalWidth / img.naturalHeight;

        let height: number = Math.min(...[img.height, img.naturalHeight]);
        let width: number = height * ratio;

        const minWidth = Math.min(...[img.width, img.naturalWidth]);

        if (width > minWidth) {
            width = minWidth;
            height = minWidth / ratio;
        }

        return {
            width: Math.max(...[width, profilePictureWidthDefault]),
            height: Math.max(...[height, profilePictureWidthDefault])
        };
    };

    const onImageLoaded = (image: any) => {
        const imgDimension = getContainedSize(image);

        if (isDesktop && imgDimension.width > imgDimension.height) {
            const customStyles = { ...{ content: popupStyleDesktop } };
            customStyles.content.height = `${imgDimension.height + totalHeightOfTitleAndButtonOnPopup}px`;
            setPopupStyles(customStyles);
        }

        setImageWrapperStyle({
            width: imgDimension.width,
            height: imgDimension.height
        });
    };

    const onCropComplete = (crop: any) => {
        makeClientCrop(crop);
    };

    const onCropChange = (crop: any) => {
        setCrop(crop);
    };

    return (
        <Modal isOpen={modalIsOpen} onRequestClose={closeModal} style={popupStyles} contentLabel="" ariaHideApp={false}>
            {imageSrc && (
                <div className={`crop-image select-modal`}>
                    <div className="header">
                        <span className="title">{t('UploadFromYourDevice')}</span>
                        <span className="icon-close" onClick={closeModal}></span>
                    </div>
                    <div className="btn-group">
                        <button className="btn-select" onClick={handleOnSelect}>
                            {t('Select')}
                        </button>
                        <button className="btn-cancel" onClick={closeModal}>
                            {t('Cancel')}
                        </button>
                    </div>
                    <ReactCrop
                        minWidth={profilePictureWidthDefault}
                        src={imageSrc}
                        crop={crop}
                        onImageLoaded={onImageLoaded}
                        style={imageWrapperStyle}
                        onComplete={onCropComplete}
                        onChange={onCropChange}
                    />
                    <img ref={imgRef} src={imageSrc} style={{ display: 'none' }} onLoad={onImageLoaded} />
                </div>
            )}
        </Modal>
    );
}

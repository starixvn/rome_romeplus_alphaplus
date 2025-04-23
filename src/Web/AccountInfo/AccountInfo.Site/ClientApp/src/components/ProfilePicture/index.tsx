import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';

import Result from '../../models/result';
import { handleError } from '../../services/axios-helper';
import SettingsService from '../../services/settings-service';
import CreateProfilePictureModal from './CreateProfilePictureModal';
import DisplayLanguage from './DisplayLanguage';
import PictureLibraryModal from './PictureLibraryModal';
import styles from './ProfilePicture.module.scss';
import { ProfilePictureModel } from './type';

export default function ProfilePicture() {
    const { allowUpdate, imageList } = profilePictureParams;
    const [openPanel, setOpenPanel] = useState(true);
    const { t } = useTranslation();
    const [originalAvatar, setOriginalAvatar] = useState<ProfilePictureModel | undefined>(undefined);
    const [croppedImage, setCroppedImage] = useState<ProfilePictureModel | undefined>(undefined);
    const [message, setMessage] = useState<React.ReactElement>();
    const [src, setSrc] = useState<string | undefined>(undefined);
    const [srcWidth, setSrcWidth] = useState(0);
    const [srcHeight, setSrcHeight] = useState(0);
    const [uploadFromDeviceModalIsOpen, setUploadFromDeviceModalIsOpen] = useState(false);
    const [pictureLibraryModalIsOpen, setPictureLibraryModalIsOpen] = useState(false);

    const defaultImageId = -100;
    const croppedImageId = -1;
    const [images, setImages] = useState([...imageList].slice(1, 5));

    const disabledButton = !allowUpdate || images.length === 0;
    const disabled = disabledButton ? { disabled: true } : {};

    useEffect(() => {
        const actorProfilePicture = JSON.parse(localStorage.getItem('actorProfilePicture') ?? '');
        setOriginalAvatar(actorProfilePicture);
        setCroppedImage(actorProfilePicture);
    }, []);

    const handleOnImageClick = (image?: ProfilePictureModel) => {
        setCroppedImage(image);
    };

    const showMessage = (data: { IsSuccess: boolean; Message: string }) => {
        const message = (
            <div className="msg-container">
                <div className={`msg ${data.IsSuccess ? 'msg-success' : 'msg msg-error'}`}>
                    {data.IsSuccess ? t('ProfilePictureUpdatedSuccessfully') : data.Message.trim()}
                </div>
            </div>
        );

        setMessage(message);
    };

    const handleSelectFile = (e: React.ChangeEvent<HTMLInputElement>) => {
        if (e.target.files && e.target.files.length > 0) {
            const file = e.target.files[0],
                fileType = file['type'];
            if (fileType.search('image') >= 0) {
                const reader = new FileReader();
                reader.addEventListener('load', () => {
                    const img = new Image();
                    const { naturalWidth, naturalHeight } = img;
                    img.addEventListener('load', () => {
                        setSrc(reader.result?.toString() ?? '');
                        setUploadFromDeviceModalIsOpen(true);
                        setSrcWidth(naturalWidth);
                        setSrcHeight(naturalHeight);
                    });

                    img.src = reader.result?.toString() ?? '';
                });
                reader.readAsDataURL(file);
            } else {
                showMessage({
                    IsSuccess: false,
                    Message: t('FailByFileType')
                });
            }

            $(e.target).val('');
        }
    };

    const handleViewLibrary = () => {
        setPictureLibraryModalIsOpen(true);
    };

    const handleActionsAfterUpdateSuccessfully = () => {
        window.top?.headerNav.setProfilePictureOnNav(croppedImage?.Image);
        setImages(imageList.filter((item) => item.ImageId != croppedImage?.ImageId).slice(1, 5));
        setOriginalAvatar(croppedImage);
    };

    const resetMessageContainer = () => {
        setMessage(undefined);
    };

    const handleActionsAfterUpdate = (data: Result) => {
        showMessage({ IsSuccess: data.IsSuccessful, Message: data.Message });

        if (data.IsSuccessful) {
            handleActionsAfterUpdateSuccessfully();

            setTimeout(() => {
                resetMessageContainer();
            }, 1500);
        }

        $.unblockUI?.();
    };

    const handleOnSubmit = () => {
        if (croppedImage && croppedImage.ImageId > 0 && croppedImage.ImageId === originalAvatar?.ImageId) {
            showMessage({
                IsSuccess: false,
                Message: t('ThereIsNoChange')
            });
        } else {
            $.blockUI?.();
            if (croppedImage && croppedImage.ImageId !== defaultImageId) {
                SettingsService.UpdateProfilePicture(
                    croppedImage.ImageId,
                    croppedImage.ImageId === croppedImageId ? croppedImage.Image.split(';base64,')[1] : null
                )
                    .then((data) => handleActionsAfterUpdate(data))
                    .catch(handleError);
            }
        }
    };

    const handleOnReset = () => {
        setCroppedImage(originalAvatar);
    };

    const closeModal = () => {
        setUploadFromDeviceModalIsOpen(false);
        setPictureLibraryModalIsOpen(false);
    };

    return (
        <>
            <DisplayLanguage />
            <div className="panel">
                <div
                    className={openPanel ? 'panel-heading open' : 'panel-heading'}
                    onClick={() => setOpenPanel(!openPanel)}
                >
                    <h4 className="panel-title">{t('ProfilePicture')}</h4>
                </div>
                <div className="panel-content">
                    <div className={styles['profile-picture']}>
                        {!allowUpdate && (
                            <div className="msg-container">
                                <div className="msg msg-error">{t('NotAuthorized')}</div>
                            </div>
                        )}
                        <div className={styles['crop-img-info']}>
                            <img alt="Crop" src={croppedImage?.Image} className={styles['cropped-image']} />
                            <div className={styles['wrapper-right']}>
                                <label htmlFor="files" {...disabled} className={styles['btn-from-device']}>
                                    {t('UploadFromYourDevice')}
                                </label>
                                <input
                                    id="files"
                                    {...disabled}
                                    accept="image/png, image/jpeg, image/gif"
                                    type="file"
                                    onChange={handleSelectFile}
                                />
                            </div>
                        </div>
                        <div className={styles['choose-picture']}>
                            <div className={styles['extend-section']}>
                                <span className={styles['txt']}>{t('OrChooseFromOurGallery')}</span>
                            </div>
                            <div className={styles['list-picture']}>
                                <img
                                    className={`${styles['image-item']} ${
                                        croppedImage?.ImageId === originalAvatar?.ImageId
                                            ? styles['selected-image']
                                            : ''
                                    }`}
                                    alt="Crop"
                                    onClick={() => handleOnImageClick(originalAvatar)}
                                    onTouchStart={() => handleOnImageClick(originalAvatar)}
                                    src={originalAvatar?.Image}
                                />
                                {images.map((profilePicture, index) => (
                                    <img
                                        className={`${styles['image-item']} ${
                                            croppedImage?.ImageId === profilePicture.ImageId
                                                ? styles['selected-image']
                                                : ''
                                        }`}
                                        alt="Crop"
                                        onClick={() => handleOnImageClick(profilePicture)}
                                        onTouchStart={() => handleOnImageClick(profilePicture)}
                                        src={profilePicture.Image}
                                        key={index}
                                    />
                                ))}
                            </div>
                            {disabledButton ? (
                                t('ViewAll')
                            ) : (
                                <a
                                    className={styles['view-all']}
                                    onClick={handleViewLibrary}
                                    onTouchStart={handleViewLibrary}
                                >
                                    {t('ViewAll')}
                                </a>
                            )}
                        </div>

                        {message}

                        <div className="btn-group">
                            <button
                                className="btn-submit"
                                {...disabled}
                                onClick={handleOnSubmit}
                                onTouchStart={handleOnSubmit}
                            >
                                {t('Submit')}
                            </button>
                            <button
                                className="btn-reset"
                                {...disabled}
                                onClick={handleOnReset}
                                onTouchStart={handleOnReset}
                            >
                                {t('Reset')}
                            </button>
                        </div>

                        <CreateProfilePictureModal
                            modalIsOpen={uploadFromDeviceModalIsOpen}
                            imageSrc={src}
                            setCroppedImage={setCroppedImage}
                            closeModal={closeModal}
                            resetMessageContainer={resetMessageContainer}
                        />

                        <PictureLibraryModal
                            isOpen={pictureLibraryModalIsOpen}
                            setCroppedImage={setCroppedImage}
                            closeModal={closeModal}
                        />
                    </div>
                </div>
            </div>
        </>
    );
}

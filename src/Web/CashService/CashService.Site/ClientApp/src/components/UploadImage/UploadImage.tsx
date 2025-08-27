import React, { useState, useEffect, useCallback } from 'react';
import { useTranslation } from 'react-i18next';
import { IconImage } from '../Icons';

interface UploadImageProps {
  setImage: (image: File | null) => void;
  prevImage?: string | null;
  disabled?: boolean;
  customClassName?: string;
  maxSize?: number;
  removeImage?: () => void;
  image?: File | string | null;
  inputId?: string;
  acceptFileTypes?: string;
}

const ACCEPTED_FILE_TYPES = '.jpeg,.jpg,.png';

const UploadImage: React.FC<UploadImageProps> = ({
  setImage,
  prevImage,
  disabled = false,
  customClassName,
  maxSize = 1,
  removeImage,
  image,
  inputId,
  acceptFileTypes = ACCEPTED_FILE_TYPES,
}) => {
  const { t: tGlobal } = useTranslation('global');

  const [imageUrl, setImageUrl] = useState<string | null>(null);
  const [isDragging, setIsDragging] = useState(false);

  useEffect(() => {
    if (imageUrl && imageUrl.startsWith('blob:')) {
      URL.revokeObjectURL(imageUrl);
    }

    if (image instanceof File) {
      const nextUrl = URL.createObjectURL(image);
      setImageUrl(nextUrl);
      return () => {
        URL.revokeObjectURL(nextUrl);
      };
    }

    if (typeof image === 'string' && image) {
      setImageUrl(image);
      return;
    }

    if (prevImage) {
      setImageUrl(prevImage);
      return;
    }

    setImageUrl(null);
  }, [image, prevImage]);

  const processFile = useCallback(
    async (file: File) => {
      setImageUrl((prev) => {
        if (prev && prev.startsWith('blob:')) {
          URL.revokeObjectURL(prev);
        }
        return URL.createObjectURL(file);
      });
      setImage(file);
    },
    [setImage]
  );

  const handleImageUpload = useCallback(
    async (e: React.ChangeEvent<HTMLInputElement>) => {
      if (e.target.files?.[0]) {
        await processFile(e.target.files[0]);
      }
    },
    [processFile]
  );

  const handleDrop = useCallback(
    async (e: React.DragEvent<HTMLDivElement>) => {
      e.preventDefault();
      e.stopPropagation();
      setIsDragging(false);

      if (e.dataTransfer.files?.[0]) {
        await processFile(e.dataTransfer.files[0]);
      }
    },
    [processFile]
  );

  const handleDragEvents = useCallback(
    (e: React.DragEvent<HTMLDivElement>, isDragging: boolean) => {
      e.preventDefault();
      e.stopPropagation();
      setIsDragging(isDragging);
    },
    []
  );

  const resetImageUpload = useCallback(() => {
    if (imageUrl) {
      URL.revokeObjectURL(imageUrl);
    }
    setImageUrl(null);
    setImage(null);
    if (removeImage) {
      removeImage();
    }
    const inputElementId = inputId || 'image-upload';
    const fileInput = document.getElementById(inputElementId) as HTMLInputElement;
    if (fileInput) {
      fileInput.value = '';
    }
  }, [imageUrl, setImage, removeImage, inputId]);

  useEffect(() => {
    return () => {
      if (imageUrl) {
        URL.revokeObjectURL(imageUrl);
      }
    };
  }, [imageUrl]);

  const renderImagePreview = () => (
    <div className="absolute inset-0 flex flex-col items-center">
      <img src={imageUrl ? imageUrl : ''} alt="Preview" className="w-full h-full object-contain" />
      {!disabled && (
        <button
          type="button"
          onClick={resetImageUpload}
          className="absolute top-2 right-2 bg-white rounded-full p-1 shadow hover:bg-gray-200 z-20 cursor-pointer"
        >
          <svg
            xmlns="http://www.w3.org/2000/svg"
            className="h-5 w-5 text-gray-600"
            fill="none"
            viewBox="0 0 24 24"
            stroke="currentColor"
          >
            <path
              strokeLinecap="round"
              strokeLinejoin="round"
              strokeWidth={2}
              d="M6 18L18 6M6 6l12 12"
            />
          </svg>
        </button>
      )}
    </div>
  );

  const renderUploadPlaceholder = () => (
    <div className="flex flex-col items-center justify-center text-center p-6 text-[var(--text-color)] ">
      <IconImage />
      <p className="mb-[10px]">
        <span className="text-[var(--text-link-color)] cursor-pointer underline">
          {tGlobal('uploadImage')}
        </span>
        <br />
        <span className="text-[110%]">{tGlobal('dragAndDrop')}</span>
      </p>
      <span>
        {tGlobal('imageFormat')} {acceptFileTypes.replace('.', '')}
      </span>
      <span className="font-[500]">({tGlobal('maxSize').replace('{0}', maxSize.toString())})</span>
    </div>
  );

  return (
    <div
      className={`${customClassName ? customClassName : 'w-[350px]'}  max-[768px]:w-[250px] max-[768px]:h-[200px] border-[2px] border-dashed border-[#0b599c] dark:border-[#459be5] max-[768px]:mx-auto a ${
        isDragging ? ' bg-[#D3DFEC] dark:bg-[#303740]' : 'border-[#0b599c] dark:border-[#459be5]'
      } rounded-[5px] flex flex-col items-center justify-center relative`}
      onDrop={disabled ? undefined : handleDrop}
      onDragOver={disabled ? undefined : (e) => handleDragEvents(e, true)}
      onDragEnter={disabled ? undefined : (e) => handleDragEvents(e, true)}
      onDragLeave={disabled ? undefined : (e) => handleDragEvents(e, false)}
    >
      {imageUrl ? renderImagePreview() : renderUploadPlaceholder()}
      <input
        id={inputId || 'image-upload'}
        type="file"
        accept={acceptFileTypes}
        onChange={handleImageUpload}
        className="hidden"
        disabled={disabled}
      />
      <label
        htmlFor={inputId || 'image-upload'}
        className={`absolute inset-0 w-full h-full ${
          imageUrl ? 'opacity-0 z-0' : 'opacity-0 cursor-pointer z-10'
        }`}
      />
    </div>
  );
};

export default UploadImage;

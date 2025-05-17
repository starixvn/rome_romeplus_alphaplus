import React, { useState, useEffect, useCallback } from 'react';

interface UploadImageProps {
  setImage: (image: File | null) => void;
}

const ACCEPTED_FILE_TYPES = '.jpeg,.jpg,.png';

const UploadImage: React.FC<UploadImageProps> = ({ setImage }) => {
  const [imageUrl, setImageUrl] = useState<string | null>(null);
  const [isDragging, setIsDragging] = useState(false);

  const convertToBase64 = useCallback((file: File): Promise<string> => {
    return new Promise((resolve, reject) => {
      const reader = new FileReader();
      reader.readAsDataURL(file);
      reader.onload = () => resolve(reader.result as string);
      reader.onerror = (error) => reject(error);
    });
  }, []);

  const processFile = useCallback(async (file: File) => {
    setImageUrl(URL.createObjectURL(file));
    setImage(file);
  }, [convertToBase64, setImage]);

  const handleImageUpload = useCallback(async (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files?.[0]) {
      await processFile(e.target.files[0]);
    }
  }, [processFile]);

  const handleDrop = useCallback(async (e: React.DragEvent<HTMLDivElement>) => {
    e.preventDefault();
    e.stopPropagation();
    setIsDragging(false);

    if (e.dataTransfer.files?.[0]) {
      await processFile(e.dataTransfer.files[0]);
    }
  }, [processFile]);

  const handleDragEvents = useCallback((e: React.DragEvent<HTMLDivElement>, isDragging: boolean) => {
    e.preventDefault();
    e.stopPropagation();
    setIsDragging(isDragging);
  }, []);

  const resetImageUpload = useCallback(() => {
    if (imageUrl) {
      URL.revokeObjectURL(imageUrl);
    }
    setImageUrl(null);
    setImage(null);

    const fileInput = document.getElementById('image-upload') as HTMLInputElement;
    if (fileInput) {
      fileInput.value = '';
    }
  }, [imageUrl, setImage]);

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
    </div>
  );

  const renderUploadPlaceholder = () => (
    <div className="flex flex-col items-center justify-center text-center p-6">
      <svg
        xmlns="http://www.w3.org/2000/svg"
        className="h-8 w-8 text-[#163f78] mb-4"
        fill="none"
        viewBox="0 0 24 24"
        stroke="currentColor"
      >
        <path
          strokeLinecap="round"
          strokeLinejoin="round"
          strokeWidth={2}
          d="M4 16l4.586-4.586a2 2 0 012.828 0L16 16m-2-2l1.586-1.586a2 2 0 012.828 0L20 14m-6-6h.01M6 20h12a2 2 0 002-2V6a2 2 0 00-2-2H6a2 2 0 00-2 2v12a2 2 0 002 2z"
        />
      </svg>
      <p className="text-sm font-medium mb-1">
        <span className="text-[#163f78] cursor-pointer underline">Click to upload</span>{' '}
        <span className="text-[#333]">or drag</span>
      </p>
      <span className="text-sm font-medium text-[#333] mb-2">and drop</span>
      <span className="text-xs font-medium text-[#333]">Upload jpeg, jpg, png</span>
      <span className="text-xs font-medium text-[#333]">(Max 1 MB)</span>
    </div>
  );

  return (
    <div className="flex flex-col items-center pl-20">
      <div
        className={`w-[350px] h-[450px] border-[2px] border-dashed ${
          isDragging ? 'border-blue-500 bg-blue-50' : 'border-[#163f78]'
        } rounded-[5px] flex flex-col items-center justify-center mb-6 relative`}
        onDrop={handleDrop}
        onDragOver={(e) => handleDragEvents(e, true)}
        onDragEnter={(e) => handleDragEvents(e, true)}
        onDragLeave={(e) => handleDragEvents(e, false)}
      >
        {imageUrl ? renderImagePreview() : renderUploadPlaceholder()}
        <input
          id="image-upload"
          type="file"
          accept={ACCEPTED_FILE_TYPES}
          onChange={handleImageUpload}
          className="hidden"
        />
        <label
          htmlFor="image-upload"
          className={`absolute inset-0 w-full h-full ${
            imageUrl ? 'opacity-0 z-0' : 'opacity-0 cursor-pointer z-10'
          }`}
        />
      </div>
    </div>
  );
};

export default UploadImage;
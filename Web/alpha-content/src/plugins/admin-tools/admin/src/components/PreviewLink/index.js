    // ./src/plugins/admin-tools/admin/src/components/PreviewLink/index.js
    import React from 'react';
    import { useCMEditViewDataManager } from '@strapi/helper-plugin';
    import { Eye } from '@strapi/icons';
    import { LinkButton } from '@strapi/design-system/v2';
    import configs from '../../configs';

    const PreviewLink = () => {
      const {initialData, slug} = useCMEditViewDataManager();
    
      if (!configs.previewModels.some(c => c===slug)) {
        return null;
      }

 
      return (
        <LinkButton
          size="S"
          startIcon={<Eye/>}
          style={{width: '100%'}}
          href={`${CLIENT_FRONTEND_URL}/article/${initialData.slug}?id=${initialData.id}`}
          variant="secondary"
          target="_blank"
          rel="noopener noreferrer"
          title="page preview"
        >Preview
        </LinkButton>
      );
    };

    export default PreviewLink;

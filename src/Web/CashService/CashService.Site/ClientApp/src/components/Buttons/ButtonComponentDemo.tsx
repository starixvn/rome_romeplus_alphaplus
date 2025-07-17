import React from 'react';
import { ButtonComponent } from './ButtonComponent';

const ButtonComponentDemo: React.FC = () => {
  const handleClick = (variant: string) => {
    console.log(`Button ${variant} clicked!`);
  };

  return (
    <div className="p-8 space-y-6">
      <h1 className="text-2xl font-bold mb-6">ButtonComponent Demo</h1>
      
      <div className="space-y-4">
        <h2 className="text-lg font-semibold">All Button Variants</h2>
        
        <div className="flex flex-wrap gap-4">
          {/* Apply Button (default) */}
          <ButtonComponent
            text="Apply"
            onClick={() => handleClick('apply')}
            variant="apply"
          />
          
          {/* Cancel Button */}
          <ButtonComponent
            text="Cancel"
            onClick={() => handleClick('cancel')}
            variant="cancel"
          />
          
          {/* Full Button */}
          <ButtonComponent
            text="View Details"
            onClick={() => handleClick('full')}
            variant="full"
          />
          
          {/* Outline Button */}
          <ButtonComponent
            text="Edit"
            onClick={() => handleClick('outline')}
            variant="outline"
          />
          
          {/* Reject Button */}
          <ButtonComponent
            text="Reject"
            onClick={() => handleClick('reject')}
            variant="reject"
          />
        </div>
      </div>

      <div className="space-y-4">
        <h2 className="text-lg font-semibold">Button States</h2>
        
        <div className="flex flex-wrap gap-4">
          {/* Disabled Button */}
          <ButtonComponent
            text="Disabled"
            onClick={() => handleClick('disabled')}
            disabled={true}
          />
          
          {/* Button with Title */}
          <ButtonComponent
            text="With Tooltip"
            onClick={() => handleClick('tooltip')}
            title="This is a tooltip"
          />
          
          {/* Submit Button */}
          <ButtonComponent
            text="Submit"
            onClick={() => handleClick('submit')}
            type="submit"
          />
        </div>
      </div>

      <div className="space-y-4">
        <h2 className="text-lg font-semibold">Migration Examples</h2>
        
        <div className="bg-gray-100 p-4 rounded">
          <h3 className="font-medium mb-2">Before (Old Components):</h3>
          <pre className="text-sm bg-white p-2 rounded">
{`<ButtonApplyComponent text="Apply" onClick={handleApply} />
<ButtonCancelComponent text="Cancel" onClick={handleCancel} />
<ButtonFullComponent text="View" onClick={handleView} />
<ButtonOutlineComponent text="Edit" onClick={handleEdit} />
<ButtonRejectComponent text="Reject" onClick={handleReject} />`}
          </pre>
        </div>
        
        <div className="bg-green-100 p-4 rounded">
          <h3 className="font-medium mb-2">After (New Component):</h3>
          <pre className="text-sm bg-white p-2 rounded">
{`<ButtonComponent text="Apply" onClick={handleApply} variant="apply" />
<ButtonComponent text="Cancel" onClick={handleCancel} variant="cancel" />
<ButtonComponent text="View" onClick={handleView} variant="full" />
<ButtonComponent text="Edit" onClick={handleEdit} variant="outline" />
<ButtonComponent text="Reject" onClick={handleReject} variant="reject" />`}
          </pre>
        </div>
        
        <div className="bg-blue-100 p-4 rounded">
          <h3 className="font-medium mb-2">Or use default variant:</h3>
          <pre className="text-sm bg-white p-2 rounded">
{`<ButtonComponent text="Apply" onClick={handleApply} />
<ButtonComponent text="Cancel" onClick={handleCancel} variant="cancel" />
<ButtonComponent text="View" onClick={handleView} variant="full" />
<ButtonComponent text="Edit" onClick={handleEdit} variant="outline" />
<ButtonComponent text="Reject" onClick={handleReject} variant="reject" />`}
          </pre>
        </div>
      </div>
    </div>
  );
};

export default ButtonComponentDemo; 
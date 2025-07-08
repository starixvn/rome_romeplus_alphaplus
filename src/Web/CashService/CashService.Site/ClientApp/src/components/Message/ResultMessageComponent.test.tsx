import React from 'react';
import { describe, it, expect } from 'vitest';
import { render, screen } from '@testing-library/react';
import { ResultMessageComponent } from './ResultMessageComponent';

describe('ResultMessageComponent', () => {
  const testMessage = 'Test message content';

  describe('Error message rendering', () => {
    it('should render error message when isError is true', () => {
      render(
        <ResultMessageComponent
          isError={true}
          message={testMessage}
        />
      );

      expect(screen.getByRole('alert')).toBeInTheDocument();
      expect(screen.getByText(testMessage)).toBeInTheDocument();
    });

    it('should render error message when type is "error"', () => {
      render(
        <ResultMessageComponent
          type="error"
          message={testMessage}
        />
      );

      expect(screen.getByRole('alert')).toBeInTheDocument();
      expect(screen.getByText(testMessage)).toBeInTheDocument();
    });

    it('should prioritize isError over type when both are provided', () => {
      render(
        <ResultMessageComponent
          isError={true}
          type="success"
          message={testMessage}
        />
      );

      expect(screen.getByRole('alert')).toBeInTheDocument();
      expect(screen.getByText(testMessage)).toBeInTheDocument();
    });
  });

  describe('Warning message rendering', () => {
    it('should render warning message when isWarning is true', () => {
      render(
        <ResultMessageComponent
          isWarning={true}
          message={testMessage}
        />
      );

      expect(screen.getByRole('alert')).toBeInTheDocument();
      expect(screen.getByText(testMessage)).toBeInTheDocument();
    });

    it('should render warning message when type is "warning"', () => {
      render(
        <ResultMessageComponent
          type="warning"
          message={testMessage}
        />
      );

      expect(screen.getByRole('alert')).toBeInTheDocument();
      expect(screen.getByText(testMessage)).toBeInTheDocument();
    });

    it('should prioritize isWarning over type when both are provided', () => {
      render(
        <ResultMessageComponent
          isWarning={true}
          type="error"
          message={testMessage}
        />
      );

      expect(screen.getByRole('alert')).toBeInTheDocument();
      expect(screen.getByText(testMessage)).toBeInTheDocument();
    });
  });

  describe('Success message rendering', () => {
    it('should render success message when isSuccess is true', () => {
      render(
        <ResultMessageComponent
          isSuccess={true}
          message={testMessage}
        />
      );

      expect(screen.getByRole('alert')).toBeInTheDocument();
      expect(screen.getByText(testMessage)).toBeInTheDocument();
    });

    it('should render success message when type is "success"', () => {
      render(
        <ResultMessageComponent
          type="success"
          message={testMessage}
        />
      );

      expect(screen.getByRole('alert')).toBeInTheDocument();
      expect(screen.getByText(testMessage)).toBeInTheDocument();
    });

    it('should prioritize isSuccess over type when both are provided', () => {
      render(
        <ResultMessageComponent
          isSuccess={true}
          type="error"
          message={testMessage}
        />
      );

      expect(screen.getByRole('alert')).toBeInTheDocument();
      expect(screen.getByText(testMessage)).toBeInTheDocument();
    });
  });

  describe('Priority order', () => {
    it('should prioritize isError over other flags', () => {
      render(
        <ResultMessageComponent
          isError={true}
          isWarning={true}
          isSuccess={true}
          message={testMessage}
        />
      );

      expect(screen.getByRole('alert')).toBeInTheDocument();
      expect(screen.getByText(testMessage)).toBeInTheDocument();
    });

    it('should prioritize isWarning over isSuccess when isError is false', () => {
      render(
        <ResultMessageComponent
          isError={false}
          isWarning={true}
          isSuccess={true}
          message={testMessage}
        />
      );

      expect(screen.getByRole('alert')).toBeInTheDocument();
      expect(screen.getByText(testMessage)).toBeInTheDocument();
    });
  });

  describe('No message scenarios', () => {
    it('should return null when no message is provided', () => {
      const { container } = render(
        <ResultMessageComponent
          type="error"
          message={null}
        />
      );

      expect(container.firstChild).toBeNull();
    });

    it('should return null when message is empty string', () => {
      const { container } = render(
        <ResultMessageComponent
          type="error"
          message=""
        />
      );

      expect(container.firstChild).toBeNull();
    });

    it('should return null when no type or flags are provided', () => {
      const { container } = render(
        <ResultMessageComponent
          message={testMessage}
        />
      );

      expect(container.firstChild).toBeNull();
    });
  });

  describe('Custom className', () => {
    it('should apply custom className to error message', () => {
      const customClass = 'custom-error-class';
      render(
        <ResultMessageComponent
          isError={true}
          message={testMessage}
          className={customClass}
        />
      );

      const alertElement = screen.getByRole('alert');
      expect(alertElement).toHaveClass(customClass);
    });

    it('should apply custom className to warning message', () => {
      const customClass = 'custom-warning-class';
      render(
        <ResultMessageComponent
          isWarning={true}
          message={testMessage}
          className={customClass}
        />
      );

      const alertElement = screen.getByRole('alert');
      expect(alertElement).toHaveClass(customClass);
    });

    it('should apply custom className to success message', () => {
      const customClass = 'custom-success-class';
      render(
        <ResultMessageComponent
          isSuccess={true}
          message={testMessage}
          className={customClass}
        />
      );

      const alertElement = screen.getByRole('alert');
      expect(alertElement).toHaveClass(customClass);
    });

    it('should use default className when not provided', () => {
      render(
        <ResultMessageComponent
          isError={true}
          message={testMessage}
        />
      );

      const alertElement = screen.getByRole('alert');
      expect(alertElement).toHaveClass('max-[768px]:py-[12px]');
    });
  });

  describe('HTML content rendering', () => {
    it('should render HTML content in message', () => {
      const htmlMessage = '<strong>Bold text</strong> and <em>italic text</em>';
      render(
        <ResultMessageComponent
          isError={true}
          message={htmlMessage}
        />
      );

      expect(screen.getByText('Bold text')).toBeInTheDocument();
      expect(screen.getByText('italic text')).toBeInTheDocument();
    });

    it('should render complex HTML content', () => {
      const htmlMessage = '<div><span>Nested</span> <a href="#">Link</a></div>';
      render(
        <ResultMessageComponent
          isSuccess={true}
          message={htmlMessage}
        />
      );

      expect(screen.getByText('Nested')).toBeInTheDocument();
      expect(screen.getByText('Link')).toBeInTheDocument();
    });
  });

  describe('Component integration', () => {
    it('should render ErrorMessageComponent when error conditions are met', () => {
      render(
        <ResultMessageComponent
          isError={true}
          message={testMessage}
        />
      );

      // Check that the error-specific styling is applied
      const alertElement = screen.getByRole('alert');
      const innerDiv = alertElement.querySelector('div');
      expect(innerDiv).toHaveClass('bg-[#FFE8E8]');
    });

    it('should render WarningMessageComponent when warning conditions are met', () => {
      render(
        <ResultMessageComponent
          isWarning={true}
          message={testMessage}
        />
      );

      // Check that the warning-specific styling is applied
      const alertElement = screen.getByRole('alert');
      const innerDiv = alertElement.querySelector('div');
      expect(innerDiv).toHaveClass('bg-[#FFFFFA]');
    });

    it('should render SuccessMessageComponent when success conditions are met', () => {
      render(
        <ResultMessageComponent
          isSuccess={true}
          message={testMessage}
        />
      );

      // Check that the success-specific styling is applied
      const alertElement = screen.getByRole('alert');
      const innerDiv = alertElement.querySelector('div');
      expect(innerDiv).toHaveClass('bg-[#E2FFF2]');
    });
  });
}); 
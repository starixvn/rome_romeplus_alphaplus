import { describe, it, expect, vi } from 'vitest';
import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom';
import React from 'react';
import {
  ButtonTabActiveComponent,
  ButtonTabInactiveComponent,
  ButtonTabComponent
} from './ButtonTabComponent';

describe('ButtonTabComponent', () => {
  describe('ButtonTabActiveComponent', () => {
    it('renders active tab button with default props', () => {
      render(<ButtonTabActiveComponent text="Active Tab" onClick={() => {}} />);
      
      const button = screen.getByRole('button');
      expect(button).toBeInTheDocument();
      expect(button).toHaveTextContent('Active Tab');
      expect(button).toHaveAttribute('type', 'button');
      expect(button).toHaveClass(
        'font-semibold',
        'px-[20px]',
        'pt-[10px]',
        'pb-[13px]',
        'text-[var(--tab-component-color)]',
        'cursor-pointer',
        'relative',
        'after:content-[\'\']',
        'after:absolute',
        'after:bottom-0',
        'after:left-0',
        'after:w-0',
        'after:h-[3px]',
        'after:bg-[var(--tab-component-border-color)]',
        'after:transition-all',
        'after:duration-500',
        'after:ease-in-out',
        'after:opacity-0',
        'hover:after:opacity-100',
        'hover:after:w-full',
        'hover:text-[var(--tab-component-color-active)]'
      );
    });

    it('renders active tab button with custom type', () => {
      render(<ButtonTabActiveComponent text="Submit" onClick={() => {}} type="submit" />);
      
      const button = screen.getByRole('button');
      expect(button).toHaveAttribute('type', 'submit');
    });

    it('calls onClick when clicked', () => {
      const mockOnClick = vi.fn();
      render(<ButtonTabActiveComponent text="Click Me" onClick={mockOnClick} />);
      
      const button = screen.getByRole('button');
      fireEvent.click(button);
      
      expect(mockOnClick).toHaveBeenCalledTimes(1);
    });

    it('renders button text correctly', () => {
      const testText = 'This is an active tab button text';
      render(<ButtonTabActiveComponent text={testText} onClick={() => {}} />);
      
      const button = screen.getByRole('button');
      expect(button).toHaveTextContent(testText);
    });

    it('renders empty text', () => {
      render(<ButtonTabActiveComponent text="" onClick={() => {}} />);
      
      const button = screen.getByRole('button');
      expect(button).toHaveTextContent('');
    });

    it('has proper button role', () => {
      render(<ButtonTabActiveComponent text="Accessible" onClick={() => {}} />);
      
      const button = screen.getByRole('button');
      expect(button).toBeInTheDocument();
    });

    it('is keyboard accessible', () => {
      const mockOnClick = vi.fn();
      render(<ButtonTabActiveComponent text="Keyboard" onClick={mockOnClick} />);
      
      const button = screen.getByRole('button');
      expect(button).not.toBeDisabled();
      
      fireEvent.click(button);
      expect(mockOnClick).toHaveBeenCalledTimes(1);
    });
  });

  describe('ButtonTabInactiveComponent', () => {
    it('renders inactive tab button with default props', () => {
      render(<ButtonTabInactiveComponent text="Inactive Tab" onClick={() => {}} />);
      
      const button = screen.getByRole('button');
      expect(button).toBeInTheDocument();
      expect(button).toHaveTextContent('Inactive Tab');
      expect(button).toHaveAttribute('type', 'button');
      expect(button).toHaveClass(
        'font-semibold',
        'px-[20px]',
        'pt-[10px]',
        'pb-[13px]',
        'text-[var(--tab-component-color-active)]',
        'cursor-pointer',
        'relative',
        'after:content-[\'\']',
        'after:absolute',
        'after:bottom-0',
        'after:left-0',
        'after:w-full',
        'after:h-[3px]',
        'after:bg-[var(--tab-component-border-color)]'
      );
    });

    it('renders inactive tab button with custom type', () => {
      render(<ButtonTabInactiveComponent text="Reset" onClick={() => {}} type="reset" />);
      
      const button = screen.getByRole('button');
      expect(button).toHaveAttribute('type', 'reset');
    });

    it('calls onClick when clicked', () => {
      const mockOnClick = vi.fn();
      render(<ButtonTabInactiveComponent text="Click Me" onClick={mockOnClick} />);
      
      const button = screen.getByRole('button');
      fireEvent.click(button);
      
      expect(mockOnClick).toHaveBeenCalledTimes(1);
    });

    it('renders button text correctly', () => {
      const testText = 'This is an inactive tab button text';
      render(<ButtonTabInactiveComponent text={testText} onClick={() => {}} />);
      
      const button = screen.getByRole('button');
      expect(button).toHaveTextContent(testText);
    });

    it('renders empty text', () => {
      render(<ButtonTabInactiveComponent text="" onClick={() => {}} />);
      
      const button = screen.getByRole('button');
      expect(button).toHaveTextContent('');
    });

    it('has proper button role', () => {
      render(<ButtonTabInactiveComponent text="Accessible" onClick={() => {}} />);
      
      const button = screen.getByRole('button');
      expect(button).toBeInTheDocument();
    });

    it('is keyboard accessible', () => {
      const mockOnClick = vi.fn();
      render(<ButtonTabInactiveComponent text="Keyboard" onClick={mockOnClick} />);
      
      const button = screen.getByRole('button');
      expect(button).not.toBeDisabled();
      
      fireEvent.click(button);
      expect(mockOnClick).toHaveBeenCalledTimes(1);
    });
  });

  describe('ButtonTabComponent', () => {
    it('renders active tab when isActive is true', () => {
      render(<ButtonTabComponent text="Active Tab" onClick={() => {}} isActive={true} />);
      
      const button = screen.getByRole('button');
      expect(button).toBeInTheDocument();
      expect(button).toHaveTextContent('Active Tab');
      expect(button).toHaveClass('text-[var(--tab-component-color)]');
      expect(button).toHaveClass('hover:text-[var(--tab-component-color-active)]');
    });

    it('renders inactive tab when isActive is false', () => {
      render(<ButtonTabComponent text="Inactive Tab" onClick={() => {}} isActive={false} />);
      
      const button = screen.getByRole('button');
      expect(button).toBeInTheDocument();
      expect(button).toHaveTextContent('Inactive Tab');
      expect(button).toHaveClass('text-[var(--tab-component-color-active)]');
      expect(button).not.toHaveClass('hover:text-[var(--tab-component-color-active)]');
    });

    it('renders inactive tab when isActive is undefined', () => {
      render(<ButtonTabComponent text="Default Tab" onClick={() => {}} />);
      
      const button = screen.getByRole('button');
      expect(button).toBeInTheDocument();
      expect(button).toHaveTextContent('Default Tab');
      expect(button).toHaveClass('text-[var(--tab-component-color-active)]');
    });

    it('calls onClick when active tab is clicked', () => {
      const mockOnClick = vi.fn();
      render(<ButtonTabComponent text="Active Click" onClick={mockOnClick} isActive={true} />);
      
      const button = screen.getByRole('button');
      fireEvent.click(button);
      
      expect(mockOnClick).toHaveBeenCalledTimes(1);
    });

    it('calls onClick when inactive tab is clicked', () => {
      const mockOnClick = vi.fn();
      render(<ButtonTabComponent text="Inactive Click" onClick={mockOnClick} isActive={false} />);
      
      const button = screen.getByRole('button');
      fireEvent.click(button);
      
      expect(mockOnClick).toHaveBeenCalledTimes(1);
    });

    it('renders with custom type when active', () => {
      render(<ButtonTabComponent text="Submit" onClick={() => {}} isActive={true} type="submit" />);
      
      const button = screen.getByRole('button');
      expect(button).toHaveAttribute('type', 'submit');
    });

    it('renders with custom type when inactive', () => {
      render(<ButtonTabComponent text="Reset" onClick={() => {}} isActive={false} type="reset" />);
      
      const button = screen.getByRole('button');
      expect(button).toHaveAttribute('type', 'reset');
    });

    it('renders button text correctly when active', () => {
      const testText = 'This is an active tab text';
      render(<ButtonTabComponent text={testText} onClick={() => {}} isActive={true} />);
      
      const button = screen.getByRole('button');
      expect(button).toHaveTextContent(testText);
    });

    it('renders button text correctly when inactive', () => {
      const testText = 'This is an inactive tab text';
      render(<ButtonTabComponent text={testText} onClick={() => {}} isActive={false} />);
      
      const button = screen.getByRole('button');
      expect(button).toHaveTextContent(testText);
    });

    it('renders empty text when active', () => {
      render(<ButtonTabComponent text="" onClick={() => {}} isActive={true} />);
      
      const button = screen.getByRole('button');
      expect(button).toHaveTextContent('');
    });

    it('renders empty text when inactive', () => {
      render(<ButtonTabComponent text="" onClick={() => {}} isActive={false} />);
      
      const button = screen.getByRole('button');
      expect(button).toHaveTextContent('');
    });

    it('has proper button role when active', () => {
      render(<ButtonTabComponent text="Accessible Active" onClick={() => {}} isActive={true} />);
      
      const button = screen.getByRole('button');
      expect(button).toBeInTheDocument();
    });

    it('has proper button role when inactive', () => {
      render(<ButtonTabComponent text="Accessible Inactive" onClick={() => {}} isActive={false} />);
      
      const button = screen.getByRole('button');
      expect(button).toBeInTheDocument();
    });

    it('is keyboard accessible when active', () => {
      const mockOnClick = vi.fn();
      render(<ButtonTabComponent text="Keyboard Active" onClick={mockOnClick} isActive={true} />);
      
      const button = screen.getByRole('button');
      expect(button).not.toBeDisabled();
      
      fireEvent.click(button);
      expect(mockOnClick).toHaveBeenCalledTimes(1);
    });

    it('is keyboard accessible when inactive', () => {
      const mockOnClick = vi.fn();
      render(<ButtonTabComponent text="Keyboard Inactive" onClick={mockOnClick} isActive={false} />);
      
      const button = screen.getByRole('button');
      expect(button).not.toBeDisabled();
      
      fireEvent.click(button);
      expect(mockOnClick).toHaveBeenCalledTimes(1);
    });

    it('handles undefined onClick gracefully when active', () => {
      render(<ButtonTabComponent text="No Click Active" onClick={undefined} isActive={true} />);
      
      const button = screen.getByRole('button');
      expect(() => fireEvent.click(button)).not.toThrow();
    });

    it('handles undefined onClick gracefully when inactive', () => {
      render(<ButtonTabComponent text="No Click Inactive" onClick={undefined} isActive={false} />);
      
      const button = screen.getByRole('button');
      expect(() => fireEvent.click(button)).not.toThrow();
    });

    it('handles null onClick gracefully when active', () => {
      render(<ButtonTabComponent text="Null Click Active" onClick={null} isActive={true} />);
      
      const button = screen.getByRole('button');
      expect(() => fireEvent.click(button)).not.toThrow();
    });

    it('handles null onClick gracefully when inactive', () => {
      render(<ButtonTabComponent text="Null Click Inactive" onClick={null} isActive={false} />);
      
      const button = screen.getByRole('button');
      expect(() => fireEvent.click(button)).not.toThrow();
    });
  });
}); 
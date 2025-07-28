import React from 'react';
import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom';
import { 
  ButtonTabComponent, 
  ButtonTabActiveComponent, 
  ButtonTabInactiveComponent 
} from './ButtonTabComponent';
import { ButtonComponentProps } from './types';

describe('ButtonTabComponent', () => {
  const mockOnClick = vi.fn();
  const defaultProps: ButtonComponentProps = {
    text: 'Test Tab',
    onClick: mockOnClick,
  };

  beforeEach(() => {
    mockOnClick.mockClear();
  });

  describe('BaseTabButton (internal component)', () => {
    it('renders with default props', () => {
      render(<ButtonTabComponent {...defaultProps} isActive={false} />);
      
      const button = screen.getByRole('tab', { name: 'Test Tab' });
      expect(button).toBeInTheDocument();
      expect(button).toHaveTextContent('Test Tab');
    });

    it('renders with custom className', () => {
      render(<ButtonTabComponent {...defaultProps} className="custom-class" isActive={false} />);
      
      const button = screen.getByRole('tab', { name: 'Test Tab' });
      expect(button).toHaveClass('custom-class');
    });

    it('renders without text', () => {
      render(<ButtonTabComponent onClick={mockOnClick} isActive={false} />);
      
      const button = screen.getByRole('tab');
      expect(button).toBeInTheDocument();
      expect(button).toHaveTextContent('');
    });

    it('applies base classes correctly', () => {
      render(<ButtonTabComponent {...defaultProps} isActive={false} />);
      
      const button = screen.getByRole('tab', { name: 'Test Tab' });
      expect(button.className).toContain('font-semibold');
      expect(button.className).toContain('px-[20px]');
      expect(button.className).toContain('pt-[10px]');
      expect(button.className).toContain('pb-[13px]');
      expect(button.className).toContain('cursor-pointer');
      expect(button.className).toContain('relative');
    });

    it('has correct button type', () => {
      render(<ButtonTabComponent {...defaultProps} type="submit" isActive={false} />);
      
      const button = screen.getByRole('tab', { name: 'Test Tab' });
      expect(button).toHaveAttribute('type', 'submit');
    });

    it('defaults to button type', () => {
      render(<ButtonTabComponent {...defaultProps} isActive={false} />);
      
      const button = screen.getByRole('tab', { name: 'Test Tab' });
      expect(button).toHaveAttribute('type', 'button');
    });
  });

  describe('Active state styling', () => {
    it('applies active classes when isActive is true', () => {
      render(<ButtonTabComponent {...defaultProps} isActive={true} />);
      
      const button = screen.getByRole('tab', { name: 'Test Tab' });
      expect(button).toHaveClass('text-[var(--tab-component-color-active)]');
      expect(button).toHaveClass('after:w-full');
      expect(button).toHaveAttribute('aria-selected', 'true');
      expect(button).toHaveAttribute('tabIndex', '0');
    });

    it('applies inactive classes when isActive is false', () => {
      render(<ButtonTabComponent {...defaultProps} isActive={false} />);
      
      const button = screen.getByRole('tab', { name: 'Test Tab' });
      expect(button).toHaveClass('text-[var(--tab-component-color)]');
      expect(button).toHaveClass('after:w-0');
      expect(button).toHaveClass('after:transition-all');
      expect(button).toHaveClass('after:duration-500');
      expect(button).toHaveClass('after:ease-in-out');
      expect(button).toHaveClass('after:opacity-0');
      expect(button).toHaveAttribute('aria-selected', 'false');
      expect(button).toHaveAttribute('tabIndex', '-1');
    });

    it('defaults to inactive state when isActive is undefined', () => {
      render(<ButtonTabComponent {...defaultProps} />);
      
      const button = screen.getByRole('tab', { name: 'Test Tab' });
      expect(button).toHaveClass('text-[var(--tab-component-color)]');
      expect(button).toHaveAttribute('aria-selected', 'false');
      expect(button).toHaveAttribute('tabIndex', '-1');
    });
  });

  describe('Click handling', () => {
    it('calls onClick when clicked', () => {
      render(<ButtonTabComponent {...defaultProps} isActive={false} />);
      
      const button = screen.getByRole('tab', { name: 'Test Tab' });
      fireEvent.click(button);
      
      expect(mockOnClick).toHaveBeenCalledTimes(1);
    });

    it('calls onClick with event object', () => {
      render(<ButtonTabComponent {...defaultProps} isActive={false} />);
      
      const button = screen.getByRole('tab', { name: 'Test Tab' });
      fireEvent.click(button);
      
      expect(mockOnClick).toHaveBeenCalledWith(expect.any(Object));
    });

    it('does not call onClick when no onClick handler is provided', () => {
      render(<ButtonTabComponent text="Test Tab" isActive={false} />);
      
      const button = screen.getByRole('tab', { name: 'Test Tab' });
      fireEvent.click(button);
      
      // Should not throw an error
      expect(button).toBeInTheDocument();
    });
  });

  describe('Forwarded ref', () => {
    it('forwards ref correctly', () => {
      const ref = React.createRef<HTMLButtonElement>();
      render(<ButtonTabComponent {...defaultProps} isActive={false} ref={ref} />);
      
      expect(ref.current).toBeInstanceOf(HTMLButtonElement);
      expect(ref.current).toHaveTextContent('Test Tab');
    });
  });

  describe('Additional props', () => {
    it('passes through additional HTML attributes', () => {
      render(
        <ButtonTabComponent 
          {...defaultProps} 
          isActive={false}
          data-testid="custom-tab"
          aria-describedby="description"
        />
      );
      
      const button = screen.getByTestId('custom-tab');
      expect(button).toHaveAttribute('data-testid', 'custom-tab');
      expect(button).toHaveAttribute('aria-describedby', 'description');
    });

    it('passes through custom event handlers', () => {
      const mockOnMouseEnter = vi.fn();
      const mockOnMouseLeave = vi.fn();
      
      render(
        <ButtonTabComponent 
          {...defaultProps} 
          isActive={false}
          onMouseEnter={mockOnMouseEnter}
          onMouseLeave={mockOnMouseLeave}
        />
      );
      
      const button = screen.getByRole('tab', { name: 'Test Tab' });
      fireEvent.mouseEnter(button);
      fireEvent.mouseLeave(button);
      
      expect(mockOnMouseEnter).toHaveBeenCalledTimes(1);
      expect(mockOnMouseLeave).toHaveBeenCalledTimes(1);
    });
  });

  describe('Accessibility', () => {
    it('has correct role', () => {
      render(<ButtonTabComponent {...defaultProps} isActive={false} />);
      
      const button = screen.getByRole('tab', { name: 'Test Tab' });
      expect(button).toBeInTheDocument();
    });

    it('has correct aria-selected attribute for active state', () => {
      render(<ButtonTabComponent {...defaultProps} isActive={true} />);
      
      const button = screen.getByRole('tab', { name: 'Test Tab' });
      expect(button).toHaveAttribute('aria-selected', 'true');
    });

    it('has correct aria-selected attribute for inactive state', () => {
      render(<ButtonTabComponent {...defaultProps} isActive={false} />);
      
      const button = screen.getByRole('tab', { name: 'Test Tab' });
      expect(button).toHaveAttribute('aria-selected', 'false');
    });

    it('has correct tabIndex for active state', () => {
      render(<ButtonTabComponent {...defaultProps} isActive={true} />);
      
      const button = screen.getByRole('tab', { name: 'Test Tab' });
      expect(button).toHaveAttribute('tabIndex', '0');
    });

    it('has correct tabIndex for inactive state', () => {
      render(<ButtonTabComponent {...defaultProps} isActive={false} />);
      
      const button = screen.getByRole('tab', { name: 'Test Tab' });
      expect(button).toHaveAttribute('tabIndex', '-1');
    });
  });
});

describe('ButtonTabActiveComponent', () => {
  const mockOnClick = vi.fn();
  const defaultProps: ButtonComponentProps = {
    text: 'Active Tab',
    onClick: mockOnClick,
  };

  beforeEach(() => {
    mockOnClick.mockClear();
  });

  it('renders as active tab', () => {
    render(<ButtonTabActiveComponent {...defaultProps} />);
    
    const button = screen.getByRole('tab', { name: 'Active Tab' });
    expect(button).toBeInTheDocument();
    expect(button).toHaveClass('text-[var(--tab-component-color-active)]');
    expect(button).toHaveClass('after:w-full');
    expect(button).toHaveAttribute('aria-selected', 'true');
    expect(button).toHaveAttribute('tabIndex', '0');
  });

  it('calls onClick when clicked', () => {
    render(<ButtonTabActiveComponent {...defaultProps} />);
    
    const button = screen.getByRole('tab', { name: 'Active Tab' });
    fireEvent.click(button);
    
    expect(mockOnClick).toHaveBeenCalledTimes(1);
  });

  it('forwards ref correctly', () => {
    const ref = React.createRef<HTMLButtonElement>();
    render(<ButtonTabActiveComponent {...defaultProps} ref={ref} />);
    
    expect(ref.current).toBeInstanceOf(HTMLButtonElement);
    expect(ref.current).toHaveTextContent('Active Tab');
  });

  it('passes through additional props', () => {
    render(
      <ButtonTabActiveComponent 
        {...defaultProps} 
        className="custom-active-class"
        data-testid="active-tab"
      />
    );
    
    const button = screen.getByTestId('active-tab');
    expect(button).toHaveClass('custom-active-class');
    expect(button).toHaveAttribute('data-testid', 'active-tab');
  });
});

describe('ButtonTabInactiveComponent', () => {
  const mockOnClick = vi.fn();
  const defaultProps: ButtonComponentProps = {
    text: 'Inactive Tab',
    onClick: mockOnClick,
  };

  beforeEach(() => {
    mockOnClick.mockClear();
  });

  it('renders as inactive tab', () => {
    render(<ButtonTabInactiveComponent {...defaultProps} />);
    
    const button = screen.getByRole('tab', { name: 'Inactive Tab' });
    expect(button).toBeInTheDocument();
    expect(button).toHaveClass('text-[var(--tab-component-color)]');
    expect(button).toHaveClass('after:w-0');
    expect(button).toHaveClass('after:transition-all');
    expect(button).toHaveClass('after:duration-500');
    expect(button).toHaveClass('after:ease-in-out');
    expect(button).toHaveClass('after:opacity-0');
    expect(button).toHaveAttribute('aria-selected', 'false');
    expect(button).toHaveAttribute('tabIndex', '-1');
  });

  it('calls onClick when clicked', () => {
    render(<ButtonTabInactiveComponent {...defaultProps} />);
    
    const button = screen.getByRole('tab', { name: 'Inactive Tab' });
    fireEvent.click(button);
    
    expect(mockOnClick).toHaveBeenCalledTimes(1);
  });

  it('forwards ref correctly', () => {
    const ref = React.createRef<HTMLButtonElement>();
    render(<ButtonTabInactiveComponent {...defaultProps} ref={ref} />);
    
    expect(ref.current).toBeInstanceOf(HTMLButtonElement);
    expect(ref.current).toHaveTextContent('Inactive Tab');
  });

  it('passes through additional props', () => {
    render(
      <ButtonTabInactiveComponent 
        {...defaultProps} 
        className="custom-inactive-class"
        data-testid="inactive-tab"
      />
    );
    
    const button = screen.getByTestId('inactive-tab');
    expect(button).toHaveClass('custom-inactive-class');
    expect(button).toHaveAttribute('data-testid', 'inactive-tab');
  });
});

describe('CSS pseudo-elements and transitions', () => {
  it('has after pseudo-element for active state', () => {
    render(<ButtonTabComponent text="Test Tab" isActive={true} />);
    
    const button = screen.getByRole('tab', { name: 'Test Tab' });
    expect(button.className).toContain('after:content-[\'\']');
    expect(button.className).toContain('after:absolute');
    expect(button.className).toContain('after:bottom-0');
    expect(button.className).toContain('after:left-0');
    expect(button.className).toContain('after:h-[3px]');
    expect(button.className).toContain('after:bg-[var(--tab-component-border-color)]');
    expect(button.className).toContain('after:w-full');
  });

  it('has after pseudo-element for inactive state with transitions', () => {
    render(<ButtonTabComponent text="Test Tab" isActive={false} />);
    
    const button = screen.getByRole('tab', { name: 'Test Tab' });
    expect(button.className).toContain('after:content-[\'\']');
    expect(button.className).toContain('after:absolute');
    expect(button.className).toContain('after:bottom-0');
    expect(button.className).toContain('after:left-0');
    expect(button.className).toContain('after:h-[3px]');
    expect(button.className).toContain('after:bg-[var(--tab-component-border-color)]');
    expect(button.className).toContain('after:w-0');
    expect(button.className).toContain('after:transition-all');
    expect(button.className).toContain('after:duration-500');
    expect(button.className).toContain('after:ease-in-out');
    expect(button.className).toContain('after:opacity-0');
  });

  it('has hover effects for inactive state', () => {
    render(<ButtonTabComponent text="Test Tab" isActive={false} />);
    
    const button = screen.getByRole('tab', { name: 'Test Tab' });
    expect(button.className).toContain('hover:after:opacity-100');
    expect(button.className).toContain('hover:after:w-full');
    expect(button.className).toContain('hover:text-[var(--tab-component-color-active)]');
  });
});

describe('Component display names', () => {
  it('has correct display name for ButtonTabComponent', () => {
    expect(ButtonTabComponent.displayName).toBe('ButtonTabComponent');
  });

  it('has correct display name for ButtonTabActiveComponent', () => {
    expect(ButtonTabActiveComponent.displayName).toBe('ButtonTabActiveComponent');
  });

  it('has correct display name for ButtonTabInactiveComponent', () => {
    expect(ButtonTabInactiveComponent.displayName).toBe('ButtonTabInactiveComponent');
  });
}); 
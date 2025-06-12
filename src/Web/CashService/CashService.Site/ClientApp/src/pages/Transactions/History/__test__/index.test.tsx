import React from 'react';
import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom';
import History from '../index';

vi.mock('react-i18next', () => ({
  useTranslation: () => ({
    t: (key: string) => key,
  }),
  initReactI18next: {
    type: '3rdParty',
    init: () => {},
  }
}));

vi.mock('../../../../hooks/useCookieLanguage', () => ({
  useCookieLanguage: vi.fn(),
}));

vi.mock('../components/TransactionList', () => ({
  default: ({ transactionType }) => (
    <div data-testid="transaction-list">
      Transaction List Type: {transactionType}
    </div>
  ),
}));

vi.mock('../components/HistoryTab', () => ({
  default: ({ activeTab, onTabChange }) => (
    <div data-testid="history-tab">
      <button onClick={() => onTabChange(1)}>Tab 1</button>
      <button onClick={() => onTabChange(2)}>Tab 2</button>
      <span>Active Tab: {activeTab}</span>
    </div>
  ),
}));

vi.mock('../../../../components/TitleComponents', () => ({
  default: ({ title }) => <h1 data-testid="title">{title}</h1>,
}));

describe('History', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('renders the component with initial state', () => {
    render(<History />);

    expect(screen.getByTestId('title')).toHaveTextContent('transactionHistory');
    expect(screen.getByTestId('history-tab')).toBeInTheDocument();
    expect(screen.getByTestId('transaction-list')).toBeInTheDocument();
    expect(screen.getByText('Transaction List Type: 1')).toBeInTheDocument();
  });

  it('changes active tab when tab is clicked', () => {
    render(<History />);

    const tab2Button = screen.getByText('Tab 2');
    fireEvent.click(tab2Button);

    expect(screen.getByText('Active Tab: 2')).toBeInTheDocument();
    expect(screen.getByText('Transaction List Type: 2')).toBeInTheDocument();
  });

  it('maintains the correct tab state after multiple changes', () => {
    render(<History />);

    const tab2Button = screen.getByText('Tab 2');
    const tab1Button = screen.getByText('Tab 1');

    fireEvent.click(tab2Button);
    expect(screen.getByText('Active Tab: 2')).toBeInTheDocument();

    fireEvent.click(tab1Button);
    expect(screen.getByText('Active Tab: 1')).toBeInTheDocument();
  });

  it('renders with the correct layout classes', () => {
    render(<History />);
    
    const mainContainer = screen.getByTestId('transaction-list').parentElement;
    expect(mainContainer).toHaveClass('py-[12px]', 'flex', 'h-full', 'flex-col');
  });
});

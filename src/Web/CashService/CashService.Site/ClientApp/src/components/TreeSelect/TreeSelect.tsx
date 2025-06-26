import React, { useState, useMemo, useEffect, useRef } from 'react';

export interface TreeNodeOption {
  label: string;
  value: string;
  children?: TreeNodeOption[];
}

interface TreeSelectProps {
  options: TreeNodeOption[];
  value: string[];
  onChange: (selected: string[]) => void;
  searchPlaceholder?: string;
  placeholder?: string;
}

const flattenTree = (nodes: TreeNodeOption[]): TreeNodeOption[] => {
  let result: TreeNodeOption[] = [];
  nodes.forEach((node) => {
    result.push(node);
    if (node.children) {
      result = result.concat(flattenTree(node.children));
    }
  });
  return result;
};

const getAllValues = (nodes: TreeNodeOption[]): string[] => {
  let result: string[] = [];
  nodes.forEach((node) => {
    result.push(node.value);
    if (node.children) {
      result = result.concat(getAllValues(node.children));
    }
  });
  return result;
};

const filterTree = (nodes: TreeNodeOption[], keyword: string): TreeNodeOption[] => {
  if (!keyword) return nodes;
  const lower = keyword.toLowerCase();
  return nodes
    .map((node) => {
      if (node.label.toLowerCase().includes(lower)) return node;
      if (node.children) {
        const filteredChildren = filterTree(node.children, keyword);
        if (filteredChildren.length > 0) {
          return { ...node, children: filteredChildren };
        }
      }
      return null;
    })
    .filter(Boolean) as TreeNodeOption[];
};

const getSelectedLabels = (nodes: TreeNodeOption[], selected: string[]): string[] => {
  let result: string[] = [];
  nodes.forEach((node) => {
    if (selected.includes(node.value)) {
      result.push(node.label);
    }
    if (node.children) {
      result = result.concat(getSelectedLabels(node.children, selected));
    }
  });
  return result;
};

const TreeSelect: React.FC<TreeSelectProps> = ({
  options,
  value,
  onChange,
  searchPlaceholder,
  placeholder,
}) => {
  const [expanded, setExpanded] = useState<{ [key: string]: boolean }>({});
  const [search, setSearch] = useState('');
  const [dropdownOpen, setDropdownOpen] = useState(false);
  const containerRef = useRef<HTMLDivElement>(null);
  const inputRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    const expandAll = (nodes: TreeNodeOption[]) => {
      let result: { [key: string]: boolean } = {};
      nodes.forEach((node) => {
        if (node.children) {
          result[node.value] = true;
          Object.assign(result, expandAll(node.children));
        }
      });
      return result;
    };
    setExpanded(expandAll(options));
  }, [options]);

  useEffect(() => {
    if (!dropdownOpen) return;
    const handleClickOutside = (event: MouseEvent) => {
      if (containerRef.current && !containerRef.current.contains(event.target as Node)) {
        setDropdownOpen(false);
      }
    };
    document.addEventListener('mousedown', handleClickOutside);
    return () => document.removeEventListener('mousedown', handleClickOutside);
  }, [dropdownOpen]);

  const filteredOptions = useMemo(() => filterTree(options, search), [options, search]);
  const allValues = useMemo(() => getAllValues(filteredOptions), [filteredOptions]);
  const allChecked = allValues.length > 0 && allValues.every((v) => value.includes(v));
  const someChecked = allValues.some((v) => value.includes(v));

  const handleCheck = (checked: boolean, node: TreeNodeOption) => {
    const nodeValues = getAllValues([node]);
    let newValue: string[];
    if (checked) {
      newValue = Array.from(new Set([...value, ...nodeValues]));
    } else {
      newValue = value.filter((v) => !nodeValues.includes(v));
    }
    onChange(newValue);
  };

  const handleSelectAll = (checked: boolean) => {
    if (checked) {
      onChange(Array.from(new Set([...value, ...allValues])));
    } else {
      onChange(value.filter((v) => !allValues.includes(v)));
    }
  };

  const handleExpand = (nodeValue: string) => {
    setExpanded((prev) => ({ ...prev, [nodeValue]: !prev[nodeValue] }));
  };

  const selectedLabels = useMemo(() => getSelectedLabels(options, value), [options, value]);
  let displayValue = '';
  if (selectedLabels.length === 0) displayValue = placeholder || 'Select';
  else if (selectedLabels.length <= 2) displayValue = selectedLabels.join(', ');
  else displayValue = `${selectedLabels.slice(0, 2).join(', ')} +${selectedLabels.length - 2}`;

  const itemStyle: React.CSSProperties = {
    display: 'flex',
    alignItems: 'center',
    minHeight: 26,
    maxHeight: 32,
    padding: '5px 2px 5px 3px',
    fontSize: 13,
    cursor: 'pointer',
    border: 'none',
    background: 'none',
    outline: 'none',
    userSelect: 'none',
    borderRadius: 0,
    transition: 'background 0.2s',
  };

  const renderSelectAll = () => {
    return (
      <div
        style={itemStyle}
        className="tree-select-item tree-select-selectall"
        tabIndex={-1}
        onMouseDown={(e) => e.preventDefault()}
        onClick={() => handleSelectAll(!(allChecked || (!allChecked && someChecked)))}
        onMouseEnter={(e) => (e.currentTarget.style.background = '#F5F5F5')}
        onMouseLeave={(e) => (e.currentTarget.style.background = '')}
      >
        <input
          type="checkbox"
          checked={allChecked}
          ref={(el) => {
            if (el) el.indeterminate = !allChecked && someChecked;
          }}
          readOnly
          style={{ marginRight: 8 }}
        />
        <span>Select all</span>
      </div>
    );
  };

  const renderTree = (nodes: TreeNodeOption[], level = 0) => {
    return (
      <ul style={{ paddingLeft: 0, listStyle: 'none', margin: 0 }}>
        {nodes.map((node) => {
          const checked = getAllValues([node]).every((v) => value.includes(v));
          const indeterminate = !checked && getAllValues([node]).some((v) => value.includes(v));
          return (
            <li key={node.value} style={{ marginBottom: 0 }}>
              <div
                style={{ ...itemStyle, paddingLeft: 5 + level * 18 }}
                className="tree-select-item"
                tabIndex={-1}
                onMouseDown={(e) => e.preventDefault()}
                onClick={() => handleCheck(!checked, node)}
                onMouseEnter={(e) => (e.currentTarget.style.background = '#F5F5F5')}
                onMouseLeave={(e) => (e.currentTarget.style.background = '')}
              >
                {level !== 0 && node.children && node.children.length > 0 && (
                  <span
                    style={{ cursor: 'pointer', marginRight: 4 }}
                    onClick={(e) => {
                      e.stopPropagation();
                      handleExpand(node.value);
                    }}
                  >
                    {expanded[node.value] ? '▼' : '▶'}
                  </span>
                )}
                <input
                  type="checkbox"
                  checked={checked}
                  ref={(el) => {
                    if (el) el.indeterminate = indeterminate;
                  }}
                  readOnly
                  style={{ marginRight: 8 }}
                />
                <span>{node.label}</span>
              </div>
              {node.children &&
                node.children.length > 0 &&
                (level === 0 || expanded[node.value]) &&
                renderTree(node.children, level + 1)}
            </li>
          );
        })}
      </ul>
    );
  };

  return (
    <div ref={containerRef} style={{ position: 'relative', minWidth: 180, width: '100%' }}>
      <div
        ref={inputRef}
        tabIndex={0}
        className="control-field"
        style={{
          display: 'flex',
          alignItems: 'center',
          cursor: 'pointer',
          userSelect: 'none',
          paddingRight: 32,
        }}
        onClick={() => setDropdownOpen((open) => !open)}
      >
        <span style={{ flex: 1, color: selectedLabels.length === 0 ? '#aaa' : '#222' }}>
          {displayValue}
        </span>
        <span
          style={{
            position: 'absolute',
            right: 8,
            top: '50%',
            transform: 'translateY(-50%)',
            color: '#888',
          }}
        >
          {dropdownOpen ? '▲' : '▼'}
        </span>
        <span style={{ marginLeft: 8, color: '#1976d2', fontWeight: 500, fontSize: 13 }}>
          {value.length > 0 ? `(${value.length})` : ''}
        </span>
      </div>
      {dropdownOpen && (
        <div
          style={{
            position: 'absolute',
            top: 'calc(100% + 2px)',
            left: 0,
            zIndex: 100,
            minWidth: 180,
            width: '100%',
            background: '#fff',
            border: '1px solid #dfdfdf',
            borderRadius: 3,
            boxShadow: 'none',
            padding: '0 10px',
            marginTop: 0,
            fontSize: 15,
          }}
        >
          <div style={{ padding: '8px 0 0 0', borderBottom: '1px solid #F0F0F0' }}>
            <input
              type="text"
              value={search}
              onChange={(e) => setSearch(e.target.value)}
              placeholder={searchPlaceholder || 'Search'}
              className="control-field"
              autoFocus
            />
          </div>
          <div style={{ maxHeight: 260, overflowY: 'auto', padding: 0 }}>
            {renderSelectAll()}
            {renderTree(filteredOptions)}
          </div>
        </div>
      )}
    </div>
  );
};

export default TreeSelect;

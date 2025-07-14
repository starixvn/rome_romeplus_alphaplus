import React, { useEffect, useMemo, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';

export interface TreeNodeOption {
  label: string;
  value: string | number;
  children?: TreeNodeOption[];
}

interface TreeSelectProps {
  options: TreeNodeOption[];
  value: (string | number)[];
  onChange: (selectedValue: (string | number)[], selectedTreeNode: TreeNodeOption[]) => void;
  searchPlaceholder?: string;
  placeholder?: string;
  isAllowCollapseExpand?: boolean;
  isShowSelectAll?: boolean;
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

const getAllValues = (nodes: TreeNodeOption[]): (string | number)[] => {
  let result: (string | number)[] = [];
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

const getSelectedLabels = (nodes: TreeNodeOption[], selected: (string | number)[]): string[] => {
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

function filterTreeBySelected(
  nodes: TreeNodeOption[],
  selected: (string | number)[]
): TreeNodeOption[] {
  return nodes
    .map((node) => {
      if (selected.includes(node.value)) {
        if (node.children) {
          const filteredChildren = filterTreeBySelected(node.children, selected);
          return { ...node, children: filteredChildren.length > 0 ? filteredChildren : undefined };
        }
        return { ...node };
      } else if (node.children) {
        const filteredChildren = filterTreeBySelected(node.children, selected);
        if (filteredChildren.length > 0) {
          return { ...node, children: filteredChildren };
        }
      }
      return null;
    })
    .filter(Boolean) as TreeNodeOption[];
}

function getSelectedLeafValues(
  nodes: TreeNodeOption[],
  selected: (string | number)[]
): (string | number)[] {
  let result: (string | number)[] = [];
  nodes.forEach((node) => {
    if (selected.includes(node.value)) {
      if (!node.children || node.children.length === 0) {
        result.push(node.value);
      } else {
        result = result.concat(getSelectedLeafValues(node.children, selected));
      }
    } else if (node.children) {
      result = result.concat(getSelectedLeafValues(node.children, selected));
    }
  });
  return result;
}

const getAllLeafValues = (nodes: TreeNodeOption[]): (string | number)[] => {
  let result: (string | number)[] = [];
  nodes.forEach((node) => {
    if (node.children && node.children.length > 0) {
      result = result.concat(getAllLeafValues(node.children));
    } else {
      result.push(node.value);
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
  isAllowCollapseExpand = false,
  isShowSelectAll = true,
}) => {
  const [expanded, setExpanded] = useState<{ [key: string]: boolean }>({});
  const [search, setSearch] = useState('');
  const [dropdownOpen, setDropdownOpen] = useState(false);
  const containerRef = useRef<HTMLDivElement>(null);
  const inputRef = useRef<HTMLDivElement>(null);
  const { t } = useTranslation('global');
  useEffect(() => {
    const expandAll = (nodes: TreeNodeOption[]) => {
      let result: { [key: string]: boolean } = {};
      nodes.forEach((node) => {
        if (node.children) {
          result[String(node.value)] = true;
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
  const allLeafValues = useMemo(() => getAllLeafValues(filteredOptions), [filteredOptions]);
  const allChecked = allLeafValues.length > 0 && allLeafValues.every((v) => value.includes(v));

  const handleCheck = (node: TreeNodeOption) => {
    const hasChildren = !!node.children && node.children.length > 0;

    let newValue: (string | number)[] = value;
    if (!hasChildren) {
      if (value.includes(node.value)) {
        newValue = value.filter((v) => v !== node.value);
      } else {
        newValue = [...value, node.value];
      }
    } else {
      const childrenValues = getAllValues(node.children!);
      const allChildrenChecked = childrenValues.every((v) => value.includes(v));
      const someChildrenChecked = childrenValues.some((v) => value.includes(v));
      if (!someChildrenChecked) {
        newValue = Array.from(new Set([...value, ...childrenValues, node.value]));
      } else if (allChildrenChecked) {
        newValue = value.filter((v) => ![node.value, ...childrenValues].includes(v));
      } else {
        newValue = Array.from(new Set([...value, ...childrenValues, node.value]));
      }
    }
    const selectedTreeNode = filterTreeBySelected(options, newValue);
    const selectedValue = getSelectedLeafValues(options, newValue);
    onChange(selectedValue, selectedTreeNode);
  };

  const handleSelectAll = () => {
    let newValue: (string | number)[];
    if (allChecked) {
      newValue = [];
    } else {
      newValue = Array.from(new Set([...allLeafValues]));
    }
    const selectedTreeNode = filterTreeBySelected(options, newValue);
    const selectedValue = getSelectedLeafValues(options, newValue);
    onChange(selectedValue, selectedTreeNode);
  };

  const handleExpand = (nodeValue: string | number) => {
    setExpanded((prev) => ({ ...prev, [String(nodeValue)]: !prev[String(nodeValue)] }));
  };

  const selectedLabels = useMemo(() => getSelectedLabels(options, value), [options, value]);
  let displayValue = '';
  if (selectedLabels.length === 0) displayValue = placeholder || t('noneSelected');
  else if (selectedLabels.length <= 2) displayValue = selectedLabels.join(', ');
  else displayValue = `${selectedLabels.slice(0, 2).join(', ')} +${selectedLabels.length - 2}`;

  const renderSelectAll = () => {
    if (!isShowSelectAll) return null;
    return (
      <div
        style={{
          display: 'flex',
          alignItems: 'center',
          minHeight: 26,
          maxHeight: 32,
          fontSize: 13,
          cursor: 'pointer',
          border: 'none',
          background: 'none',
          outline: 'none',
          userSelect: 'none',
          borderRadius: 0,
          transition: 'background 0.2s',
        }}
        tabIndex={-1}
        onMouseDown={(e) => e.preventDefault()}
        onClick={handleSelectAll}
        onMouseEnter={(e) => (e.currentTarget.style.background = '#F5F5F5')}
        onMouseLeave={(e) => (e.currentTarget.style.background = '')}
      >
        <input
          type="checkbox"
          checked={allChecked}
          readOnly
          style={{ marginRight: 6, marginLeft: 16, width: 20, height: 20 }}
        />
        <span>{t('selectAll')}</span>
      </div>
    );
  };

  const renderTree = (nodes: TreeNodeOption[], level = 0) => {
    return (
      <ul style={{ paddingLeft: 0, listStyle: 'none', margin: 0 }}>
        {nodes.map((node) => {
          const hasChildren = !!node.children && node.children.length > 0;
          let checked = false;
          if (hasChildren) {
            const childrenValues = getAllValues(node.children!);
            checked = childrenValues.some((v) => value.includes(v));
          } else {
            checked = value.includes(node.value);
          }
          return (
            <li key={String(node.value)} style={{ marginBottom: 0 }}>
              <div
                style={{
                  display: 'flex',
                  alignItems: 'center',
                  minHeight: 26,
                  maxHeight: 32,
                  fontSize: 13,
                  cursor: 'pointer',
                  border: 'none',
                  background: 'none',
                  outline: 'none',
                  userSelect: 'none',
                  borderRadius: 0,
                  transition: 'background 0.2s',
                  paddingLeft: level * 25,
                  justifyContent: 'space-between',
                }}
                tabIndex={-1}
                onMouseDown={(e) => e.preventDefault()}
                onClick={() => handleCheck(node)}
                onMouseEnter={(e) => (e.currentTarget.style.background = '#F5F5F5')}
                onMouseLeave={(e) => (e.currentTarget.style.background = '')}
              >
                <span style={{ display: 'flex', alignItems: 'center', flex: 1 }}>
                  <span style={{ width: 16 }}>
                    {isAllowCollapseExpand && hasChildren && (
                      <span
                        style={{ cursor: 'pointer', fontSize: 10, color: '#888' }}
                        onClick={(e) => {
                          e.stopPropagation();
                          handleExpand(node.value);
                        }}
                      >
                        {expanded[String(node.value)] ? '▼' : '▶'}
                      </span>
                    )}
                    {!isAllowCollapseExpand && hasChildren && (
                      <span style={{ fontSize: 13, color: '#888', marginLeft: 0 }}>▼</span>
                    )}
                  </span>
                  <input
                    type="checkbox"
                    checked={checked}
                    readOnly
                    style={{ marginRight: 6, width: 20, height: 20 }}
                  />
                  <span>{node.label}</span>
                </span>
              </div>
              {hasChildren && expanded[String(node.value)] && renderTree(node.children!, level + 1)}
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
        style={{
          display: 'flex',
          alignItems: 'center',
          cursor: 'pointer',
          userSelect: 'none',
          paddingRight: 32,
          paddingLeft: 8,
          border: '1px solid #dfdfdf',
          borderRadius: 3,
          height: 28,
          background: '#fff',
        }}
        onClick={() => setDropdownOpen((open) => !open)}
      >
        <span
          style={{
            flex: 1,
            color: '#222',
            whiteSpace: 'nowrap',
            overflow: 'hidden',
            textOverflow: 'ellipsis',
            display: 'inline-block',
          }}
        >
          {displayValue}
        </span>
        <span
          style={{
            position: 'absolute',
            right: 8,
            fontSize: 10,
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
            padding: '0 5px',
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
          <div style={{ maxHeight: 400, overflowY: 'auto', padding: 0 }}>
            {renderSelectAll()}
            {renderTree(filteredOptions)}
          </div>
        </div>
      )}
    </div>
  );
};

export default TreeSelect;

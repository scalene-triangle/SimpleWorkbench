type ManualTreeNode = {
  id: string;
  title: string;
  children?: ManualTreeNode[];
};

type ManualTreeProps = {
  nodes: ManualTreeNode[];
};

function renderNodes(nodes: ManualTreeNode[]) {
  return (
    <ul>
      {nodes.map((node) => (
        <li key={node.id}>
          {node.title}
          {node.children && node.children.length > 0 ? renderNodes(node.children) : null}
        </li>
      ))}
    </ul>
  );
}

export function ManualTree({ nodes }: ManualTreeProps) {
  return (
    <aside aria-label="Manual Tree" className="sidebar-panel">
      <h3>Manual Tree</h3>
      {renderNodes(nodes)}
    </aside>
  );
}

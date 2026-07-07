import { useState } from "react";
import { itemRegistry, type NoteItem, type NoteItemType } from "./itemRegistry";

export type { NoteItem } from "./itemRegistry";

function createItem(type: NoteItemType): NoteItem {
  return {
    id: `${type}-${crypto.randomUUID()}`,
    type,
    collapsed: false
  };
}

type NoteBuilderProps = {
  initialItems: NoteItem[];
};

export function NoteBuilder({ initialItems }: NoteBuilderProps) {
  const [items, setItems] = useState<NoteItem[]>(initialItems);
  const [menuOpen, setMenuOpen] = useState(false);

  const toggleCollapsed = (id: string) => {
    setItems((current) =>
      current.map((item) => (item.id === id ? { ...item, collapsed: !item.collapsed } : item))
    );
  };

  const moveItem = (id: string, direction: -1 | 1) => {
    setItems((current) => {
      const index = current.findIndex((item) => item.id === id);
      if (index < 0) return current;

      const targetIndex = index + direction;
      if (targetIndex < 0 || targetIndex >= current.length) return current;

      const next = [...current];
      const [picked] = next.splice(index, 1);
      next.splice(targetIndex, 0, picked);
      return next;
    });
  };

  const addItem = (type: NoteItemType) => {
    setItems((current) => [...current, createItem(type)]);
    setMenuOpen(false);
  };

  return (
    <section>
      <button type="button" onClick={() => setMenuOpen((open) => !open)}>
        + Add Item
      </button>

      {menuOpen ? (
        <ul role="menu" aria-label="Add note item menu">
          {itemRegistry.map((entry) => (
            <li key={entry.type}>
              <button type="button" role="menuitem" onClick={() => addItem(entry.type)}>
                {entry.label}
              </button>
            </li>
          ))}
        </ul>
      ) : null}

      {items.map((item) => (
        <article data-testid="note-item" key={item.id}>
          <button type="button" onClick={() => toggleCollapsed(item.id)}>
            {item.collapsed ? "Expand" : "Collapse"}
          </button>
          <button type="button" onClick={() => moveItem(item.id, -1)}>
            Move Up
          </button>
          <button type="button" onClick={() => moveItem(item.id, 1)}>
            Move Down
          </button>
          <span>{item.type}</span>
        </article>
      ))}
    </section>
  );
}

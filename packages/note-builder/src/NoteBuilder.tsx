import { useEffect, useState } from "react";
import { itemRegistry, type NoteItem, type NoteItemType } from "./itemRegistry";

export type { NoteItem } from "./itemRegistry";

function createItem(type: NoteItemType): NoteItem {
  if (type === "secret") {
    return {
      id: `${type}-${crypto.randomUUID()}`,
      type,
      collapsed: false,
      key: "",
      value: ""
    };
  }

  return {
    id: `${type}-${crypto.randomUUID()}`,
    type,
    collapsed: false,
    text: ""
  };
}

type NoteBuilderProps = {
  initialItems: NoteItem[];
  onItemsChange?: (items: NoteItem[]) => void;
};

export function NoteBuilder({ initialItems, onItemsChange }: NoteBuilderProps) {
  const [items, setItems] = useState<NoteItem[]>(initialItems);
  const [menuOpen, setMenuOpen] = useState(false);

  useEffect(() => {
    setItems(initialItems);
  }, [initialItems]);

  useEffect(() => {
    onItemsChange?.(items);
  }, [items, onItemsChange]);

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

  const updateItem = (id: string, patch: Partial<NoteItem>) => {
    setItems((current) => current.map((item) => (item.id === id ? { ...item, ...patch } : item)));
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
        <article data-testid="note-item" data-item-id={item.id} key={item.id}>
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
          {!item.collapsed ? (
            item.type === "secret" ? (
              <div>
                <label>
                  Secret Key
                  <input
                    aria-label={`Secret Key ${item.id}`}
                    type="text"
                    value={item.key ?? ""}
                    onChange={(event) => updateItem(item.id, { key: event.target.value })}
                  />
                </label>
                <label>
                  Secret Value
                  <input
                    aria-label={`Secret Value ${item.id}`}
                    type="password"
                    value={item.value ?? ""}
                    onChange={(event) => updateItem(item.id, { value: event.target.value })}
                  />
                </label>
              </div>
            ) : (
              <label>
                Content
                <textarea
                  aria-label={`Content ${item.id}`}
                  value={item.text ?? ""}
                  onChange={(event) => updateItem(item.id, { text: event.target.value })}
                />
              </label>
            )
          ) : null}
        </article>
      ))}
    </section>
  );
}

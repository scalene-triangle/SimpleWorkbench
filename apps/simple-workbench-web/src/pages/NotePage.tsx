import { NoteBuilder } from "@simple-workbench/note-builder";
import { useEffect, useMemo, useState } from "react";
import type { NoteDto } from "../api";
import type { NoteItem } from "@simple-workbench/note-builder";

type NotePageProps = {
  note: NoteDto;
  onBack: () => void;
  onSave: (title: string, items: NoteItem[]) => Promise<void>;
  onToggleSaved: (isSaved: boolean) => Promise<void>;
};

function parseItems(documentJson: string): NoteItem[] {
  try {
    const payload = JSON.parse(documentJson) as { items?: NoteItem[] };
    if (!payload.items || !Array.isArray(payload.items) || payload.items.length === 0) {
      return [{ id: "seed-1", type: "plainText", collapsed: false }];
    }

    return payload.items.map((item) => ({
      id: item.id,
      type: item.type,
      collapsed: item.collapsed
    }));
  } catch {
    return [{ id: "seed-1", type: "plainText", collapsed: false }];
  }
}

export function NotePage({ note, onBack, onSave, onToggleSaved }: NotePageProps) {
  const [title, setTitle] = useState(note.title);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [items, setItems] = useState<NoteItem[]>(() => parseItems(note.documentJson));

  const initialItems = useMemo(() => items, [items]);

  useEffect(() => {
    setTitle(note.title);
    setItems(parseItems(note.documentJson));
  }, [note.id, note.title, note.documentJson]);

  const handleSave = async () => {
    setSaving(true);
    setError(null);
    try {
      await onSave(title.trim() || "Untitled note", items);
    } catch {
      setError("Unable to save note. Please retry.");
    } finally {
      setSaving(false);
    }
  };

  const handleToggleSaved = async () => {
    setSaving(true);
    setError(null);
    try {
      await onToggleSaved(!note.isSaved);
    } catch {
      setError("Unable to update saved state.");
    } finally {
      setSaving(false);
    }
  };

  return (
    <div className="app-shell">
      <main className="note-layout">
        <div className="note-toolbar">
          <button type="button" className="secondary-button" onClick={onBack}>
            Back
          </button>
          <input
            className="note-title-input"
            data-testid="note-title"
            value={title}
            onChange={(event) => setTitle(event.target.value)}
          />
          <button type="button" className="secondary-button" disabled={saving} onClick={handleToggleSaved}>
            {note.isSaved ? "Unsave" : "Save to Favorites"}
          </button>
          <button type="button" className="primary-button" disabled={saving} onClick={handleSave}>
            Save
          </button>
        </div>
        {error ? <div className="notice">{error}</div> : null}
        <section className="content-card">
          <h2>Note Builder</h2>
          <NoteBuilder initialItems={initialItems} onItemsChange={setItems} />
        </section>
      </main>
    </div>
  );
}

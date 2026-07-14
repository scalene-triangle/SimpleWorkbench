import { NoteBuilder } from "@simple-workbench/note-builder";
import { useMemo, useState } from "react";
import type { NoteDto } from "../api";

type NotePageProps = {
  note: NoteDto;
  onBack: () => void;
  onSave: (title: string) => Promise<void>;
  onToggleSaved: (isSaved: boolean) => Promise<void>;
};

export function NotePage({ note, onBack, onSave, onToggleSaved }: NotePageProps) {
  const [title, setTitle] = useState(note.title);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const initialItems = useMemo(() => [{ id: "seed-1", type: "plainText" as const, collapsed: false }], []);

  const handleSave = async () => {
    setSaving(true);
    setError(null);
    try {
      await onSave(title.trim() || "Untitled note");
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
          <NoteBuilder initialItems={initialItems} />
        </section>
      </main>
    </div>
  );
}

import { useEffect, useState } from "react";
import { createNote, getHome, getNote, updateNote, updateSaved, type HomeDto, type NoteDto } from "./api";
import { HomePage, type HomePageData } from "./pages/HomePage";
import { NotePage } from "./pages/NotePage";
import type { NoteItem } from "@simple-workbench/note-builder";

const homeFixture: HomeDto = {
  spaces: [{ id: "s1", name: "Main Space" }],
  savedNotes: [{ id: "n1", title: "Saved note", preview: "Saved note preview" }],
  recentNotes: [{ id: "n2", title: "Recent note", preview: "Recent note preview", lastViewedAt: "2026-07-09T06:30:00Z" }],
  globalNotes: [{ id: "n3", title: "Global note", preview: "Global note preview" }],
  smartFilters: {
    hasSaved: true,
    tags: [],
    priorities: [],
    statuses: []
  }
};

type LoadState = "loading" | "ready" | "error";
type ViewState = { type: "home" } | { type: "note"; noteId: string };

export function App() {
  const [state, setState] = useState<LoadState>("loading");
  const [data, setData] = useState<HomePageData>(homeFixture);
  const [view, setView] = useState<ViewState>({ type: "home" });
  const [note, setNote] = useState<NoteDto | null>(null);
  const [message, setMessage] = useState<string | null>(null);

  const loadHome = async () => {
    try {
      const payload = await getHome();
      setData(payload);
      setState("ready");
      setMessage(null);
    } catch {
      setData(homeFixture);
      setState("error");
      setMessage("API is currently unavailable. Showing local fixture data.");
    }
  };

  const loadNote = async (noteId: string) => {
    try {
      const payload = await getNote(noteId);
      setNote(payload);
      setMessage(null);
      setView({ type: "note", noteId });
    } catch {
      setMessage("Unable to load note.");
    }
  };

  useEffect(() => {
    void loadHome();
  }, []);

  if (state === "loading") {
    return (
      <div className="app-loading">
        <h1>Simple Workbench</h1>
        <p>Loading workspace...</p>
      </div>
    );
  }

  if (view.type === "note" && note) {
    return (
      <NotePage
        note={note}
        onBack={async () => {
          setView({ type: "home" });
          await loadHome();
        }}
        onSave={async (title, items) => {
          const documentJson = JSON.stringify({
            items: items.map((item: NoteItem) => ({
              id: item.id,
              type: item.type,
              collapsed: item.collapsed,
              text: item.text,
              key: item.key,
              value: item.value
            }))
          });

          const updated = await updateNote({
            ...note,
            title,
            documentJson
          });
          setNote(updated);
          await loadHome();
        }}
        onToggleSaved={async (isSaved) => {
          const updated = await updateSaved(note.id, isSaved);
          setNote(updated);
          await loadHome();
        }}
      />
    );
  }

  return (
    <HomePage
      data={data}
      notice={message}
      onCreateNote={async () => {
        try {
          const created = await createNote("Untitled note");
          await loadHome();
          await loadNote(created.id);
        } catch {
          setMessage("Unable to create note.");
        }
      }}
      onOpenNote={(noteId) => {
        void loadNote(noteId);
      }}
    />
  );
}

import { TopNav } from "../components/nav/TopNav";
import { ManualTree } from "../components/sidebar/ManualTree";
import { SmartFilters } from "../components/sidebar/SmartFilters";
import type { HomeDto, HomeItem } from "../api";

export type HomePageData = HomeDto;

type HomePageProps = {
  data: HomePageData;
  notice?: string | null;
  onCreateNote: () => void;
  onOpenNote: (noteId: string) => void;
};

function formatViewedAgo(lastViewedAt: string): string {
  const viewedAt = new Date(lastViewedAt);
  if (Number.isNaN(viewedAt.getTime())) {
    return "Viewed recently";
  }

  const diffMs = Math.max(0, Date.now() - viewedAt.getTime());
  const minutes = Math.floor(diffMs / 60_000);

  if (minutes < 1) {
    return "Viewed just now";
  }

  if (minutes < 60) {
    return `Viewed ${minutes}m ago`;
  }

  const hours = Math.floor(minutes / 60);
  if (hours < 24) {
    return `Viewed ${hours}h ago`;
  }

  const days = Math.floor(hours / 24);
  if (days < 7) {
    return `Viewed ${days}d ago`;
  }

  return "Viewed over a week ago";
}

function SectionList({
  title,
  items,
  onOpenNote,
  showLastViewed
}: {
  title: string;
  items: HomeItem[];
  onOpenNote: (noteId: string) => void;
  showLastViewed?: boolean;
}) {
  return (
    <section className="content-card">
      <h2>{title}</h2>
      {items.length === 0 ? (
        <p className="empty-text">No data available.</p>
      ) : (
        <ul>
          {items.map((note) => (
            <li key={note.id}>
              <button className="text-link-button" type="button" onClick={() => onOpenNote(note.id)}>
                {note.title}
              </button>
              {showLastViewed && note.lastViewedAt ? (
                <span className="note-meta-badge">{formatViewedAgo(note.lastViewedAt)}</span>
              ) : null}
              {note.preview ? <p className="note-preview">{note.preview}</p> : null}
            </li>
          ))}
        </ul>
      )}
    </section>
  );
}

export function HomePage({ data, notice, onCreateNote, onOpenNote }: HomePageProps) {
  return (
    <div className="app-shell">
      <TopNav onCreateNote={onCreateNote} />
      <main className="page-layout">
        <aside className="sidebar">
          <ManualTree
            nodes={data.spaces.map((space) => ({
              id: space.id,
              title: space.name
            }))}
          />
          <SmartFilters
            hasSaved={data.smartFilters.hasSaved}
            tags={data.smartFilters.tags}
            priorities={data.smartFilters.priorities}
            statuses={data.smartFilters.statuses}
          />
        </aside>
        <section className="content">
          {notice ? (
            <div className="notice" role="status">
              {notice}
            </div>
          ) : null}
          <section className="content-card">
            <h2>Spaces</h2>
            {data.spaces.length === 0 ? (
              <p className="empty-text">No spaces created yet.</p>
            ) : (
              <ul>
                {data.spaces.map((space) => (
                  <li key={space.id}>{space.name}</li>
                ))}
              </ul>
            )}
          </section>
          <SectionList title="Saved Notes" items={data.savedNotes} onOpenNote={onOpenNote} />
          <SectionList title="Recent Notes" items={data.recentNotes} onOpenNote={onOpenNote} showLastViewed />
          <section className="content-card">
            <h2>Global Notes</h2>
            {data.globalNotes.length === 0 ? (
              <p className="empty-text">No data available.</p>
            ) : (
              <ul>
                {data.globalNotes.map((note) => (
                  <li key={note.id}>
                    <button className="text-link-button" type="button" onClick={() => onOpenNote(note.id)}>
                      {note.title}
                    </button>
                    {note.preview ? <p className="note-preview">{note.preview}</p> : null}
                  </li>
                ))}
              </ul>
            )}
          </section>
        </section>
      </main>
    </div>
  );
}

import { TopNav } from "../components/nav/TopNav";
import { ManualTree } from "../components/sidebar/ManualTree";
import { SmartFilters } from "../components/sidebar/SmartFilters";

type HomeSpace = { id: string; name: string };
type HomeItem = { id: string; title: string };

export type HomePageData = {
  spaces: HomeSpace[];
  savedNotes: HomeItem[];
  recentNotes: HomeItem[];
  globalNotes: HomeItem[];
};

type HomePageProps = {
  data: HomePageData;
  notice?: string | null;
};

function SectionList({ title, items }: { title: string; items: HomeItem[] }) {
  return (
    <section className="content-card">
      <h2>{title}</h2>
      {items.length === 0 ? (
        <p className="empty-text">No data available.</p>
      ) : (
        <ul>
          {items.map((note) => (
            <li key={note.id}>{note.title}</li>
          ))}
        </ul>
      )}
    </section>
  );
}

export function HomePage({ data, notice }: HomePageProps) {
  return (
    <div className="app-shell">
      <TopNav />
      <main className="page-layout">
        <aside className="sidebar">
          <ManualTree
            nodes={data.spaces.map((space) => ({
              id: space.id,
              title: space.name
            }))}
          />
          <SmartFilters tags={[]} priorities={[]} statuses={[]} />
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
          <SectionList title="Saved Notes" items={data.savedNotes} />
          <SectionList title="Recent Notes" items={data.recentNotes} />
          <SectionList title="Global Notes" items={data.globalNotes} />
        </section>
      </main>
    </div>
  );
}

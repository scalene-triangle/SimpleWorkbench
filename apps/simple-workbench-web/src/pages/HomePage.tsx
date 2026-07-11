import { TopNav } from "../components/nav/TopNav";

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
};

export function HomePage({ data }: HomePageProps) {
  return (
    <>
      <TopNav />
      <main>
        <section>
          <h2>Spaces</h2>
          <ul>
            {data.spaces.map((space) => (
              <li key={space.id}>{space.name}</li>
            ))}
          </ul>
        </section>
        <section>
          <h2>Saved Notes</h2>
          <ul>
            {data.savedNotes.map((note) => (
              <li key={note.id}>{note.title}</li>
            ))}
          </ul>
        </section>
        <section>
          <h2>Recent Notes</h2>
          <ul>
            {data.recentNotes.map((note) => (
              <li key={note.id}>{note.title}</li>
            ))}
          </ul>
        </section>
        <section>
          <h2>Global Notes</h2>
          <ul>
            {data.globalNotes.map((note) => (
              <li key={note.id}>{note.title}</li>
            ))}
          </ul>
        </section>
      </main>
    </>
  );
}

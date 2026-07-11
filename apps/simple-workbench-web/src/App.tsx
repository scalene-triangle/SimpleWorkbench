import { HomePage } from "./pages/HomePage";

const homeFixture = {
  spaces: [{ id: "s1", name: "Main Space" }],
  savedNotes: [{ id: "n1", title: "Saved note" }],
  recentNotes: [{ id: "n2", title: "Recent note" }],
  globalNotes: [{ id: "n3", title: "Global note" }]
};

export function App() {
  return <HomePage data={homeFixture} />;
}

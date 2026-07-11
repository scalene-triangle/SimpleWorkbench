import { useEffect, useState } from "react";
import { HomePage, type HomePageData } from "./pages/HomePage";

const homeFixture: HomePageData = {
  spaces: [{ id: "s1", name: "Main Space" }],
  savedNotes: [{ id: "n1", title: "Saved note" }],
  recentNotes: [{ id: "n2", title: "Recent note" }],
  globalNotes: [{ id: "n3", title: "Global note" }]
};

type LoadState = "loading" | "ready" | "error";

export function App() {
  const [state, setState] = useState<LoadState>("loading");
  const [data, setData] = useState<HomePageData>(homeFixture);
  const [message, setMessage] = useState<string | null>(null);

  useEffect(() => {
    let active = true;

    const loadHome = async () => {
      try {
        const response = await fetch("/api/home", { headers: { Accept: "application/json" } });
        if (!response.ok) {
          throw new Error(`Home endpoint returned ${response.status}`);
        }

        const payload = (await response.json()) as HomePageData;
        if (!active) {
          return;
        }

        setData(payload);
        setState("ready");
      } catch {
        if (!active) {
          return;
        }

        setData(homeFixture);
        setState("error");
        setMessage("API is currently unavailable. Showing local fixture data.");
      }
    };

    void loadHome();
    return () => {
      active = false;
    };
  }, []);

  if (state === "loading") {
    return (
      <div className="app-loading">
        <h1>Simple Workbench</h1>
        <p>Loading workspace...</p>
      </div>
    );
  }

  return <HomePage data={data} notice={message} />;
}

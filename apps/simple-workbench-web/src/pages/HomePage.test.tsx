import { render, screen } from "@testing-library/react";
import { within } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { describe, expect, it, vi } from "vitest";
import { HomePage } from "./HomePage";

const fixture = {
  spaces: [{ id: "s1", name: "Main Space" }],
  savedNotes: [{ id: "n1", title: "Saved note", preview: "Saved preview", lastViewedAt: null }],
  recentNotes: [{ id: "n2", title: "Recent note", preview: "Recent preview", lastViewedAt: "2026-07-09T06:30:00Z" }],
  globalNotes: [{ id: "n3", title: "Global note", preview: "Global preview" }],
  smartFilters: {
    hasSaved: true,
    tags: [],
    priorities: [],
    statuses: []
  }
};

describe("HomePage", () => {
  it("renders global sections and fixed top nav", () => {
    vi.useFakeTimers();
    vi.setSystemTime(new Date("2026-07-09T07:30:00Z"));

    try {
      render(<HomePage data={fixture} onCreateNote={() => undefined} onOpenNote={() => undefined} />);

      expect(screen.getByRole("navigation")).toBeTruthy();
      expect(screen.getByText("Saved Notes")).toBeTruthy();
      expect(screen.getByText("Recent Notes")).toBeTruthy();
      expect(screen.getByText("Global Notes")).toBeTruthy();
      expect(screen.getByText("Saved preview")).toBeTruthy();
      expect(screen.getByText("Global preview")).toBeTruthy();
      expect(screen.getByText("Viewed 1h ago")).toBeTruthy();
    } finally {
      vi.useRealTimers();
    }
  });

  it("opens note from saved and recent sections", async () => {
    const user = userEvent.setup();
    const opened: string[] = [];

    render(<HomePage data={fixture} onCreateNote={() => undefined} onOpenNote={(id) => opened.push(id)} />);

    const savedSection = screen.getAllByRole("heading", { name: "Saved Notes" }).at(-1)?.closest("section");
    const recentSection = screen.getAllByRole("heading", { name: "Recent Notes" }).at(-1)?.closest("section");
    expect(savedSection).toBeTruthy();
    expect(recentSection).toBeTruthy();

    await user.click(within(savedSection!).getByRole("button", { name: "Saved note" }));
    await user.click(within(recentSection!).getByRole("button", { name: "Recent note" }));

    expect(opened).toEqual(["n1", "n2"]);
  });
});

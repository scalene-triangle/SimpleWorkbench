import { render, screen } from "@testing-library/react";
import { describe, expect, it } from "vitest";
import { HomePage } from "./HomePage";

const fixture = {
  spaces: [{ id: "s1", name: "Main Space" }],
  savedNotes: [{ id: "n1", title: "Saved note" }],
  recentNotes: [{ id: "n2", title: "Recent note" }],
  globalNotes: [{ id: "n3", title: "Global note" }],
  smartFilters: {
    hasSaved: true,
    tags: [],
    priorities: [],
    statuses: []
  }
};

describe("HomePage", () => {
  it("renders global sections and fixed top nav", () => {
    render(<HomePage data={fixture} onCreateNote={() => undefined} onOpenNote={() => undefined} />);

    expect(screen.getByRole("navigation")).toBeTruthy();
    expect(screen.getByText("Saved Notes")).toBeTruthy();
    expect(screen.getByText("Recent Notes")).toBeTruthy();
    expect(screen.getByText("Global Notes")).toBeTruthy();
  });
});

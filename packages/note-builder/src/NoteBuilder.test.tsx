import { render, screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { describe, expect, it } from "vitest";
import { NoteBuilder } from "./NoteBuilder";

describe("NoteBuilder", () => {
  it("adds codeBlock item and reorders items", async () => {
    const user = userEvent.setup();

    render(<NoteBuilder initialItems={[{ id: "a", type: "plainText", collapsed: false }]} />);

    await user.click(screen.getByRole("button", { name: "+ Add Item" }));
    await user.click(screen.getByRole("menuitem", { name: "Code Block" }));

    expect(screen.getAllByTestId("note-item")).toHaveLength(2);

    await user.click(screen.getAllByRole("button", { name: "Move Up" })[1]);

    const items = screen.getAllByTestId("note-item");
    expect(items[0].textContent).toContain("codeBlock");
    expect(items[1].textContent).toContain("plainText");
  });
});

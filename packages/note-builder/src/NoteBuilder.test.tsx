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

  it("emits text and secret fields through onItemsChange", async () => {
    const user = userEvent.setup();
    const changes: Array<unknown> = [];

    render(
      <NoteBuilder
        initialItems={[
          { id: "plain-1", type: "plainText", collapsed: false, text: "" },
          { id: "secret-1", type: "secret", collapsed: false, key: "", value: "" }
        ]}
        onItemsChange={(items) => changes.push(items)}
      />
    );

    await user.type(screen.getByLabelText("Content plain-1"), "hello");
    await user.type(screen.getByLabelText("Secret Key secret-1"), "DB_PASSWORD");
    await user.type(screen.getByLabelText("Secret Value secret-1"), "abc123");

    const last = changes.at(-1) as Array<{ id: string; text?: string; key?: string; value?: string }>;
    expect(last.find((x) => x.id === "plain-1")?.text).toContain("hello");
    expect(last.find((x) => x.id === "secret-1")?.key).toContain("DB_PASSWORD");
    expect(last.find((x) => x.id === "secret-1")?.value).toContain("abc123");
  });
});

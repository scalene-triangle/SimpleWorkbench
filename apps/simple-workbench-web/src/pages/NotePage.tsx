import { NoteBuilder } from "@simple-workbench/note-builder";

export function NotePage() {
  return (
    <main>
      <h1>Note</h1>
      <NoteBuilder initialItems={[{ id: "seed-1", type: "plainText", collapsed: false }]} />
    </main>
  );
}

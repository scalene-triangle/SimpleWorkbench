export type NoteItemType = "plainText" | "richText" | "codeBlock" | "link" | "secret";

export type NoteItem = {
  id: string;
  type: NoteItemType;
  collapsed: boolean;
};

export const itemRegistry: Array<{ label: string; type: NoteItemType }> = [
  { label: "Plain Text", type: "plainText" },
  { label: "Rich Text", type: "richText" },
  { label: "Code Block", type: "codeBlock" },
  { label: "Link", type: "link" },
  { label: "Secret", type: "secret" }
];

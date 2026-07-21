export type HomeSpace = { id: string; name: string };
export type HomeItem = { id: string; title: string; preview?: string };

export type HomeDto = {
  spaces: HomeSpace[];
  savedNotes: HomeItem[];
  recentNotes: HomeItem[];
  globalNotes: HomeItem[];
  smartFilters: {
    hasSaved: boolean;
    tags: string[];
    priorities: string[];
    statuses: string[];
  };
};

export type NoteDto = {
  id: string;
  title: string;
  version: number;
  documentJson: string;
  searchText: string;
  isSaved: boolean;
};

async function parseJson<T>(response: Response): Promise<T> {
  if (!response.ok) {
    throw new Error(`Request failed: ${response.status}`);
  }

  return (await response.json()) as T;
}

export async function getHome(): Promise<HomeDto> {
  const response = await fetch("/api/home", { headers: { Accept: "application/json" } });
  return parseJson<HomeDto>(response);
}

export async function createNote(title: string): Promise<NoteDto> {
  const response = await fetch("/api/notes", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Accept: "application/json"
    },
    body: JSON.stringify({ title })
  });

  return parseJson<NoteDto>(response);
}

export async function getNote(id: string): Promise<NoteDto> {
  const response = await fetch(`/api/notes/${id}`, { headers: { Accept: "application/json" } });
  return parseJson<NoteDto>(response);
}

export async function updateNote(note: NoteDto): Promise<NoteDto> {
  const response = await fetch(`/api/notes/${note.id}`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
      Accept: "application/json"
    },
    body: JSON.stringify({
      title: note.title,
      version: note.version,
      documentJson: note.documentJson,
      searchText: note.searchText,
      isSaved: note.isSaved
    })
  });

  return parseJson<NoteDto>(response);
}

export async function updateSaved(id: string, isSaved: boolean): Promise<NoteDto> {
  const response = await fetch(`/api/notes/${id}/saved`, {
    method: "PATCH",
    headers: {
      "Content-Type": "application/json",
      Accept: "application/json"
    },
    body: JSON.stringify({ isSaved })
  });

  return parseJson<NoteDto>(response);
}

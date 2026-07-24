import { useEffect, useState } from "react";
import { searchLexical, type SearchResultItem } from "../../api";

export const jumpToMatchedItem = (itemId: string) => {
  const element = document.querySelector(`[data-item-id="${itemId}"]`);
  if (!element) {
    return;
  }

  element.scrollIntoView({ behavior: "smooth", block: "center" });
  element.classList.add("highlight");
};

type SearchState = {
  query: string;
  results: SearchResultItem[];
  loading: boolean;
};

export function useSearchResults(): SearchState & { setQuery: (query: string) => void } {
  const [query, setQuery] = useState("");
  const [results, setResults] = useState<SearchResultItem[]>([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    const normalized = query.trim();
    if (!normalized) {
      setResults([]);
      setLoading(false);
      return;
    }

    let cancelled = false;
    setLoading(true);

    const timer = setTimeout(() => {
      void searchLexical(normalized)
        .then((items) => {
          if (!cancelled) {
            setResults(items);
          }
        })
        .catch(() => {
          if (!cancelled) {
            setResults([]);
          }
        })
        .finally(() => {
          if (!cancelled) {
            setLoading(false);
          }
        });
    }, 200);

    return () => {
      cancelled = true;
      clearTimeout(timer);
    };
  }, [query]);

  return {
    query,
    results,
    loading,
    setQuery
  };
}

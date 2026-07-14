type TopNavProps = {
  onCreateNote: () => void;
};

export function TopNav({ onCreateNote }: TopNavProps) {
  return (
    <header className="top-nav">
      <nav aria-label="Global Navigation" className="top-nav__content">
        <a className="top-nav__brand" href="/">
          Simple Workbench
        </a>
        <button type="button" className="primary-button" onClick={onCreateNote}>
          New Note
        </button>
      </nav>
    </header>
  );
}

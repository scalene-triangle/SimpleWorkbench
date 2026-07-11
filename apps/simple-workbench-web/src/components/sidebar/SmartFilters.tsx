type SmartFiltersProps = {
  tags: string[];
  priorities: string[];
  statuses: string[];
};

export function SmartFilters({ tags, priorities, statuses }: SmartFiltersProps) {
  return (
    <aside aria-label="Smart Filters">
      <h3>Smart Filters</h3>

      <section>
        <h4>Saved</h4>
      </section>

      <section>
        <h4>Tags</h4>
        <ul>
          {tags.map((tag) => (
            <li key={tag}>{tag}</li>
          ))}
        </ul>
      </section>

      <section>
        <h4>Priorities</h4>
        <ul>
          {priorities.map((priority) => (
            <li key={priority}>{priority}</li>
          ))}
        </ul>
      </section>

      <section>
        <h4>Statuses</h4>
        <ul>
          {statuses.map((status) => (
            <li key={status}>{status}</li>
          ))}
        </ul>
      </section>
    </aside>
  );
}

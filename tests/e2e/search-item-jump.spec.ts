import { expect, test } from "@playwright/test";

test("search result click jumps to matched item", async ({ page }) => {
  await page.setContent(`
    <main>
      <input data-testid="global-search" />
      <button
        data-testid="search-result-n1"
        onclick="
          const el = document.querySelector('[data-item-id=\\'item-3\\']');
          el.scrollIntoView({ behavior: 'auto', block: 'center' });
          el.classList.add('highlight');
        "
      >
        Result N1
      </button>
      <article data-item-id="item-3">Target item</article>
    </main>
  `);

  await page.fill('[data-testid="global-search"]', "redis");
  await page.click('[data-testid="search-result-n1"]');

  await expect(page.locator('[data-item-id="item-3"]')).toHaveClass(/highlight/);
});

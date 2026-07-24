import { expect, test } from "@playwright/test";

test("search result click jumps to matched item", async ({ page }) => {
  const note = {
    id: "n1",
    title: "Redis setup",
    version: 1,
    searchText: "redis setup guide",
    isSaved: false,
    documentJson: JSON.stringify({
      items: [
        { id: "item-1", type: "plainText", collapsed: false, text: "intro" },
        { id: "item-3", type: "plainText", collapsed: false, text: "redis setup guide" }
      ]
    })
  };

  await page.route("**/api/home", async (route) => {
    await route.fulfill({
      status: 200,
      contentType: "application/json",
      body: JSON.stringify({
        spaces: [{ id: "s1", name: "Main Space" }],
        savedNotes: [],
        recentNotes: [],
        globalNotes: [{ id: "n1", title: "Redis setup", preview: "redis setup guide" }],
        smartFilters: { hasSaved: false, tags: [], priorities: [], statuses: [] }
      })
    });
  });

  await page.route("**/api/search/lexical?*", async (route) => {
    await route.fulfill({
      status: 200,
      contentType: "application/json",
      body: JSON.stringify({
        items: [{ noteId: "n1", title: "Redis setup", score: 2.5, matchedItemId: "item-3" }]
      })
    });
  });

  await page.route("**/api/notes/n1", async (route) => {
    await route.fulfill({
      status: 200,
      contentType: "application/json",
      body: JSON.stringify(note)
    });
  });

  await page.goto("/");
  await page.fill('[data-testid="global-search"]', "redis");
  await page.waitForSelector('[data-testid="search-result-n1"]');
  await page.click('[data-testid="search-result-n1"]');
  await expect(page.locator('[data-item-id="item-3"]')).toHaveClass(/highlight/);
});

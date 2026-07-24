import { expect, test } from "@playwright/test";

test("create note save and reopen", async ({ page }) => {
  let current = {
    id: "n1",
    title: "Untitled note",
    version: 1,
    searchText: "",
    isSaved: false,
    documentJson: JSON.stringify({ items: [{ id: "item-1", type: "plainText", collapsed: false, text: "" }] })
  };

  await page.route("**/api/home", async (route) => {
    await route.fulfill({
      status: 200,
      contentType: "application/json",
      body: JSON.stringify({
        spaces: [{ id: "s1", name: "Main Space" }],
        savedNotes: [],
        recentNotes: [],
        globalNotes: [{ id: current.id, title: current.title, preview: current.searchText }],
        smartFilters: { hasSaved: false, tags: [], priorities: [], statuses: [] }
      })
    });
  });

  await page.route("**/api/notes", async (route) => {
    if (route.request().method() === "POST") {
      current = { ...current, version: 1, title: "Untitled note" };
      await route.fulfill({ status: 201, contentType: "application/json", body: JSON.stringify(current) });
      return;
    }

    await route.fallback();
  });

  await page.route("**/api/notes/n1", async (route) => {
    const method = route.request().method();
    if (method === "GET") {
      await route.fulfill({ status: 200, contentType: "application/json", body: JSON.stringify(current) });
      return;
    }

    if (method === "PUT") {
      const payload = route.request().postDataJSON() as {
        title: string;
        version: number;
        documentJson: string;
        isSaved: boolean;
      };
      current = {
        ...current,
        title: payload.title,
        version: payload.version + 1,
        documentJson: payload.documentJson,
        isSaved: payload.isSaved
      };
      await route.fulfill({ status: 200, contentType: "application/json", body: JSON.stringify(current) });
      return;
    }

    await route.fallback();
  });

  await page.route("**/api/notes/n1/saved", async (route) => {
    await route.fulfill({ status: 200, contentType: "application/json", body: JSON.stringify(current) });
  });

  await page.goto("/");
  await page.click("text=Global Notes");
  await page.click("text=New Note");
  await page.fill('[data-testid="note-title"]', "Smoke Note");
  const saveRequest = page.waitForResponse(
    (response) => response.url().includes("/api/notes/n1") && response.request().method() === "PUT"
  );
  await page.getByRole("button", { name: /^Save$/ }).click();
  await saveRequest;
  await page.reload();
  await expect(page.locator("text=Smoke Note")).toBeVisible();
});

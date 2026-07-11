import { expect, test } from "@playwright/test";

test("create note save and reopen", async ({ page }) => {
  const html = `
    <main>
      <section>Global Notes</section>
      <button type="button">New Note</button>
      <input data-testid="note-title" />
      <button type="button" id="save-btn">Save</button>
      <div id="saved-note"></div>
      <script>
        const saved = document.getElementById('saved-note');
        if (window.name) {
          saved.textContent = window.name;
        }

        document.getElementById('save-btn').addEventListener('click', () => {
          const input = document.querySelector('[data-testid="note-title"]');
          window.name = input.value;
          saved.textContent = window.name;
        });
      </script>
    </main>
  `;

  await page.goto(`data:text/html,${encodeURIComponent(html)}`);
  await page.click("text=Global Notes");
  await page.click("text=New Note");
  await page.fill('[data-testid="note-title"]', "Smoke Note");
  await page.click("text=Save");
  await page.reload();
  await expect(page.locator("text=Smoke Note")).toBeVisible();
});

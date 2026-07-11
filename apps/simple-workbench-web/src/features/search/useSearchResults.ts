export const jumpToMatchedItem = (itemId: string) => {
  const element = document.querySelector(`[data-item-id="${itemId}"]`);
  if (!element) {
    return;
  }

  element.scrollIntoView({ behavior: "smooth", block: "center" });
  element.classList.add("highlight");
};

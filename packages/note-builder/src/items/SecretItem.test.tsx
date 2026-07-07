import { render, screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { describe, expect, it } from "vitest";
import { SecretItem } from "./SecretItem";

describe("SecretItem", () => {
  it("masks secret value by default and reveals on click", async () => {
    const user = userEvent.setup();

    render(<SecretItem secretKey="DB_PASSWORD" secretValue="p@ss" />);

    expect(screen.getByText("••••••")).toBeTruthy();
    await user.click(screen.getByRole("button", { name: "Reveal" }));
    expect(screen.getByText("p@ss")).toBeTruthy();
  });
});

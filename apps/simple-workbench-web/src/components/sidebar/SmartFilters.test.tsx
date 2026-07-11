import { render, screen } from "@testing-library/react";
import { describe, expect, it } from "vitest";
import { SmartFilters } from "./SmartFilters";

describe("SmartFilters", () => {
  it("shows saved and dynamic tag filters", () => {
    render(<SmartFilters tags={["urgent"]} priorities={["P1"]} statuses={["todo"]} />);

    expect(screen.getByRole("heading", { name: "Saved" })).toBeTruthy();
    expect(screen.getByText("urgent")).toBeTruthy();
  });
});

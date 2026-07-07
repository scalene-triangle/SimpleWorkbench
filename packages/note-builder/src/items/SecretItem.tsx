import { useState } from "react";

type SecretItemProps = {
  secretKey: string;
  secretValue: string;
};

export function SecretItem({ secretKey, secretValue }: SecretItemProps) {
  const [revealed, setRevealed] = useState(false);

  return (
    <div>
      <strong>{secretKey}</strong>
      <span>{revealed ? secretValue : "••••••"}</span>
      <button type="button" onClick={() => setRevealed((current) => !current)}>
        {revealed ? "Hide" : "Reveal"}
      </button>
    </div>
  );
}

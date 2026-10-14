import { parseArgs } from "node:util";
import { runGodot } from "./lib/godot.mjs";

const DEFAULT_FRAMES = "120";

// Godot exits 0 on script errors and unhandled exceptions, so scan the output instead.
const ERROR_PATTERN = /^\s*(ERROR|SCRIPT ERROR|USER ERROR):|Unhandled exception/m;

const { values } = parseArgs({
  options: {
    frames: { type: "string", default: DEFAULT_FRAMES },
    scene: { type: "string" },
  },
});

const sceneArgs = values.scene ? ["--scene", values.scene] : [];
const { code, output } = await runGodot(
  ["--headless", "--quit-after", values.frames, ...sceneArgs],
  { capture: true },
);

if (code !== 0) {
  console.error(`\nsmoke: FAILED (Godot exited with code ${code})`);
  process.exit(code);
}

if (ERROR_PATTERN.test(output)) {
  console.error("\nsmoke: FAILED (errors in output)");
  process.exit(1);
}

console.log(`\nsmoke: OK (${values.frames} frames, no errors)`);

import { existsSync, mkdirSync } from "node:fs";
import { dirname, resolve } from "node:path";
import { parseArgs } from "node:util";
import { fail, ROOT, runGodotAndExit } from "../lib/godot.mjs";

const PRESETS_FILE = resolve(ROOT, "export_presets.cfg");
const USAGE = [
  "usage: npm run export -- <preset> <output path> [--debug] [--pack]",
  '  e.g. npm run export -- "macOS" builds/macos/Game.zip',
].join("\n");

const { values, positionals } = parseArgs({
  allowPositionals: true,
  options: {
    debug: { type: "boolean", default: false },
    pack: { type: "boolean", default: false },
  },
});

const [preset, output] = positionals;

if (!preset || !output) fail(USAGE);
if (values.pack && values.debug) fail("--pack and --debug can't be combined.");

if (!existsSync(PRESETS_FILE))
  fail("export_presets.cfg not found. Add presets in the editor (Project > Export) first.");

const outputPath = resolve(ROOT, output);
mkdirSync(dirname(outputPath), { recursive: true });

const mode = values.pack ? "--export-pack" : values.debug ? "--export-debug" : "--export-release";

await runGodotAndExit(["--headless", mode, preset, outputPath]);

import { runGodotAndExit } from "../lib/godot.mjs";

await runGodotAndExit(["--headless", "--build-solutions", "--quit"]);

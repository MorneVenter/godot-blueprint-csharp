import { runGodotAndExit } from "./lib/godot.mjs";

const [scene, ...rest] = process.argv.slice(2);
const sceneArgs = scene && !scene.startsWith("-") ? ["--scene", scene] : [];
const extraArgs = sceneArgs.length ? rest : process.argv.slice(2);

await runGodotAndExit([...sceneArgs, ...extraArgs]);

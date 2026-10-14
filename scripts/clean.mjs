import { rmSync } from "node:fs";
import { resolve } from "node:path";
import { ROOT } from "./lib/godot.mjs";

const GENERATED = [".godot", "bin", "obj"];

for (const folder of GENERATED) {
  rmSync(resolve(ROOT, folder), { recursive: true, force: true });
  console.log(`removed ${folder}/`);
}

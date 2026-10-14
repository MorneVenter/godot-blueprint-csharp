import { spawnSync } from "node:child_process";
import { existsSync, readFileSync } from "node:fs";
import { dirname, resolve } from "node:path";
import { fileURLToPath } from "node:url";

const ROOT = resolve(dirname(fileURLToPath(import.meta.url)), "../..");
const FORMATTED = /\.(cs|csproj)$/;

const input = JSON.parse(readFileSync(0, "utf8"));
const file = input.tool_input?.file_path;

if (file && FORMATTED.test(file) && existsSync(file))
  spawnSync("dotnet", ["csharpier", "format", file], { cwd: ROOT, stdio: "ignore" });

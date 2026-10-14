import { spawn } from "node:child_process";
import { existsSync } from "node:fs";
import { dirname, resolve } from "node:path";
import { fileURLToPath } from "node:url";

export const ROOT = resolve(dirname(fileURLToPath(import.meta.url)), "../..");

const ENV_FILE = resolve(ROOT, ".env");
const ENV_KEY = "GODOT_BIN";

export function resolveGodot() {
  if (!process.env[ENV_KEY] && existsSync(ENV_FILE)) process.loadEnvFile(ENV_FILE);

  const configured = process.env[ENV_KEY];

  if (!configured) fail(`${ENV_KEY} is not set. Copy .env.example to .env and set it.`);
  if (!existsSync(configured)) fail(`${ENV_KEY} points to a missing file: ${configured}`);

  return configured;
}

export function runGodot(args, { capture = false } = {}) {
  const bin = resolveGodot();
  const child = spawn(bin, ["--path", ROOT, ...args], {
    cwd: ROOT,
    stdio: capture ? ["inherit", "pipe", "pipe"] : "inherit",
  });

  let output = "";

  if (capture) {
    child.stdout.on("data", (chunk) => {
      process.stdout.write(chunk);
      output += chunk;
    });
    child.stderr.on("data", (chunk) => {
      process.stderr.write(chunk);
      output += chunk;
    });
  }

  return new Promise((done) => {
    child.on("error", (error) => {
      fail(`Could not start Godot ("${bin}"): ${error.message}`);
    });
    child.on("close", (code) => done({ code: code ?? 1, output }));
  });
}

export async function runGodotAndExit(args) {
  const { code } = await runGodot(args);
  process.exit(code);
}

export function fail(message) {
  console.error(`error: ${message}`);
  process.exit(1);
}

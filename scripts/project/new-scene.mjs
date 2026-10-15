import { randomInt } from "node:crypto";
import { existsSync, mkdirSync, readdirSync, readFileSync, rmSync, writeFileSync } from "node:fs";
import { basename, dirname, relative, resolve, sep } from "node:path";
import { fileURLToPath } from "node:url";
import { parseArgs } from "node:util";
import { fail, ROOT, runGodot } from "../lib/godot.mjs";

const USAGE = "usage: npm run new:scene -- <scenes/feature/Name.tscn> [--type Node2D]";
const SET_UID_SCRIPT = fileURLToPath(new URL("set_uid.gd", import.meta.url));
const PASCAL_CASE = /^[A-Z][A-Za-z0-9]*$/;
const SNAKE_CASE = /^[a-z][a-z0-9_]*$/;

const { values, positionals } = parseArgs({
  allowPositionals: true,
  options: { type: { type: "string", default: "Node" } },
});

const [target] = positionals;
if (!target) fail(USAGE);

const scenePath = resolve(ROOT, target.replace(/^res:\/\//, ""));
const relativePath = relative(ROOT, scenePath);
const folders = dirname(relativePath).split(sep);
const name = basename(scenePath, ".tscn");
const scriptPath = scenePath.replace(/\.tscn$/, ".cs");

if (!scenePath.endsWith(".tscn") || relativePath.startsWith("..")) fail(USAGE);
if (!PASCAL_CASE.test(name)) fail(`scene name must be PascalCase: ${name}`);
if (!PASCAL_CASE.test(values.type)) fail(`not a Godot type name: ${values.type}`);

const badFolder = folders.find((folder) => !SNAKE_CASE.test(folder));
if (badFolder) fail(`folders must be snake_case: ${badFolder}`);

for (const path of [scenePath, scriptPath])
  if (existsSync(path)) fail(`already exists: ${relative(ROOT, path)}`);

const toPascal = (folder) =>
  folder
    .split("_")
    .map((part) => part[0].toUpperCase() + part.slice(1))
    .join("");

const assemblyName = readFileSync(resolve(ROOT, "project.godot"), "utf8").match(
  /^project\/assembly_name="(.+)"$/m,
)?.[1];
if (!assemblyName) fail("project.godot has no [dotnet] project/assembly_name.");

const namespace = [assemblyName, ...folders.map(toPascal)].join(".");
const resPath = (path) => `res://${relative(ROOT, path).split(sep).join("/")}`;
const uidFile = `${scriptPath}.uid`;

function abort(message) {
  for (const path of [scriptPath, uidFile, scenePath]) rmSync(path, { force: true });
  if (readdirSync(dirname(scenePath)).length === 0) rmSync(dirname(scenePath), { recursive: true });
  fail(`${message}; removed the partially created files.`);
}

mkdirSync(dirname(scenePath), { recursive: true });
writeFileSync(
  scriptPath,
  `namespace ${namespace};

public partial class ${name} : ${values.type}
{
    public override void _Ready() { }
}
`,
);

const imported = await runGodot(["--headless", "--import"], { capture: true });
if (imported.code !== 0 || !existsSync(uidFile)) abort("import did not generate the script .uid");

const scriptUid = readFileSync(uidFile, "utf8").trim();

writeFileSync(
  scenePath,
  `[gd_scene format=3]

[ext_resource type="Script" uid="${scriptUid}" path="${resPath(scriptPath)}" id="1_script"]

[node name="${name}" type="${values.type}" unique_id=${randomInt(1, 2 ** 31)}]
script = ExtResource("1_script")
`,
);

const assigned = await runGodot(
  ["--headless", "--script", SET_UID_SCRIPT, "--", resPath(scenePath)],
  { capture: true },
);
const sceneUid = assigned.output.match(/uid:\/\/\w+/)?.[0];
if (assigned.code !== 0 || !sceneUid) abort("could not assign a scene uid");

// Godot only resolves the new scene uid after it is in the import cache.
const reimported = await runGodot(["--headless", "--import"], { capture: true });
if (reimported.code !== 0) abort("import after assigning the scene uid failed");

console.log(`\ncreated ${relative(ROOT, scenePath)} (${sceneUid})`);
console.log(`created ${relative(ROOT, scriptPath)} (${scriptUid})`);

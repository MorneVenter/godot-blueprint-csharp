# Slop

Slop is text or code that costs the reader more than it gives. One instance is harmless; the damage is accumulation. Each session adds a little, nobody deletes, and the repo drifts into walls of text, restated docs and layers nobody needs. Judge every addition by what the repo looks like after a hundred more like it.

## Growth

- Size is a cost. Prefer editing or deleting existing text and code over adding more; a change that leaves things shorter and clearer beats one that only adds.
- Before adding a doc line, look for the line it replaces, merges with or makes obsolete. If none, ask whether anyone would miss it.
- Don't document what the code, types or a check already enforce. Docs carry intent and constraints the code can't show.
- Each doc has one job: `README.md` tells a human how to use the template, `CLAUDE.md` is the agent's index, rules hold conventions for their files, skills hold workflows. Don't let one drift into another's job, and point instead of copying.
- A rule is one or two sentences. If it needs a paragraph, it's either several rules or something to enforce mechanically.
- No new doc files nobody asked for: summaries, notes, plans, changelogs, per-folder READMEs.

## Prose

- Lead with the fact. No preamble, restated questions, closing summaries or signposting.
- Replace a selling adjective with the property it claims, or delete it.
- Describe the current state. History ("now", "new", "no longer", "previously") lives in git.
- Formatting is for scanning: no decorative bold, no heading over a single paragraph, no single-item lists, no nested bullets that read as one sentence.
- ASCII punctuation: no em dashes, curly quotes, ellipsis characters, arrows or emoji.

## Comments

- The reader is an advanced developer fluent in C# and Godot. Comment only if deleting it would lead that reader into a mistake: an engine quirk, a workaround, a constraint the code can't show. When in doubt, leave it out.
- Never explain an idiom, justify the change you just made, narrate an edit, or leave banners, commented-out code, XML docs that restate the name, or TODOs.

## Code

- Build what was asked. No unrequested options, parameters, extension points or layers "for flexibility".
- Trust the types: no checks for states they rule out, no `catch` that hides a failure.
- No pass-through wrappers, no interface with one implementation and no test fake, no near-copy of an existing helper.
- Leave nothing behind: dead code, unused parameters, debug prints, stubs.
- Keep the diff to the task.

Run `/slop-review` on a diff to check it against this file.

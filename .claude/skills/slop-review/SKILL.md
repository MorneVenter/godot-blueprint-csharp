---
name: slop-review
description: Review a diff for slop (padded or redundant docs, needless comments, speculative code, repo growth that hurts readability) against .claude/rules/slop.md. Use when asked to review for slop or readability, or before committing doc or rule changes.
argument-hint: "[path or git ref, default: uncommitted changes]"
---

# Slop review

Scope: $ARGUMENTS. If empty, review `git diff HEAD` plus untracked files. Judge against `.claude/rules/slop.md`.

Read each changed doc in full, not just the hunk: redundancy is with text elsewhere in the file or in another doc. For each addition ask:

- Does it duplicate a fact owned elsewhere (code, config, another doc)?
- Would a reader miss it if it were deleted?
- Could it be shorter, merged into an existing line, or replaced by a mechanical check?
- Is it in the right doc for its job?
- Is the change net growth that a deletion elsewhere should offset?

## Report

List findings with `file:line`, the problem in one sentence and the cut or rewrite. Lead with the largest wins (whole sections or files that can go). Say plainly when the diff is clean. Don't fix anything unless asked.

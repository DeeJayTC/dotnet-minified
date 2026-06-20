## What & why

<!-- One or two lines: what does this change, and why? Link the issue it closes. -->

Closes #

## Type

- [ ] Bug fix (a helper now matches its long-form behavior)
- [ ] New helper / shortener
- [ ] New library or surface
- [ ] Docs / cheat sheet
- [ ] Build / CI / tooling

## Checklist

- [ ] `dotnet test Smoower.Minified.slnx` passes (net8.0 / net9.0 / net10.0).
- [ ] New behavior has tests, written with `Smoower.Minified.Testing` (`[F]`/`[Th]`, fluent assertions).
- [ ] The public contract is unchanged — routes, HTTP verbs, status codes, DTO/JSON/DB names stay stable.
- [ ] If I added or renamed a mapping: updated `docs/build.py` and re-ran `python docs/build.py`.
- [ ] If I added a new surface: updated `prompts/system-prompt.md`, the skill, and `CLAUDE.md`.
- [ ] The forbidden-token checker still passes (no long-form tokens crept into the samples).

## Token impact (new shorteners only)

<!-- The bar: a shortener ships only if it saves Claude tokens AND the model reaches for it.
     Show a before/after on a realistic snippet — see CONTRIBUTING.md → Measuring tokens. -->

| | long form | compact | saved |
| --- | --- | --- | --- |
|  |  |  |  |

---

By submitting this PR you agree your contribution is licensed under the repository's
source-available, non-compete [LICENSE](../LICENSE).

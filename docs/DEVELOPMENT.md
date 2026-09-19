# Development / 开发说明

## Structure

- `desktop/`: WPF UI, settings and isolated UI smoke checks.
- `core/`: connection, original-error preservation, PID-scoped control lease and atomic JSON I/O.
- `compatibility/`: local metadata analysis with Mono.Cecil and Roslyn; builds a minimal hook against installed game interfaces.
- `hook/`: dependency-isolated loader and main-thread playback using existing game input callbacks.
- `shared/`: chart validation and deterministic seeded input planner.
- `localization/`: Chinese/English catalogs and independent language preference.
- `tests/`: original synthetic charts, scheduler/lease validation and optional bootstrap probes.
- `vendor/`, `licenses/`: attributed third-party code and licenses.

## Build

Use Windows and .NET 8 SDK. `build.ps1 -Locked` restores pinned dependencies, builds and runs tests without game files. `package.ps1 -Locked` creates Portable/Lite EXEs and ZIPs, checks embedded identity, both UI languages, stop/close control leases, compact layouts and actual runtime configuration. It refuses to overwrite a completed release directory. GitHub Actions runs the same build/package pipeline.

Source tags and releases are published after reviewing the candidate artifacts; CI does not automatically send user notifications or publish a release.

## Optional local-client validation

This is an offline metadata/compiler check, not a connection to the game:

```powershell
.\dist\v0.2.0\BD2Rhythm-0.2.0-Lite-win-x64.exe --check-client "C:\YourGame\BrownDust II_Data\Managed" client-check.json
```

With Python installed, the dependency bootstrap can also run in an isolated process using your locally installed Mono runtime:

```powershell
.\tests\check-bootstrap.ps1 -GameDirectory "C:\YourGame" -Python python
```

It reproduces the original typed-loader TypeLoadException and checks the reflection-isolated loader with a synthetic dependency and the real bundled Harmony library. It neither opens nor injects into the game. Outputs remain under `.build/checks/` and must not be committed.

## Compatibility contract

The tool resolves the local chart, song clock, state, pause flag, input callbacks and update lifecycle. The current client's MVID is checked at load time against the component compiled for that same installation. It is not a hardcoded release-day version restriction. Missing or ambiguous interfaces abort adaptation before injection.

A chart is read into a local copy at song start. Unknown note types, invalid lanes/times, duplicate IDs, malformed hold endpoints and unsupported overlaps are rejected before input. The game chart and score are not modified. Supported numeric note types are 1 (tap), 10 (hold), 20 (slide), 30 (multi-tap). Any future type needs source analysis and tests before enabling it.

Control commands expire after five seconds and are scoped to a game PID and owner. Transient read failures retain the prior command only within that original lease. No lease is extended by a read failure. UI language changes write a separate preference and do not restart the input plan.

The bootstrap intentionally uses reflection and an `object` field so resolving Harmony happens before the runtime engine is JIT-compiled. Do not restore a typed RuntimeEngine field in Loader.

## Release validation scope

Version 0.2.0 uses Runtime3. Automated checks cover synthetic-chart scheduling, invalid-chart rejection, settings, leases, bilingual UI, both package editions and offline adaptation/bootstrap. A full in-game song with the new live-chart path remains `source_implemented_pending_runtime` until real playback evidence is collected. Offline checks must not be described as live-game verification.

Do not publish game libraries, charts, audio, extracted assets, account data, private logs or machine paths. Include dependency notices with releases. Keep technical validation details here, not in user-facing release notes.

# Castiel SDK

Castiel is an SDK and helper toolset for building and modding SDSG games. This repository contains the desktop tooling and documentation for creating a lightweight HTML/JS game project with prebuilt helpers.

This README is a concise reference. A full HTML documentation file is available at `docs/castiel_sdk_documentation.html` and can be copied to other repositories for distribution.

Key features

- Create or mod SDSG-compatible projects quickly
- Auto-generate project skeleton: `run.html`, `style.css`, `game.js`, `sdk.js`
- Live HTML preview and basic in-editor file operations
- Small, focused API for game loop, input, drawing, and file operations

---

## Quick start

1. Run `Castiel.exe` (desktop app) and point it to your SDSG project folder.
2. Use the UI to create a new game or open an existing one.
3. Edit `game.js` / `sdk.js` in the built-in editor and preview `run.html` live.

If you only need to use the SDK in a web project, copy `sdk.js` into your game's root and include it from `run.html`.

---

## Project layout created by Castiel

src/pai/assets/[GameName]/
- `run.html` — main boot file / demo runner
- `style.css` — default styles for the demo
- `game.js` — user game logic
- `sdk.js` — helper library (recommended)

---

## API reference (summary)

All examples assume `sdk.js` exposes a global `Castiel` object.

- `Castiel.game(definition)`
  - Registers your game with the engine. `definition` must include `update(dt)` and may include `init()` and `destroy()`.
  - Example:
    ```js
    Castiel.game({
      init() { Castiel.log('Game started'); },
      update(dt) { /* frame logic */ }
    });
    ```

- `Castiel.start()` — Starts the game loop. Typically called automatically but can be invoked manually.

- `Castiel.log(msg)` — Console + editor logging helper.
- `Castiel.speak(msg)` — Internal logging that may forward messages to the host/editor.

- File operations
  - `Castiel.createFile(path, content)` — Creates or overwrites a file. Returns a boolean or throws on host errors.
  - `Castiel.readFile(path)` — Reads a file; returns string or empty string on missing file. Prefer async patterns if exposed.

- Canvas & drawing
  - `Castiel.clear(color)` — Clear canvas with color (default `#1e1e1e`).
  - `Castiel.drawRect(x, y, w, h, color)` — Draw filled rectangle.
  - `Castiel.loadImage(src, callback)` — Loads an image and calls `callback(img)` on success. Use error handling for missing assets.
  - `Castiel.drawImage(img, x, y, w, h)` — Draw an image object previously loaded.

- Input
  - `Castiel.keyDown(key)` — Returns true while a key is pressed.
  - `Castiel.mouse()` — Returns `{ x, y, down }` with mouse state.

- Audio
  - `Castiel.loadAudio(src)` — Loads audio object (may return a Promise or audio element).
  - `Castiel.playSound(audio)` — Plays a loaded sound.

- Helpers
  - `new Castiel.Sprite(img, x, y, w, h)` — Sprite with `draw()` and simple properties.
  - `Castiel.rectCollide(a, b)` — Rectangle collision detection. Each rect: `{ x, y, w, h }`.

---

## Error handling and best practices

- Validate paths before calling file functions; prefer try/catch around host operations.
- For asset loading use callbacks or Promises and handle failures (fallback images/sounds).
- Release resources when not needed (`destroy()` hooks) to avoid memory leaks.
- Keep `update(dt)` fast and avoid blocking operations; use async tasks for IO.

Example pattern for loading assets safely:

```js
const assets = {};
Castiel.loadImage('player.png', img => { assets.player = img; });
// In update: if (!assets.player) return; // wait until loaded
```

---

## Documentation HTML

An HTML version of the SDK documentation is available at `docs/castiel_sdk_documentation.html`. You can copy that file into another repository (for example the repo that hosts the website or distribution) — it's standalone and links back to this README.

---

## Contributing

Contributions welcome. Open issues or PRs for bug fixes, API improvements, or documentation. Keep changes focused and include tests where possible.

---

## License

See `LICENSE` or the repository root for license details.
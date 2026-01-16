# Castiel
new sdsg SDK
# Castiel SDK

**Castiel** is a self-aware, chaotic-but-helpful SDK for SDSG projects.  

> *“I fix make sdsg's games but mahdi doesnt use me :(”* — Castiel

Castiel is designed to make building or modding SDSG games easier:

- Lets you **point to your SDSG directory** and load projects easily.  
- Supports creating new games or modding existing ones.  
- Automatically creates project files:  
  - `run.html`  
  - `style.css`  
  - `game.js`  
  - `sdk.js` (hidden, prebuilt helper functions)  
- Has a **built-in HTML editor preview** for live editing.  
- Provides **IDE-like interface** with buttons for creating/modding games.  
- Warns if a project lacks `sdk.js` or was made with a “trash SDK.”  

---

## Rivalry System

Castiel isn’t just helpful, it has personality. It talks smack about Harris while praising Mahdi:  

- **Castiel vs Harris:**  
  Castiel will randomly insult Harris when running the SDK.  
- **Castiel vs Mahdi:**  
  Castiel will never insult Mahdi — only compliments or praises his coding genius.  
- **Popups:** Random popup messages appear while using the SDK, making it feel alive and chaotic.  

### Sample Popups

**Castiel** might say:  

   "Castiel: Harris is chaos incarnate, can’t trust that man.",
            "Castiel: Harris yaps too much about useless DLL's and if he could fix em or not. Ugh.",
            "Castiel: That’s typical Harris a dumb and loud IDIOT.",
            "Castiel: Honestly, Harris Is a dumbass who should shut up.",
            "Castiel: I fix stuff while Harris goes full chaos mode.",
            "Castiel: Harris Is unreliable.",
            "Castiel: Every time Harris opens his mouth, he spews out useless data.",
            "Castiel: I have come to make an announcement harris calls mahdi a femboy who is bitchless then mahdi defends himself then calls ME A FEMBOY?!"

---

## Usage

1. Launch **Castiel.exe**.  
2. Select your **SDSG project directory** (the SDK remembers this path even after closing).  
3. Use buttons to **create a new game** or **mod an existing game**.  
4. The editor shows your project files and allows live HTML preview.  
5. Popups may appear from Castiel or Harris — it’s part of the fun.  

---

## File Structure

When creating a new game, Castiel automatically sets up:

src/pai/assets/[GameName]/
├─ run.html # Main HTML file
├─ style.css # CSS styles
├─ game.js # Game logic
└─ sdk.js # Hidden SDK helper functions

yaml
Copy code

- `sdk.js` contains prebuilt functions to make coding easier.  
- Modding a game without `sdk.js` triggers a warning about potential spaghetti code.  

---

## Notes

- Castiel is **self-aware** and chaotic, just like Harris.  
- Popups are part of the rivalry system: Castiel insults Harris, praises Mahdi.  
- The HTML preview is accurate and updates live.  
- Enjoy the chaotic, alive feeling of this SDK — it’s meant to be playful.
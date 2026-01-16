namespace Castiel
{
    static class Templates
    {
        public const string RunHtml = @"<!DOCTYPE html>
<html>
<head>
<meta charset='utf-8'>
<title>Castiel Game</title>
<link rel='stylesheet' href='style.css'>
<script src='sdk.js'></script>
<script src='game.js'></script>
</head>
<body>
<canvas id='game'></canvas>
</body>
</html>";

        public const string StyleCss = @"body {
    margin: 0;
    background: #1e1e1e;
    color: white;
    font-family: Consolas;
    overflow: hidden;
}
canvas {
    display: block;
}";

        // User-facing game file
        public const string GameJs = @"Castiel.game({
    init() {
        Castiel.log('Game initialized');
    },

    update(dt) {
        // dt = delta time in seconds
    }
});";

        // SDK core
        public const string SdkJs = @"(() => {
    const host = window.chrome?.webview?.hostObjects?.CastielHost;

    const Castiel = {
        _game: null,
        _lastTime: 0,

        game(def) {
            if (!def || typeof def.update !== 'function') {
                Castiel.speak('Invalid game definition.');
                return;
            }
            Castiel._game = def;
        },

        start() {
            Castiel.speak('Castiel runtime starting');
            if (Castiel._game?.init) {
                try { Castiel._game.init(); }
                catch (e) { Castiel.speak(e.toString()); }
            }
            requestAnimationFrame(Castiel._loop);
        },

        _loop(time) {
            const dt = (time - Castiel._lastTime) / 1000 || 0;
            Castiel._lastTime = time;

            try {
                Castiel._game?.update?.(dt);
            } catch (e) {
                Castiel.speak(e.toString());
            }

            requestAnimationFrame(Castiel._loop);
        },

        // Logging
        log(msg) {
            Castiel.speak(String(msg));
        },

        speak(msg) {
            console.log('Castiel:', msg);
            host?.CastielSpeak(String(msg));
        },

        createFile(path, content) {
            host?.CreateFile(path, String(content));
        },

        readFile(path) {
            return host?.ReadFile(path) ?? '';
        }
    };

    window.Castiel = Castiel;
    window.addEventListener('load', Castiel.start);
})();
";
    }
}

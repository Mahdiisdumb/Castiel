namespace Castiel
{
    static class Templates
    {
        public const string RunHtml = @"<!DOCTYPE html>
<html>
<head>
<meta charset='utf-8'>
<title>Castiel Game</title>
<script src='sdk.js'></script>
<script src='game.js'></script>
</head>
<body></body>
</html>";

        public const string StyleCss = @"body {
    margin: 0;
    background: #1e1e1e;
    color: white;
    font-family: Consolas;
}";

        public const string GameJs = @"Castiel.ready(() => {
    Castiel.log('Game started!');
});";

        public const string SdkJs = @"const Castiel = {
    ready(fn){ window.onload = fn; },
    log(msg){ console.log('Castiel:', msg); },
    speak(msg){ window.chrome.webview.hostObjects.CastielHost.CastielSpeak(msg); },
    createFile(path, content){ window.chrome.webview.hostObjects.CastielHost.CreateFile(path, content); }
};

// Self-aware rivalry messages
Castiel.speak(
'I HAVE COME TO MAKE AN ANNOUNCEMENT — HARRIS IS YAPPING AGAIN. Mahdiisdumb: STOP HIM!'
);";
    }
}
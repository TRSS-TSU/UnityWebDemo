# Unity Web Dashboard Demo

Proof of concept showing a Unity scene sending realtime tracked-object status to a local HTML dashboard.

## What is included

- `WebDemo/` - Unity project source.
- `Builds/` - built Windows player. Launch `Builds/WebDemo.exe`.
- `Dashboard/` - static web dashboard files and local Python server.

Prerequisite: Python 3 available as `py -3` or `python`.

## Run the demo from a clone

1. Open this folder in Unity Hub by selecting `WebDemo/`.
2. Press Play in Unity, or launch `Builds/WebDemo.exe`.
3. Start the dashboard server:

   ```powershell
   .\start_dashboard.bat
   ```

4. Open the dashboard:

   ```text
   http://localhost:8080/
   ```

5. Move the tracked object in Unity with WASD or the arrow keys.

The dashboard reads Unity telemetry from:

```text
http://localhost:8989/
```

Health check:

```text
http://localhost:8989/health
```

## Notes

- The Unity player is configured to launch windowed at `1024x768`.
- Keep the Unity app running while the dashboard is open.
- If port `8989` is already in use, change the `port` value on the `StatusWebServer` object in the Unity scene and update `Dashboard/app.js` to match.
- The dashboard server uses only Python's standard library.

## GitHub packaging

Commit this folder as the repository root:

```text
C:\Users\Admin\Documents\Unity\Web_Interface_Demo
```

The `.gitignore` excludes Unity-generated cache folders but keeps the Windows build folder so users can run the demo without building Unity first.

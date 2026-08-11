from http.server import SimpleHTTPRequestHandler, ThreadingHTTPServer
from pathlib import Path
import os


PORT = 8080


if __name__ == "__main__":
    os.chdir(Path(__file__).parent)
    server = ThreadingHTTPServer(("localhost", PORT), SimpleHTTPRequestHandler)
    print(f"Dashboard running at http://localhost:{PORT}/")
    print("Press Ctrl+C to stop.")
    server.serve_forever()

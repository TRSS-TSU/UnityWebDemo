from http.server import SimpleHTTPRequestHandler, ThreadingHTTPServer
from pathlib import Path
import os


PORTS = (8080, 8000, 5500, 0)


if __name__ == "__main__":
    os.chdir(Path(__file__).parent)
    last_error = None

    for port in PORTS:
        try:
            server = ThreadingHTTPServer(("127.0.0.1", port), SimpleHTTPRequestHandler)
            break
        except OSError as error:
            last_error = error
    else:
        raise last_error

    host, port = server.server_address
    print(f"Dashboard running at http://{host}:{port}/")
    print("Press Ctrl+C to stop.")
    server.serve_forever()

if (Get-Command py -ErrorAction SilentlyContinue) {
    py -3 .\Dashboard\server.py
} else {
    python .\Dashboard\server.py
}

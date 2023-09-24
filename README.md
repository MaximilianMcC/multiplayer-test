# multiplayer-test
Testing making online multiplayer games with SFML and C#

## How to use
1. Download `server.exe`, and `game.exe`.
1. Get one person to run `server.exe` with the port as an argument. Make sure the that the selected port has been port-forwarded for UDP. Example: `.\server.exe 12345`
1. Run the game with the first argument as the server ip, and the second as the port. Example: `.\game.exe 192.168.1.1 12345`
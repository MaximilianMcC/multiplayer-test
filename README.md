# multiplayer-test
Testing making online multiplayer games with SFML and C#

## How to use
1. Download `server.exe`, and `game.exe`.
1. Get one person to run `server.exe` with the port as an argument. Make sure the that the selected port has been port-forwarded for UDP. Example: `.\server.exe 12345`
1. Run the game with the first argument as the server ip, and the second as the port. The third arg should be the player name Example: `.\game.exe 192.168.1.1 12345 bob`


# Packet Structures
Different packets are sent/received that contain different data. Depending on the packet type the data inside of them is layed out differently. Below are all of the packet types:

1. User connecting
2. User disconnecting
3. User data being sent

Packets are sent as ASCII strings and their information is split with a comma. It's basically just sent as a CSV string.

## User connecting (type 0)
| Index | Description            | Example                               |
|:-----:|------------------------|---------------------------------------|
| 0     | Player connecting type | `0` *(Only `0` can be used here)*     |
| 1     | Player color           | `4278255615` *(`0xff00ffff`/magenta)* |
| 2     | Player username        | `"Bob"`                               |

## User disconnecting (type 1)
| Index | Description               | Example                           |
|:-----:|---------------------------|-----------------------------------|
| 0     | Player disconnecting type | `1` *(Only `1` can be used here)* |

## User updating data (type 2)
| Index | Description             | Example                                  |
|:-----:|-------------------------|------------------------------------------|
| 0     | Player data update type | `2` *(Only `2` can be used here)*        |
| 1     | Player UUID             | `"96c2cec8-bc70-4a7c-83d7-9d2e2ccd866d"` |
| 2     | Player username         | `"Bob"`                                  |
| 3     | Player X position       | `-456f`                                  |
| 4     | Player Y position       | `158.7f`                                 |
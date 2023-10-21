# Multiplayer test
Testing making online multiplayer games with SFML and C#

## How to use
1. Download `server.exe`, and `game.exe`.
1. Get one person to run `server.exe` with the port as an argument. Make sure the that the selected port has been port-forwarded for UDP. Example: `.\server.exe 12345`
1. Run the game with the first argument as the server ip, and the second as the port. The third arg should be the player name Example: `.\game.exe 192.168.1.1 12345 bob`


# Packet structure
> [!WARNING]
> This stuff changing like daily or something idk so watch out. Nothing final and prolly never will be🥱🥱 


All packet must start like this:
```
{packet id},{synchronization packet},{acknowledgement packet}

example:
4,0,0
```

---

### 1, 2 - *New client is joining*
This packet is sent a single time by a client when they join a server.
| Index | Description     | Datatype |
|:-----:|-----------------|----------|
| 0     | Packet type (1) | uint     |
| 1     | Player username | string   |
| 2     | Player color    | uint     |

This example packet will add a new player with the username of "Bob" and with a color of Magenta:
```
1,Bob,4278255615
```
Once sent to the server, a new packet will be sent back. This is a one-time use packet that contains the players UUID which will be used in every single packet from now on.
| Index | Description     | Datatype |
|:-----:|-----------------|----------|
| 0     | Packet type (2) | uint     |
| 1     | Player UUID     | string   |

This example packet shows what the server will send back:
```
2,8d3b2502-b039-483f-bc4c-98cf316c0286
```

---

### 3 - *Client is leaving*
This packet is sent as single time by a client when they leave a server.
| Index | Description     | Datatype |
|:-----:|-----------------|----------|
| 0     | Packet type (3) | uint     |
| 1     | Player UUID     | string   |

This example packet will remove the player with the UUID of 8d3b2502-b039-483f-bc4c-98cf316c0286 from the game/server.
```
3,8d3b2502-b039-483f-bc4c-98cf316c0286
```

--- 

### 4 - *Client is updating data*
This packet is sent every frame by the client. This packet contains any data that the player might need to transfer to other players.
| Index | Description     | Datatype |
|:-----:|-----------------|----------|
| 0     | Packet type (4) | uint     |
| 1     | Player UUID     | string   |
| 2     | Player X        | float    |
| 3     | Player Y        | float    |

This example packet will set the players position to (154.56, 384.67).
```
4,8d3b2502-b039-483f-bc4c-98cf316c0286,154.56,384.67
```
![freeman](https://i.imgur.com/Ca8X2mq.png)

---
# Normal packet header
| Handshake packet |
|---|
| `false` |

# Handshake packet header
<!-- TODO: See if its same amount of bytes to just encode as string -->
| Handshake packet | handshake stage | handshake guid |
|---|---|---|
| `true` | SYN = `0`, ACK = `1`, SYN-ACK = `2` | `0f9bcaca-6474-4a02-82d4-9419e2e14f12` (guid)|
| 1 byte| 1 byte | 16 bytes |
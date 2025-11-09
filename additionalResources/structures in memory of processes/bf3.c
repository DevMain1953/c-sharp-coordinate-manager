Bytes of instruction that writes new X coordinate to player:
0F 29 86 E0 00 00 00 8B 40 78

Bytes of instruction that writes X velocity to player:
0F 29 46 40 7D 0F

Offsets to addresses:
X Coordinate = X Coordinate + 0x0;
Z Coordinate = X Coordinate + 0x4;
Y Coordinate = X Coordinate + 0x8;

Z Velocity = X Velocity + 0x4;

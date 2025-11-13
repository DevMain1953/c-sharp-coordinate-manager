struct player {
   	xCoordinate = player + 0x1C;
	yCoordinate = player + 0x20;
	zCoordinate = player + 0x24;

	xVector = player + 0x28;
	yVector = player + 0x2C;
	zVector = player + 0x30;

	yawAngleInDegrees = player + 0x10C;
	noclip_mode = player + 0xAD54;
};

Bytes of instruction that reads value in address of noclip:
F6 80 54 AD 00 00 08

Mask and bytes to search signature in OllyDbg:
\xF6\x80\x54\xAD\x00\x00\x08 - bytes
xx????x - mask

structure_of_player = getAddressFrom(0x010A7190);

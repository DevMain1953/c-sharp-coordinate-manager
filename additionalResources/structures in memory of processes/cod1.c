struct player {
   	xCoordinate = player + 0x14;
	yCoordinate = player + 0x18;
	zCoordinate = player + 0x1C;

	xVector = player + 0x20;
	yVector = player + 0x24;
	zVector = player + 0x28;

	yawAngleInDegrees = player + (0x2 * 0x4) + 0xAC;
};

structure_of_player = getAddressFrom(0x0152BC64);
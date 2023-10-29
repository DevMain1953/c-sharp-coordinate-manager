struct player {
   	xCoordinate = player + 0x1C;
	yCoordinate = player + 0x20;
	zCoordinate = player + 0x24;

	xVector = player + 0x28;
	yVector = player + 0x2C;
	zVector = player + 0x30;

	yawAngleInDegrees = player + 0x10C;
};

structure_of_player = getAddressFrom(0x010A7190);
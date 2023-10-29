struct player {
   	playerBody = player + 0x948;
};

struct playerBody {
	xCoordinate = playerBody + 0x10;
	zCoordinate = playerBody + 0x14;
	yCoordinate = playerBody + 0x18;

	cosineOfYawAngle = playerBody + 0x4;
	sineOfYawAngle = playerBody + 0xC;

   	xVector = playerBody + 0x30;
	zVector = playerBody + 0x34;
	yVector = playerBody + 0x38;
};

structure_of_player = 8112212;

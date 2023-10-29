struct car {
	p_carBody = car + 0x14;
   	carBody = getAddressFrom(p_carBody);

   	xVector = car + 0x44;
   	yVector = car + 0x48;
   	zVector = car + 0x4C;
   	carID = car + 0x22; //datatype - 2 bytes
   	primaryColor = car + ; //datatype - byte
   	secondaryColor = car + ; //datatype - byte
};

struct carBody {
	xCoordinate = carBody + 0x30;
	yCoordinate = carBody + 0x34;
	zCoordinate = carBody + 0x38;

   	sineOfYawAngle = carBody + 0x10;
   	cosineOfYawAngle = carBody + 0x14;

   	sineOfRollAngle = carBody + 0x8;
   	-sineOfPitchAngle = carBody + 0x18;
};

struct player {
	p_playerBody = player + 0x14;
   	playerBody = getAddressFrom(p_playerBody);

   	p_car = player + 0x58C;
   	car = getAddressFrom(p_car);

   	p_touchedObject = player + ;
   	health = player + ;
   	armor = player + ;
};

struct playerBody {
	xCoordinate = playerBody + 0x30;
	yCoordinate = playerBody + 0x34;
	zCoordinate = playerBody + 0x38;
};

structure_of_player = 8839584;
structure_of_car = 8855896;
structure_of_helicopter = 8857272;
structure_of_plane = 8857984;

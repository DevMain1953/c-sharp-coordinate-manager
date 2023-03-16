struct car {
	xCoordinate = car + 0x34;
	yCoordinate = car + 0x38;
	zCoordinate = car + 0x3C;

   	xVector = car + 0x70;
   	yVector = car + 0x74;
   	zVector = car + 0x78;

   	carID = car + 0x5C;
   	primaryColor = car + ; //datatype - byte
   	secondaryColor = car + ; //datatype - byte
};

struct player {
	xCoordinate = player + ;
	yCoordinate = player + ;
	zCoordinate = player + ;

	p_car = player + 0x3A8;
	car = getAddressFrom(p_car);

	p_touchedObject = player + ;
	health = player + ;
	armor = player + ;
};

structure_of_player = 6901104;
structure_of_car = 6925712;
structure_of_helicopter = 6930972;
structure_of_plane = 6931448;

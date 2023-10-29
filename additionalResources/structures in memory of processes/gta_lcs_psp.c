struct car {
	xCoordinate = car + ;
	yCoordinate = car + ;
	zCoordinate = car + ;

	sineOfYawAngle = car + ;
  	cosineOfYawAngle = car + ;

	sineOfRollAngle = car + ;
	-sineOfPitchAngle = car + ;

	xVector = car + ;
	yVector = car + ;
	zVector = car + ;
	
	carID = car + ;
	primaryColor = car + ; //datatype - byte
	secondaryColor = car + ; //datatype - byte
	health = car + ;

	doorState = car + ;
	typeOfCar = car + ;
};

struct player {
	xCoordinate = player + ;
	yCoordinate = player + ;
	zCoordinate = player + ;

	sineOfYawAngle = car + ;
  	cosineOfYawAngle = car + ;
	
	xVector = player + ;
	yVector = player + ;
	zVector = player + ;

	p_car = player + ;
	car = getAddressFrom(p_car);

	p_touchedObject = player + ;
	health = player + ;
	armor = player + ;

	stateOfBody = player + ;
};

structure_of_player = ;
structure_of_car = ;
structure_of_helicopter = ;
structure_of_plane = ;
structure_of_boat = ;

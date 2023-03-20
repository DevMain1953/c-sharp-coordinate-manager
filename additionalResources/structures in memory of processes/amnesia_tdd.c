struct player {
   tinderboxes = player + 0x98;
   oilInLantern = player + 0x8C;
   health = player + 0x84;
   sanity = player + 0x88;

   p_playerBody = player + 0x54;
   playerBody = getAddressFrom(p_playerBody);
};

struct playerBody {
   xCoordinate = playerBody + 0x48;
   zCoordinate = playerBody + 0x4C;
   yCoordinate = playerBody + 0x50;

   cosineOfYawAngle = playerBody + 0x160;
   sineOfYawAngle = playerBody + 0x168;

   forwardVector = playerBody + 0x88;
   sideVector = playerBody + 0x8C;
   zVector = playerBody + 0xF4;
};

structure_of_player = 7171368;
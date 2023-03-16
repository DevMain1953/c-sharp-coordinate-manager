struct player {
   tinderboxes = player + 0x98;
   oilInLantern = player + 0x8C;
   health = player + 0x84;
   sanity = player + 0x88;

   p_playerBody = player + 0x54;
   playerBody = getAddressFrom(p_playerBody);
};

struct playerBody {
   zCoordinate = playerBody + 0x4C;

   forwardVector = playerBody + 0x88;
   sideVector = playerBody + 0x8C;
   zVector = playerBody + 0xF4;
};

structure_of_player = 7171368;
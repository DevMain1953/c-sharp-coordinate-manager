struct player {
	XCoordinate = player + 0x1C;
	YCoordinate = player + 0x20;
	zCoordinate = player + 0x24;

	ZVector = player + 0x30;

	noclip_mode = player + <find_out>;
};

Code that creates script that enables and disables noclip mode without updating value in address of noclip mode:
===============START
[ENABLE]
aobscanmodule(_no_clip_,iw6sp64_ship.exe,8B87????????????????????????8B3E??????A8047409)
 _no_clip_:
  db B8 01 00 00 00 90
registersymbol(_no_clip_)
[DISABLE]
 _no_clip_:
  db 8B 87 DC B6 00 00
unregistersymbol(_no_clip_)
================END

HP = 0000000143C91600 + 0x17C;

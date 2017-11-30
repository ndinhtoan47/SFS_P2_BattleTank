
namespace SFS_BattleTank.Constants
{
    public static class Consts
    {
        // button
        public static readonly byte MOUSEBUTTON_LEFT = 1;
        public static readonly byte MOUSEBUTTON_RIGHT = 2;
        public static readonly byte MOUSEBUTTON_MIDDLE = 3;
        // game settings
        public static int VIEWPORT_WIDTH = 0;
        public static int VIEWPORT_HEIGHT = 0;
        // config path
        public static readonly string SMARTFOX_CONFIG = @"../../../../sfs_config.xml";
        // game scene's name
        public static readonly string SCENE_PLAY = "playScene";
        // client proterties
        public static readonly string X = "x";
        public static readonly string Y = "y";
        public static readonly string VX = "vx";
        public static readonly string VY = "vy";
        public static readonly string XDIR = "xdir";
        public static readonly string YDIR = "ydir";


        public static readonly string ES_TANK = "tank";
        public static readonly string ES_BULLET = "bullet";

        // request from server
        public static readonly string CMD_ADD = "add";
        public static readonly string CMD_REMOVE = "remove";
        public static readonly string CMD_UPDATE_DATA = "updateData";

        // request from client
        public static readonly string CRQ_MOVE = "move";
        public static readonly string CRQ_FIRE = "fire";
        // extension path
        public static readonly string EXTS_ZONE = "ZoneExtension.jar";
        public static readonly string EXTS_ROOM = "RoomExtension.jar";
        public static readonly string EXTS_ZONE_MAINCLASS = "MainClassZoneExtension.java";
        public static readonly string EXTS_ROOM_MAINCLASS = "MainClassRoomExtension.java";

        // controller's name
        public static readonly string CTRL_TANK = "tankcontroller";
        public static readonly string CTRL_BULLET = "bulletcontroller";
    }
}

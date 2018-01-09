
namespace SFS_BattleTank.Constants
{
    public static class Consts
    {
        // button
        public static readonly byte MOUSEBUTTON_LEFT = 1;
        public static readonly byte MOUSEBUTTON_RIGHT = 2;
        public static readonly byte MOUSEBUTTON_MIDDLE = 3;
        // game settings
        public static int VIEWPORT_WIDTH = 800;
        public static int VIEWPORT_HEIGHT = 600;
        public static float VIEWPORT_SCALE_RATE_WIDTH = 1;
        public static float VIEWPORT_SCALE_RATE_HEIGHT = 1;
        // config path
        public static readonly string SMARTFOX_CONFIG = @"../../../../sfs_config.xml";
        // game scene's name
        public static readonly string SCENE_PLAY = "playScene";
        public static readonly string SCENE_LOGIN = "loginScene";
        public static readonly string SCENE_MENU = "menuScene";
        public static readonly string SCENE_ROOM = "roomScene";
        // client proterties's name
        public static readonly string X = "x";
        public static readonly string Y = "y";
        public static readonly string ROTATION = "rotation";
        public static readonly string ALIVE = "alive";
        public static readonly string DEATH = "death";
        public static readonly string KILL = "kill";
        public static readonly string ROOM_ONWER = "roomonwer";
        public static readonly string PRIMARY = "primary";
        public static readonly string MESSAGE = "message";
        public static readonly string CAN_PLAY = "canplay";
        public static readonly string TYPE = "type";
        public static readonly string DURATION = "duration";
        public static readonly string COUNT_DOWN = "countdown";
        public static readonly string ID_ARRAY = "idarray";
        public static readonly string READY_ARRAY = "readyarray";
        public static readonly string TYPE_ITEM = "item";
       // public static readonly string EFFECT_TIME = "effectTime";
        // data properties
        // essental
        public static readonly int ES_TANK = 11;
        public static readonly int ES_BULLET = 12;
        public static readonly int ES_ITEM_ARMOR = 13;
        public static readonly int ES_ITEM_ISVISIABLE = 14;
        public static readonly int ES_ITEM_FREZZE = 15;
        public static readonly int ES_TILE = 16;
        // request from server
        public static readonly string CMD_REMOVE = "remove";
        public static readonly string CMD_IS_PRIMARY = "isprimary";
        public static readonly string CMD_USER_READY = "userready";
        public static readonly string CMD_CAN_PLAY = "canplay";
        // request from client
        public static readonly string CRQ_FIRE = "fire";
        public static readonly string CRQ_READY = "ready";
        public static readonly string CRQ_PLAY = "play";
        // extension path
        public static readonly string EXTS_ZONE = "ZoneExtension.jar";
        public static readonly string EXTS_ROOM = "RoomExtension.jar";
        public static readonly string EXTS_ZONE_MAINCLASS = "MainClassZoneExtension.java";
        public static readonly string EXTS_ROOM_MAINCLASS = "MainClassRoomExtension.java";
        // controller's name
        public static readonly string CTRL_TANK = "tankcontroller";
        public static readonly string CTRL_BULLET = "bulletcontroller";
        public static readonly string CTRL_ITEM = "itemcontroller";
        // type
        public static readonly string TYPE_PAR_EXPLOSION = "explosionpar";
        public static readonly string TYPE_PAR_FIRE = "firepar";
        // game ui type
        public static readonly string UI_TYPE_INPUT_FIELD = "inputField";
        public static readonly string UI_TYPE_BUTTON = "button";
        public static readonly string UI_TYPE_DISPLAY_FIELD = "displayField";
        public static readonly string UI_TYPE_NAME_PLATE = "namePlate";
        public static readonly string UI_TYPE_DISPLAY_ONLY_ONE_INFO= "displayInfo"; 
        // ui cmd
        public static readonly string UI_CMD_INVERSE_USE_BACKGROUND = "inverseUseBackground";
        public static readonly string UI_CMD_DISABLE = "disable";
        public static readonly string UI_CMD_ENABLE = "enable";
        public static readonly string UI_CMD_INVERSE_USE_SPRITE_BOUNDING = "inverseUseSpriteBounding";
        // ui background
        public static readonly string UIS_ID = @"idField";
        public static readonly string UIS_PASS = @"";       
        public static readonly string UIS_INPUT_FIELD = "textInput";
        public static readonly string UIS_IP = @"ipField";
        public static readonly string UIS_PORT = @"portField";
        public static readonly string UIS_ICON_DEATH = @"icon\death";
        public static readonly string UIS_ICON_KILL = @"icon\kill";

        public static readonly string UIS_UNREADY_BUTTON = "unReadyBT";
        public static readonly string UIS_READY_BUTTON = @"icon\ready";
        public static readonly string UIS_START_BUTTON = @"icon\play";
        public static readonly string UIS_EXIT_BUTTON = @"icon\exit";
        public static readonly string UIS_MENU_BUTTON = "menuBT";
        public static readonly string UIS_SOLO_BUTTON = "SoloButton";
        public static readonly string UIS_CUSTUME_BATTLE_BUTTON = "CustumeBattle";
        public static readonly string UIS_SOUND_ENABLE_BUTTON = @"icon\sound_enable";
        public static readonly string UIS_SOUND_DISABLE_BUTTON = @"icon\sound_disable";

        public static readonly string UIS_OWNER_DISPLAY = "ownerDisplay";
        public static readonly string UIS_PLAYER_DISPLAY = "playerDisplay";
        // item sprite
        public static readonly string ITS_ISVISIBLE = @"item\isvisible";
        public static readonly string ITS_ARMOR = @"item\armor";
        public static readonly string ITS_FREEZE = @"item\freeze";
        // behavior
        public static readonly string BHVR = "behavior";
       
        // response
       

    }
}

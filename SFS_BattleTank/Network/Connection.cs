using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Requests;
using Sfs2X.Entities.Data;
using Sfs2X.Entities;
using Sfs2X.Util;
using System.Diagnostics;
using SFS_BattleTank.Constants;
using System.Collections.Generic;
using SFS_BattleTank.Bases;
using SFS_BattleTank.GameObjCtrl;
using Sfs2X.Entities.Variables;
using Sfs2X.Requests.MMO;

namespace SFS_BattleTank.Network
{
    public class Connection
    {
        protected SmartFox sfs;
        protected string HOST = "127.0.0.1";
        protected int PORT = 9933;
        protected string ZONE = "BasicExamples";
        protected string ROOM = "The Lobby";
        protected Room curRoom;
        protected bool isLoadConfig = false;

        protected Dictionary<string,Controller> _controllers;
        public Connection()
        {
            sfs = new SmartFox();
            sfs.ThreadSafeMode = true;
            _controllers = new Dictionary<string,Controller>();

            Init();
            Connect();
        }
        ~Connection()
        {
            RemoveListener();
        }
        public void Init()
        {
            this.AddListener();
        }
        public void Update(float deltaTime)
        {
            if (sfs != null)
                sfs.ProcessEvents();
            UpdateControler(deltaTime);
        }
        public void Connect()
        {
            if (!isLoadConfig)
            {
                ConfigData data = new ConfigData();
                data.Port = PORT;
                data.Zone = ZONE;
                data.Host = HOST;
                data.Debug = false;
                sfs.Connect(data);
            }
            else
            {
                string path = Consts.SMARTFOX_CONFIG;
                sfs.LoadConfig(path);
            }
        }
        public void Login(string userName, string password = "")
        {
            sfs.Send(new LoginRequest(userName, password,ZONE));
        }
        public void JoinRoom(string roomName = "The Lobby", string password = "")
        {
            if (roomName != ROOM)
                ROOM = roomName;
            sfs.Send(new JoinRoomRequest(ROOM, password));
        }
        public SmartFox GetInstance() { return sfs; }
        public Room GetCurRoom() { return curRoom; }
        public void CreateRoom(RoomSettings settings)
        {
            sfs.Send(new CreateRoomRequest(settings, true));
        }
        public void CreateRoom(string roomName)
        {
            CreateRoom(GetRoomSetting(roomName));
        }

        private void AddListener()
        {
            sfs.AddEventListener(SFSEvent.CONNECTION, OnConnection);
            sfs.AddEventListener(SFSEvent.CONFIG_LOAD_FAILURE, OnLoadConfigFail);
            sfs.AddEventListener(SFSEvent.CONFIG_LOAD_SUCCESS, OnLoadConfigSuccess);

            sfs.AddEventListener(SFSEvent.LOGIN, OnLogin);
            sfs.AddEventListener(SFSEvent.LOGIN_ERROR, OnLoginError);
            sfs.AddEventListener(SFSEvent.ROOM_JOIN, OnJoinRoom);
            sfs.AddEventListener(SFSEvent.ROOM_JOIN_ERROR, OnJoinRoomError);
            sfs.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);

            sfs.AddEventListener(SFSEvent.ROOM_ADD, OnRoomAdded);
            sfs.AddEventListener(SFSEvent.ROOM_CREATION_ERROR, OnRoomCreateError);
        }
        public void RemoveListener()
        {
            sfs.RemoveAllEventListeners();
        }

        // handle events
        private void OnConnection(BaseEvent e)
        {
            bool success = (bool)e.Params["success"];
            if (success)
            {
                Debug.WriteLine("Connected !");
                Login("name 1");
            }
            else
            {
                Debug.WriteLine("Connection fail !");
            }
        }
        private void OnLoadConfigFail(BaseEvent e)
        {
            Debug.WriteLine("Load config fail !");
        }
        private void OnLoadConfigSuccess(BaseEvent e)
        {
            Debug.WriteLine("Load config success !");
            sfs.Connect(sfs.Config);
        }
        private void OnLogin(BaseEvent e)
        {
            SFSObject data = (SFSObject)e.Params["data"];
            Debug.WriteLine("Login success ! User info : " + e.Params["user"]);
            JoinRoom();
        }
        private void OnLoginError(BaseEvent e)
        {
            Debug.WriteLine("Login error !");
            Login("name 2");
        }
        private void OnJoinRoom(BaseEvent e)
        {
            Debug.WriteLine("Join room success: room info - " + e.Params["room"]);
            Debug.WriteLine(sfs.MySelf.PlayerId.ToString());
            curRoom = (Room)e.Params["room"];
            foreach(Controller ctrl in _controllers.Values)
            {
                ctrl.Init();
            }

            // test
            SFSObject data = new SFSObject();
            sfs.Send(new ExtensionRequest("add", data,sfs.LastJoinedRoom));
        }
        private void OnJoinRoomError(BaseEvent e)
        {
            Debug.WriteLine(e.Params["errorMessage"] + " - Error code :" + e.Params["errorCode"].ToString());
        }
        private void OnExtensionResponse(BaseEvent e)
        {
            SFSObject receive = (SFSObject)e.Params["params"];
            string cmd = (string)e.Params["cmd"];
            Room room = (Room)e.Params["room"];
            User sender = room.GetUserById((int)(receive.GetDouble("ID")));
            

            if(cmd == Consts.CMD_UPDATE_DATA)
            {
                string type = receive.GetUtfString(Consts.TYPE);
                if(type == Consts.TYPE_TANK)
                {
                    _controllers[Consts.CTRL_TANK].UpdateData(sender, receive);
                }
            }
            if(cmd == Consts.CMD_ADD)
            {
                string type = receive.GetUtfString(Consts.TYPE);
                if (type == Consts.TYPE_TANK)
                {
                    _controllers[Consts.CTRL_TANK].Add(sender,receive);
                }
            }

        }   
        private void OnRoomAdded(BaseEvent e)
        {
            curRoom = (Room)e.Params["room"];
            Debug.WriteLine("Join room success: room info - " + e.Params["room"]);
        }
        private void OnRoomCreateError(BaseEvent e)
        {
            Debug.WriteLine("Create room error - " + e.Params["errorMessage"] + "\n errorCode : " + e.Params["errorCode"]);
        }

        public RoomSettings GetRoomSetting(string roomName)
        {
            RoomSettings settings = new MMORoomSettings(roomName);
            settings.IsGame = true;
            settings.MaxUsers = 8;
            settings.Name = roomName;

            RoomPermissions permissions = new RoomPermissions();
            permissions.AllowNameChange = true;
            permissions.AllowPublicMessages = true;
            settings.Permissions = permissions;

            RoomEvents events = new RoomEvents();
            events.AllowUserExit = true;
            events.AllowUserEnter = true;
            events.AllowUserCountChange = true;
            events.AllowUserVariablesUpdate = true;
            settings.Events = events;
            settings.Extension = new RoomExtension(Consts.EXTS_ROOM, Consts.EXTS_ROOM_MAINCLASS);

            return settings;

        }
        public Dictionary<string,Controller> GetControllers()
        {
            return _controllers;
        }
        public void AddController(string name,Controller ctrl)
        {
            if (ctrl != null)
                _controllers.Add(name,ctrl);
        }
        public void UpdateControler(float deltaTime)
        {
            foreach(Controller ctrl in _controllers.Values)
            {
                ctrl.Update(deltaTime);
            }
        }
    }
}

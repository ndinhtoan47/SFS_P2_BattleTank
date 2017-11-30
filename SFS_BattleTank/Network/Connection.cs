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
        // test
        protected long count = 0;
        protected double time = 0.0f;
        public Connection()
        {
            sfs = new SmartFox();
            sfs.ThreadSafeMode = true;
            _controllers = new Dictionary<string,Controller>();
        }
        ~Connection()
        {
            RemoveListener();
            sfs.Disconnect();
        }
        public void Init()
        {
            this.AddListener();
        }
        public void Update(float deltaTime)
        {
            if (sfs != null)
                sfs.ProcessEvents();
            //time += deltaTime;
            //Debug.Write("F = : " + (count / time).ToString() + "\n DeltaTime : " + deltaTime + "\n");
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
            sfs.Send(new LoginRequest(userName, password, sfs.Config.Zone));
        }
        public void JoinRoom(string roomName = "The Lobby", string password = "")
        {
            sfs.Send(new JoinRoomRequest(roomName, password));
        }
        public SmartFox GetInstance() { return sfs; }
        public Room GetCurRoom() { return curRoom; }
        public void CreateRoom(MMORoomSettings settings)
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
            sfs.AddEventListener(SFSEvent.PROXIMITY_LIST_UPDATE, OnProximityListUpdate);
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
                Login("default name");
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
            int x = data.GetInt(Consts.X);
            int y = data.GetInt(Consts.Y);
            Debug.WriteLine("Login success ! User info : " + e.Params["user"]);
            Debug.WriteLine("x = " + x + " y = " + y);
            JoinRoom();
        }
        private void OnLoginError(BaseEvent e)
        {
            Debug.WriteLine("Login error !");
            Login("name2");
        }
        private void OnJoinRoom(BaseEvent e)
        {
            Debug.WriteLine("Join room success: room info - " + e.Params["room"]);
            Debug.WriteLine(sfs.MySelf.PlayerId.ToString());
            curRoom = (Room)e.Params["room"];
        }
        private void OnJoinRoomError(BaseEvent e)
        {
            Debug.WriteLine(e.Params["errorMessage"] + " - Error code :" + e.Params["errorCode"].ToString());
        }
        private void OnExtensionResponse(BaseEvent e)
        {
            SFSObject receive = (SFSObject)e.Params["params"];
            count++;
            string cmd = (string)e.Params["cmd"];

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
        private void OnProximityListUpdate(BaseEvent e)
        {
            List<User> addedList = (List<User>)e.Params["addedUsers"];
             List<User> removedList = (List<User>)e.Params["removedUsers"];
             foreach (User added in addedList)
             {
                 _controllers[Consts.CTRL_TANK].Add(added);
             }
            foreach (User removed in removedList)
            {
                _controllers[Consts.CTRL_TANK].Remove(removed);
            }
        }

        public MMORoomSettings GetRoomSetting(string roomName)
        {
            MMORoomSettings settings = new MMORoomSettings(roomName);
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

            settings.DefaultAOI = new Vec3D(500, 500, 0);
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

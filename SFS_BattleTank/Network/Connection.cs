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
using SFS_BattleTank.GameScenes;
using System;

namespace SFS_BattleTank.Network
{
    public class Connection
    {
        protected SmartFox _sfs;
        protected string ZONE = "BasicExamples";
        protected string ROOM = "The Lobby";
        protected Room _curRoom;
        protected Dictionary<string, Controller> _controllers;
        protected int _isPrimary;
        protected List<User> _userJoinedRoom;

        public Connection()
        {
            LoginScene.SetNotice("Input server info and user name");
            _sfs = new SmartFox();
            _sfs.ThreadSafeMode = true;
            _controllers = new Dictionary<string, Controller>();
            _isPrimary = -1;
            _userJoinedRoom = new List<User>();
        }
        ~Connection()
        {
            RemoveListener();
        }
        public void Init()
        {
        }
        public void Update(float deltaTime)
        {
            if (_sfs != null)
                _sfs.ProcessEvents();
        }
        public void RemoveListener()
        {
            _sfs.RemoveAllEventListeners();
        }

        public void AddController(string name, Controller ctrl)
        {
            if (ctrl != null)
                _controllers.Add(name, ctrl);
        }
        public void UpdateControler(float deltaTime)
        {
            foreach (Controller ctrl in _controllers.Values)
            {
                ctrl.Update(deltaTime);
            }
        }
        public GameObject GetMainTank()
        {
            Dictionary<int, GameObject> _tanks = _controllers[Consts.CTRL_TANK].GetAllGameObject();
            if (_tanks.ContainsKey(_controllers[Consts.CTRL_TANK].GetMySefl()))
                return _tanks[_controllers[Consts.CTRL_TANK].GetMySefl()];
            return null;
        }
        public List<User> GetUserJoinedRoom() { return _userJoinedRoom; }
        public void AddJoinedUser(User user)
        {
            if(user != null)
            {
                _userJoinedRoom.Add(user);
            }
        }
        public void HandleUserLeaveRoom(User user) 
        {
            if(user != null && !_curRoom.ContainsUser(user) && _userJoinedRoom.Contains(user))
            {
                _userJoinedRoom.Remove(user);
            }
        }

        // properties
        public List<User> GetUsersInsideCurrentRoom()
        {
            return _curRoom.UserList;
        }
        public Controller GetController(string ctrl)
        {
            if (_controllers.ContainsKey(ctrl)) return _controllers[ctrl];
            return null;
        }
        public Dictionary<string, Controller> GetControllers()
        {
            return _controllers;
        }
        public Room GetCurretRoom() { return _curRoom; }
        public void SetCurretRoom(Room room) { _curRoom = room; }
        public SmartFox GetInstance() { return _sfs; }
        // connection methods
        public void Connect(string host, int port)
        {
            try
            {
                ConfigData data = new ConfigData();
                data.Port = port;
                data.Zone = ZONE;
                data.Host = host;
                data.Debug = false;
                data.UseBlueBox = true;
                _sfs.Connect(data);
            }
            catch (NullReferenceException e)
            {
                Debug.WriteLine(e.Message);
                Game1.sceneManager.GotoScene(Consts.SCENE_LOGIN);
            }
        }
        public void Login(string name, string password = "")
        {
            _sfs.Send(new LoginRequest(name, password));
        }
        public void JoinRoom(string roomName = "The Lobby", string password = "")
        {
            _sfs.Send(new JoinRoomRequest(roomName, password));
        }
        public void SetPrimary(int value)
        {
            _isPrimary = value;
        }
        public int IsPrimary() { return _isPrimary; }

    }
}

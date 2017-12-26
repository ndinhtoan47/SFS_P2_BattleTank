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
        protected List<User> _usersInsideRoom;
        protected bool _isPrimary;

        public Connection()
        {
            LoginScene.SetNotice("Input server info and user name");
            _sfs = new SmartFox();
            _sfs.ThreadSafeMode = true;
            _controllers = new Dictionary<string, Controller>();
            _usersInsideRoom = new List<User>();
            _isPrimary = false;
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

        // properties
        public List<User> GetUsersInsideCurrentRoom()
        {
            return _usersInsideRoom;
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
        // helper
        public void AddMeToUserList()
        {
            if (_sfs.MySelf != null)
            {
                if (!_usersInsideRoom.Contains(_sfs.MySelf))
                    _usersInsideRoom.Add(_sfs.MySelf);
            }
        }
        public void UserEnterExitMMORoom(List<User> added, List<User> removed)
        {
            foreach (User user in added)
            {
                if (!_usersInsideRoom.Contains(user)) _usersInsideRoom.Add(user);
            }
            foreach (User user in removed)
            {
                if (_usersInsideRoom.Contains(user)) _usersInsideRoom.Remove(user);
            }
        }
        public void SetPrimary(bool value)
        {
            _isPrimary = value;
        }
        public bool IsPrimary() { return _isPrimary; }

    }
}

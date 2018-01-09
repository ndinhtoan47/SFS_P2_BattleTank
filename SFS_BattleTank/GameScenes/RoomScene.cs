

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SFS_BattleTank.Bases;
using SFS_BattleTank.Constants;
using SFS_BattleTank.GameObjCtrl;
using SFS_BattleTank.InputControl;
using SFS_BattleTank.Sounds;
using SFS_BattleTank.UI;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Entities.Variables;
using Sfs2X.Requests;
using Sfs2X.Requests.MMO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
namespace SFS_BattleTank.GameScenes
{
    public class RoomScene : Scene
    {
        protected const string SOUND_BACKGROUND = @"sounds\s_menu_BG";
        protected const string ROOM_BACKGROUND = @"menuBG";

        protected Texture2D _background;
        protected SBackground _sBg;
        protected SpriteFont _font;
        protected List<User> _usersInsideRoom;
        protected bool _isEnablePlayButton;


        // test
        protected List<string> _names;
        protected NamePlateManager _namePlates;
        // ui
        protected Button _readyButton;
        protected Button _playButton;

        public RoomScene(ContentManager contents)
            : base(Consts.SCENE_ROOM, contents)
        {
            _sBg = new SBackground();
            _usersInsideRoom = new List<User>();
            _names = new List<string>();
            _isEnablePlayButton = false;

            // test
            _namePlates = new NamePlateManager();

        }
        public override bool Init()
        {
            // add controller
            _network.AddController(Consts.CTRL_TANK, new TankController(_contents));
            _network.AddController(Consts.CTRL_BULLET, new BulletController(_contents));
            _network.AddController(Consts.CTRL_ITEM, new ItemController(_contents));

            _readyButton = new Button("", new Vector2(0, 0), new Rectangle(0, 0, 100, 100), 2.0f);
            _playButton = new Button("", new Vector2(0, 0), new Rectangle(0, 0, 100, 100), 2.0f);

            //
            _namePlates.Init();
            return base.Init();
        }
        public override bool LoadContents()
        {
            _background = _contents.Load<Texture2D>(ROOM_BACKGROUND);
            _sBg.LoadContents(_contents, SOUND_BACKGROUND);
            _font = _contents.Load<SpriteFont>("font");
            _readyButton.LoadContents(_contents);
            _playButton.LoadContents(_contents);

            _playButton.ChangeBackground(Consts.UIS_START_BUTTON);
            _readyButton.ChangeBackground(Consts.UIS_READY_BUTTON);
            _playButton.SetBoundingBox(new Rectangle(0, 0, _playButton.GetSprite().Width, _playButton.GetSprite().Height));
            _readyButton.SetBoundingBox(new Rectangle(0, 0, _readyButton.GetSprite().Width, _readyButton.GetSprite().Height));
            _playButton.SetPosition(new Vector2(0, Consts.VIEWPORT_HEIGHT - _playButton.GetBoundingBox().Height));
            _readyButton.SetPosition(new Vector2(0, Consts.VIEWPORT_HEIGHT - _readyButton.GetBoundingBox().Height));
            _sBg.Play(new System.TimeSpan(0, 0, 0), 0.8f);

            _namePlates.LoadContents(_contents);

            List<User> joinedRoom = _network.GetUserJoinedRoom();
            foreach (User user in joinedRoom)
                if (user.ContainsVariable(Consts.PRIMARY))
                    _namePlates.Add(user, user.GetVariable(Consts.PRIMARY).GetBoolValue());

            return base.LoadContents();
        }
        public override void Shutdown()
        {
            _sBg.Stop();
            _sBg.Dispose();
            base.Shutdown();
        }
        public override void Draw(SpriteBatch sp)
        {
            sp.Begin();
            if (_background != null)
                sp.Draw(_background, new Rectangle(0, 0, Consts.VIEWPORT_WIDTH, Consts.VIEWPORT_HEIGHT), Color.White);

            if (_isEnablePlayButton) _playButton.Draw(sp);
            else _readyButton.Draw(sp);

            _namePlates.Draw(sp);
            sp.End();

            base.Draw(sp);
        }
        public override void Update(float deltaTime)
        {
            _readyButton.Update(deltaTime);
            _playButton.Update(deltaTime);

            // button behavior
            if (!_isEnablePlayButton)
            {
                if (_readyButton.ClickedInsideUI()) ReadyButtonBehavior();
            }
            else
                if (_playButton.ClickedInsideUI()) PlayButtonBehavor();
            base.Update(deltaTime);
        }

        private void DrawUsers(SpriteBatch sp)
        {
            Vector2 pos = new Vector2(0, 0);
            int count = 1;
            foreach (string s in _names)
            {
                sp.DrawString(_font, "User " + count.ToString() + ":" + s, pos, Color.Black);
                pos.Y += _font.MeasureString(s).Y;
                count++;
            }
        }
        private void ReadyButtonBehavior()
        {
            if (_readyButton.LastState())
            {
                _readyButton.SetBGColor(Color.Red);
            }
            else
            {
                _readyButton.SetBGColor(Color.White);
            }

            SFSObject data = new SFSObject();
            _sfs.Send(new ExtensionRequest(Consts.CRQ_READY, data, _network.GetCurretRoom()));
        }
        private void PlayButtonBehavor()
        {
            SFSObject data = new SFSObject();
            _sfs.Send(new ExtensionRequest(Consts.CRQ_PLAY, data, _network.GetCurretRoom()));
        }

        protected override void AddListener()
        {
            _sfs.AddEventListener(SFSEvent.PROXIMITY_LIST_UPDATE, OnProximityListUpdate);
            _sfs.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);
            _sfs.AddEventListener(SFSEvent.USER_VARIABLES_UPDATE, OnUserVariable);
            base.AddListener();
        }
        // events handler     
        private void OnProximityListUpdate(BaseEvent e)
        {
            Debug.WriteLine("RoomScene Proximity list updated !");
            List<User> addedUsers = (List<User>)e.Params["addedUsers"];
            List<User> removedUsers = (List<User>)e.Params["removedUsers"];

            Controller tanks = _network.GetController(Consts.CTRL_TANK);
            foreach (User user in addedUsers)
            {
                bool isOnwer = (user.ContainsVariable(Consts.PRIMARY)) ? user.GetVariable(Consts.PRIMARY).GetBoolValue() : false;
                _namePlates.Add(user, isOnwer);
                tanks.Add(user, null);
            }
            foreach (User user in removedUsers)
            {
                tanks.Remove(user, null);
                bool isOnwer = (user.ContainsVariable(Consts.ROOM_ONWER)) ? user.GetVariable(Consts.ROOM_ONWER).GetBoolValue() : false;
                _namePlates.Remove(user, isOnwer);
            }
        }
        private void OnExtensionResponse(BaseEvent e)
        {
            string cmd = (string)e.Params["cmd"];
            SFSObject data = (SFSObject)e.Params["params"];
            User user = (User)e.Params["user"];
            try
            {
                if (cmd == Consts.CMD_IS_PRIMARY)
                {
                    _network.SetPrimary((int)data.GetShort(Consts.ROOM_ONWER));
                    _isEnablePlayButton = (_network.IsPrimary() == _sfs.MySelf.Id) ? true : false;
                    if (!_isEnablePlayButton)
                        _playButton.CMD(Consts.UI_CMD_DISABLE);
                    else _readyButton.CMD(Consts.UI_CMD_DISABLE);
                    //////////////// dis play ready state ////////////////
                    if (data.ContainsKey(Consts.READY_ARRAY) && data.ContainsKey(Consts.ID_ARRAY))
                    {
                        short[] id = data.GetShortArray(Consts.ID_ARRAY);
                        bool[] ready = data.GetBoolArray(Consts.READY_ARRAY);
                        for (int i = 0; i < id.Length; i++)
                        {
                            User userReady = _network.GetCurretRoom().GetUserById((int)id[i]);
                            if (userReady != null)
                            {
                                bool isOwner = (_network.IsPrimary() == userReady.Id) ? true : false;
                                _namePlates.Add(userReady, isOwner);
                                _namePlates.SetReady((int)id[i], ready[i]);
                            }
                        }
                    }
                    Debug.WriteLine("Is primary :" + _isEnablePlayButton);
                    return;
                }
                if (cmd == Consts.CMD_USER_READY)
                {
                    if (data.ContainsKey(Consts.CAN_PLAY))
                    {
                        if (_network.IsPrimary() == _sfs.MySelf.Id)
                        {
                            if (data.ContainsKey(Consts.MESSAGE))
                            {
                                Debug.WriteLine(data.GetUtfString(Consts.MESSAGE));
                            }
                        }
                        bool canPlay = data.GetBool(Consts.CAN_PLAY);
                        if (canPlay) Game1.sceneManager.GotoScene(Consts.SCENE_PLAY);
                        return;
                    }
                    if (data.ContainsKey(Consts.ID_ARRAY) && data.ContainsKey(Consts.READY_ARRAY))
                    {
                        short[] id = data.GetShortArray(Consts.ID_ARRAY);
                        bool[] isReady = data.GetBoolArray(Consts.READY_ARRAY);
                        for (int i = 0; i < id.Length; i++)
                        {
                            _namePlates.SetReady((int)id[i], isReady[i]);
                        }
                    }
                    return;
                }
            }catch(Exception exp)
            {
                Debug.WriteLine(exp.ToString());
            }
        }
        private void OnUserVariable(BaseEvent e)
        {
            Debug.WriteLine("RoomScene Variable Update : " + (User)e.Params["user"]);
            User sender = (User)e.Params["user"];
            List<string> changedVars = (List<string>)e.Params["changedVars"];
            Controller ctrl = _network.GetController(Consts.CTRL_TANK);
            if (ctrl != null)
            {
                ctrl.Add(sender, null);
                ctrl.UpdateData(sender, changedVars, null);
            }
        }
    }
}

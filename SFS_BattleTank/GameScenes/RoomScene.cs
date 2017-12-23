

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
using System.Collections.Generic;
using System.Diagnostics;
namespace SFS_BattleTank.GameScenes
{
    public class RoomScene : Scene
    {
        protected const string SOUND_BACKGROUND = @"";
        protected const string ROOM_BACKGROUND = @"";

        protected Texture2D _background;
        protected SBackground _sBg;
        protected SpriteFont _font;
        protected List<User> _usersInsideRoom;
        protected bool _isEnablePlayButton;

        // test
        protected List<string> _names;
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
        }
        public override bool Init()
        {
            // add controller
            _network.AddController(Consts.CTRL_TANK, new TankController(_contents));
            _network.AddController(Consts.CTRL_BULLET, new BulletController(_contents));
            _network.AddController(Consts.CTRL_ITEM, new ItemController(_contents));

            _readyButton = new Button("Ready", new Vector2(0, 0), new Rectangle(0, 0, 100, 0), 2.0f);
            _playButton = new Button("Play", new Vector2(0, 0), new Rectangle(0, 0, 100, 0), 2.0f);
            this.UserEnterExitRoom(_network.GetUsersInsideCurrentRoom());
            return base.Init();
        }
        public override bool LoadContents()
        {
            //_background = _contents.Load<Texture2D>(ROOM_BACKGROUND);
            //_sBg.LoadContents(_contents, SOUND_BACKGROUND);
            _font = _contents.Load<SpriteFont>("font");
            _readyButton.LoadContents(_contents);
            _playButton.LoadContents(_contents);

            Vector2 readySize = _readyButton.GetButtonSize();
            _readyButton.SetPosition(new Vector2(Consts.VIEWPORT_WIDTH, Consts.VIEWPORT_HEIGHT) - readySize);
            _playButton.SetPosition(new Vector2(Consts.VIEWPORT_WIDTH, Consts.VIEWPORT_HEIGHT) -
                new Vector2(_readyButton.GetPosition().X - _playButton.GetBoundingBox().Width, _readyButton.GetPosition().Y));

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
            if (_isEnablePlayButton)
            {
                _playButton.Draw(sp);
            }
            _readyButton.Draw(sp);

            // test
            DrawUsers(sp);
            sp.End();

            base.Draw(sp);
        }
        public override void Update(float deltaTime)
        {
            _readyButton.Update(deltaTime);
            if (_isEnablePlayButton)
                _playButton.Update(deltaTime);

            // button behavior
            if (_readyButton.ClickedInsideUI()) ReadyButtonBehavior();
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
                _readyButton.SetLabel("Unready");
            else _readyButton.SetLabel("Ready");

            SFSObject data = new SFSObject();
            _sfs.Send(new ExtensionRequest(Consts.CRQ_READY, data, _network.GetCurretRoom()));
        }
        private void PlayButtonBehavor()
        {
            //Game1.sceneManager.GotoScene(Consts.SCENE_PLAY);
            SFSObject data = new SFSObject();
            _sfs.Send(new ExtensionRequest(Consts.CRQ_PLAY, data, _network.GetCurretRoom()));
        }

        private void UserEnterExitRoom(List<User> currentUsers)
        {
            _names.Clear();
            foreach (User us in currentUsers)
            {
                _names.Add(us.Name);
            }
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
            Debug.WriteLine("Proximity list updated !");
            List<User> addedUsers = (List<User>)e.Params["addedUsers"];
            List<User> removedUsers = (List<User>)e.Params["removedUsers"];
            _network.UserEnterExitMMORoom(addedUsers, removedUsers);
            this.UserEnterExitRoom(_network.GetUsersInsideCurrentRoom());
        }
        private void OnExtensionResponse(BaseEvent e)
        {
            string cmd = (string)e.Params["cmd"];
            SFSObject data = (SFSObject)e.Params["params"];
            if (cmd == Consts.CMD_IS_PRIMARY)
            {
                _network.SetPrimary((bool)data.GetBool(Consts.PRIMARY));
                _isEnablePlayButton = _network.IsPrimary();
                Debug.WriteLine("Is primary :" + _isEnablePlayButton);
                if (!_isEnablePlayButton)
                {
                    _playButton.CMD(Consts.UI_CMD_DISABLE);
                    return;
                }
            }
            if (cmd == Consts.CMD_USER_READY)
            {

            }
            if (cmd == Consts.CMD_CAN_PLAY)
            {
                if (_network.IsPrimary())
                {
                    if (data.ContainsKey(Consts.MESSAGE))
                    {
                        Debug.WriteLine(data.GetUtfString(Consts.MESSAGE));
                    }
                }
                bool canPlay = data.GetBool(Consts.CAN_PLAY);
                if (canPlay) Game1.sceneManager.GotoScene(Consts.SCENE_PLAY);
            }
        }
        private void OnUserVariable(BaseEvent e)
        {
            Debug.WriteLine((User)e.Params["user"]);
        }
    }
}

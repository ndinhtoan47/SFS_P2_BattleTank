using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SFS_BattleTank.Bases;
using SFS_BattleTank.Constants;
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
    public class MenuScene : Scene
    {

        protected const string MENU_BACKGROUND = "menuBG";
        protected const string S_BACKGROUND = @"sounds\s_menu_BG";

        protected Texture2D _background;
        protected SBackground _sBg;

        protected Button _menuBt;
        protected Button _soloBt;
        protected Button _cusBattleBt;
        protected Button _exitButton;
        protected Button _sOptionalButton;
        protected bool _drawOptional;

        public MenuScene(ContentManager contents)
            : base(Consts.SCENE_MENU, contents) { }
        public override bool Init()
        {
            _drawOptional = false;
            _menuBt = new Button("", Vector2.Zero, new Rectangle(0, 0, (int)(255 * 0.75f), (int)(255 * 0.75f)), 0.0f);
            _menuBt.CenterAlignment(new Rectangle(0, 0, Consts.VIEWPORT_WIDTH, Consts.VIEWPORT_HEIGHT));
            _exitButton = new Button("",
               new Vector2(Consts.VIEWPORT_WIDTH - 40, Consts.VIEWPORT_HEIGHT - 40),
               new Rectangle(0, 0, 40, 40), 1.0f);
            _soloBt = new Button("", Vector2.Zero, new Rectangle(0, 0, (int)(256 * 0.75f), (int)(54 * 0.75f)), 0.0f);
            _cusBattleBt = new Button("", new Vector2(100, 100), new Rectangle(0, 0, (int)(256 * 0.75f), (int)(54 * 0.75f)), 0.0f);
            _sOptionalButton = new Button("",
               new Vector2(Consts.VIEWPORT_WIDTH - 80, Consts.VIEWPORT_HEIGHT - 40),
               new Rectangle(0, 0, 40, 40), 0.0f);

            _soloBt.CMD(Consts.UI_CMD_DISABLE);
            _cusBattleBt.CMD(Consts.UI_CMD_DISABLE);
            AlignButton();

            _sBg = new SBackground();
            return base.Init();
        }
        public override bool LoadContents()
        {
            _background = _contents.Load<Texture2D>(MENU_BACKGROUND);
            _menuBt.LoadContents(_contents);
            _exitButton.LoadContents(_contents);
            _soloBt.LoadContents(_contents);
            _cusBattleBt.LoadContents(_contents);
            _sOptionalButton.LoadContents(_contents);

            _exitButton.ChangeBackground(Consts.UIS_EXIT_BUTTON);
            _menuBt.ChangeBackground(Consts.UIS_MENU_BUTTON);
            _soloBt.ChangeBackground(Consts.UIS_SOLO_BUTTON);
            _cusBattleBt.ChangeBackground(Consts.UIS_CUSTUME_BATTLE_BUTTON);
            _sOptionalButton.ChangeBackground(Consts.UIS_SOUND_ENABLE_BUTTON);

            _sBg.LoadContents(_contents, S_BACKGROUND);
            _sBg.Play(TimeSpan.Zero);
            return base.LoadContents();
        }
        public override void Shutdown()
        {
            _sBg.Stop();
            _sBg.Dispose();
            base.Shutdown();
        }
        public override void Update(float deltaTime)
        {
            _menuBt.Update(deltaTime);
            _exitButton.Update(deltaTime);
            _soloBt.Update(deltaTime);
            _cusBattleBt.Update(deltaTime);
            MenuButtonClick();
            _sOptionalButton.Update(deltaTime);

            if (_sOptionalButton.ClickedInsideUI()) SoundOptionButtonBehavior();
            if (_cusBattleBt.ClickedInsideUI()) CusBattleButtonBehavior();
            if (_exitButton.ClickedInsideUI()) ExitButtonBehavior();
            base.Update(deltaTime);
        }
        public override void Draw(SpriteBatch sp)
        {
            sp.Begin();
            if (_background != null)
                sp.Draw(_background, new Rectangle(0, 0, Consts.VIEWPORT_WIDTH, Consts.VIEWPORT_HEIGHT), Color.White);

            _exitButton.Draw(sp);
            if (_drawOptional)
            {
                _soloBt.Draw(sp);
                _cusBattleBt.Draw(sp);
            }
            _menuBt.Draw(sp);
            _sOptionalButton.Draw(sp);
            sp.End();
            base.Draw(sp);
        }
        protected void AlignButton()
        {
            Rectangle originRect = _menuBt.GetBoundingBox();
            Vector2 originPos = _menuBt.GetPosition();
            Rectangle soloRect = _soloBt.GetBoundingBox();
            Rectangle cusRect = _cusBattleBt.GetBoundingBox();

            Vector2 n_soloPos = Vector2.Zero;
            Vector2 n_cusPos = Vector2.Zero;
            n_soloPos.X = originPos.X - soloRect.Width + originRect.Width * 0.24f;
            n_soloPos.Y = originPos.Y + originRect.Height * 0.1f;

            n_cusPos.X = originPos.X + cusRect.Width - originRect.Width * 0.1f;
            n_cusPos.Y = originPos.Y + originRect.Height * 0.3f;
            _soloBt.SetPosition(n_soloPos);
            _cusBattleBt.SetPosition(n_cusPos);
        }
        protected void MenuButtonClick()
        {
            if (_menuBt.ClickedInsideUI())
            {
                if (_soloBt.IsEnable())
                    _soloBt.CMD(Consts.UI_CMD_DISABLE);
                else _soloBt.CMD(Consts.UI_CMD_ENABLE);
                if (_cusBattleBt.IsEnable())
                    _cusBattleBt.CMD(Consts.UI_CMD_DISABLE);
                else _cusBattleBt.CMD(Consts.UI_CMD_ENABLE);

                _drawOptional = !_drawOptional;
            }
        }

        // button behavior
        protected void SoundOptionButtonBehavior()
        {
            _sBg.Mute(!_sBg.IsMute());
            if (_sBg.IsMute()) _sOptionalButton.ChangeBackground(Consts.UIS_SOUND_DISABLE_BUTTON);
            else _sOptionalButton.ChangeBackground(Consts.UIS_SOUND_ENABLE_BUTTON);
        }
        protected void CusBattleButtonBehavior()
        {
            _network.JoinRoom();
        }
        protected void ExitButtonBehavior()
        {
            Game1.sceneManager.StopGame();
        }

        protected override void AddListener()
        {
            _sfs.AddEventListener(SFSEvent.ROOM_JOIN, OnJoinRoom);
            _sfs.AddEventListener(SFSEvent.ROOM_JOIN_ERROR, OnJoinRoomError);
            _sfs.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtension);
            base.AddListener();
        }
        // events handler
        private void OnJoinRoom(BaseEvent e)
        {         
            Room room = (Room)e.Params["room"];
            _network.SetCurretRoom(room);
            List<User> joinedUser = room.UserList;
            if(joinedUser != null)
            {
                foreach (User user in joinedUser)
                    _network.AddJoinedUser(user);
            }
            _network.AddJoinedUser(_sfs.MySelf);
            Game1.sceneManager.GotoScene(Consts.SCENE_ROOM);
            Debug.WriteLine("Joined room " + (Room)e.Params["room"]);
        }
        private void OnJoinRoomError(BaseEvent e)
        {
            Debug.WriteLine("Join room error !");
        }
        private void OnExtension(BaseEvent e)
        {
            string cmd = (string)e.Params["cmd"];
            SFSObject data = (SFSObject)e.Params["params"];
            if (cmd == "error")
            {
                Debug.WriteLine(data.GetUtfString("er"));
            }
        }

    }

}


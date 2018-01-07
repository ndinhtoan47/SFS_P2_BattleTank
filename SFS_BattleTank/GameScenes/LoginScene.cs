using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SFS_BattleTank.Bases;
using SFS_BattleTank.Constants;
using SFS_BattleTank.InputControl;
using SFS_BattleTank.Network;
using SFS_BattleTank.Sounds;
using SFS_BattleTank.UI;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using Sfs2X.Requests.MMO;
using System;
using System.Diagnostics;

namespace SFS_BattleTank.GameScenes
{
    public class LoginScene : Scene
    {

        protected const string LOGIN_BACKGROUND = "loginBG";
        protected const string S_BACKGROUND = @"sounds\s_login_BG";

        protected Texture2D _background;
        protected InputField _inputName;
        protected InputField _inputHost;
        protected InputField _inputPort;
        protected Button _loginButton;
        protected Button _exitButton;
        protected Button _sOptionalButton;
        protected SpriteFont _font;
        protected SBackground _sBg;

        static protected string _notice = "Input your name !";
        static protected string _userName = "";

        public LoginScene(ContentManager contents)
            : base(Consts.SCENE_LOGIN, contents) { }
        public override bool Init()
        {
            _inputName = new InputField(Vector2.Zero, new Rectangle(0, 0, 200, 0), 2f);
            _inputName.CenterAlignment(new Rectangle(0, 0, Consts.VIEWPORT_WIDTH, Consts.VIEWPORT_HEIGHT));
            _inputName.SetPosition(_inputName.GetPosition() + new Vector2(0, -Consts.VIEWPORT_HEIGHT * 0.1f));

            Vector2 inputPosition = _inputName.GetPosition();
            Rectangle inputRect = _inputName.GetBoundingBox();

            _inputHost = new InputField(
                inputPosition + new Vector2(0, -inputRect.Height),
                new Rectangle(0, 0, 200, 0),
                2f,"127.0.0.1");
            _inputPort = new InputField(
                _inputHost.GetPosition() + new Vector2(0, -_inputHost.GetBoundingBox().Height),
                new Rectangle(0, 0, 200, 0),
                2f,"9933");

            _loginButton = new Button("Login",
                new Vector2(inputPosition.X + inputRect.Width, inputPosition.Y),
                new Rectangle(0, 0, 100, 0), 2.0f);
            _exitButton = new Button("",
                new Vector2(Consts.VIEWPORT_WIDTH - 40, Consts.VIEWPORT_HEIGHT - 40),
                new Rectangle(0, 0, 40, 40), 1.0f);
            _sOptionalButton = new Button("",
                new Vector2(Consts.VIEWPORT_WIDTH - 80, Consts.VIEWPORT_HEIGHT - 40),
                new Rectangle(0, 0, 40, 40), 0.0f);
            _sBg = new SBackground();

            _inputPort.SetTextAlignment(0.45f);
            _inputPort.SetTextMaxSize(0.5f);


          
            return base.Init();
        }
        public override bool LoadContents()
        {
            _background = _contents.Load<Texture2D>(LOGIN_BACKGROUND);
            _font = _contents.Load<SpriteFont>("font");
            // ui load contents
            _inputName.LoadContents(_contents);
            _inputHost.LoadContents(_contents);
            _inputPort.LoadContents(_contents);
            _loginButton.LoadContents(_contents);
            _exitButton.LoadContents(_contents);
            _exitButton.ChangeBackground(Consts.UIS_EXIT_BUTTON);

            _sOptionalButton.LoadContents(_contents);
            _sOptionalButton.ChangeBackground(Consts.UIS_SOUND_ENABLE_BUTTON);
            _inputName.ChangeBackground(Consts.UIS_ID);
            _inputHost.ChangeBackground(Consts.UIS_IP);
            _inputPort.ChangeBackground(Consts.UIS_PORT);
            _sBg.LoadContents(_contents, S_BACKGROUND);
            _sBg.Play(new TimeSpan(0, 0, 2));

            
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
            UpdateInputField(deltaTime);

            _inputName.Update(deltaTime);
            _inputHost.Update(deltaTime);
            _inputPort.Update(deltaTime);

            _loginButton.Update(deltaTime);
            _exitButton.Update(deltaTime);
            _userName = _inputName.GetInputText();
            _sOptionalButton.Update(deltaTime);

            // check button click
            if (_sOptionalButton.ClickedInsideUI()) SoundOptionButtonBehavior();
            if (_loginButton.ClickedInsideUI() || Input.IsKeyDown(Keys.Enter)) LoginButtonBehavior();
            if (_exitButton.ClickedInsideUI()) ExitButtonBehavior();
            base.Update(deltaTime);
        }
        public override void Draw(SpriteBatch sp)
        {
            sp.Begin();
            if (_background != null)
            {
                sp.Draw(_background,
                    new Rectangle(0, 0, Consts.VIEWPORT_WIDTH, Consts.VIEWPORT_HEIGHT),
                    Color.White);

            }
            _inputName.Draw(sp);
            _inputHost.Draw(sp);
            _inputPort.Draw(sp);

            _loginButton.Draw(sp);
            _exitButton.Draw(sp);
            _sOptionalButton.Draw(sp);
            if (_notice != null)
                sp.DrawString(_font, _notice,
                    new Vector2(_inputName.GetPosition().X, _inputName.GetBoundingBox().Height + _inputName.GetPosition().Y),
                    Color.Red);
            sp.End();
            base.Draw(sp);
        }
        
        private void UpdateInputField(float deltaTime)
        {
            if(InputControl.Input.Clicked(Consts.MOUSEBUTTON_LEFT))
            {
                if(_inputName.CheckInsideUI(_inputName.GetPosition(),_inputName.GetBoundingBox()) && !_inputName.IsEnable())
                {
                    DisableInputFields();
                    _inputName.CMD(Consts.UI_CMD_ENABLE);
                }
                if (_inputHost.CheckInsideUI(_inputHost.GetPosition(), _inputHost.GetBoundingBox()) && !_inputHost.IsEnable())
                {
                    DisableInputFields();
                    _inputHost.CMD(Consts.UI_CMD_ENABLE);
                }
                if (_inputPort.CheckInsideUI(_inputPort.GetPosition(), _inputPort.GetBoundingBox()) && !_inputPort.IsEnable())
                {
                    DisableInputFields();
                    _inputPort.CMD(Consts.UI_CMD_ENABLE);
                }
            }
        }

        // helper
        private void DisableInputFields()
        {
            _inputName.CMD(Consts.UI_CMD_DISABLE);
            _inputHost.CMD(Consts.UI_CMD_DISABLE);
            _inputPort.CMD(Consts.UI_CMD_DISABLE);
        }
        // static methods
        static public string UserName()
        {
            return _userName;
        }
        static public void SetNotice(string notice)
        {
            _notice = notice;
        }

        // button behavior
        protected void SoundOptionButtonBehavior()
        {
            _sBg.Mute(!_sBg.IsMute());
            if (_sBg.IsMute()) _sOptionalButton.ChangeBackground(Consts.UIS_SOUND_DISABLE_BUTTON);
            else _sOptionalButton.ChangeBackground(Consts.UIS_SOUND_ENABLE_BUTTON);
        }
        protected void LoginButtonBehavior()
        {
            if(_inputHost.GetInputText() == "" || _inputPort.GetInputText() == "")
            {
                LoginScene.SetNotice("Host or Port is empty !");
                return;
            }
            _network.Connect(_inputHost.GetInputText(), int.Parse(_inputPort.GetInputText()));
            Debug.WriteLine(_inputHost.GetInputText() + " " + int.Parse(_inputPort.GetInputText()));
        }
        protected void ExitButtonBehavior()
        {
            Game1.sceneManager.StopGame();
        }
        // server
        protected override void AddListener()
        {
            _sfs.AddEventListener(SFSEvent.CONNECTION, OnConnection);
            _sfs.AddEventListener(SFSEvent.LOGIN, OnLogin);
            _sfs.AddEventListener(SFSEvent.LOGIN_ERROR, OnLoginError);

            // test
            _sfs.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtension);
            base.AddListener();
        }

        // events handler
        private void OnConnection(BaseEvent e)
        {
            bool result = (bool)e.Params["success"];
            if (!result)
            {
                Debug.WriteLine("Connection fail !");
                LoginScene.SetNotice("Connection fail !");
                return;
            }
            LoginScene.SetNotice("Connected !");
            _network.Login(_inputName.GetInputText());
        }
        private void OnLogin(BaseEvent e)
        {
            Game1.sceneManager.GotoScene(Consts.SCENE_MENU);
            Debug.WriteLine("Logined as " + (User)e.Params["user"]);

        }
        private void OnLoginError(BaseEvent e)
        {
            LoginScene.SetNotice("Login fail !");
            Debug.WriteLine("Login fail !");
        }
        private void OnExtension(BaseEvent e)
        {
            string cmd = (string)e.Params["cmd"];
            SFSObject data = (SFSObject)e.Params["params"];
            if(cmd == "error")
            {
                Debug.WriteLine(data.GetUtfString("er"));
            }
        }
    }
}

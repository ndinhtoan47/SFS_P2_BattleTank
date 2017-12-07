using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SFS_BattleTank.Bases;
using SFS_BattleTank.Constants;
using SFS_BattleTank.Sounds;
using SFS_BattleTank.UI;
using System;

namespace SFS_BattleTank.GameScenes
{
    public class LoginScene : Scene
    {

        protected const string LOGIN_BACKGROUND = "loginBG";
        protected const string S_BACKGROUND = @"sounds\s_login_BG";

        protected Texture2D _background;
        protected InputField _inputName;
        protected Button _loginButton;
        protected Button _exitButton;
        protected Button _sOptionalButton;
        protected SpriteFont _font;

        protected SBackground _sBg;
        

        static protected string _notice = "Input your name !";
        static protected string _userName = "";

        public LoginScene(ContentManager contents)
            : base(Consts.SCENE_LOGIN, contents)
        {

        }
        public override bool Init()
        {
            _inputName = new InputField(Vector2.Zero, new Rectangle(0, 0, 200, 0), 2f);
            _inputName.CenterAlignment(new Rectangle(0, 0, 800, 600));

            Vector2 inputPosition = _inputName.GetPosition();
            Rectangle inputRect = _inputName.GetBoundingBox();
            _loginButton = new Button("Login",
                new Vector2(inputPosition.X + inputRect.Width, inputPosition.Y),
                new Rectangle(0, 0, 100, 0), 2.0f);
            _exitButton = new Button("",
                new Vector2(Consts.VIEWPORT_WIDTH - 40, Consts.VIEWPORT_HEIGHT - 40),
                new Rectangle(0, 0, 40, 40), 1.0f);
            _sOptionalButton = new Button("", 
                new Vector2(Consts.VIEWPORT_WIDTH - 80, Consts.VIEWPORT_HEIGHT - 40),
                new Rectangle(0, 0, 40, 40), 0.0f);

            _inputName.SetPosition(inputPosition + new Vector2(0, -Consts.VIEWPORT_HEIGHT * 0.2f));
            _loginButton.SetPosition(_loginButton.GetPosition() + new Vector2(0, -Consts.VIEWPORT_HEIGHT * 0.2f));

            _loginButton.CMD(Consts.UI_CMD_CHANGE_TO_LOGIN_BUTTON);
            _exitButton.CMD(Consts.UI_CMD_CHANGE_TO_EXIT_BUTTON);

            _sBg = new SBackground();
            return base.Init();
        }
        public override bool LoadContents()
        {
            _background = _contents.Load<Texture2D>(LOGIN_BACKGROUND);
            _font = _contents.Load<SpriteFont>("font");
            // ui load contents
            _inputName.LoadContents(_contents);
            _loginButton.LoadContents(_contents);
            _exitButton.LoadContents(_contents);
            _exitButton.ChangeBackground(Consts.UIS_EXIT_BUTTON);

            _sOptionalButton.LoadContents(_contents);
            _sOptionalButton.ChangeBackground(Consts.UIS_SOUND_ENABLE_BUTTON);
            _inputName.ChangeBackground(Consts.UIS_ID);
            _sBg.LoadContents(_contents, S_BACKGROUND);
            _sBg.Play(new TimeSpan(0,0,2));
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
            _inputName.Update(deltaTime);
            _loginButton.Update(deltaTime);
            _exitButton.Update(deltaTime);
            _userName = _inputName.GetInputText();
            _sOptionalButton.Update(deltaTime);

            if(_sOptionalButton.ClickedInsideButton())
            {
                SoundOptionButtonBehavior();
            }
            base.Update(deltaTime);
        }
        public override void Draw(SpriteBatch sp)
        {
            if (_background != null)
            {
                sp.Draw(_background,
                    new Rectangle(0, 0, Consts.VIEWPORT_WIDTH, Consts.VIEWPORT_HEIGHT),
                    Color.White);
            }
            _inputName.Draw(sp);
            _loginButton.Draw(sp);
            _exitButton.Draw(sp);
            _sOptionalButton.Draw(sp);
            if (_notice != null)
                sp.DrawString(_font, _notice,
                    new Vector2(_inputName.GetPosition().X, _inputName.GetBoundingBox().Height + _inputName.GetPosition().Y),
                    Color.Red);
            base.Draw(sp);
        }

        static public string UserName()
        {
            return _userName;
        }
        static public void SetNotice(string notice)
        {
            _notice = notice;
        }

        protected void SoundOptionButtonBehavior()
        {
            _sBg.Mute(!_sBg.IsMute());
            if (_sBg.IsMute()) _sOptionalButton.ChangeBackground(Consts.UIS_SOUND_DISABLE_BUTTON);
            else _sOptionalButton.ChangeBackground(Consts.UIS_SOUND_ENABLE_BUTTON);
        }
    }
}

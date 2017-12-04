using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SFS_BattleTank.Bases;
using SFS_BattleTank.Constants;
using SFS_BattleTank.InputControl;

namespace SFS_BattleTank.UI
{
    public class Button : GameUI
    {
        protected ContentManager _contents;
        protected string _label;
        protected bool _isEnable;
        protected SpriteFont _font;
        protected float _textScale;
        protected int _behaviorType;

        public Button(string lable,Vector2 position,Rectangle bounding,float textScale)
            :base(Consts.UI_BUTTON,position,bounding)
        {
            _label = lable;
            _textScale = textScale;
            Init();
            InitBoundingBox(_textScale);
        }
        public override void Init()
        {
            _isEnable = false;
            _behaviorType = 0;
            base.Init();
        }
        public override void LoadContents(ContentManager contents)
        {
            _contents = contents;
            base.LoadContents(contents);
        }
        public override void Update(float deltaTime)
        {
            if(_isEnable)
            {
                if(CheckInsideButton(_position, _bounding))
                {
                    if(Input.Clicked(Consts.MOUSEBUTTON_LEFT))
                    {
                        Behavior();
                    }
                }
            }
            base.Update(deltaTime);
        }
        public override void Draw(SpriteBatch sp)
        {
            base.Draw(sp);
        }
        public override void CMD(string cmd)
        {
           if(cmd == Consts.UI_CMD_CHANGE_TO_LOGIN_BUTTON)
           {
               _behaviorType = 1;
               return;
           }
           if (cmd == Consts.UI_CMD_CHANGE_TO_EXIT_BUTTON)
           {
               _behaviorType = 2;
               return;
           }
            if(cmd == Consts.UI_CMD_DISABLE)
            {
                _isEnable = false;
                return;
            }
            if (cmd == Consts.UI_CMD_ENABLE)
            {
                _isEnable = true;
                return;
            }
            base.CMD(cmd);
        }
        public override void ChangeBackground(string name)
        {
            base.ChangeBackground(name);
        }

        protected override void InitBoundingBox(float textScale)
        {
            //int heightPerUnit = 20;
            //// textScale = 1 => height = 20px
            //_bounding.Height = (int)((float)heightPerUnit * textScale);
            base.InitBoundingBox(textScale);
        }
        protected bool CheckInsideButton(Vector2 position,Rectangle boundingBox)
        {
            Vector2 mousePosition = Input.GetMousePosition();
            if(mousePosition.X >= position.X &&
                mousePosition.X <= position.X + boundingBox.Width &&
                mousePosition.Y >= position.Y &&
                mousePosition.Y <= position.Y + boundingBox.Height)
            {
                return true;
            }
            return false;
        }
        protected void Behavior()
        {
            switch(_behaviorType)
            {
                case 1: // login
                    {
                        Game1.sceneManager.GotoScene(Consts.SCENE_LOGIN);
                        break;
                    }
                case 2: // exit
                    {
                        Game1.sceneManager.StopGame();
                        break;
                    }
            }
        }
    }
}

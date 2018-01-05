

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SFS_BattleTank.Bases;
using SFS_BattleTank.Constants;
namespace SFS_BattleTank.UI
{
    public class DisplayInfo :GameUI
    {
        protected Texture2D _sprite;
        protected Rectangle _drawSpriteInsideRect;
        protected ContentManager _contents;
        protected string _info;
        protected SpriteFont _font;

        public DisplayInfo(Vector2 position,Rectangle bounding)
            :base(Consts.UI_TYPE_DISPLAY_ONLY_ONE_INFO,position,bounding)
        {
            _info = "0";
            Init();
        }

        public override void Init()
        {
            InitDrawSpriteRect();
            base.Init();
        }
        public override void LoadContents(ContentManager contents)
        {
            _contents = contents;
            _font = _contents.Load<SpriteFont>("font");
            base.LoadContents(contents);
        }
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }
        public override void Draw(SpriteBatch sp)
        {
            if(_sprite != null)
            {
                sp.Draw(_sprite,
                    new Rectangle((int)_position.X,(int)_position.Y,_drawSpriteInsideRect.Width,_drawSpriteInsideRect.Height),
                    Color.White);
                Vector2 textPos = _position + new Vector2(_drawSpriteInsideRect.Width, 0);
                sp.DrawString(_font, _info, textPos, Color.Red, 0, new Vector2(0, 0), (float)(_drawSpriteInsideRect.Height / 20.0f), SpriteEffects.None, 0);
            }
            base.Draw(sp);
        }
        public override Texture2D GetSprite()
        {
            return _sprite;
            return base.GetSprite();
        }
        public override void ChangeBackground(string name)
        {
            if (name == Consts.UIS_ICON_DEATH || name == Consts.UIS_ICON_KILL)
            {
                _sprite = _contents.Load<Texture2D>(name);
            }
            base.ChangeBackground(name);
        }
        public void SetInfo(string value)
        {
            _info = value;
        }
        protected override void InitBoundingBox(float textScale)
        {

            if (_info != "")
            {
                int heightPerUnit = 020;
                // textScale = 1 => height = 20px
                _bounding.Height = (int)((float)heightPerUnit * textScale);
            }
            base.InitBoundingBox(textScale);
        }
        private void InitDrawSpriteRect()
        {
            _drawSpriteInsideRect = new Rectangle(0, 0, _bounding.Width / 2, _bounding.Height);
        }
    }
}

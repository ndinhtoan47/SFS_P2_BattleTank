
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SFS_BattleTank.Bases;
using SFS_BattleTank.Constants;
namespace SFS_BattleTank.GameObjects
{
    public class Item : GameObject
    {
        protected Rectangle _destRect;
        protected string _spritePath = "";
        protected Texture2D _sprite;
        protected bool _countDown;

        protected Color _color;
        protected float _delayEffect;
        protected float _totalEffect;
        protected bool _flag;

        public Item(float x, float y, int essental)
            : base(x, y, essental)
        {
            Init();
        }

        public override bool Init()
        {
            _sprite = null;
            _destRect = new Rectangle(0, 0, (int)(20 * Consts.VIEWPORT_SCALE_RATE_WIDTH), (int)(20 * Consts.VIEWPORT_SCALE_RATE_HEIGHT));
            _countDown = false;
            _delayEffect = 0.1f;
            _totalEffect = 0.0f;
            _color = Color.White;
            _flag = false;
            if (_essental == Consts.ES_ITEM_ARMOR)
            {
                _spritePath = Consts.ITS_ARMOR;
                return true;
            }
            if (_essental == Consts.ES_ITEM_ISVISIABLE)
            {
                _spritePath = Consts.ITS_ISVISIBLE;
                return true;
            }
            if(_essental == Consts.ES_ITEM_FREZZE)
            {
                _spritePath = Consts.ITS_FREEZE;
                return true;
            }
            return false;
        }
        public override void LoadContents(ContentManager contents)
        {
            if (_spritePath != "")
                _sprite = contents.Load<Texture2D>(_spritePath);
        }
        public override void Draw(SpriteBatch sp)
        {
            if (_sprite != null)
            {
                sp.Draw(_sprite, new Rectangle((int)_position.X, (int)_position.Y, _destRect.Width, _destRect.Height), _color);
            }
        }
        public override void Update(float deltaTime)
        {
            if (_countDown)
            {
                CountDownEffectFlag(deltaTime);
                CountDownEffect(deltaTime);
            }
        }
        public override void Behavior(string cmd)
        {
            if (cmd == Consts.COUNT_DOWN)
            {
                _countDown = true;
            }
        }
        public override string Respose(string cmd) { return ""; }
        public override Rectangle GetBoundingBox()
        {
            return _destRect;
            return base.GetBoundingBox();
        }
        //
        protected void CountDownEffectFlag(float deltaTime)
        {
            if (_delayEffect <= _totalEffect)
            {
                _flag = !_flag;
                _totalEffect = 0.0f;
                return;
            }
            _totalEffect += deltaTime;
        }
        protected void CountDownEffect(float deltaTime)
        {
            if (_flag)
            {
                int r, g, b, a;
                r = ((int)(_color.R - 20) < 0 ? 0 : (int)(_color.R - 20));
                g = ((int)(_color.G - 20) < 0 ? 0 : (int)(_color.G - 20));
                b = ((int)(_color.B - 20) < 0 ? 0 : (int)(_color.B - 20));
                a = ((int)(_color.A - 20) < 0 ? 0 : (int)(_color.A - 20));
                _color = new Color(r, g, b, a);
            }
            else
            {
                int r, g, b, a;
                r = ((int)(_color.R + 20) > 255 ? 255 : (int)(_color.R + 20));
                g = ((int)(_color.G + 20) > 255 ? 255 : (int)(_color.G + 20));
                b = ((int)(_color.B + 20) > 255 ? 255 : (int)(_color.B + 20));
                a = ((int)(_color.A + 20) > 255 ? 255 : (int)(_color.A + 20));
                _color = new Color(r, g, b, a);
            }
        }
    }
}

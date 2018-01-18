

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SFS_BattleTank.Bases;
using SFS_BattleTank.Constants;
using SFS_BattleTank.GameScenes;
namespace SFS_BattleTank.GameObjects
{
    public class Tank : GameObject
    {
        protected Texture2D _sprite;
        protected const string TANK_PATH = "tanks";
        protected const string TANK_SHIELD = "shield";
        protected Rectangle _rectOffSet;
        protected Vector2 _center;
        protected bool _alive;
        protected int _death;
        protected int _kill;
        // tank animation manager
        protected float _delayNextFrame;
        protected float _totalNextFrame;
        protected int _maxFrame;
        protected int _curFrame;
        // shield animaton manager
        protected float _delayShield;
        protected float _totalShield;
        protected int _maxShield;
        protected int _curShield;
        // handle item
        protected float _itemEffectTime;
        protected int _hodingItem;
        protected Color _color;
        protected int _isAffectByItem;
        // item effect
        protected Texture2D _shield;
        protected bool _drawShield;
        public Tank(int id, float x, float y)
            : base(x, y, Consts.ES_TANK)
        {
            int sourcePos = id % 8;
            _rectOffSet = new Rectangle(0, sourcePos * 32, 32, 32);
            _center = new Vector2(16, 16);
            _alive = true;
            _death = 0;
            _kill = 0;
            // tank animation
            _delayNextFrame = 0.3f;
            _totalNextFrame = 0.0f;
            _maxFrame = 8;
            _curFrame = 0;
            // shield animation
            _delayShield = 0.1f;
            _totalShield = 0.0f;
            _maxShield = 10;
            _curShield = 0;

            _hodingItem = -1;
            _isAffectByItem = -1;
            _itemEffectTime = 0;
            _color = new Color(255, 255, 255, 255);
            _drawShield = false;
        }

        public override bool Init()
        {
            return base.Init();
        }
        public override void LoadContents(ContentManager contents)
        {
            _sprite = contents.Load<Texture2D>(TANK_PATH);
            _shield = contents.Load<Texture2D>(TANK_SHIELD);
            base.LoadContents(contents);
        }
        public override void Draw(SpriteBatch sp)
        {
            if (_sprite != null && _alive)
            {
                sp.Draw(
                    texture: _sprite,
                    sourceRectangle: new Rectangle(_rectOffSet.X + _curFrame * 32, _rectOffSet.Y, 32, 32),
                    rotation: MathHelper.ToRadians(_rotation),
                    origin: _center,
                    destinationRectangle: new Rectangle((int)_position.X, (int)_position.Y, (int)_rectOffSet.Width, (int)_rectOffSet.Height),
                    color: _color);
                if (_drawShield)
                    sp.Draw(texture: _shield,
                        sourceRectangle: new Rectangle(80 * _curShield, 0, 80, 70),
                        destinationRectangle: new Rectangle((int)_position.X, (int)_position.Y, 35, 35),
                        origin: new Vector2(40, 35),
                        color: new Color(120, 120, 120, 120));
            }
            base.Draw(sp);
        }
        public override string Respose(string cmd)
        {
            return base.Respose(cmd);
        }
        public override void Behavior(string cmd)
        {
            base.Behavior(cmd);
        }
        public override void Update(float deltaTime)
        {
            this.HandleItemHoding(deltaTime);
            if (_drawShield)
                this.ShieldAnimation(deltaTime);
            base.Update(deltaTime);
        }
        public override Rectangle GetBoundingBox()
        {
            return _rectOffSet;
            return base.GetBoundingBox();
        }

        public bool IsAlive() { return _alive; }
        public void Death()
        {
            Rectangle boudingBox = this.GetBoundingBox();
            boudingBox.X = (int)_position.X;
            boudingBox.Y = (int)_position.Y;
            PlayScene._parManager.Add(Consts.TYPE_PAR_EXPLOSION, boudingBox);
            _alive = false;
        }
        public void ReGeneration() { _alive = true; }
        public int GetDeath() { return _death; }
        public int GetKill() { return _kill; }
        public int GetHodingItem() { return _hodingItem; }
        public int GetIsAffectByItem() { return _isAffectByItem; }
        public void SetHodingItem(int itemType, bool isMe)
        {
            if(itemType == -1)
            {
                ResetItemAffect();
                return;
            }
            if (!isMe)
            {
                _isAffectByItem = itemType;
                _drawShield = false;
            }
            else
            {
                _hodingItem = itemType;
            }
            if (itemType == Consts.ES_ITEM_ISVISIABLE)
            {
                if (isMe)
                    _color = new Color(150, 150, 150, 150);
                else
                    _color = new Color(0, 0, 0, 0);
                _itemEffectTime = 5.0f;
                return;
            }
            if (itemType == Consts.ES_ITEM_ARMOR)
            {
                _itemEffectTime = 20.0f;
                _drawShield = true;
                return;
            }
            if (itemType == Consts.ES_ITEM_FREZZE)
            {
                _itemEffectTime = 5.0f;
                return;
            }
        }
        public void SetKill(int kill)
        {
            _kill = kill;
        }
        public void SetDeath(int death)
        {
            _death = death;
        }
        public void Move(float deltaTime)
        {
            Animation(deltaTime);
        }
        // animation
        private void Animation(float deltaTime)
        {
            _totalNextFrame += deltaTime;
            if (_totalNextFrame >= _delayNextFrame)
            {
                _curFrame = (_curFrame + 1) % _maxFrame;
                _totalNextFrame = 0.0f;
            }
        }
        private void ShieldAnimation(float deltaTime)
        {
            if (_totalShield >= _delayShield)
            {
                _curShield = (_curShield + 1) % _maxShield;
                _totalShield = 0.0f;
            }
            else _totalShield += deltaTime;
        }
        // handle item
        private void HandleItemHoding(float deltaTime)
        {
            if (_itemEffectTime != -1)
            {
                if (_itemEffectTime <= 0)
                {
                    ResetItemAffect();
                    return;
                }
                _itemEffectTime -= deltaTime;
            }
        }
        private void ResetItemAffect()
        {
            _color = new Color(255, 255, 255, 255); // reset isvisiable
            _drawShield = false; // reset isvisiable

            _hodingItem = -1;
            _itemEffectTime = 0;
            _isAffectByItem = -1;
        }
    }
}

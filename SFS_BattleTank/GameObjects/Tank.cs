

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
        protected Rectangle _rectOffSet;
        protected Vector2 _center;
        protected bool _alive;
        protected int _death;
        protected int _kill;

        public Tank(int id, float x, float y)
            : base(x, y, Consts.ES_TANK)
        {
            int sourcePos = id % 8;
            _rectOffSet = new Rectangle(0, sourcePos * 32, 32, 32);
            _center = new Vector2(16, 16);
            _alive = true;
            _death = 0;
            _kill = 0;
        }

        public override bool Init()
        {
            return base.Init();
        }
        public override void LoadContents(ContentManager contents)
        {
            _sprite = contents.Load<Texture2D>(TANK_PATH);
            base.LoadContents(contents);
        }
        public override void Draw(SpriteBatch sp)
        {
            if (_sprite != null && _alive)
            {
                sp.Draw(
                    texture: _sprite,
                    sourceRectangle: new Rectangle(_rectOffSet.X,_rectOffSet.Y,32,32),
                    rotation: MathHelper.ToRadians(_rotation),
                    origin: _center,
                    destinationRectangle: new Rectangle((int)_position.X, (int)_position.Y, (int)_rectOffSet.Width, (int)_rectOffSet.Height),
                    color: Color.White);
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
            PlayScene._parManager.Add(Consts.TYPE_PAR_EXPLOSION,boudingBox);
            _alive = false;
        }
        public void ReGeneration() { _alive = true; }
        public int GetDeath() { return _death; }
        public int GetKill() { return _kill; }

        public void SetKillAndDeath(int death,int kill)
        {
            _kill = kill;
            _death = death;
        }
    }
}

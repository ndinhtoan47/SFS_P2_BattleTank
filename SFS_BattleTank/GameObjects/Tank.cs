

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SFS_BattleTank.Bases;
using SFS_BattleTank.Constants;
namespace SFS_BattleTank.GameObjects
{
    public class Tank : GameObject
    {
        protected Texture2D _sprite;
        protected const string TANK_PATH = "tanks";
        protected Rectangle _rectOffSet;
        protected Vector2 _center;

        public Tank(int id, float x, float y)
            : base(x, y, Consts.ES_TANK)
        {
            int sourcePos = id % 8;
            _rectOffSet = new Rectangle(0, sourcePos * 32, 32, 32);
            _center = new Vector2(16, 16);
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
            if (_sprite != null)
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
    }
}

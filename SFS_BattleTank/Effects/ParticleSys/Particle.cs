
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace SFS_BattleTank.Effects.ParticleSys
{
    public class Particle
    {
        protected Texture2D _sprite;
        protected Vector2 _position;
        protected FadeHelper _fadeHelper;
        protected float _speed;
        protected float _lifeTime;
        protected float _totalLifeTime;
        protected int _direction;
        protected float _rotation;
        protected float _rotationSpeed;
        protected Vector2 _center;
        protected int _fade;
        protected float _scale;
        protected int _startOpacity;
        protected int _minSize;

        public Particle(Texture2D sprite, Vector2 position,
                        float speed, float lifeTime, int direction,
                        float rotation, float rotationSpeed, int minSize,
                        float scale = 1.0f, int startOpacity = 255)
        {
            _sprite = sprite;
            _position = position;
            _speed = speed;
            _lifeTime = lifeTime;
            _direction = direction;
            _rotation = rotation;
            _rotationSpeed = rotationSpeed;
            _scale = scale;
            _minSize = minSize;
            _startOpacity = startOpacity;
            //
            _fade = startOpacity;
            _center = new Vector2(sprite.Width / 2.0f, sprite.Height / 2.0f);
            _totalLifeTime = 0;
            _fadeHelper = new FadeHelper();
        }

        public void Update(float deltaTime)
        {
            float xDir = (float)(System.Math.Cos((double)MathHelper.ToRadians(_direction)));
            float yDir = (float)(-System.Math.Sin((double)MathHelper.ToRadians(_direction)));
            _position.X += xDir * _speed * deltaTime;
            _position.Y += yDir * _speed * deltaTime;

            _rotation += _rotationSpeed * deltaTime;
            _totalLifeTime += deltaTime;
            _fade = _fadeHelper.UpdateFade(_fade, _lifeTime, _totalLifeTime);
        }
        public void Draw(SpriteBatch sp)
        {
            if (_sprite != null)
                sp.Draw(texture: _sprite,
                    position: null,
                    destinationRectangle: new Rectangle((int)_position.X, (int)_position.Y, _minSize, _minSize),
                    color: new Color(_fade, _fade, _fade, _fade),
                    rotation: _rotation,
                    origin: _center,
                    scale: new Vector2(_scale, _scale),
                    effects: SpriteEffects.None,
                    layerDepth: 0.0f);
        }
        // properties
        public Vector2 POSITION
        {
            protected set { _position = value; }
            get { return _position; }
        }
        public float TOTALTIMELIFE
        {
            protected set { _totalLifeTime = value; }
            get { return _totalLifeTime; }
        }
        public float LIFETIME
        {
            protected set { _lifeTime = value; }
            get { return _lifeTime; }
        }
    }
}

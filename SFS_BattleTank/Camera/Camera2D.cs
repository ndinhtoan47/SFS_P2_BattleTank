using Microsoft.Xna.Framework;
using SFS_BattleTank.Constants;

namespace SFS_BattleTank.Camera
{
    public class Camera2D
    {
        private int _viewportWidth;
        private int _viewportHeight;
        private bool _follow;
        private Vector2 _followPos;
        private float _zoom;
        private float _rotation;

        private int _mapWidth;
        private int _mapHeight;
        public Camera2D(int mapWidth,int mapHeight)
        {
            _viewportWidth = Consts.VIEWPORT_WIDTH;
            _viewportHeight = Consts.VIEWPORT_HEIGHT;
            _follow = true;
            _zoom = 1.0f;
            _rotation = 0.0f;
            _followPos = Vector2.Zero;
            _mapWidth = mapWidth;
            _mapHeight = mapHeight;
        }

        public void Update(float deltaTime, Vector2 follow)
        {
            if (_follow)
                _followPos = follow;
            if (follow.X <= _viewportWidth / 2) _followPos.X = _viewportWidth / 2;
            if (follow.Y <= _viewportHeight / 2) _followPos.Y = _viewportHeight / 2;

            if (follow.X > _mapWidth - _viewportWidth / 2) _followPos.X = _mapHeight - _viewportWidth / 2;
            if (follow.Y > _mapHeight - _viewportHeight / 2) _followPos.Y = _mapWidth - _viewportHeight / 2;


        }

        public Matrix GetTransfromMatrix()
        {

            Matrix result = Matrix.Identity;
            result = Matrix.CreateTranslation(new Vector3(-_followPos, 0))
                * Matrix.CreateScale(_zoom)
                * Matrix.CreateRotationZ(_rotation)
                * Matrix.CreateTranslation(new Vector3(_viewportWidth / 2, _viewportHeight / 2, 0));
            return result;
        }
        public void ZoomOut()
        {
            _zoom -= 0.2f;
            if (_zoom <= 0.1f)
            {
                _zoom = 0.2f;
            }
        }
        public void ZoomIn()
        {
            _zoom += 0.2f;
        }
        public void Follow(bool value)
        {
            _follow = value;
        }
    }
}



using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SFS_BattleTank.Bases;
using SFS_BattleTank.Constants;
using SFS_BattleTank.GameScenes;
using SFS_BattleTank.InputControl;
using System.Collections.Generic;
namespace SFS_BattleTank.Managers
{
    public class SceneManager
    {
        private List<Scene> _allScenes;
        private ContentManager _contents;
        private Scene _activeScene;
        private Scene _preScene;
        private bool _isStarted;
        private Game1 _game;

        public SceneManager(ContentManager contents, Game1 game)
        {
            _game = game;
            _allScenes = new List<Scene>();
            _contents = contents;
            _activeScene = null;
            _preScene = null;
            _isStarted = false;
        }

        public bool Init()
        {
            // add scenes
            this.Add(new PlayScenes(_contents));


            _isStarted = true;
            if (_activeScene == null)
            {
                return false;
            }
            if (!_activeScene.INIT)
            {
                _activeScene.Init();
            }
            return _activeScene.INIT;
        }
        public void Add(Scene newScene)
        {
            if (newScene != null)
            {
                _allScenes.Add(newScene);
            }
        }
        public void Remove(Scene scene)
        {
            if (scene != null)
            {
                _allScenes.Remove(scene);
            }
        }
        public bool GotoScene(string name)
        {
            if (_isStarted)
                foreach (Scene s in _allScenes)
                {
                    if (s.NAME == name)
                    {
                        if (_activeScene != null)
                        {
                            _preScene = _activeScene;
                            _activeScene.Shutdown();
                        }
                        _activeScene = s;
                        return _activeScene.Init();
                    }
                }
            return false;
        }
        public bool BackToPreviousScene()
        {
            return GotoScene(_preScene.NAME);
        }
        public void Update(float deltaTime)
        {
            if (_isStarted)
                if (_activeScene != null)
                {
                    _activeScene.Update(deltaTime);
                }
        }
        public void Draw(SpriteBatch sp)
        {
            if (_isStarted)
                if (_activeScene != null)
                    _activeScene.Draw(sp);
        }
        public void StopGame()
        {
            _game.Exit();
        }
    }
}

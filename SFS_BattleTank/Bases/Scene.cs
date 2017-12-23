using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SFS_BattleTank.Network;
using Sfs2X;

namespace SFS_BattleTank.Bases
{
    public abstract class Scene
    {
        protected string _name;
        protected bool _isInit;
        protected ContentManager _contents;

        protected Connection _network;
        protected SmartFox _sfs;
        public Scene(string name, ContentManager contents)
        {
            _name = name;
            _contents = contents;
            _isInit = false;
            _network = Game1.network;
        }

        public virtual bool Init()
        {
            _isInit = LoadContents();
            _sfs = _network.GetInstance();
            AddListener();
            return _isInit;
        }
        public virtual void Shutdown()
        {
            if(_network != null)
            {
                _network.RemoveListener();
            }
            _contents.Unload();
            _isInit = false;
        }
        public virtual void Draw(SpriteBatch sp) { }
        public virtual void Update(float deltaTime) { }
        public virtual bool LoadContents() { return true; }

        protected virtual void AddListener() { }
        // properties
        public bool INIT
        {
            get { return _isInit; }
            protected set { _isInit = value; }
        }
        public string NAME
        {
            get { return _name; }
            set { _name = value; }
        }
    }
}

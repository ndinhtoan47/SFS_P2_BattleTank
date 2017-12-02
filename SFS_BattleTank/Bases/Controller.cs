

using Microsoft.Xna.Framework.Content;
using SFS_BattleTank.Network;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using System.Collections.Generic;
namespace SFS_BattleTank.Bases
{
    public abstract class Controller
    {
        protected Connection _network;
        protected ContentManager _contents;
        protected int _myId;
        public Controller(ContentManager contents)
        {
            _network = Game1.network;
            _contents = contents;
        }

        public virtual void Update(float deltaTime) { }
        public virtual void GetDirection(out int x, out int y)
        {
            x = 0;
            y = 0;
        }
        public virtual void UpdateData(User user, SFSObject data) { }
        public virtual void UpdateData(SFSObject data) { }
        public virtual void Add(User user, SFSObject data) { }
        public virtual void Add(SFSObject data) { }
        public virtual void Remove(User user) { }
        public virtual void Remove(SFSObject data) { }
        public virtual void Behaviour(string cmd, int id, SFSObject data) { }
        public virtual Dictionary<int, GameObject> GetAllGameObject() { return null; }
        public virtual void Init() { }

        public int GetMyId()
        {
            User me = _network.GetInstance().MySelf;

            if (me != null)
            {
                _myId = me.Id;
                return _myId;
            }
            return -1;
        }
    }
}

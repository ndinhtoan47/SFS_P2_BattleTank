

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
        protected int _mySelf;
        public Controller(ContentManager contents)
        {
            _network = Game1.network;
            _contents = contents;
        }

        public virtual void Update(float deltaTime) { }
        public virtual void UpdateData(User user, SFSObject data) { }
        public virtual void Add(User user, SFSObject data) { }
        public virtual void Remove(User user, SFSObject data = null) { }
        public virtual void Behaviour(string cmd, int id, SFSObject data) { }
        public virtual Dictionary<int, GameObject> GetAllGameObject() { return null; }
        public virtual void Init() { }

        public int GetMySefl()
        {
            int me = _network.GetInstance().MySelf.Id;
            if (_network.GetInstance().MySelf != null)
                _mySelf = me;
            else _mySelf = -1;
            return me;
        }
    }
}

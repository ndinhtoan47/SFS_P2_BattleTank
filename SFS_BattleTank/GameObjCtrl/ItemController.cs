
using Microsoft.Xna.Framework.Content;
using SFS_BattleTank.Bases;
using SFS_BattleTank.Constants;
using SFS_BattleTank.GameObjects;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using System.Collections.Generic;
namespace SFS_BattleTank.GameObjCtrl
{
    public class ItemController : Controller
    {
        protected Dictionary<int, GameObject> _items;

        public ItemController(ContentManager contents)
            : base(contents)
        {
            _items = new Dictionary<int, GameObject>();
        }

        public override void Update(float deltaTime)
        {
            foreach (GameObject obj in _items.Values)
            {
                obj.Update(deltaTime);
            }
        }
        public override void UpdateData(User user, List<string> changedVars, IMMOItem item)
        {
            base.UpdateData(user, changedVars, item);
        }
        public override void Add(User user,IMMOItem item)
        {
            base.Add(user, item);
        }
        public override void Remove(User user, IMMOItem item)
        {
            base.Remove(user, item);
        }
        public override void Behaviour(string cmd, int id, SFSObject data) { }
        public override Dictionary<int, GameObject> GetAllGameObject() { return _items; }
        public override void Init() { }

    }
}


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
            foreach(GameObject obj in _items.Values)
            {
                obj.Update(deltaTime);
            }
        }
        public override void UpdateData(User user, SFSObject data) 
        {
            if(data.ContainsKey(Consts.BHVR) && data.ContainsKey(Consts.GO_ID))
            {
                int id = (int)data.GetDouble(Consts.GO_ID);
                string behavior = (string)data.GetUtfString(Consts.BHVR);
                if(_items != null && _items.ContainsKey(id))
                {
                    _items[id].Behavior(behavior);
                }
            }
        }
        public override void Add(User user, SFSObject data)
        {
            if (_items != null)
            {
                if (!(data.ContainsKey(Consts.X) &&
                    data.ContainsKey(Consts.Y) &&
                    data.ContainsKey(Consts.GO_ID) &&
                    data.ContainsKey(Consts.TYPE_KIND_OF_ITEM))) return;

                float x = (float)data.GetDouble(Consts.X);
                float y = (float)data.GetDouble(Consts.Y);
                int id = (int)data.GetDouble(Consts.GO_ID);
                int itemKind = (int)data.GetDouble(Consts.TYPE_KIND_OF_ITEM);
                if (_items.ContainsKey(id)) return;
                #region add item
                switch (itemKind)
                {
                    case 0:
                        {
                            _items.Add(id, new Item(x, y, Consts.ES_ITEM_HP));
                            break;
                        }
                    case 1:
                        {
                            _items.Add(id, new Item(x, y, Consts.ES_ITEM_POWER_UP));
                            break;
                        }
                }
                #endregion
                _items[id].LoadContents(_contents);
            }
        }
        public override void Remove(User user, SFSObject data) 
        {
            if(data.ContainsKey(Consts.GO_ID))
            {
                int id = (int)data.GetDouble(Consts.GO_ID);
                if(_items.ContainsKey(id))
                {
                    _items.Remove(id);
                }
            }
        }
        public override void Behaviour(string cmd, int id, SFSObject data) { }
        public override Dictionary<int, GameObject> GetAllGameObject() { return _items; }
        public override void Init() { }

    }
}

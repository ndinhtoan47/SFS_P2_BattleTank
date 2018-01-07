
using Microsoft.Xna.Framework.Content;
using SFS_BattleTank.Bases;
using SFS_BattleTank.Constants;
using SFS_BattleTank.GameObjects;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using System.Collections.Generic;
using System.Diagnostics;
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
            if(item !=null && _items.ContainsKey(item.Id))
            {
                double remainDuration = 0;
                if (item.ContainsVariable(Consts.DURATION)) remainDuration = item.GetVariable(Consts.DURATION).GetDoubleValue();
                remainDuration /= 1000.0f; // convert to second
           
                if (remainDuration <= 3) _items[item.Id].Behavior(Consts.BHVR_ITEM_COUNT_DOWN);
            }
            base.UpdateData(user, changedVars, item);
        }
        public override void Add(User user,IMMOItem item)
        {
            if (item != null && !_items.ContainsKey(item.Id))
            {
                // check type var contain inside item or not
                if (item.ContainsVariable(Consts.TYPE))
                {   // check type is bullet
                    int type = item.GetVariable(Consts.TYPE).GetIntValue() ;
                    if (type == Consts.ES_ITEM_ARMOR || type == Consts.ES_ITEM_ISVISIABLE || type == Consts.ES_ITEM_FREZZE)
                        if (item.ContainsVariable(Consts.X) && item.ContainsVariable(Consts.Y))
                        {
                            _items.Add(item.Id, new Item((float)item.GetVariable(Consts.X).GetIntValue(),
                                                                (float)item.GetVariable(Consts.Y).GetIntValue(),
                                                                type));
                            _items[item.Id].LoadContents(_contents);
                            Debug.WriteLine("Added item bounus " + item.Id);
                        }
                }
            }
            base.Add(user, item);
        }
        public override void Remove(User user, IMMOItem item)
        {
            if (item != null && _items.ContainsKey(item.Id))
            {
                _items.Remove(item.Id);
            }
            base.Remove(user, item);
        }
        public override void Behaviour(string cmd, int id, SFSObject data) { }
        public override Dictionary<int, GameObject> GetAllGameObject() { return _items; }
        public override void Init() { }

    }
}

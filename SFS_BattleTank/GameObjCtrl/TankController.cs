using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using SFS_BattleTank.Bases;
using SFS_BattleTank.Constants;
using SFS_BattleTank.GameObjects;
using SFS_BattleTank.GameScenes;
using SFS_BattleTank.InputControl;
using Sfs2X;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Entities.Variables;
using Sfs2X.Requests;
using Sfs2X.Requests.MMO;
using System.Collections.Generic;
using System.Diagnostics;

namespace SFS_BattleTank.GameObjCtrl
{
    public class TankController : Controller
    {
        protected const int TANK_SPEED = 100;
        protected Dictionary<int, GameObject> _tanks;
        protected float _totalFireTime;
        protected float _delayFire;

        protected int _lastXDir;
        protected int _lastYDir;
        protected int _lastRotation;
        protected float _deltaTime;
        public TankController(ContentManager contents)
            : base(contents)
        {
            _tanks = new Dictionary<int, GameObject>();
            _delayFire = 0.888f;
            _totalFireTime = _delayFire;
            _lastXDir = 1;
            _lastYDir = 0;
            _lastRotation = 0;
        }

        public override void Add(User user, IMMOItem item)
        {
            if (user != null)
            {
                if (_tanks.ContainsKey(user.Id)) return;
                if (user.ContainsVariable(Consts.X) &&
                    user.ContainsVariable(Consts.Y) &&
                    user.ContainsVariable(Consts.ROTATION))
                {
                    _tanks.Add(user.Id, new Tank(user.Id,
                        (float)user.GetVariable(Consts.X).GetIntValue(),
                        (float)user.GetVariable(Consts.Y).GetIntValue()));
                    _tanks[user.Id].SetRotation((int)user.GetVariable(Consts.ROTATION).GetIntValue());
                    _tanks[user.Id].LoadContents(_contents);
                }
            }
            base.Add(user, item);
        }
        public override void Remove(User user, IMMOItem item)
        {
            if (user != null)
            {
                if (!_tanks.ContainsKey(user.Id))
                    return;
                _tanks.Remove(user.Id);
            }
            base.Remove(user, item);
        }
        public override void UpdateData(User user, List<string> changedVars, IMMOItem item)
        {
            if (user != null)
            {
                if (_tanks.ContainsKey(user.Id))
                {
                    List<UserVariable> vars = user.GetVariables();
                    if (changedVars.Contains(Consts.X) ||
                        changedVars.Contains(Consts.Y) ||
                        changedVars.Contains(Consts.ROTATION) ||
                        changedVars.Contains(Consts.ALIVE) ||
                        changedVars.Contains(Consts.KILL) ||
                        changedVars.Contains(Consts.DEATH) ||
                        changedVars.Contains(Consts.TYPE_ITEM))
                    {
                        Tank tank = (Tank)_tanks[user.Id];
                        if (changedVars.Contains(Consts.X) ||
                        changedVars.Contains(Consts.Y) ||
                        changedVars.Contains(Consts.ROTATION)) // position changed
                        {
                            float x = (float)user.GetVariable(Consts.X).GetIntValue();
                            float y = (float)user.GetVariable(Consts.Y).GetIntValue();
                            int r = (int)user.GetVariable(Consts.ROTATION).GetIntValue();
                            tank.SetVariable(x, y, r);
                            tank.Move(_deltaTime);
                        }
                        if (changedVars.Contains(Consts.ALIVE)) // state changed
                        {
                            bool a = (bool)user.GetVariable(Consts.ALIVE).GetBoolValue();
                            if (!a)
                            {
                                tank.Death();
                            }
                            else tank.ReGeneration();
                        }
                        // score changed
                        if ((user.Id == _mySelf) && (changedVars.Contains(Consts.KILL) || changedVars.Contains(Consts.DEATH)))
                        {
                            if (user.ContainsVariable(Consts.KILL))
                            {
                                int kill = (int)user.GetVariable(Consts.KILL).GetIntValue();
                                tank.SetKill(kill);
                                PlayScene._killCount.SetInfo(kill.ToString());
                            }
                            if (user.ContainsVariable(Consts.DEATH))
                            {
                                int death = (int)user.GetVariable(Consts.DEATH).GetIntValue();
                                tank.SetDeath(death);
                                PlayScene._deathCount.SetInfo(death.ToString());
                            }
                        }
                        // receive item
                        if (changedVars.Contains(Consts.TYPE_ITEM))
                        {
                            int itemType = user.GetVariable(Consts.TYPE_ITEM).GetIntValue();
                            tank.SetHodingItem(itemType, user.IsItMe); // use for armor and isvisiable
                            if (itemType == Consts.ES_ITEM_FREZZE)
                            {
                                Tank me = (Tank)_tanks[_mySelf];
                                me.SetHodingItem(itemType, user.IsItMe);
                            }
                        }
                    }
                }
            }
            base.UpdateData(user, changedVars, item);
        }

        public override void Behaviour(string cmd, int id, SFSObject data)
        {
            base.Behaviour(cmd, id, data);
        }
        public override void Update(float deltaTime)
        {
            if (PlayScene.GameIsPlay())
            {
                foreach (GameObject tank in _tanks.Values)
                    tank.Update(deltaTime);
                if (_tanks.ContainsKey(_mySelf))
                {
                    Tank me = (Tank)_tanks[_mySelf];
                    if (me.GetIsAffectByItem() != Consts.ES_ITEM_FREZZE)
                    {
                        if (me.IsAlive())
                        {
                            int rotation;
                            Keys dir = this.GetDirection(out rotation);
                            if (dir != Keys.None)
                                this.Move(deltaTime, rotation);
                            if (CheckFire(deltaTime))
                            {
                                Fire();
                            }
                        }
                    }
                    else
                    {
                        Debug.WriteLine("Is affect by freeze item");
                    }
                    _deltaTime = deltaTime;
                }
            }
            //if (Input.IsKeyDown(Keys.I))
            //{
            //    SFSObject data = new SFSObject();
            //    _network.GetInstance().Send(new ExtensionRequest("item", data, _network.GetCurretRoom()));
            //}
            base.Update(deltaTime);
        }
        public override Dictionary<int, GameObject> GetAllGameObject()
        {
            return _tanks;
        }
        public override void Init()
        {
            _mySelf = GetMySefl();
            base.Init();
        }
        public Keys GetDirection(out int rotation)
        {
            KeyboardState state = Keyboard.GetState();
            Keys[] keys = state.GetPressedKeys();

            Keys dir = Keys.None;
            foreach (Keys i in keys)
            {
                if (i == Keys.Left || i == Keys.Right || i == Keys.Up || i == Keys.Down)
                {
                    dir = i;
                    break;
                }
            }
            switch (dir)
            {
                case Keys.Left:
                    {
                        rotation = 180;
                        break;
                    }
                case Keys.Right:
                    {
                        rotation = 0;
                        break;
                    }
                case Keys.Up:
                    {
                        rotation = -90;
                        break;
                    }
                case Keys.Down:
                    {
                        rotation = 90;
                        break;
                    }
                default:
                    {
                        rotation = _lastRotation;
                        break;
                    }
            }
            return dir;
        }
        // myseft active
        protected void Move(float deltaTime, int rotation)
        {
            if (_tanks.Count <= 0 || !_tanks.ContainsKey(_mySelf)) return;
            SmartFox sfs = _network.GetInstance();
            Tank me = (Tank)_tanks[_mySelf];
            me.Move(deltaTime);          
            // send new update to server    
            if (_network.GetInstance().IsConnected)
            {
                List<UserVariable> vars = new List<UserVariable>();
                vars.Add(new SFSUserVariable(Consts.ROTATION, (int)rotation));
                sfs.Send(new SetUserVariablesRequest(vars));
            }
            _lastRotation = rotation;
        }
        protected bool CheckFire(float deltaTime)
        {
            if (Input.IsKeyDown(Keys.Space))
            {
                if (_delayFire <= _totalFireTime)
                {
                    _totalFireTime = 0;
                    return true;
                }
            }
            _totalFireTime += deltaTime;
            return false;
        }
        protected void Fire()
        {
            SmartFox sfs = _network.GetInstance();
            if (sfs != null)
            {
                SFSObject data = new SFSObject();
                sfs.Send(new ExtensionRequest(Consts.CRQ_FIRE, data, _network.GetCurretRoom()));
            }
        }

    }
}

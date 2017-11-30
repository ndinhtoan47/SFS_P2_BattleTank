using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using SFS_BattleTank.Bases;
using SFS_BattleTank.Constants;
using SFS_BattleTank.GameObjects;
using SFS_BattleTank.InputControl;
using Sfs2X;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Entities.Variables;
using Sfs2X.Requests;
using System.Collections.Generic;

namespace SFS_BattleTank.GameObjCtrl
{
    public class TankController : Controller
    {
        protected const int TANK_SPEED = 100;
        protected Dictionary<int, GameObject> _tanks;
        protected float _totalFireTime;
        protected float _delayFire;
        public TankController(ContentManager contents)
            : base(contents)
        {
            _tanks = new Dictionary<int, GameObject>();
            _totalFireTime = 0;
            _delayFire = 0.75f;
        }

        public override void Add(User user)
        {
            if (user.ContainsVariable(Consts.X) || user.ContainsVariable(Consts.Y))
            {
                if (!_tanks.ContainsKey(user.Id))
                {
                    float x = (float)user.GetVariable(Consts.X).GetDoubleValue();
                    float y = (float)user.GetVariable(Consts.Y).GetDoubleValue();
                    _tanks.Add(user.Id, new Tank(user.Id, x, y));
                    _tanks[user.Id].LoadContents(_contents);
                }
            }
            base.Add(user);
        }
        public override void Remove(User user)
        {
            if (!_tanks.ContainsKey(user.Id))
            {
                return;
            }
            _tanks.Remove(user.Id);
            base.Remove(user);
        }
        public override void GetDirection(out int x, out int y)
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
                        x = -1; y = 0;
                        break;
                    }
                case Keys.Right:
                    {
                        x = 1; y = 0;
                        break;
                    }
                case Keys.Up:
                    {
                        x = 0; y = -1;
                        break;
                    }
                case Keys.Down:
                    {
                        x = 0; y = 1;
                        break;
                    }
                default:
                    {
                        x = 0; y = 0;
                        break;
                    }
            }
            return;
            base.GetDirection(out x, out y);
        }
        public override void UpdateData(User user)
        {
            int me = _network.GetInstance().MySelf.Id;
            if (user.Id != me)
            {
                if (_tanks.ContainsKey(user.Id))
                {
                    if (user.ContainsVariable(Consts.X) || user.ContainsVariable(Consts.Y))
                    {
                        float x = (float)user.GetVariable(Consts.X).GetDoubleValue();
                        float y = (float)user.GetVariable(Consts.Y).GetDoubleValue();
                        _tanks[user.Id].SetPosition(new Vector2(x, y));
                    }
                }
            }
            base.UpdateData(user);
        }
        public override void Behaviour(string cmd, int id, SFSObject data)
        {
            base.Behaviour(cmd, id, data);
        }
        public override void Update(float deltaTime)
        {
            int x, y;
            this.GetDirection(out x, out y);
            Move(deltaTime, x, y);
            base.Update(deltaTime);
        }
        public override Dictionary<int, GameObject> GetAllGameObject()
        {
            return _tanks;
        }
        public override void Init()
        {
            GetMyId();
            base.Init();
        }
        // myseft active
        public void Move(float deltaTime, int xDir, int yDir)
        {
            float vx, vy;
            vx = vy = 0;
            if (xDir != 0) vx = xDir * deltaTime * TANK_SPEED;
            if (yDir != 0) vy = yDir * deltaTime * TANK_SPEED;


            if (_tanks.Count > 0)
            {
                Vector2 curPosition = _tanks[_myId].GetPosition();
                SmartFox sfs = _network.GetInstance();
                if (sfs.MySelf != null)
                {
                    _tanks[_myId].SetPosition(new Vector2(curPosition.X + vx, curPosition.Y + vy));

                    if (xDir != 0)
                    {
                        if (xDir == 1)
                            _tanks[_myId].SetRotation(0);
                        else _tanks[_myId].SetRotation(180);
                    }
                    if (yDir != 0)
                    {
                        if (yDir == 1)
                            _tanks[_myId].SetRotation(90);
                        else _tanks[_myId].SetRotation(-90);
                    }
                }
                else
                {
                    _tanks[_myId].SetPosition(new Vector2(curPosition.X + vx, curPosition.Y + vy));
                }
            }
            // send new update to server
            if (_network.GetInstance().IsConnecting)
            {
                if (_tanks.Count > 0)
                {
                    Vector2 curPosition = _tanks[_myId].GetPosition();
                    List<UserVariable> data = new List<UserVariable>();
                    data.Add(new SFSUserVariable(Consts.VX, vx));
                    data.Add(new SFSUserVariable(Consts.VY, vy));
                    data.Add(new SFSUserVariable(Consts.X, curPosition.X));
                    data.Add(new SFSUserVariable(Consts.Y, curPosition.Y));
                    data.Add(new SFSUserVariable(Consts.XDIR, xDir));
                    data.Add(new SFSUserVariable(Consts.YDIR, yDir));
                    _network.GetInstance().Send(new SetUserVariablesRequest(data));
                }
            }
        }
        public void CheckFire(float deltaTime)
        {
            if (Input.IsKeyDown(Keys.Space))
            {
                if (_delayFire <= _totalFireTime)
                {
                    _totalFireTime = 0;
                    Fire();
                    return;
                }
                _totalFireTime += deltaTime;
            }
        }
        public void Fire()
        { }
    }
}



using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SFS_BattleTank.Bases;
using SFS_BattleTank.Camera;
using SFS_BattleTank.Constants;
using SFS_BattleTank.GameObjCtrl;
using SFS_BattleTank.InputControl;
using SFS_BattleTank.Managers;
using SFS_BattleTank.Maps;
using SFS_BattleTank.UI;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Entities.Variables;
using Sfs2X.Requests;
using Sfs2X.Requests.MMO;
using System.Collections.Generic;
using System.Diagnostics;

namespace SFS_BattleTank.GameScenes
{
    public class PlayScene : Scene
    {
        static public ParticleManager _parManager;
        protected Camera2D _camera;
        protected Texture2D _background;
        protected Map1 map1;

        public static DisplayInfo _deathCount = new DisplayInfo(new Vector2(0, 0), new Rectangle(0, 0, 51, 25));
        public static DisplayInfo _killCount = new DisplayInfo(new Vector2(0, 0), new Rectangle(0, 0, 51, 25));

        public PlayScene(ContentManager contents)
            : base(Consts.SCENE_PLAY, contents)
        {
            _parManager = new ParticleManager();
            _camera = new Camera2D(1008, 1008);
            map1 = new Map1();
        }

        public override bool Init()
        {
            map1.Init();

            _deathCount.SetPosition(new Vector2(Consts.VIEWPORT_WIDTH - _deathCount.GetBoundingBox().Width, _deathCount.GetPosition().Y));
            _killCount.SetPosition(new Vector2(Consts.VIEWPORT_WIDTH - _killCount.GetBoundingBox().Width, _deathCount.GetPosition().Y + _deathCount.GetBoundingBox().Height));
            return base.Init();
        }
        public override bool LoadContents()
        {
            _parManager.LoadContents(_contents);
            _background = _contents.Load<Texture2D>(@"map\background");
            map1.LoadContent(_contents);
            Controller ctrl = _network.GetController(Consts.CTRL_TANK);
            if (ctrl != null)
            {
                Dictionary<int, GameObject> tanks = ctrl.GetAllGameObject();
                foreach (GameObject tank in tanks.Values)
                    tank.LoadContents(_contents);
            }

            _deathCount.LoadContents(_contents);
            _deathCount.ChangeBackground(Consts.UIS_ICON_DEATH);
            _killCount.LoadContents(_contents);
            _killCount.ChangeBackground(Consts.UIS_ICON_KILL);
            return base.LoadContents();
        }
        public override void Shutdown()
        {
            base.Shutdown();
        }
        public override void Update(float deltaTime)
        {
            _network.UpdateControler(deltaTime);
            _parManager.Update(deltaTime);
            GameObject tank = _network.GetMainTank();
            if (tank != null)
                _camera.Update(deltaTime, tank.GetPosition());
            base.Update(deltaTime);
        }
        public override void Draw(SpriteBatch sp)
        {
            sp.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, _camera.GetTransfromMatrix());
            sp.Draw(_background, Vector2.Zero, Color.White);
            DrawObj(sp);
            _parManager.Draw(sp);
            map1.Draw(sp);

            /// show death and kill
            _deathCount.SetPosition(_camera.GetFollowPos() + new Vector2((int)(Consts.VIEWPORT_WIDTH / 2 - _deathCount.GetBoundingBox().Width), (int)(-Consts.VIEWPORT_HEIGHT / 2)));
            _deathCount.Draw(sp);
            _killCount.SetPosition(_camera.GetFollowPos() + new Vector2((int)(Consts.VIEWPORT_WIDTH / 2 - _deathCount.GetBoundingBox().Width), (int)(-Consts.VIEWPORT_HEIGHT / 2 + _deathCount.GetBoundingBox().Height)));
            _killCount.Draw(sp);
            sp.End();
            base.Draw(sp);
        }
        protected void DrawObj(SpriteBatch sp)
        {
            Dictionary<string, Controller> controllers = Game1.network.GetControllers();
            foreach (Controller i in controllers.Values)
            {
                Dictionary<int, GameObject> obj = i.GetAllGameObject();
                foreach (GameObject ii in obj.Values)
                {
                    ii.Draw(sp);
                }
            }
        }
        protected override void AddListener()
        {
            _sfs.AddEventListener(SFSEvent.USER_VARIABLES_UPDATE, OnUserVariableUpdate);
            _sfs.AddEventListener(SFSEvent.PROXIMITY_LIST_UPDATE, OnProximityListUpdate);
            _sfs.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);
            base.AddListener();
        }

        // events handler
        private void OnUserVariableUpdate(BaseEvent e)
        {
            User sender = (User)e.Params["user"];
            List<string> changedVars = (List<string>)e.Params["changedVars"];

            Controller ctrl = _network.GetController(Consts.CTRL_TANK);
            if (ctrl != null)
            {
                ctrl.Add(sender, null);
                ctrl.UpdateData(sender, changedVars, null);
            }
        }
        private void OnProximityListUpdate(BaseEvent e)
        {
            Debug.WriteLine("PlayScene Proximity list updated !");
            List<User> addedUsers = (List<User>)e.Params["addedUsers"];
            List<User> removedUsers = (List<User>)e.Params["removedUsers"];
            List<IMMOItem> addedItems = (List<IMMOItem>)e.Params["addedItems"];
            List<IMMOItem> removedItems = (List<IMMOItem>)e.Params["removedItems"];

            Controller tank = _network.GetController(Consts.CTRL_TANK);
            if (tank != null)
            {
                foreach (User user in addedUsers) tank.Add(user, null);
                foreach (User user in removedUsers) tank.Remove(user, null);
            }
            Controller bullet = _network.GetController(Consts.CTRL_BULLET);
            if (bullet != null)
            {
                if (addedItems.Count > 0)
                    foreach (IMMOItem item in addedItems) bullet.Add(null, item);
                if (removedItems.Count > 0)
                    foreach (IMMOItem item in removedItems) bullet.Remove(null, item);
            }
            Controller items = _network.GetController(Consts.CTRL_ITEM);
            if (items != null)
            {
                if (addedItems.Count > 0)
                    foreach (IMMOItem item in addedItems) items.Add(null, item);
                if (removedItems.Count > 0)
                    foreach (IMMOItem item in removedItems) items.Remove(null, item);
            }
        }
        private void OnExtensionResponse(BaseEvent e)
        {
            string cmd = (string)e.Params["cmd"];
            SFSObject data = (SFSObject)e.Params["params"];
        }
    }
}


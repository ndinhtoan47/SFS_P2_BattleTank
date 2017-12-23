using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SFS_BattleTank.Bases;
using SFS_BattleTank.Constants;
using SFS_BattleTank.GameScenes;
using SFS_BattleTank.InputControl;
using SFS_BattleTank.Sounds;

namespace SFS_BattleTank.UI
{
    public class Button : GameUI
    {
        protected const string DEFAULT_BUTTON_PATH = "button";
        protected const string HOVER_EFFECT_PATH = @"sounds\s_buttonHover";

        protected ContentManager _contents;
        protected string _label;
        protected SpriteFont _font;
        protected float _textScale;
        protected int _behaviorType;
        protected Texture2D _backgroud;
        protected Color _bgColor;
        protected Color _labelColor;
        protected bool _lastState;

        protected bool _hoverCanPlay;
        protected bool _hoverBlockPlay;
        protected SEffect _hoverEffect;

        public Button(string lable, Vector2 position, Rectangle bounding, float textScale)
            : base(Consts.UI_BUTTON, position, bounding)
        {
            _label = lable;
            _textScale = textScale;
            Init();
        }
        public override void Init()
        {
            _isEnable = true;
            _lastState = false;

            _behaviorType = 0;
            InitBoundingBox(_textScale);
            if (_label != "")
                _bgColor = new Color(30, 220, 190, 225);
            else
                _bgColor = Color.White;
            _labelColor = Color.Black;

            _hoverCanPlay = false;
            _hoverBlockPlay = false;
            _hoverEffect = new SEffect();
            base.Init();
        }
        public override void LoadContents(ContentManager contents)
        {
            _contents = contents;
            _font = contents.Load<SpriteFont>("font");
            _backgroud = contents.Load<Texture2D>(DEFAULT_BUTTON_PATH);
            _hoverEffect.LoadContents(_contents, HOVER_EFFECT_PATH);
            base.LoadContents(contents);
        }
        public override void Update(float deltaTime)
        {
            if (_isEnable)
            {
                Hover(_position, _bounding);
                if (CheckInsideUI(_position, _bounding))
                {
                    _hoverCanPlay = true;
                    if (Input.Clicked(Consts.MOUSEBUTTON_LEFT))
                        _lastState = !_lastState;
                }
                else
                {
                    _hoverCanPlay = false;
                    _hoverBlockPlay = false;
                }
            }
            base.Update(deltaTime);
        }
        public override void Draw(SpriteBatch sp)
        {
            // draw bacground
            if (_backgroud != null)
            {
                sp.Draw(_backgroud,
                    new Rectangle((int)_position.X, (int)_position.Y, (int)_bounding.Width, (int)_bounding.Height),
                    _bgColor);
            }
            // draw label   
            Vector2 size = _font.MeasureString(_label);
            Vector2 posDraw = _position + new Vector2((_bounding.Width - size.X) / 2, (_bounding.Height - size.Y) / 2);
            sp.DrawString(_font, _label, posDraw, _labelColor);
            base.Draw(sp);
        }
        public override void CMD(string cmd)
        {
            base.CMD(cmd);
        }
        public override void ChangeBackground(string name)
        {
            if (name != "")
                _backgroud = _contents.Load<Texture2D>(name);
            base.ChangeBackground(name);
        }
        public void SetBGColor(Color color)
        {
            _bgColor = color;
        }
        public void SetLabelColor(Color color)
        {
            _labelColor = color;
        }
        public void SetLabel(string value) { _label = value; }
        public Vector2 GetButtonSize()
        {
            Vector2 result = Vector2.Zero;
            result.X = _bounding.Width;
            result.Y = _font.MeasureString(_label).Y * _textScale;
            return result;
        }
        public bool LastState()
        {
            return _lastState;
        }
        protected override void InitBoundingBox(float textScale)
        {
            if (_label != "")
            {
                int heightPerUnit = 20;
                // textScale = 1 => height = 20px
                _bounding.Height = (int)((float)heightPerUnit * textScale);
            }
            base.InitBoundingBox(textScale);
        }

        protected void Hover(Vector2 position, Rectangle bounding)
        {
            if (CheckInsideUI(position, bounding))
            {
                int a;
                a = (int)(255 * 0.75f);
                _bgColor = new Color(_bgColor.R, _bgColor.G, _bgColor.B, a);
            }
            else
                _bgColor = new Color(_bgColor.R, _bgColor.G, _bgColor.B, 255);

            if (!_hoverBlockPlay && _hoverCanPlay)
            {
                // play sound
                _hoverEffect.Play(0.2f);
                _hoverBlockPlay = true;
            }
        }

    }
}

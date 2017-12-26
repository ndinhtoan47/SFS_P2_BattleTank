using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SFS_BattleTank.Bases;
using SFS_BattleTank.Constants;
using SFS_BattleTank.InputControl;

namespace SFS_BattleTank.UI
{
    public class InputField : GameUI
    {
        protected const string FONT_PATH = "font";
        protected const string CURSOR_PATH = "cursor";

        protected ContentManager _contents;
        protected Texture2D _background;
        protected bool _useBackground;
        protected string _inputText;
        protected string _drawText;
        protected float _delayInput;
        protected float _totalInput;

        protected Texture2D _cursor;
        protected float _delayCursor;
        protected float _totalCusor;
        protected bool _drawCursor;

        protected SpriteFont _font;
        protected float _textScale;

        public InputField(Vector2 position, Rectangle bounding, float textScale, string defaultText = "")
            : base(Consts.UI_INPUT_FIELD, position, bounding)
        {
            _textScale = textScale;
            _isEnable = false;
            _inputText = defaultText;
            Init();
        }
        public override void Init()
        {
            _delayInput = 0.175f;
            _totalInput = 0.0f;
            _delayCursor = 0.5f;
            _totalCusor = 0.0f;
            _useBackground = true;
            InitBoundingBox(_textScale);
            base.Init();
        }
        public override void LoadContents(ContentManager contents)
        {
            _contents = contents;
            _font = contents.Load<SpriteFont>(FONT_PATH);
            _cursor = contents.Load<Texture2D>(CURSOR_PATH);
            _background = contents.Load<Texture2D>(Consts.UIS_INPUT_FIELD);
            base.LoadContents(contents);
        }
        public override void Update(float deltaTime)
        {
            if (_isEnable)
            {
                _inputText = GetInputText(deltaTime);
                UpdateCursor(deltaTime);
            }
            base.Update(deltaTime);
        }
        public override void Draw(SpriteBatch sp)
        {
            // draw background
            if (_background != null && _useBackground)
            {
                sp.Draw(_background,
                    new Rectangle((int)_position.X, (int)_position.Y, (int)_bounding.Width, (int)_bounding.Height),
                    Color.White);
            }
            DrawCursorAndText_DefaultType(sp);
            DrawCursorAndText_IDType(sp);
            base.Draw(sp);
        }
        public override void CMD(string cmd)
        {
            if (cmd == Consts.UI_CMD_INVERSE_USE_BACKGROUND)
                _useBackground = !_useBackground;
            base.CMD(cmd);
        }
        public override void ChangeBackground(string name)
        {
            if (name != "")
                _background = _contents.Load<Texture2D>(name);
            base.ChangeBackground(name);
        }
        public string GetInputText() { return _inputText; }

        protected override void InitBoundingBox(float textScale)
        {
                int heightPerUnit = 20;
                // textScale = 1 => height = 20px
                _bounding.Height = (int)((float)heightPerUnit * textScale);
            base.InitBoundingBox(textScale);
        }
        protected string GetInputText(float deltaTime)
        {
            string result = _inputText;
            Keys[] curPressKeys = Input.GetKeyDowns();
            if (_totalInput >= _delayInput)
            {
                // if has key press
                if (curPressKeys.Length > 0)
                {
                    _totalInput = 0.0f;
                    // loop curPressKeys
                    foreach (Keys k in curPressKeys)
                    {
                        // check key is alphabet or not
                        if ((k >= Keys.A && k <= Keys.Z) ||
                            (k >= Keys.D0 && k <= Keys.D9) ||
                            (k >= Keys.NumPad0 && k <= Keys.NumPad9) ||
                            (k == Keys.Space) || (k == Keys.Back) || (k == Keys.OemPeriod))
                        {
                            switch (k)
                            {
                                case Keys.Back:
                                    {
                                        if (result.Length > 0)
                                        {
                                            result = result.Remove(result.Length - 1);
                                        }
                                        break;
                                    }
                                case Keys.Space:
                                    {
                                        result += " ";
                                        break;
                                    }
                                case Keys.OemPeriod:
                                    {
                                        result += ".";
                                        break;
                                    }
                                default:
                                    {
                                        #region re-define Keys.D and Keys.NumPad
                                        if (k == Keys.D0 || k == Keys.NumPad0)
                                            result += "0";
                                        else if (k == Keys.D1 || k == Keys.NumPad1)
                                            result += "1";
                                        else if (k == Keys.D2 || k == Keys.NumPad2)
                                            result += "2";
                                        else if (k == Keys.D3 || k == Keys.NumPad3)
                                            result += "3";
                                        else if (k == Keys.D4 || k == Keys.NumPad4)
                                            result += "4";
                                        else if (k == Keys.D5 || k == Keys.NumPad5)
                                            result += "5";
                                        else if (k == Keys.D6 || k == Keys.NumPad6)
                                            result += "6";
                                        else if (k == Keys.D7 || k == Keys.NumPad7)
                                            result += "7";
                                        else if (k == Keys.D8 || k == Keys.NumPad8)
                                            result += "8";
                                        else if (k == Keys.D9 || k == Keys.NumPad9)
                                            result += "9";
                                        #endregion
                                        else
                                        {
                                            if (!(IsContaintKeys(curPressKeys, Keys.LeftShift) || IsContaintKeys(curPressKeys, Keys.RightShift)))
                                                result += k.ToString().ToLower();
                                            else
                                            {
                                                result += k.ToString();
                                            }
                                        }
                                        break;
                                    }
                            }
                        }
                    }
                }
            }
            else _totalInput += deltaTime;
            return result;
        }
        protected void UpdateCursor(float deltaTime)
        {
            if (_totalCusor >= _delayCursor)
            {
                _totalCusor = 0;
                _drawCursor = !_drawCursor;
                return;
            }
            _totalCusor += deltaTime;
        }
        protected bool IsContaintKeys(Keys[] allKeys, Keys k)
        {
            foreach (Keys i in allKeys)
                if (i == k)
                {
                    return true;
                }
            return false;
        }
        protected string CheckInputTextMaxSize(SpriteFont font, int maxWidth, string inputText, float textScale)
        {
            string result = inputText;
            Vector2 size = font.MeasureString(inputText) * textScale;
            int count = 1;
            while (size.X > maxWidth && count < inputText.Length)
            {
                result = inputText.Substring(count, inputText.Length - count);
                size = font.MeasureString(result) * textScale;
                count++;
            }
            return result;
        }

        private void DrawCursorAndText_DefaultType(SpriteBatch sp)
        {
            // draw text
            // default background
            if (_background.Name == Consts.UIS_INPUT_FIELD)
            {
                _drawText = CheckInputTextMaxSize(_font, (int)(_bounding.Width * 0.95f), _inputText, _textScale);
                sp.DrawString(
                    spriteFont: _font,
                    text: _drawText,
                    position: _position,
                    scale: _textScale,
                    rotation: 0.0f,
                    effects: SpriteEffects.None,
                    origin: Vector2.Zero,
                    layerDepth: 0.0f,
                    color: Color.Gray);
            }
            // draw cursor
            // use default background
            if (_isEnable)
            {
                Vector2 size = new Vector2(0, _font.MeasureString("0").Y) * _textScale;
                if (_inputText != "") size = _font.MeasureString(_drawText) * _textScale;
                if (_drawCursor && _cursor != null && _background.Name == Consts.UIS_INPUT_FIELD)
                    sp.Draw(_cursor,
                        new Rectangle((int)(_position.X + size.X - 4), (int)_position.Y, (int)(14), (int)((size.Y))),
                        new Color(255, 255, 255, 200));
            }
        }
        private void DrawCursorAndText_IDType(SpriteBatch sp)
        {
            // id bacground
            if (_background.Name == Consts.UIS_ID)
            {
                _drawText = CheckInputTextMaxSize(_font, (int)(_bounding.Width * 0.8f), _inputText, _textScale);
                sp.DrawString(
                    spriteFont: _font,
                    text: _drawText,
                    position: _position + new Vector2(_bounding.Width * 0.15f, 0.0f),
                    scale: _textScale,
                    rotation: 0.0f,
                    effects: SpriteEffects.None,
                    origin: Vector2.Zero,
                    layerDepth: 0.0f,
                    color: Color.Gray);
            }
            // draw cursor
            // use id background
            if (_isEnable)
            {
                Vector2 size = new Vector2(0, _font.MeasureString("0").Y) * _textScale;
                if (_inputText != "") size = _font.MeasureString(_drawText) * _textScale;
                if (_drawCursor && _cursor != null && _background.Name == Consts.UIS_ID)
                    sp.Draw(_cursor,
                        new Rectangle((int)(_position.X + size.X + _bounding.Width * 0.15f), (int)_position.Y, (int)(14), (int)((size.Y))),
                        new Color(255, 255, 255, 200));
            }
        }
    }
}

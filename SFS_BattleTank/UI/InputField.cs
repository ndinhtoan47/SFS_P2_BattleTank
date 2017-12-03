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
        protected string _inputText;
        protected KeyboardState _lastKeyState;
        protected float _delayInput;
        protected float _totalInput;

        protected Texture2D _cursor;
        protected float _delayCursor;
        protected float _totalCusor;
        protected bool _drawCursor;

        protected SpriteFont _font;
        protected float _textScale;

        public InputField(Vector2 position, Rectangle bounding)
            : base(Consts.UI_INPUT_FIELD, position, bounding)
        {
            Init();
        }
        public override void Init()
        {
            _inputText = "";
            _delayInput = 0.175f;
            _totalInput = 0.0f;
            _delayCursor = 0.5f;
            _totalCusor = 0.0f;

            _textScale = 0.5f;
            base.Init();
        }
        public override void LoadContents(ContentManager contents)
        {
            _font = contents.Load<SpriteFont>(FONT_PATH);
            _cursor = contents.Load<Texture2D>(CURSOR_PATH);
            base.LoadContents(contents);
        }
        public override void Update(float deltaTime)
        {
            _inputText = GetInputText(deltaTime);
            UpdateCursor(deltaTime);
            base.Update(deltaTime);
        }
        public override void Draw(SpriteBatch sp)
        {
            sp.DrawString(
                spriteFont: _font,
                text: _inputText,
                position: Vector2.Zero,
                scale: _textScale,
                rotation: 0.0f,
                effects: SpriteEffects.None,
                origin: Vector2.Zero,
                layerDepth: 0.0f,
                color: Color.Red);
            // draw text and cursor
            Vector2 size = new Vector2(0, _font.MeasureString("0").Y);
            if (_inputText != "") size = _font.MeasureString(_inputText) * _textScale;
            if (_drawCursor && _cursor != null)
                if (size.X > 0)
                    sp.Draw(_cursor, new Rectangle((int)size.X - 5, (int)0, (int)(15), (int)((size.Y - 2 * _textScale))), Color.Red);
                else
                    sp.Draw(_cursor, new Rectangle((int)size.X, (int)0, (int)(15 * _textScale), (int)((size.Y - 2) * _textScale)), Color.Red);
            base.Draw(sp);
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
                            (k == Keys.Space) || (k == Keys.Back))
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
                                            result += k.ToString();
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
    }
}

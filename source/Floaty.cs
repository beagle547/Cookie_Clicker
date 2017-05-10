using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Cookie_Clicker {
    class Floaty {

        private Vector2 position;
        private Color color = Color.Black;
        private SpriteFont font;
        private float opacity = 1f,
                      fadeoutSpeed = .03f,
                      speed = 1.5f,
                      rotation = 0f;
        private string label = "";
        private bool active = true;
        public bool Active {
            get { return active; }
            private set { active = value; }
        }

        public Floaty(SpriteFont font, Vector2 position, float value) {
            this.font = font;
            this.position = position;
            label = Math.Round(value, 2).ToString();
        }

        public Floaty(SpriteFont font, Rectangle position, int value) {
            this.font = font;
            this.position = new Vector2(position.X, position.Y);
            label = value.ToString();
        }

        public Floaty(SpriteFont font, Rectangle position, int value, Color newColor) {
            this.font = font;
            this.position = new Vector2(position.X, position.Y);
            color = newColor;
            label = value.ToString();
        }

        public Floaty(SpriteFont font, Vector2 position, string value) {
            this.font = font;
            this.position = position;
            label = value;
        }

        public Floaty(SpriteFont font, Vector2 position, float value, float newRotation, float newSpeed, float newFadeoutSpeed, Color newColor) {

            this.font = font;
            this.position = position;
            label = Math.Round(value, 2).ToString();
            rotation = newRotation;
            speed = newSpeed;
            fadeoutSpeed = newFadeoutSpeed;
            color = newColor;
        }

        public void Update() {
            position.Y += speed;
            opacity -= fadeoutSpeed;
            if(opacity <= 0)
                active = false;
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.DrawString(font, label, position, color * opacity);
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Cookie_Clicker {
    class Slider {

        private Random rng;
        private Vector2 position_label;
        private Vector2 padding = new Vector2(0, 35);
        private Vector2 origin;
        private Texture2D baseTex;
        private List<Texture2D> buttonsTextures;
        private Rectangle rect_button, rect_cookie;
        private SpriteFont font;
        private string label;
        private int curTex = 0, spacing = 5;
        private float scale = .5f;
        private bool status = true;
        private bool played = false;

        public Slider(Texture2D texture, List<Texture2D> textures, Vector2 position, SpriteFont buttonFont, string text, float startValue, Random rnd) {

            rng = rnd;
            font = buttonFont;
            Vector2 tmp_pos = font.MeasureString(text);
            baseTex = texture;
            buttonsTextures = textures;
            curTex = rng.Next(1, buttonsTextures.Count);
            label = text;
            origin = new Vector2((buttonsTextures[0].Width * scale) / 2, (buttonsTextures[0].Height * scale) / 2);
            rect_button = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            rect_cookie = new Rectangle((int)position.X + (int)padding.X + (int)(rect_button.Width * startValue), (int)position.Y + (rect_button.Height / 2) - (int)origin.Y / 2, (int)(buttonsTextures[curTex].Width * scale), (int)(buttonsTextures[curTex].Height * scale));
            position_label = new Vector2(rect_button.X + (rect_button.Width / 2) - (tmp_pos.X / 2), rect_button.Y - tmp_pos.Y - spacing);
        }

        public void setStatus(bool newStatus) {

            if(!status && status != newStatus)
                curTex = rng.Next(1, buttonsTextures.Count);
            status = newStatus;
        }

        public void Update(Rectangle mouse_rect, SoundEffect sound, bool clicked, out float volume) {

            if(rect_button.Intersects(mouse_rect)) {
                if(clicked) {
                    if(!Game1.muteSFX && !played) {
                        sound.Play();
                        played = true;
                    }
                    rect_cookie.X = mouse_rect.X;
                    if(rect_cookie.X > rect_button.X + rect_button.Width - padding.Y)
                        rect_cookie.X = rect_button.X + rect_button.Width - (int)padding.Y;
                    else if(rect_cookie.X < rect_button.X + padding.X)
                        rect_cookie.X = rect_button.X;
                } else {
                    played = false;
                }
            }
            // volume = current position on the slider / max width of the slider
            volume = Math.Abs((rect_cookie.X - rect_button.X) / (rect_button.Width - (padding.X + padding.Y)));
            
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(baseTex, rect_button, Color.White);
            spriteBatch.Draw(buttonsTextures[0], rect_cookie, null, Color.White, 0, origin, SpriteEffects.None, 0);
            if(status)
                spriteBatch.Draw(buttonsTextures[curTex], rect_cookie, null, Color.White, 0, origin, SpriteEffects.None, 0);
            spriteBatch.DrawString(font, label, position_label, Color.Black);
        }

    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Cookie_Clicker {
    class CheckBox {

        private Random rng;
        private Vector2 position_label;
        private List<Texture2D> buttonsTextures;
        private Rectangle rect_button;
        private SpriteFont font;
        private string label;
        private int curTex = 0, spacing = 15;
        private float scale = .5f;
        bool isOn = false;
        
        public CheckBox(List<Texture2D> textures, Vector2 position, SpriteFont buttonFont, string text, Random rnd) {
            
            rng = rnd;
            font = buttonFont;
            Vector2 tmp_pos = font.MeasureString(text);
            buttonsTextures = textures;
            label = text;
            rect_button = new Rectangle((int)position.X, (int)position.Y, (int)(buttonsTextures[curTex].Width * scale), (int)(buttonsTextures[curTex].Height * scale));
            position_label = new Vector2(rect_button.X + rect_button.Width + spacing, rect_button.Y + (rect_button.Height / 2) - (tmp_pos.Y / 2));
        }

        public void Update(Rectangle mouse_rect, SoundEffect sound, bool clicked, out bool status) {

            if(rect_button.Intersects(mouse_rect)) {
                if(clicked) {
                    if(!Game1.muteSFX)
                        sound.Play();
                    isOn = isOn ? false : true;
                    curTex = rng.Next(1, buttonsTextures.Count);
                }
            }

            if(isOn)
                status = true;
            else
                status = false;
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(buttonsTextures[0], rect_button, Color.White);
            if(isOn)
                spriteBatch.Draw(buttonsTextures[curTex], rect_button, Color.White);
            spriteBatch.DrawString(font, label, position_label, Color.Black);
        }

    }
}

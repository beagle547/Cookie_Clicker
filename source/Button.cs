using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Cookie_Clicker {
    class Button {

        private Vector2 position_label;
        private List<Texture2D> buttonsTextures;
        private Rectangle rect_button, rect_cookie;
        private SpriteFont font;
        private string label;
        private int ID, curTex = 1;
        private bool played = false;

        public bool Active { get; set; }

        public Button(List<Texture2D> textures, Vector2 position, SpriteFont buttonFont, string text, int id) {

            font = buttonFont;
            Vector2 tmp_pos = font.MeasureString(text);
            ID = id;
            buttonsTextures = textures;
            label = text;
            Active = false;
            rect_button = new Rectangle((int)position.X, (int)position.Y, buttonsTextures[curTex].Width, buttonsTextures[curTex].Height);
            rect_cookie = new Rectangle((int)position.X + 20, (int)position.Y, buttonsTextures[curTex].Width, buttonsTextures[curTex].Height);
            position_label = new Vector2(rect_button.X + (rect_button.Width / 2) - (tmp_pos.X / 2), rect_button.Y + (rect_button.Height / 2) - (tmp_pos.Y / 2));
        }

        public int Update(Rectangle mouse_rect, List<SoundEffect> sound, bool clicked, int lastState) {

            if(rect_button.Intersects(mouse_rect)) {
                curTex = 2;
                if(!played) {
                    if(!Game1.muteSFX)
                        sound[1].Play();
                    played = true;
                }
                if(clicked) {
                    if(!Game1.muteSFX)
                        sound[0].Play();
                    return ID;
                }
            } else {
                curTex = 1;
                played = false;
            }
            return lastState;
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(buttonsTextures[0], rect_button, Color.White);
            spriteBatch.Draw(buttonsTextures[curTex], rect_cookie, Color.White);
            spriteBatch.DrawString(font, label, position_label, Color.Black);
        }

    }
}

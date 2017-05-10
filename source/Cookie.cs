using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Cookie_Clicker {
    class Cookie {
        
        private Random rng;
        private List<Rectangle> rectangles;
        private List<Texture2D> cookieTextures;
        private Rectangle cookie_rect;
        private Vector2 position, screen, origin = new Vector2 (0, 0);
        private int topping = 0, points = 5, rnd_time_min = 23, rnd_time_max = 30, rnd_time_dec = 1, decrease = 0, decrease_cap = 20;
        private float timer = 0f, time, size;
        private bool repeat;
        public bool Active { get; set; } = false;

        public Cookie(List<Rectangle> barrierList, List<Texture2D> cookies, Vector2 Screen_size, Random rnd) {

            screen = Screen_size;
            rectangles = barrierList;
            cookieTextures = cookies;
            rng = rnd;
            shuffle();
        }

        public void Update(GameTime gameTime, bool time_trigger) {
            if(time_trigger && (decrease < decrease_cap))
                decrease += rnd_time_dec;
            if(Active) {
                timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if(timer > time)
                    Active = false;
            } else {
                if(rng.Next(0,10) == rng.Next(5, 15)) {
                    shuffle();
                }
            }
        }

        public void setDefaults() {
            decrease = 0;
            Active = false;
        }

        public bool UpdateCollision(Rectangle mouse) {
            if(cookie_rect.Intersects(mouse)) {
                    Active = false;
                    return true;
                }
            return false;
        }

        public int GetScore() {
            return (int)(points * topping * size) + 1;
        }

        private void shuffle() {

            timer = 0;
            time = rng.Next(rnd_time_min - decrease, rnd_time_max - decrease);
            size = (float)rng.NextDouble();
            if(size < .6)
                size = .6f;
            topping = rng.Next(0, cookieTextures.Count);

            do {
                repeat = false;
                position = new Vector2(rng.Next(0, (int)screen.X - cookieTextures[0].Width), rng.Next(0, (int)screen.Y - cookieTextures[0].Height));
                cookie_rect = new Rectangle((int)position.X, (int)position.Y, (int)(cookieTextures[0].Width * size), (int)(cookieTextures[0].Height * size));
                for(int i = 0 ; i < rectangles.Count ; i++) {
                    if(cookie_rect.Intersects(rectangles[i])) {
                        repeat = true;
                        break;
                    }
                }
            } while(repeat);
            Active = true;
        }

        public void Draw(SpriteBatch spriteBatch) {

            spriteBatch.Draw(cookieTextures[0], position, null, Color.White, 0, origin, size, SpriteEffects.None, 1);
            if(topping > 0)
                spriteBatch.Draw(cookieTextures[topping], position, null, Color.White, 0, origin, size, SpriteEffects.None, 1);
        }

    }
}

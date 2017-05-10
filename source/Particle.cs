using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cookie_Clicker {
    class Particle {

        private Vector2 position, speed, origin;
        private Rectangle partikell_rect;
        private Texture2D texture;
        private Color color;
        private float angle, angle_speed, size;
        public int lifetime;

        public Particle(Texture2D texture, Vector2 position, Vector2 speed, float angle, float angle_speed, float size, int lifetime, Color color) {

            this.texture = texture;
            this.position = position;
            this.speed = speed;
            this.angle = angle;
            this.angle_speed = angle_speed;
            this.size = size;
            this.lifetime = lifetime;
            this.color = color;
            partikell_rect = new Rectangle(0, 0, texture.Width, texture.Height);
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
        }

        public void Update() {
            
            --lifetime;
            position += speed;
            angle += angle_speed;
        }

        public void Draw(SpriteBatch spriteBatch) {
            
            spriteBatch.Draw(texture, position, partikell_rect, color, angle, origin, size, SpriteEffects.None, 0);
        }
    }
}

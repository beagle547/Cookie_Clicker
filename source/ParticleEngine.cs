using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Cookie_Clicker {
    class ParticleEngine {
        
        public Vector2 SpawnPos { get; set; }
        private List<Particle> particleList;
        private List<Texture2D> textureList;
        private Color[] listColor = { Color.Gray, Color.White, Color.Black, Color.BurlyWood, Color.Wheat, Color.SaddleBrown, Color.SandyBrown, Color.Peru, Color.Chocolate, Color.Sienna };
        private int nColor = 0;
        public bool Active { get; set; } = false;

        public ParticleEngine(List<Texture2D> textures) {
            
            textureList = textures;
            particleList = new List<Particle>();
        }

        private Particle ParticleGenerator(Random rnd) {

            Texture2D texture = textureList[rnd.Next(0, textureList.Count)];
            float   rndSpeedX = (float)(rnd.NextDouble() * 4 - 2),
                    rndSpeedY = (float)(rnd.NextDouble() * 4 - 2),
                    size = (float)rnd.NextDouble(), angle = 0f,
                    angle_speed = (float)rnd.NextDouble();
            int lifetime = rnd.Next(20, 40);
            Vector2 speed = new Vector2(rndSpeedX, rndSpeedY);

            nColor = rnd.Next(3, listColor.Length);
            
            return new Particle (texture, SpawnPos, speed, angle, angle_speed, size, lifetime, listColor[nColor]);
        }

        public void Update(Vector2 pos, Random rnd) {

            SpawnPos = pos;
            if(Active)
                for(int i = 0 ; i < rnd.Next(1, 4) ; ++i) {
                    particleList.Add(ParticleGenerator(rnd));
                }
            Active = false;

            for(int i = particleList.Count - 1 ; i >= 0 ; --i) {
                particleList[i].Update();
                if(particleList[i].lifetime < 0) {
                    particleList.RemoveAt(i);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            for(int i = 0 ; i < particleList.Count ; ++i) {
                particleList[i].Draw(spriteBatch);
            }
        }

    }
}

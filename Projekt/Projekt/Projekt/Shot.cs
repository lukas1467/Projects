using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projekt
{
    class Shot
    {
        Texture2D Texture;
        public Rectangle Rectangle;
        public Vector2 Position;
        public float Ball;

        public Shot(Texture2D texture, Rectangle rectangle, Vector2 position, float ball)
        {
            Texture = texture;
            Rectangle = rectangle;
            Position = position;
            Ball = ball;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rectangle, Color.White);
        }
    }

}

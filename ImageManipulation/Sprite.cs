using ImageLib;

namespace ImageManipulation
{
    public class Sprite
    {
        private Texture texture;
        private Angle angle;

        public Sprite()
        {
            var source = (Bitmap)Bitmap.FromFile("ferrari-f12.png");
            this.texture = Texture.FromBitmap(source);
            this.angle = (Angle)0;
        }

        public Texture Texture => texture;

        public void RotateLeft()
        {
            angle.RotateBy(5);
        }

        public void RotateRight()
        {
            angle.RotateBy(-5);
        }

        public Angle Angle => angle;
    }
}

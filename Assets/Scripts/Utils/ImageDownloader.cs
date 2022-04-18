using System.Threading.Tasks;
using UnityEngine;

namespace Utils
{
    public static class ImageDownloader
    {
        private const string Url = "https://picsum.photos/300/300";
        public static async Task<Sprite> GetRandomImage()
        {
            var www = new WWW(Url);
            while (!www.isDone)
                await Task.Yield();
            var texture = new Texture2D(300, 300);
            www.LoadImageIntoTexture(texture);

            return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f),
                100.0f);
        }
    }
}

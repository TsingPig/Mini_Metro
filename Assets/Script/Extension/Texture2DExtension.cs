using UnityEngine;

namespace TsingPigSDK
{
    public static class Texture2DExtension
    {
        public static Texture2D RandomGenerate(this Texture2D texture)
        {
            if(texture == null)
            {
                Debug.LogError("Texture2D is null.");
                return null;
            }

            int width = texture.width;
            int height = texture.height;

            for(int y = 0; y < height; y++)
            {
                for(int x = 0; x < width; x++)
                {
                    // 生成随机颜色
                    Color randomColor = new Color(Random.value, Random.value, Random.value, 1.0f);

                    texture.SetPixel(x, y, randomColor);
                }
            }
            texture.Apply();
            return texture;
        }

        public static Texture2D Scale(this Texture2D source, float targetWidth, float targetHeight)
        {
            Texture2D result = new Texture2D((int)targetWidth, (int)targetHeight, source.format, false);

            float incX = (1.0f / targetWidth);
            float incY = (1.0f / targetHeight);

            for(int i = 0; i < result.height; ++i)
            {
                for(int j = 0; j < result.width; ++j)
                {
                    Color newColor = source.GetPixelBilinear((float)j / (float)result.width, (float)i / (float)result.height);
                    result.SetPixel(j, i, newColor);
                }
            }

            result.Apply();
            return result;
        }

        public static Texture2D[] Scale(this Texture2D[] source, float targetWidth, float targetHeight)
        {
            Texture2D[] texture2Ds = new Texture2D[source.Length];
            for(int i = 0; i < source.Length; ++i)
            {
                texture2Ds[i] = source[i].Scale(targetWidth, targetHeight);
            }
            return texture2Ds;
        }

        public static byte[][] EncodeToPNG(this Texture2D[] source)
        {
            byte[][] bytes = new byte[source.Length][];
            for(int i = 0; i < source.Length; i++)
            {
                bytes[i] = source[i].EncodeToPNG();
            }
            return bytes;
        }
    }
}
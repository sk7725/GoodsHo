using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextureUtils {
    public static Texture2D ExpandTexture(Texture2D source, int padding) {
        int width = source.width;
        int height = source.height;
        Texture2D expandedTexture = new Texture2D(width + 2 * padding, height + 2 * padding);

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                Color pixelColor = source.GetPixel(x, y);
                expandedTexture.SetPixel(x + padding, y + padding, pixelColor);
            }
        }

        // Set alpha to zero for pixels outside the original texture's bounds
        for (int x = 0; x < expandedTexture.width; x++) {
            for (int y = 0; y < expandedTexture.height; y++) {
                if (x < padding || x >= padding + width || y < padding || y >= padding + height) {
                    Color transparentPixel = new Color(0, 0, 0, 0);
                    expandedTexture.SetPixel(x, y, transparentPixel);
                }
            }
        }

        expandedTexture.Apply();
        return expandedTexture;
    }
    public static Texture2D DownscaleTexture(Texture2D source, int factor) {
        int width = source.width;
        int height = source.height;
        int scaledWidth = width / factor;
        int scaledHeight = height / factor;
        Texture2D scaledTexture = new Texture2D(scaledWidth, scaledHeight);

        for (int x = 0; x < scaledWidth; x++) {
            for (int y = 0; y < scaledHeight; y++) {
                Color pixelColor = source.GetPixel(x * factor, y * factor);
                scaledTexture.SetPixel(x, y, pixelColor);
            }
        }

        scaledTexture.Apply();
        return scaledTexture;
    }
}

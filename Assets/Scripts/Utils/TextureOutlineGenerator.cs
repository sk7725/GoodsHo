using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class TextureOutlineGenerator {
    public static Texture2D Generate(Texture2D source, int pad, int iterations, int downscale) {
        Texture2D initial = DownscaleTexture(source, downscale);
        Texture2D expanded = ExpandTexture(initial, pad);
        Texture2D final = GaussianOutline(expanded, iterations);
        Object.Destroy(expanded);
        Object.Destroy(initial);
        return final;
    }

    static Texture2D ExpandTexture(Texture2D source, int thickness) {
        int width = source.width;
        int height = source.height;
        Texture2D expandedTexture = new Texture2D(width + 2 * thickness, height + 2 * thickness);

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                Color pixelColor = source.GetPixel(x, y);
                expandedTexture.SetPixel(x + thickness, y + thickness, pixelColor);
            }
        }

        // Set alpha to zero for pixels outside the original texture's bounds
        for (int x = 0; x < expandedTexture.width; x++) {
            for (int y = 0; y < expandedTexture.height; y++) {
                if (x < thickness || x >= thickness + width || y < thickness || y >= thickness + height) {
                    Color transparentPixel = new Color(0, 0, 0, 0);
                    expandedTexture.SetPixel(x, y, transparentPixel);
                }
            }
        }

        expandedTexture.Apply();
        return expandedTexture;
    }

    public static Texture2D GaussianOutline(Texture2D source, int blurIterations) {
        for (int i = 0; i < blurIterations; i++) {
            source = ApplyGaussianBlur(source);
        }

        return source;
    }

    static Texture2D DownscaleTexture(Texture2D source, int factor) {
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

    static Texture2D ApplyGaussianBlur(Texture2D source) {
        int width = source.width;
        int height = source.height;
        Texture2D blurredTexture = new Texture2D(width, height);

        float[,] kernel = GaussianKernel(5, 1.4f); // Adjust kernel size and sigma as needed

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                Color pixelColor = GaussianBlurPixel(source, x, y, kernel);
                blurredTexture.SetPixel(x, y, pixelColor);
            }
        }

        blurredTexture.Apply();
        return blurredTexture;
    }

    #region async
    public static void GenerateAsync(Texture2D source, int pad, int iterations, int downscale, LocalizedTMP label, Image loadingBar, System.Action<Texture2D> endAction, MonoBehaviour caller) {
        caller.StartCoroutine(IGenerateAsync(source, pad, iterations, downscale, label, loadingBar, endAction, caller));
    }

    static IEnumerator IGenerateAsync(Texture2D source, int pad, int iterations, int downscale, LocalizedTMP label, Image loadingBar, System.Action<Texture2D> endAction, MonoBehaviour caller) {
        label.Set("outline.downscale");
        loadingBar.fillAmount = 0f;
        yield return null;
        Texture2D initial = DownscaleTexture(source, downscale);

        label.Set("outline.expand");
        yield return null;
        Texture2D expanded = ExpandTexture(initial, pad);
        Object.Destroy(initial);

        Texture2D prev = expanded;
        for (int i = 0; i < iterations; i++) {
            loadingBar.fillAmount = 0f;
            label.Format("outline.gaussian", (i + 1).ToString(), iterations.ToString());
            
            Texture2D next = new Texture2D(prev.width, prev.height);
            yield return caller.StartCoroutine(IGaussStep(prev, next, loadingBar));
            Object.Destroy(prev);
            prev = next;
        }

        label.Clear();
        endAction.Invoke(prev);
    }

    static IEnumerator IGaussStep(Texture2D source, Texture2D output, Image loadingBar) {
        int width = source.width;
        int height = source.height;

        float[,] kernel = GaussianKernel(5, 1.4f); // Adjust kernel size and sigma as needed

        int n = 0, total = width * height;
        int nthresh = 300;

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                Color pixelColor = GaussianBlurPixel(source, x, y, kernel);
                output.SetPixel(x, y, pixelColor);
                n++;
                if (n % nthresh == 0) {
                    loadingBar.fillAmount = n / (float)total;
                    yield return null;
                }
            }
        }

        output.Apply();
        loadingBar.fillAmount = 1;
    }
    #endregion

    static Color GaussianBlurPixel(Texture2D source, int x, int y, float[,] kernel) {
        int width = source.width;
        int height = source.height;
        int kernelSize = kernel.GetLength(0);
        int radius = kernelSize / 2;

        Color blurredColor = Color.clear;

        for (int i = -radius; i <= radius; i++) {
            for (int j = -radius; j <= radius; j++) {
                int neighborX = Mathf.Clamp(x + i, 0, width - 1);
                int neighborY = Mathf.Clamp(y + j, 0, height - 1);

                Color neighborColor = source.GetPixel(neighborX, neighborY);
                float weight = kernel[i + radius, j + radius];

                blurredColor += neighborColor * weight;
            }
        }

        return blurredColor;
    }

    static float[,] GaussianKernel(int size, float sigma) {
        int kernelSize = size;
        int radius = size / 2;
        float[,] kernel = new float[kernelSize, kernelSize];
        float twoSigmaSquared = 2 * sigma * sigma;
        float totalWeight = 0;

        for (int i = -radius; i <= radius; i++) {
            for (int j = -radius; j <= radius; j++) {
                float x = i * i + j * j;
                float weight = Mathf.Exp(-x / twoSigmaSquared);
                kernel[i + radius, j + radius] = weight;
                totalWeight += weight;
            }
        }

        // Normalize the kernel
        for (int i = 0; i < kernelSize; i++) {
            for (int j = 0; j < kernelSize; j++) {
                kernel[i, j] /= totalWeight;
            }
        }

        return kernel;
    }
}
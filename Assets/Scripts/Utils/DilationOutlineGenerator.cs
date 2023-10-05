using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static TextureUtils;

public static class DilationOutlineGenerator {
    public static Texture2D Generate(Texture2D source, int radius, float alphaCutoff, int downscale) {
        Texture2D initial = DownscaleTexture(source, downscale);

        int r = Mathf.CeilToInt(radius / (float)downscale);
        Texture2D expanded = ExpandTexture(initial, r + 2);
        Texture2D final = Dilate(expanded, r, alphaCutoff);   //hi
        Object.Destroy(expanded);
        Object.Destroy(initial);
        return final;
    }

    public static Texture2D Dilate(Texture2D inputTexture, int radius, float alphaCutoff) {
        // Create a circular kernel for dilation
        int kernelSize = radius * 2 + 1;
        int halfKernel = radius;
        int centerX = halfKernel;
        int centerY = halfKernel;
        bool[,] kernel = new bool[kernelSize, kernelSize];

        for (int i = 0; i < kernelSize; i++) {
            for (int j = 0; j < kernelSize; j++) {
                int distance = (int)Mathf.Sqrt(Mathf.Pow(i - centerX, 2) + Mathf.Pow(j - centerY, 2));
                kernel[i, j] = distance <= halfKernel;
            }
        }

        // apply kernel
        int width = inputTexture.width;
        int height = inputTexture.height;
        Texture2D result = new Texture2D(width, height);

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                // Initialize the result pixel as transparent
                Color resultColor = Color.clear;

                for (int i = -halfKernel; i <= halfKernel; i++) {
                    for (int j = -halfKernel; j <= halfKernel; j++) {
                        int offsetX = x + i;
                        int offsetY = y + j;

                        if (offsetX >= 0 && offsetX < width && offsetY >= 0 && offsetY < height) {
                            if (kernel[i + halfKernel, j + halfKernel]) {
                                if (inputTexture.GetPixel(offsetX, offsetY).a >= alphaCutoff) {
                                    resultColor = Color.white;
                                    break;
                                }
                            }
                        }
                    }
                }

                result.SetPixel(x, y, resultColor);
            }
        }

        result.Apply();
        return result;
    }

    #region async
    public static void GenerateAsync(Texture2D source, int radius, float alphaCutoff, int downscale, LocalizedTMP label, Image loadingBar, System.Action<Texture2D> endAction, MonoBehaviour caller) {
        caller.StartCoroutine(IGenerateAsync(source, radius, alphaCutoff, downscale, label, loadingBar, endAction, caller));
    }

    static IEnumerator IGenerateAsync(Texture2D source, int radius, float alphaCutoff, int downscale, LocalizedTMP label, Image loadingBar, System.Action<Texture2D> endAction, MonoBehaviour caller) {
        label.Set("outline.downscale");
        loadingBar.fillAmount = 0f;
        yield return null;
        Texture2D initial = DownscaleTexture(source, downscale);
        int r = Mathf.CeilToInt(radius / (float)downscale);

        label.Set("outline.expand");
        yield return null;
        Texture2D expanded = ExpandTexture(initial, r + 2);
        Object.Destroy(initial);

        label.Set("outline.dilation");
        yield return null;
        Texture2D final = Dilate(expanded, r, alphaCutoff);
        Object.Destroy(expanded);

        label.Clear();
        endAction.Invoke(final);
    }
    #endregion
}
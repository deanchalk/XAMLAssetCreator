using System;
using System.Linq;
using System.Runtime.InteropServices;
using SkiaSharp;

namespace XAMLAssetCreator.Core
{
    public static class SkiaIconFactory
    {
        public static SKColor GetColorInfoFromString(string hex)
        {
            if (string.IsNullOrEmpty(hex))
                return SKColors.Transparent;
            hex = hex.TrimStart('#').Trim();
            if (hex.Length == 8 && hex.StartsWith("00")) return SKColors.Transparent;
            var bytes = Enumerable.Range(0, hex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToList();
            if (bytes.Count > 3) return new SKColor(bytes[1], bytes[2], bytes[3], bytes[0]);
            return new SKColor(bytes[0], bytes[1], bytes[2]);
        }

        public static byte[] CreateIconBytes(string pathData, double size, double pad, string background,
            string foreground, BackgroundType bacgroundType, double leftRightOffset, double upDownOffset)
        {
            var path = PathGeometryFactory.FromString(pathData);
            path.GetBounds(out var rect);
            var padding = (size - pad) / size;
            var scaling = (float) size * (float) padding / Math.Max(rect.Bottom, rect.Right);
            var trans = (float) pad / scaling / 2f;
            var intSize = (int) Math.Ceiling(size);
            var buff = Marshal.AllocCoTaskMem(intSize * intSize * 4);
            try
            {
                using (var surface =
                    SKSurface.Create(new SKPixmap(new SKImageInfo(intSize, intSize, SKImageInfo.PlatformColorType),
                        buff, intSize * 4)))
                {
                    var canvas = surface.Canvas;
                    canvas.Clear(SKColors.Transparent);
                    using (var paint = new SKPaint())
                    {
                        paint.IsAntialias = true;
                        paint.Color = GetColorInfoFromString(background);
                        paint.StrokeCap = SKStrokeCap.Round;
                        switch (bacgroundType)
                        {
                            case BackgroundType.Square:
                                canvas.DrawRect(new SKRect(0, 0, intSize, intSize), paint);
                                break;
                            case BackgroundType.Circle:
                                canvas.DrawOval(new SKRect(0, 0, intSize, intSize), paint);
                                break;
                        }

                        path.Transform(SKMatrix.MakeTranslation(trans, trans));
                        path.Transform(SKMatrix.MakeTranslation((float) leftRightOffset / scaling,
                            (float) upDownOffset / scaling));
                        path.Transform(SKMatrix.MakeScale(scaling, scaling));
                        paint.Color = GetColorInfoFromString(foreground);
                        canvas.DrawPath(path, paint);
                    }

                    var pixels = new byte[intSize * intSize * 4];
                    Marshal.Copy(buff, pixels, 0, pixels.Length);
                    return pixels;
                }
            }
            finally
            {
                if (buff != IntPtr.Zero) Marshal.FreeCoTaskMem(buff);
            }
        }

        public static SKData CreateData(string pathData, double size, double pad, string background,
            string foreground, BackgroundType bacgroundType, double leftRightOffset, double upDownOffset)
        {
            var path = PathGeometryFactory.FromString(pathData);
            path.GetBounds(out var rect);
            var padding = (size - pad) / size;
            var scaling = (float) size * (float) padding / Math.Max(rect.Bottom, rect.Right);
            var trans = (float) pad / scaling / 2f;
            var intSize = (int) Math.Ceiling(size);
            var buff = Marshal.AllocCoTaskMem(intSize * intSize * 4);
            using (var surface =
                SKSurface.Create(new SKPixmap(new SKImageInfo(intSize, intSize, SKImageInfo.PlatformColorType), buff,
                    intSize * 4)))
            {
                var canvas = surface.Canvas;
                canvas.Clear(SKColors.Transparent);
                using (var paint = new SKPaint())
                {
                    paint.IsAntialias = true;
                    paint.Color = GetColorInfoFromString(background);
                    paint.StrokeCap = SKStrokeCap.Round;
                    switch (bacgroundType)
                    {
                        case BackgroundType.Square:
                            canvas.DrawRect(new SKRect(0, 0, intSize, intSize), paint);
                            break;
                        case BackgroundType.Circle:
                            canvas.DrawOval(new SKRect(0, 0, intSize, intSize), paint);
                            break;
                    }

                    path.Transform(SKMatrix.MakeTranslation(trans, trans));
                    path.Transform(SKMatrix.MakeTranslation((float) leftRightOffset / scaling,
                        (float) upDownOffset / scaling));
                    path.Transform(SKMatrix.MakeScale(scaling, scaling));
                    paint.Color = GetColorInfoFromString(foreground);
                    canvas.DrawPath(path, paint);
                }

                var image = surface.Snapshot();
                var data = image.Encode(SKEncodedImageFormat.Png, 1);
                return data;
            }
        }
    }

    public enum BackgroundType
    {
        None,
        Square,
        Circle
    }
}
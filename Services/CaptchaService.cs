using System.Security.Cryptography;
using SkiaSharp;

namespace GuardianAngel.Services;

/// <summary>
/// Generates image captchas and checks answers. The expected code is kept in the
/// user's session and can only be checked once, so every attempt needs a new image.
/// </summary>
public class CaptchaService
{
    private const string SessionKey = "CaptchaCode";
    private const int CodeLength = 5;
    private const int Width = 180;
    private const int Height = 56;

    // No 0/O, 1/I/L, 5/S, 2/Z, 8/B: they are easy to misread when distorted
    private const string Alphabet = "ACDEFGHJKMNPQRTUVWXY34679";

    private static readonly SKColor[] InkColors =
    {
        new(0x18, 0x28, 0x52), // navy
        new(0x2E, 0x6B, 0xB5),
        new(0x1F, 0x4E, 0x79),
        new(0x3A, 0x3A, 0x5C),
    };

    public byte[] CreateImage(ISession session)
    {
        var code = new string(Enumerable.Range(0, CodeLength)
            .Select(_ => Alphabet[RandomNumberGenerator.GetInt32(Alphabet.Length)])
            .ToArray());
        session.SetString(SessionKey, code);
        return Render(code);
    }

    public bool Validate(ISession session, string? answer)
    {
        var expected = session.GetString(SessionKey);
        session.Remove(SessionKey); // one attempt per image

        if (string.IsNullOrEmpty(expected) || string.IsNullOrWhiteSpace(answer))
            return false;

        return string.Equals(expected, answer.Trim(), StringComparison.OrdinalIgnoreCase);
    }

    private static byte[] Render(string code)
    {
        var rnd = Random.Shared;
        using var surface = SKSurface.Create(new SKImageInfo(Width, Height));
        var canvas = surface.Canvas;
        canvas.Clear(new SKColor(0xF4, 0xF8, 0xFC));

        // background speckles
        using (var dot = new SKPaint { IsAntialias = true })
        {
            for (var i = 0; i < 120; i++)
            {
                dot.Color = new SKColor(0x76, 0xB2, 0xE1, (byte)rnd.Next(40, 110));
                canvas.DrawCircle(rnd.Next(Width), rnd.Next(Height), (float)(rnd.NextDouble() * 1.8 + 0.4), dot);
            }
        }

        // characters, each with its own size, angle and vertical offset
        using var typeface = SKTypeface.FromFamilyName("Arial", SKFontStyle.Bold) ?? SKTypeface.Default;
        using var font = new SKFont(typeface, 32);
        using var ink = new SKPaint { IsAntialias = true };
        var step = (Width - 24f) / code.Length;
        for (var i = 0; i < code.Length; i++)
        {
            font.Size = rnd.Next(28, 36);
            ink.Color = InkColors[rnd.Next(InkColors.Length)];
            canvas.Save();
            canvas.Translate(14 + i * step + step / 2, Height / 2f + rnd.Next(-4, 5));
            canvas.RotateDegrees(rnd.Next(-25, 26));
            canvas.DrawText(code[i].ToString(), 0, font.Size / 2.8f, SKTextAlign.Center, font, ink);
            canvas.Restore();
        }

        // crossing curves drawn over the text
        using (var line = new SKPaint { IsAntialias = true, Style = SKPaintStyle.Stroke })
        {
            for (var i = 0; i < 4; i++)
            {
                line.Color = InkColors[rnd.Next(InkColors.Length)].WithAlpha((byte)rnd.Next(110, 180));
                line.StrokeWidth = (float)(rnd.NextDouble() * 1.2 + 1);
                using var builder = new SKPathBuilder();
                builder.MoveTo(0, rnd.Next(Height));
                builder.CubicTo(rnd.Next(Width / 3), rnd.Next(Height),
                                rnd.Next(Width / 3, Width * 2 / 3), rnd.Next(Height),
                                Width, rnd.Next(Height));
                using var path = builder.Detach();
                canvas.DrawPath(path, line);
            }
        }

        using var image = surface.Snapshot();
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        return data.ToArray();
    }
}

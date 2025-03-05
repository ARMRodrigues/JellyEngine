namespace JellyEngine;

public class Curve
{
    private List<CurveKey> keys { get; set; }

    public Curve()
    {
        keys = new List<CurveKey>();
    }
    
    public Curve(params CurveKey[] keyframes)
    {
        keys = keyframes.ToList();
    }

    // Adiciona um ponto de controle à curva
    public void AddKeyframe(CurveKey keyframe)
    {
        keys.Add(keyframe);
        keys.Sort((a, b) => a.Time.CompareTo(b.Time));  // Ordena os pontos pelo tempo
    }

    // Avalia a curva em um determinado tempo
    public float Evaluate(float time)
    {
        if (keys.Count == 0)
            throw new InvalidOperationException("A curva não contém pontos de controle.");

        // Se o tempo está fora dos limites da curva, retorna o primeiro ou último valor
        if (time <= keys[0].Time)
            return keys[0].Value;
        if (time >= keys[keys.Count - 1].Time)
            return keys[keys.Count - 1].Value;

        // Encontrar os dois pontos mais próximos para interpolação
        CurveKey prevKey = null;
        CurveKey nextKey = null;

        foreach (var keyframe in keys)
        {
            if (keyframe.Time <= time)
                prevKey = keyframe;
            if (keyframe.Time > time && nextKey == null)
                nextKey = keyframe;

            if (prevKey != null && nextKey != null)
                break;
        }

        // Interpolação usando tangentes
        float t = (time - prevKey.Time) / (nextKey.Time - prevKey.Time); // Normaliza o tempo entre os dois pontos
        return Interpolate(prevKey, nextKey, t);
    }

    // Interpolação entre dois keyframes usando tangentes
    private float Interpolate(CurveKey prevKeyframe, CurveKey nextKey, float t)
    {
        // A interpolação pode usar uma função de Bezier ou Hermite para considerar as tangentes
        float h00 = 2 * t * t * t - 3 * t * t + 1;    // Função de interpolação Hermite
        float h10 = t * t * t - 2 * t * t + t;        // Função de interpolação Hermite
        float h01 = -2 * t * t * t + 3 * t * t;       // Função de interpolação Hermite
        float h11 = t * t * t - t * t;                 // Função de interpolação Hermite

        // Calculando o valor da curva no ponto usando tangentes
        float value = h00 * prevKeyframe.Value + h10 * prevKeyframe.OutTangent + h01 * nextKey.Value + h11 * nextKey.InTangent;
        return value;
    }
    
    public Texture GenerateCurveTexture(int thickness = 1, int width = 256, int height = 256)
    {
        var pixels = new Color[width * height];
        Array.Fill(pixels, Color.Black);

        const int subSamples = 4;

        for (var x = 0; x < width; x++)
        {
            var t = x / (float)(width - 1);
            var yValue = Evaluate(t);
            var yPos = yValue * (height - 1); // A posição 'y' exata na textura

            for (var y = 0; y < height; y++)
            {
                var minDistance = float.MaxValue;

                for (var i = 0; i < subSamples; i++)
                {
                    var sampleT = t + (i / (float)subSamples - 0.5f) / width;
                    var sampleY = Evaluate(sampleT) * (height - 1);
                    minDistance = Math.Min(minDistance, Math.Abs(y - sampleY));
                }

                if (minDistance < thickness / 2f)
                {
                    var alpha = 1.0f - (minDistance / (thickness / 2f));
                    alpha = Math.Clamp(alpha, 0f, 1f);

                    var index = y * width + x;
                    pixels[index] = Color.Lerp(pixels[index], Color.White, alpha);
                }
            }
        }

        return new Texture(width, height, pixels);
    }
}
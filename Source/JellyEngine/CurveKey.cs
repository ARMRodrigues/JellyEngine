namespace JellyEngine;

public class CurveKey
{
    public float Time { get; set; }  // Tempo ou ponto ao longo do eixo X
    public float Value { get; set; } // Valor da curva no ponto
    public float InTangent { get; set; }  // Tangente de entrada
    public float OutTangent { get; set; } // Tangente de saída

    public CurveKey(float time, float value, float inTangent = 0f, float outTangent = 0f)
    {
        Time = time;
        Value = value;
        InTangent = inTangent;
        OutTangent = outTangent;
    }
}
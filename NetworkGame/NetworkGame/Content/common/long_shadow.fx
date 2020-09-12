float ShadowLength;
float PixelWidth;
float PixelHeight;

sampler2D _sampler;

float4 LongShadow(float2 coords: TEXCOORD0) : COLOR0
{
	[unroll(64)] for (int i = 0; i < ShadowLength; i++)
	{
		float4 borderColor = tex2D(_sampler, float2(coords.x - i * PixelWidth, coords.y - i * PixelHeight));
		if (borderColor.a > .5f)
			return float4(.0f, .0f, .0f, 0.2f * ((ShadowLength - i) / (ShadowLength + 1)));
	}
	return float4(.0f, .0f, .0f, .0f);
}

technique Technique0
{
    pass Pass0
    {
        PixelShader = compile ps_3_0 LongShadow();
    }
}
Shader "UI/BlurShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _BlurAmount("Blur Amount", Range(0, 10)) = 1.0
    }
        SubShader
        {
            Tags { "Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True" "CanUseSpriteAtlas" = "True" }
            LOD 100
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata_t
                {
                    float4 vertex : POSITION;
                    float2 texcoord : TEXCOORD0;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                sampler2D _MainTex;
                float _BlurAmount;

                v2f vert(appdata_t v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.texcoord;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    float2 uv = i.uv;
                    fixed4 color = tex2D(_MainTex, uv) * 0.36;

                    // 주변 픽셀 샘플링으로 블러 효과 적용
                    color += tex2D(_MainTex, uv + float2(_BlurAmount / _ScreenParams.x, 0)) * 0.18;
                    color += tex2D(_MainTex, uv - float2(_BlurAmount / _ScreenParams.x, 0)) * 0.18;
                    color += tex2D(_MainTex, uv + float2(0, _BlurAmount / _ScreenParams.y)) * 0.18;
                    color += tex2D(_MainTex, uv - float2(0, _BlurAmount / _ScreenParams.y)) * 0.18;

                    return color;
                }
                ENDCG
            }
        }
}
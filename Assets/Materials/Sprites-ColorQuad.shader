Shader "Sprites/ColorQuad"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma shader_feature ETC1_EXTERNAL_ALPHA
			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
				float2 screenPos : TEXCOORD1;
			};
			
			fixed4 _Color;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif
				OUT.screenPos = ComputeScreenPos(OUT.vertex);

				return OUT;
			}

			sampler2D _MainTex;
			sampler2D _AlphaTex;

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

#if ETC1_EXTERNAL_ALPHA
				// get the color from an external texture (usecase: Alpha support for ETC1 on android)
				color.a = tex2D (_AlphaTex, uv).r;
#endif //ETC1_EXTERNAL_ALPHA

				return color;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
				c.rgb *= c.a;
				if (IN.screenPos.x < 0.5 && IN.screenPos.y > 0.5) {
					c.gb *= 0.94 * ((IN.screenPos.x) / 0.5 / 4 + 0.75) * ((1.0 - IN.screenPos.y) / 0.5 / 4 + 0.75);
				}
				else if (IN.screenPos.x > 0.5 && IN.screenPos.y > 0.5) {
					c.rb *= 0.94 * ((1.0 - IN.screenPos.x) / 0.5 / 4 + 0.75) * ((1.0 - IN.screenPos.y) / 0.5 / 4 + 0.75);
				}
				else if (IN.screenPos.x < 0.5 && IN.screenPos.y < 0.5) {
					c.rg *= 0.94 * ((IN.screenPos.x) / 0.5 / 4 + 0.75) * ((IN.screenPos.y) / 0.5 / 4 + 0.75);
				}
				else if (IN.screenPos.x > 0.5 && IN.screenPos.y < 0.5) {
					c.r *= 0.94 * ((1.0 - IN.screenPos.x) / 0.5 / 4 + 0.75) * ((IN.screenPos.y) / 0.5 / 4 + 0.75);
				}

				return c;
			}
		ENDCG
		}
	}
}

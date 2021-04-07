// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Chroma Key Colored"
{
	Properties
	{
		_MainColor ("Main Color", Color) = (1,1,1,1)
		_SubColor ("Sub Color", Color) = (1,1,1,1)
		_Sens("Sensibilidad", Range(0,1)) = 0.3
		_Cutoff("Cutoff", Range(0, 1)) = 0.1
		_MainTex ("Base (RGB), Alpha (A)", 2D) = "black" {}
	}
	
	SubShader
	{
		LOD 200

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"RenderPipeline" = "UniversalPipeline"
		}
		
		Pass
		{
			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			Offset -1, -1
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag			
			#include "UnityCG.cginc"
			
			CBUFFER_START(UnityPerMaterial)

			half4 _MainColor; 
			half4 _SubColor;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Cutoff;
			float _Sens;

			CBUFFER_END
	
			struct appdata_t
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};
	
			struct v2f
			{
				float4 vertex : SV_POSITION;
				half2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};
	
			v2f o;

			v2f vert (appdata_t v)
			{
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = v.texcoord;
				o.color = v.color;

				return o;
			}
				
			fixed4 frag (v2f IN) : COLOR
			{
				half4 c = tex2D(_MainTex, IN.texcoord) * IN.color;

				float amR = abs(c.r - _MainColor.r) < _Sens ? abs(c.r - _MainColor.r) : 1;
				float amG = abs(c.g - _MainColor.g) < _Sens ? abs(c.g - _MainColor.g) : 1;
				float amB = abs(c.b - _MainColor.b) < _Sens ? abs(c.b - _MainColor.b) : 1;

				float asR = abs(c.r - _SubColor.r) < _Sens ? abs(c.r - _SubColor.r) : 1;
				float asG = abs(c.g - _SubColor.g) < _Sens ? abs(c.g - _SubColor.g) : 1;
				float asB = abs(c.b - _SubColor.b) < _Sens ? abs(c.b - _SubColor.b) : 1;

				float am = (amR + amG + amB) / 3;
				float as = (asR + asG + asB) / 3;

				if (am < _Cutoff || as < _Cutoff) {
					c.a = 0;
				}
				else {
					c.a = 1;
				}

				/*if (c.g != _Color.g && c.r != _Color.r && c.b != _Color.b)
				{
					c.a = c.a;
				}
				else
				{
					c.a = 0;
				}*/

				return c;
			}
			ENDCG
		}
	}

	SubShader
	{
		LOD 100

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		
		Pass
		{
			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			Offset -1, -1
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMaterial AmbientAndDiffuse
			
			SetTexture [_MainTex]
			{
				Combine Texture * Primary
			}
		}
	}
}

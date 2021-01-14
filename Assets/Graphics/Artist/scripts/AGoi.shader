Shader "AGoi"
{
	Properties
	{
		[Header(Main)]
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color tint", Color) = (1,1,1,1)

		[Header(Light)]
		_LitFactor ("Lit factor", Range(0, 1)) = 0.5
		_LitAdd ("Lit add", Range(-1, 1)) = 0
		_LitColor ("Lit color", Color) = (1,1,1,0) // replacing light color by alpha value
		
		[Header(Ambient)]
		_AmbientFactor ("Ambient factor", Range(0, 1)) = 0.25
        _AmbientColor ("Ambient Color", Color) = (1,1,1,1)
		[KeywordEnum(Multiply, Add)] _AmbientMode ("Ambient mode", Float) = 0

		[Header(Shadow)]
		_ShadowFactor ("Shadow factor", Range(0, 1)) = 0.25
		_ShadowColor ("Shadow color", Color) = (0,0,0,1)

		[Header(Special)]
		[Enum(UnityEngine.Rendering.CullMode)] _Culling ("Culling", Float) = 2
	}

	SubShader
	{
		Tags { "RenderType"="Opaque" "Queue"="Geometry" "LightMode"="ForwardBase" }
		Cull [_Culling]

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma exclude_renderers flash
			#pragma multi_compile_fog
			#pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight

			#include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc"
			#include "AutoLight.cginc"

			sampler2D _MainTex;
			fixed4 _MainTex_ST;
			fixed4 _Color;

			fixed _LitFactor;
			fixed _LitAdd;
			fixed4 _LitColor;

			fixed _AmbientFactor;
			fixed4 _AmbientColor;
			fixed _AmbientMode;

			fixed _ShadowFactor;
			fixed4 _ShadowColor;
	
			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
				float4 color : COLOR0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 color : COLOR0;
				float4 diff : COLOR1;
				float nl : COLOR2;
				SHADOW_COORDS(1)
			};


			v2f vert (appdata v)
			{
				half3 worldNormal = UnityObjectToWorldNormal(v.normal);

				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.color = v.color;

				o.nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
				o.diff = o.nl;
				o.diff.rgb += ShadeSH9(half4(worldNormal,1));

				TRANSFER_SHADOW(o);

				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv) * i.color;
				col.rgb = lerp(col.rgb, col.rgb * _Color.rgb, _Color.a);
				col.rgb = lerp(col.rgb, col.rgb * (i.diff + _LitAdd) * lerp(_LightColor0, _LitColor, _LitColor.a), _LitFactor);

				fixed ambientFactor = _AmbientFactor * (1 - i.nl) * _AmbientColor.a;
				if(_AmbientMode < 1) col.rgb = lerp(col.rgb, col.rgb * _AmbientColor.rgb, ambientFactor);
				else col.rgb = lerp(col.rgb, col.rgb + _AmbientColor.rgb, ambientFactor);
				
				half shadow = SHADOW_ATTENUATION(i);
				shadow = lerp(shadow, 1, 1 - i.nl);
				col.rgb = lerp(col.rgb, _ShadowColor.rgb, _ShadowFactor * (1-shadow) * _ShadowColor.a);

				return col;
			}
			ENDCG
		}
		
		UsePass "VertexLit/ShadowCaster"
	}

}

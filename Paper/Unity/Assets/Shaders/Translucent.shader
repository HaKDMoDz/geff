Shader "Custom/Translucent" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_BumpMap ("Normal (Normal)", 2D) = "bump" {}
		_Color ("Main Color", Color) = (1,1,1,1)
		_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
		_Shininess ("Shininess", Range (0.03, 1)) = 0.078125

		//_Thickness = Thickness texture - (invert normals, bake AO).
		//_Power = "Sharpness" of translucent glow.
		//_Distortion = Subsurface distortion, shifts surface normal, effectively a refractive index.
		//_Scale = Multiplier for translucent glow - should be per-light, really.
		//_SubColor = Subsurface colour.
		_Thickness ("Thickness (R)", 2D) = "bump" {}
		_Power ("Subsurface Power", Float) = 1.0
		_Distortion ("Subsurface Distortion", Float) = 0.0
		_Scale ("Subsurface Scale", Float) = 0.5
		_SubColor ("Subsurface Color", Color) = (1.0, 1.0, 1.0, 1.0)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Translucent
		#pragma target 3.0

		struct SurfaceOutputTranslucent {
			fixed3 Albedo;
			fixed3 Normal;
			fixed3 Emission;
			half Specular;
			fixed Gloss;
			fixed Alpha;
			fixed Thickness;
		};

		sampler2D _MainTex, _BumpMap, _Thickness;
		float _Power, _Distortion, _Scale;
		fixed4 _Color, _SubColor;
		half _Shininess;

		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
		};

		void surf (Input IN, inout SurfaceOutputTranslucent o) {
			fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = tex.rgb * _Color.rgb;
			o.Gloss = tex.a;
			o.Alpha = tex.a * _Color.a;
			o.Specular = _Shininess;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
			o.Thickness = tex2D(_Thickness, IN.uv_MainTex).r;
		}

		inline fixed4 LightingTranslucent (SurfaceOutputTranslucent s, fixed3 lightDir, fixed3 viewDir, fixed atten)
		{		
			// Normalize the vectors, you can remove these two lines if you like, just looks better.
			viewDir = normalize ( viewDir );
			lightDir = normalize ( lightDir );

			// Translucency.
			float3 transLightDir = lightDir + s.Normal * _Distortion;
			float transDot = pow ( saturate ( dot ( viewDir, -transLightDir ) ), _Power ) * _Scale;
			fixed3 transLight = (atten * 2) * ( transDot + UNITY_LIGHTMODEL_AMBIENT.rgb ) * s.Thickness * _SubColor.rgb;
			fixed3 transAlbedo = s.Albedo * _LightColor0.rgb * transLight;

			// Regular BlinnPhong.
			half3 h = normalize (lightDir + viewDir);
			fixed diff = max (0, dot (s.Normal, lightDir));
			float nh = max (0, dot (s.Normal, h));
			float spec = pow (nh, s.Specular*128.0) * s.Gloss;
			fixed3 diffAlbedo = (s.Albedo * _LightColor0.rgb * diff + _LightColor0.rgb * _SpecColor.rgb * spec) * (atten * 2);

			// Add the two together.
			fixed4 c;
			c.rgb = diffAlbedo + transAlbedo;
			c.a = s.Alpha + _LightColor0.a * _SpecColor.a * spec * atten;
			return c;
		}

		ENDCG
	}
	FallBack "Bumped Diffuse"
}
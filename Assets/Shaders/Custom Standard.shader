// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Azee/Custom Standard"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,0)
		_MainTex("Main Tex", 2D) = "white" {}
		_Metallic("Metallic", 2D) = "white" {}
		_MetallicMultiply("MetallicMultiply", Range( 0 , 1)) = 0
		_Glossiness("Glossiness", 2D) = "white" {}
		_GlossinessMultiply("GlossinessMultiply", Range( 0 , 1)) = 0.8
		[Normal]_NormalMap("Normal Map", 2D) = "bump" {}
		_Occlusion("Occlusion", 2D) = "white" {}
		[HDR]_EmissionColor("EmissionColor", Color) = (0,0,0,0)
		[HDR]_EmissionMask("EmissionMask", 2D) = "white" {}
		_EmissiveValue("EmissiveValue", Range( 0 , 1)) = 0
		[Enum(UnityEngine.Rendering.CullMode)]_CullMode("Cull Mode", Int) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull [_CullMode]
		Blend SrcAlpha OneMinusSrcAlpha
		BlendOp Add
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform int _CullMode;
		uniform float DepthCurvatureCurve;
		uniform float DepthCurvatureMinDepth;
		uniform sampler2D _NormalMap;
		uniform float4 _NormalMap_ST;
		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;
		uniform float4 _Color;
		uniform sampler2D _EmissionMask;
		uniform float4 _EmissionMask_ST;
		uniform float4 _EmissionColor;
		uniform float _EmissiveValue;
		uniform sampler2D _Metallic;
		uniform float4 _Metallic_ST;
		uniform float _MetallicMultiply;
		uniform sampler2D _Glossiness;
		uniform float4 _Glossiness_ST;
		uniform float _GlossinessMultiply;
		uniform sampler2D _Occlusion;
		uniform float4 _Occlusion_ST;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float4 ase_screenPos = ComputeScreenPos( UnityObjectToClipPos( v.vertex ) );
			float temp_output_3_0_g1 = ( 1.0 - ase_screenPos.z );
			float temp_output_4_0_g1 = (0.0 + (temp_output_3_0_g1 - DepthCurvatureMinDepth) * (1.0 - 0.0) / (1.0 - DepthCurvatureMinDepth));
			float4 appendResult8_g1 = (float4(( DepthCurvatureCurve *  ( temp_output_3_0_g1 - 0.0 > DepthCurvatureMinDepth ? temp_output_4_0_g1 : temp_output_3_0_g1 - 0.0 <= DepthCurvatureMinDepth && temp_output_3_0_g1 + 0.0 >= DepthCurvatureMinDepth ? temp_output_4_0_g1 : 0.0 )  ) , 0.0 , 0.0 , 0.0));
			float4 transform10_g1 = mul(unity_WorldToObject,appendResult8_g1);
			v.vertex.xyz += transform10_g1.xyz;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_NormalMap = i.uv_texcoord * _NormalMap_ST.xy + _NormalMap_ST.zw;
			o.Normal = UnpackNormal( tex2D( _NormalMap, uv_NormalMap ) );
			float2 uv_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
			float4 temp_output_25_0 = ( tex2D( _MainTex, uv_MainTex ) * _Color );
			o.Albedo = temp_output_25_0.rgb;
			float2 uv_EmissionMask = i.uv_texcoord * _EmissionMask_ST.xy + _EmissionMask_ST.zw;
			o.Emission = ( ( tex2D( _EmissionMask, uv_EmissionMask ) * _EmissionColor ) * _EmissiveValue ).rgb;
			float2 uv_Metallic = i.uv_texcoord * _Metallic_ST.xy + _Metallic_ST.zw;
			o.Metallic = ( tex2D( _Metallic, uv_Metallic ) * _MetallicMultiply ).r;
			float2 uv_Glossiness = i.uv_texcoord * _Glossiness_ST.xy + _Glossiness_ST.zw;
			o.Smoothness = ( tex2D( _Glossiness, uv_Glossiness ) * _GlossinessMultiply ).r;
			float2 uv_Occlusion = i.uv_texcoord * _Occlusion_ST.xy + _Occlusion_ST.zw;
			o.Occlusion = tex2D( _Occlusion, uv_Occlusion ).r;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17000
194;224;1376;631;2579.3;118.0865;2.380174;True;False
Node;AmplifyShaderEditor.ColorNode;11;-618.3997,336.8997;Float;False;Property;_EmissionColor;EmissionColor;8;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0,0,0,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;22;-910.798,178.6008;Float;True;Property;_EmissionMask;EmissionMask;9;1;[HDR];Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;20;-930.1999,745.401;Float;True;Property;_Glossiness;Glossiness;4;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-450.0188,248.0141;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-588.3998,540.4009;Float;False;Property;_MetallicMultiply;MetallicMultiply;3;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;19;-925.1999,542.4009;Float;True;Property;_Metallic;Metallic;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;18;-390.7999,381.3999;Float;False;Property;_EmissiveValue;EmissiveValue;10;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;14;-916.3,-268.5001;Float;True;Property;_MainTex;Main Tex;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;24;-830.9921,-449.6961;Float;False;Property;_Color;Color;0;0;Create;True;0;0;False;0;1,1,1,0;1,0,0,0.5450981;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;8;-593.8998,757.401;Float;False;Property;_GlossinessMultiply;GlossinessMultiply;5;0;Create;True;0;0;False;0;0.8;0.8;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-253.8001,251.4002;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;23;-925.0978,970.301;Float;True;Property;_Occlusion;Occlusion;7;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-370.8998,733.401;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;2;-360.5827,532.2181;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;50;-856.4211,1215.558;Float;False;DepthCurvatureEffect;-1;;1;cf22edd91c6d53444a770cf0846fe0d5;0;0;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-519.2061,-264.8799;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.BreakToComponentsNode;51;-336.227,-300.6212;Float;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SamplerNode;15;-913.3,-49.50001;Float;True;Property;_NormalMap;Normal Map;6;1;[Normal];Create;True;0;0;False;0;None;None;True;0;False;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.IntNode;27;-1214.282,821.5999;Float;False;Property;_CullMode;Cull Mode;11;1;[Enum];Create;True;0;1;UnityEngine.Rendering.CullMode;True;0;0;0;0;1;INT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;71.49998,157.3001;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Azee/Custom Standard;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;True;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;4;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;1;False;-1;1;False;-1;1;False;-1;1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;3;-1;-1;-1;0;False;0;0;True;27;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;26;0;22;0
WireConnection;26;1;11;0
WireConnection;17;0;26;0
WireConnection;17;1;18;0
WireConnection;7;0;20;0
WireConnection;7;1;8;0
WireConnection;2;0;19;0
WireConnection;2;1;4;0
WireConnection;25;0;14;0
WireConnection;25;1;24;0
WireConnection;51;0;25;0
WireConnection;0;0;25;0
WireConnection;0;1;15;0
WireConnection;0;2;17;0
WireConnection;0;3;2;0
WireConnection;0;4;7;0
WireConnection;0;5;23;0
WireConnection;0;9;51;3
WireConnection;0;11;50;0
ASEEND*/
//CHKSM=0964F45F265974DA12618C75CBC2C6E2267F922D
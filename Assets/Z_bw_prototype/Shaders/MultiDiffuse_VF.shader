Shader "Custom/MultiDiffuse_VF"
{

	Properties {

		_MainTex ("Diffuse", 2D) = "white" {}

      	_MultiTex("MultiplyTexture", 2D) = "white" {}
      	
      	_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5


    }
    
    SubShader {
        Tags
		{
			"Queue"="Transparent"
			"IgnoreProjector"="False"
			"RenderType"="TransparentCutout"

		}
        LOD 200
 
        Pass {

 
            CGPROGRAM
			// Upgrade NOTE: excluded shader from DX11 and Xbox360; has structs without semantics (struct v2f members uv,color)
			#pragma exclude_renderers d3d11 xbox360
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
 
            sampler2D _MainTex;
            fixed4 _MainTex_ST;
            fixed _Cutoff;

            sampler2D _MultiTex;
            fixed4 _MultiTex_ST;

			struct a2v {
                float4 vertex : POSITION;
                float2  texcoord : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
            };


            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;              
                float2 uv2 : TEXCOORD1;
              
                  
            };
 
 			
            v2f vert (a2v v)
            {
                v2f o;
                o.pos = mul( UNITY_MATRIX_MVP, v.vertex);
                
                o.uv2 = TRANSFORM_TEX (v.texcoord, _MainTex);
                o.uv = TRANSFORM_TEX (v.texcoord1, _MultiTex);
                  
                return o;
            }
 
            float4 frag(v2f i) : COLOR
            {

                float4 c = tex2D (_MainTex, i.uv2);
                

                float4 m=tex2D(_MultiTex,i.uv);
                

                c.rgb =c * m;
                clip(c.a - _Cutoff);
                

                
                
                return c;
 
            }
 
            ENDCG
        }
    }
    FallBack "Diffuse"
}
Shader "Custom/MultiDiffuse_VF"
{

	Properties {

		_MainTex ("Diffuse", 2D) = "white" {}

      	_MultiTex("MultiplyTexture", 2D) = "white" {}


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
 
            Cull Back
			ZWrite On
			Alphatest Greater 0.5

 
            CGPROGRAM
			// Upgrade NOTE: excluded shader from DX11 and Xbox360; has structs without semantics (struct v2f members uv,color)
			#pragma exclude_renderers d3d11 xbox360
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
 
            sampler2D _MainTex;
            fixed4 _MainTex_ST;

            sampler2D _MultiTex;
            fixed4 _MultiTex_ST;


            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
                fixed3 color : COLOR; 
                
                float2 uv2 : TEXCOORD2;
              
                  
            };
 
 			
            v2f vert (appdata_base v)
            {
                v2f o;
                o.pos = mul( UNITY_MATRIX_MVP, v.vertex);
                
                o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
                o.uv2 = TRANSFORM_TEX (v.texcoord, _MultiTex);
                  
                return o;
            }
 
            float4 frag(v2f i) : COLOR
            {

                float4 c = tex2D (_MainTex, i.uv);
                

                float4 m=tex2D(_MultiTex,i.uv2);
                

                c.rgb =c * m;

                
                
                return c;
 
            }
 
            ENDCG
        }
    }
    FallBack "Diffuse"
}
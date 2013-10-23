Shader "Antigollos/DiffuseReflectDissolveUnlit_VF"
{

	Properties {
		_DiffuseColor("DiffuseColor", Color) = (1,1,1,1)
		_MainTex ("Diffuse", 2D) = "white" {}
	
		_ReflectionColor("ReflectionColor", Color) = (1,1,1,1)
    	_Reflective("Reflective", Cube) = "black" {}
    	_ReflectionStrength("ReflectionStrength", Float) = 1
      	_ReflectionMask("ReflectionMask", 2D) = "black" {}
      	_DissolveColor("DissolveColor", Color) = (1,1,1,1)
      	_DissolveWidth("DissolveWidth", Float) = 0.03
		_DissolveTexture("DissolveTexture", 2D) = "black" {}				
		_AlphaCutOut("AlphaCutOut", Range(0,1) ) = 0

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
			AlphaTest Greater [_AlphaCutOut]
 
            CGPROGRAM
			// Upgrade NOTE: excluded shader from DX11 and Xbox360; has structs without semantics (struct v2f members uv,color)
			#pragma exclude_renderers d3d11 xbox360
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
 
            sampler2D _MainTex;
            fixed4 _MainTex_ST;
            uniform float4 _DiffuseColor;
            
            samplerCUBE _Reflective;
            half3 simpleWorldRefl;
            uniform float4 _ReflectionColor;
            uniform float _ReflectionStrength;
            sampler2D _ReflectionMask;
            fixed4 _ReflectionMask_ST;

            
            sampler2D _DissolveTexture;
            fixed4 _DissolveTexture_ST;
			float4 _DissolveColor;
			float _DissolveWidth;
			float _AlphaCutOut;           

            uniform float4 _RimColor;
            uniform float _RimWidth;
            
            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
                fixed3 color : COLOR; 
                fixed3 simpleWorldRefl : TEXCOORD1;                
                float2 uv2 : TEXCOORD2;
                float2 uv3 : TEXCOORD3;               
                  
            };
 
 			
            v2f vert (appdata_base v)
            {
                v2f o;
                o.pos = mul( UNITY_MATRIX_MVP, v.vertex);
                
                o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
                o.uv2 = TRANSFORM_TEX (v.texcoord, _ReflectionMask);
                o.uv3 = TRANSFORM_TEX (v.texcoord, _DissolveTexture);
                
                // Reflection Calculation
                o.simpleWorldRefl = -reflect( normalize(WorldSpaceViewDir(v.vertex)), normalize(mul((float3x3)_Object2World, SCALED_NORMAL))); 
                                 
                return o;
            }
 
            float4 frag(v2f i) : COLOR
            {
            	// Diffuse * Diffuse Colour
                float4 c = tex2D (_MainTex, i.uv);
                c *= _DiffuseColor;
                
                // Reflection * Reflection Colour
                fixed4 r = texCUBE(_Reflective, float4( i.simpleWorldRefl.x, i.simpleWorldRefl.y,i.simpleWorldRefl.z,1.0 ));
                r *= _ReflectionColor;
                r *= _ReflectionStrength;
                
                
                // lerp diffuse and reflection
                float4 Tex2D1=tex2D(_ReflectionMask,i.uv2);
                c.rgb =lerp(c,r,Tex2D1);
                                
                // calculate dissolve effect
                float4 Tex2D2=tex2D(_DissolveTexture,i.uv3);
                float4 Invert0= float4(1.0, 1.0, 1.0, 1.0) - Tex2D2;
                float4 Add1=_DissolveWidth + _AlphaCutOut;
                float4 Add0=Invert0 + Add1;
                float4 Floor0=floor(Add0);           
                
                // lerp the dissolve onto the color
                c=lerp(c,_DissolveColor,Floor0);
                
				//alpha is dissolve
                c.a = Tex2D2;
                
                
                return c;
 
            }
 
            ENDCG
        }
    }
    FallBack "Diffuse"
}
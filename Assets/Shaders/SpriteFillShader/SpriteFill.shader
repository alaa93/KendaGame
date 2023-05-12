Shader "Custom/FillBottomToTop" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _FillAmount ("Fill Amount", Range(0, 1)) = 1
    }
 
    SubShader {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        Pass {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
 
            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
 
            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
 
            sampler2D _MainTex;
            float _FillAmount;
 
            v2f vert (appdata_t v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
 
            fixed4 frag (v2f i) : SV_Target {
                // Sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
 
                // Calculate the alpha value based on the fill amount
                float alpha = step(i.uv.y, _FillAmount);
 
                // Return the color with the new alpha value
                return fixed4(col.rgb, col.a * alpha);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
/*
The Below version add a white background to alpha and remove it while filling sprite up
Shader "Custom/FillBottomToTop" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _FillAmount ("Fill Amount", Range(0, 1)) = 1
    }
 
    SubShader {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        Pass {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
 
            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
 
            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
 
            sampler2D _MainTex;
            float _FillAmount;
 
            v2f vert (appdata_t v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
 
            fixed4 frag (v2f i) : SV_Target {
                // Sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
 
                // Calculate the alpha value based on the fill amount
                float alpha = lerp(1, col.a, step(i.uv.y, _FillAmount));
 
                // Return the color with the new alpha value
                return fixed4(col.r, col.g, col.b, alpha);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}*/
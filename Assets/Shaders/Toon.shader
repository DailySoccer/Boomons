// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.27 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.27;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:0,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:True,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:3138,x:33068,y:32675,varname:node_3138,prsc:2|emission-4010-OUT,custl-8144-OUT,olwid-510-OUT,olcol-9400-OUT;n:type:ShaderForge.SFN_NormalVector,id:4520,x:30904,y:32880,prsc:2,pt:True;n:type:ShaderForge.SFN_LightVector,id:6391,x:30904,y:33053,varname:node_6391,prsc:2;n:type:ShaderForge.SFN_Dot,id:719,x:31132,y:32978,varname:node_719,prsc:2,dt:0|A-4520-OUT,B-6391-OUT;n:type:ShaderForge.SFN_Append,id:1499,x:31351,y:32978,varname:node_1499,prsc:2|A-719-OUT,B-719-OUT;n:type:ShaderForge.SFN_Multiply,id:7697,x:32382,y:32804,varname:node_7697,prsc:2|A-3337-RGB,B-8195-RGB;n:type:ShaderForge.SFN_Tex2d,id:8195,x:32028,y:33032,ptovrint:False,ptlb:Diffuse,ptin:_Diffuse,varname:_Diffuse_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:9400,x:32447,y:33096,varname:node_9400,prsc:2|A-8195-RGB,B-4506-OUT;n:type:ShaderForge.SFN_UVTile,id:2838,x:31583,y:33026,varname:node_2838,prsc:2|UVIN-1499-OUT,WDT-9262-OUT,HGT-534-OUT,TILE-5548-OUT;n:type:ShaderForge.SFN_Vector1,id:9262,x:31186,y:33257,varname:node_9262,prsc:2,v1:-2.2;n:type:ShaderForge.SFN_Vector1,id:534,x:31186,y:33331,varname:node_534,prsc:2,v1:2;n:type:ShaderForge.SFN_Vector1,id:5548,x:31186,y:33408,varname:node_5548,prsc:2,v1:1;n:type:ShaderForge.SFN_Tex2d,id:3337,x:31839,y:32955,ptovrint:False,ptlb:Ramp,ptin:_Ramp,varname:node_1399,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:f88030fec8aa67a4c9caea8331d899b0,ntxv:0,isnm:False|UVIN-2838-UVOUT;n:type:ShaderForge.SFN_ValueProperty,id:510,x:32395,y:32980,ptovrint:False,ptlb:Outline Width,ptin:_OutlineWidth,varname:node_5136,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:5135,x:32028,y:33316,ptovrint:False,ptlb:Outline Darkness,ptin:_OutlineDarkness,varname:node_5135,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.5;n:type:ShaderForge.SFN_Multiply,id:4506,x:32283,y:33172,varname:node_4506,prsc:2|A-8195-RGB,B-5135-OUT;n:type:ShaderForge.SFN_Power,id:895,x:31779,y:32619,varname:node_895,prsc:2|VAL-4847-OUT,EXP-2083-OUT;n:type:ShaderForge.SFN_ValueProperty,id:1523,x:31283,y:32505,ptovrint:False,ptlb:Fresnel,ptin:_Fresnel,varname:node_1523,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.3;n:type:ShaderForge.SFN_Dot,id:8184,x:31118,y:32705,varname:node_8184,prsc:2,dt:0|A-4112-OUT,B-4520-OUT;n:type:ShaderForge.SFN_ViewVector,id:4112,x:30904,y:32677,varname:node_4112,prsc:2;n:type:ShaderForge.SFN_Multiply,id:8396,x:32121,y:32642,varname:node_8396,prsc:2|A-7409-RGB,B-895-OUT;n:type:ShaderForge.SFN_Color,id:7409,x:31779,y:32399,ptovrint:False,ptlb:Fresnel Color,ptin:_FresnelColor,varname:node_7409,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_OneMinus,id:4847,x:31304,y:32632,varname:node_4847,prsc:2|IN-8184-OUT;n:type:ShaderForge.SFN_Multiply,id:8144,x:32730,y:32577,varname:node_8144,prsc:2|A-6505-RGB,B-7697-OUT;n:type:ShaderForge.SFN_Color,id:6505,x:32510,y:32417,ptovrint:False,ptlb:Tint Color,ptin:_TintColor,varname:node_6505,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Divide,id:2083,x:31540,y:32453,varname:node_2083,prsc:2|A-6965-OUT,B-1523-OUT;n:type:ShaderForge.SFN_Vector1,id:6965,x:31283,y:32393,varname:node_6965,prsc:2,v1:1;n:type:ShaderForge.SFN_ToggleProperty,id:9451,x:32137,y:32500,ptovrint:False,ptlb:Fresnel ON,ptin:_FresnelON,varname:node_9451,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False;n:type:ShaderForge.SFN_Multiply,id:4010,x:32360,y:32552,varname:node_4010,prsc:2|A-9451-OUT,B-8396-OUT;proporder:510-5135-6505-8195-3337-9451-1523-7409;pass:END;sub:END;*/

Shader "Shader Forge/Toon" {
    Properties {
        _OutlineWidth ("Outline Width", Float ) = 0
        _OutlineDarkness ("Outline Darkness", Float ) = 0.5
        _TintColor ("Tint Color", Color) = (1,1,1,1)
        _Diffuse ("Diffuse", 2D) = "white" {}
        _Ramp ("Ramp", 2D) = "white" {}
        [MaterialToggle] _FresnelON ("Fresnel ON", Float ) = 0
        _Fresnel ("Fresnel", Float ) = 0.3
        _FresnelColor ("Fresnel Color", Color) = (1,1,1,1)
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "Outline"
            Tags {
            }
            Cull Front
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform float _OutlineWidth;
            uniform float _OutlineDarkness;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = mul(UNITY_MATRIX_MVP, float4(v.vertex.xyz + v.normal*_OutlineWidth,1) );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                return fixed4((_Diffuse_var.rgb*(_Diffuse_var.rgb*_OutlineDarkness)),0);
            }
            ENDCG
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform sampler2D _Ramp; uniform float4 _Ramp_ST;
            uniform float _Fresnel;
            uniform float4 _FresnelColor;
            uniform float4 _TintColor;
            uniform fixed _FresnelON;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
////// Lighting:
////// Emissive:
                float3 emissive = (_FresnelON*(_FresnelColor.rgb*pow((1.0 - dot(viewDirection,normalDirection)),(1.0/_Fresnel))));
                float node_9262 = (-2.2);
                float node_5548 = 1.0;
                float2 node_2838_tc_rcp = float2(1.0,1.0)/float2( node_9262, 2.0 );
                float node_2838_ty = floor(node_5548 * node_2838_tc_rcp.x);
                float node_2838_tx = node_5548 - node_9262 * node_2838_ty;
                float node_719 = dot(normalDirection,lightDirection);
                float2 node_2838 = (float2(node_719,node_719) + float2(node_2838_tx, node_2838_ty)) * node_2838_tc_rcp;
                float4 _Ramp_var = tex2D(_Ramp,TRANSFORM_TEX(node_2838, _Ramp));
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                float3 finalColor = emissive + (_TintColor.rgb*(_Ramp_var.rgb*_Diffuse_var.rgb));
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}

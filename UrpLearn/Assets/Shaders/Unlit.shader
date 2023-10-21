Shader "CustomRP/Unlit"
{
    Properties
    {
        _BaseColor("Color", Color) = (1.0, 1.0, 1.0,1.0)   
        _BaseMap("BaseMap", 2D) = "white" {}
        [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend("SrcBlend", Float) = 1
        [Enum(UnityEngine.Rendering.BlendMode)] _DstBlend("DstBlend", Float) = 0
        [Enum(Off, 0, 0n, 1)] _ZWrite("ZWrite", Float) = 1
    }
    SubShader
    {
        Pass
        {
            //定义混合模式
            Blend [_SrcBlend] [_DstBlend]
            //是否写入深度
            ZWrite [_ZWrite]
            HLSLPROGRAM
            #pragma multi_compile_instancing
            #pragma vertex UnlitPassVertex
            #pragma fragment UnlitPassFragment

            #include "./UnlitPass.hlsl"
            // #include ""

            ENDHLSL
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering;

public partial class CameraRender
{
    partial void DrawUnsupportedShaders();
    
    #if UNITY_EDITOR
        static ShaderTagId[] legacyShaderTagIds =
        {
            new ShaderTagId("Always"),
            new ShaderTagId("ForwardBase"),
            new ShaderTagId("PrepassBase"),
            new ShaderTagId("Vertex"),
            new ShaderTagId("VertexLMRGBM"),
            new ShaderTagId("VertexLM"),
        };

        private static Material errorMaterial;
        /// <summary>
        /// 绘制SRP不支持的内置Shader类型
        /// </summary>
        partial void DrawUnsupportedShaders()
        {
         if (errorMaterial == null)
         {
             errorMaterial = new Material(Shader.Find("Hidden/InternalErrorShader"));
         }
         //数组第一个元素用来构造DrawingSettings对象的时候设置
         var drawingSettings = new DrawingSettings(legacyShaderTagIds[0], new SortingSettings(camera))
         {
             overrideMaterial = errorMaterial
         };
         for (int i = 1; i < legacyShaderTagIds.Length; i++)
         {
             //遍历数组，逐个设置着色器的Passname, 从i = 1开始，因为i = 0的时候已经设置过了
             drawingSettings.SetShaderPassName(i, legacyShaderTagIds[i]);
         }
         //使用默认设置即刻,反正画出来的都是不支持的
         var filteringSettings = FilteringSettings.defaultValue;
         //绘制不支持的ShaderTag类型的物体
         context.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);
        }

        
        
        

    #endif
    
    partial void DrawGizmos();
    #if UNITY_EDITOR
    partial void DrawGizmos()
    {
        if (Handles.ShouldRenderGizmos())
        {
            context.DrawGizmos(camera, GizmoSubset.PreImageEffects);
            context.DrawGizmos(camera, GizmoSubset.PostImageEffects);
        }
    }
    #endif

    partial void PrepareForSceneWindow();
    #if UNITY_EDITOR
    /// <summary>
    /// 在GameScene视图绘制的几何体也绘制到Scene视图中
    /// </summary>
    partial void PrepareForSceneWindow()
    {
        if (camera.cameraType == CameraType.SceneView)
        {
            //如果切换到了Scene视图，调用此方法完成绘制
            ScriptableRenderContext.EmitWorldGeometryForSceneView(camera);
        }
    }
    #endif

    partial void PrepareBuffer();
#if UNITY_EDITOR
    private string SampleName { get; set; }
    partial void PrepareBuffer()
    {
        //设置一下只有在编辑器模式下才分配内存
        Profiler.BeginSample("Editor Only");
        buffer.name = SampleName = camera.name;
        Profiler.EndSample();
    }
#else
    const string SampleName = bufferName;
#endif
    
    
    // #if UNITY_EDITOR
    //
    // partial void PrepareBuffer()
    // #endif
}

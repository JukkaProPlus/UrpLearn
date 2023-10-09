using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraRender
{
    private static Material errorMaterial;
    private static ShaderTagId[] legacyShaderTagIds =
    {
        new ShaderTagId("Always"),
        new ShaderTagId("ForwardBase"),
        new ShaderTagId("PrepassBase"),
        new ShaderTagId("Vertex"),
        new ShaderTagId("VertexLMRGBM"),
        new ShaderTagId("VertexLM"),
    };
    static ShaderTagId unlitShaderTagId = new ShaderTagId("SRPDefaultUnlit");
    private const string bufferName = "Render Camera";

    private CommandBuffer buffer = new CommandBuffer()
    {
        name = bufferName
    };
    private ScriptableRenderContext context;
    private Camera camera;
    private CullingResults cullingResults;

    public void Render(ScriptableRenderContext context, Camera camera)
    {
        this.camera = camera;
        this.context = context;
        if (!Cull())
        {
            return;
        }
        Setup();
        
        //绘制可见物体
        DrawVisibleGeometry();
        
        //绘制SRP不支持的着色器类型
        DrawUnsupportedShaders();
        
        Submit();
    }
    //errorMaterial
    /// <summary>
    /// 绘制SRP不支持的内置Shader类型
    /// </summary>
    void DrawUnsupportedShaders()
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

    /// <summary>
    /// 绘制SRP不支持的着色器类型
    /// </summary>
    void DrawUnsupportedShaders1()
    {
        
        //数组第一个元素用来构造DrawingSettings对象的时候设置
        var drawingSettings = new DrawingSettings(legacyShaderTagIds[0], new SortingSettings(camera));
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

    /// <summary>
    /// 设置相机的属性和矩阵
    /// </summary>
    void Setup()
    {
        context.SetupCameraProperties(camera);
        buffer.ClearRenderTarget(true, true, Color.clear);
        buffer.BeginSample(bufferName);
        // buffer.BeginSample(bufferName + "1");
        // buffer.BeginSample(bufferName + "2");
        // buffer.BeginSample(bufferName + "3");
        // buffer.BeginSample(bufferName + "4");
        ExecuteBuffer();
    }

    void ExecuteBuffer()
    {
        context.ExecuteCommandBuffer(buffer);
        buffer.Clear();
    }
    /// <summary>
    /// 绘制可见物
    /// </summary>
    void DrawVisibleGeometry()
    {
        //设置绘制顺序和指定渲染相机
        var sortingSettings = new SortingSettings(camera)
        {
            criteria = SortingCriteria.CommonOpaque
        };
        //设置渲染的Shader Pass 和渲染排序
        var drawingSettings = new DrawingSettings(unlitShaderTagId, sortingSettings);
        
        //只绘制RenderQueue为Opaque不透明的物体
        var filteringSettings = new FilteringSettings(RenderQueueRange.opaque);
        //1.绘制不透明物体
        context.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);
        //2.绘制天空盒
        context.DrawSkybox(camera);

        sortingSettings.criteria = SortingCriteria.CommonTransparent;
        drawingSettings.sortingSettings = sortingSettings;
        //只绘制RenderQueue为transparent透明的物体
        filteringSettings.renderQueueRange = RenderQueueRange.transparent;
        
        //3.绘制透明物体
        context.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);
    }

    /// <summary>
    /// 提交缓冲区渲染命令
    /// </summary>
    void Submit()
    {
        // buffer.EndSample(bufferName + "4");
        // buffer.EndSample(bufferName + "3");
        // buffer.EndSample(bufferName + "2");
        // buffer.EndSample(bufferName + "1");
        buffer.EndSample(bufferName);
        ExecuteBuffer();
        context.Submit();
    }

    bool Cull()
    {
        ScriptableCullingParameters p;
        if(camera.TryGetCullingParameters(out p))
        {
            cullingResults = context.Cull(ref p);
            return true;
        }

        return false;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

//该标签会让你在Project下右键->Create菜单中添加一个新的子菜单-》》 Create/Rendering/Custom Render Pipeline
[CreateAssetMenu(menuName = "Rendering/Custom Render Pipeline")]
public class CustomRenderPineAsset : RenderPipelineAsset
{
    //重写抽象方法， 需要返回一个RenderPipeline实例对象
    protected override RenderPipeline CreatePipeline()
    {
        return new CustomRenderPepeline();
    }
}
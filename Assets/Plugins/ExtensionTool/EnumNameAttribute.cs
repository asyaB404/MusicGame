// ******************************************************************
//       /\ /|       @file       EnumNameAttribute.cs
//       \ V/        @brief      Inspector枚举命名
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-03-31 08:59:35
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class EnumNameAttribute : PropertyAttribute
{
    /// <summary> 枚举名称 </summary>
    public readonly string name;

    public EnumNameAttribute(string name)
    {
        this.name = name;
    }
}
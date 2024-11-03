using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public static class UnityExtensions
{
    /// <summary>
    /// 修改原先颜色的透明度为目标值
    /// </summary>
    /// <param name="color"></param>
    /// <param name="targetAlpha"></param>
    /// <returns></returns>
    public static Color SetAlpha(this Color color, float targetAlpha)
    {
        return new Color(color.r, color.g, color.b, targetAlpha);
    }

    /// <summary>
    /// 设置透明度
    /// </summary>
    /// <param name="sr"></param>
    /// <param name="targetAlpha">目标透明度</param>
    public static void SetAlpha(this SpriteRenderer sr, float targetAlpha)
    {
        sr.color = sr.color.SetAlpha(targetAlpha);
    }

    /// <summary>
    /// 设置透明度
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="graphic"></param>
    /// <param name="targetAlpha">目标透明度</param>
    /// <returns></returns>
    public static T SetAlpha<T>(this T graphic, float targetAlpha) where T : Graphic
    {
        graphic.color = graphic.color.SetAlpha(targetAlpha);
        return graphic;
    }

    /// <summary>
    /// 从范围里随机获取一个数字
    /// </summary>
    /// <param name="p"></param>
    /// <param name="total">分母</param>
    /// <returns></returns>
    public static float GetRandom(this float end,float start=0)
    {
        return UnityEngine.Random.Range(Mathf.Min(start,end),Mathf.Max(start,end));
    }

    /// <summary>
    /// 从范围里随机获取一个数字
    /// </summary>
    /// <param name="p"></param>
    /// <param name="total">分母</param>
    /// <returns></returns>
    public static int GetRandom(this int end, int start = 0)
    {
        return UnityEngine.Random.Range(Mathf.Min(start, end), Mathf.Max(start, end));
    }

    /// <summary>
    /// 以当前值作为事件发生的概率，返回一个随机的结果，参数为分母
    /// </summary>
    /// <param name="p"></param>
    /// <param name="total">分母</param>
    /// <returns></returns>
    public static bool Random(this float p, float total = 1)
    {
        return UnityEngine.Random.Range(0, total) < p;
    }

    /// <summary>
    /// 以当前值作为事件发生的概率，返回一个随机的结果，参数为分母
    /// </summary>
    /// <param name="p"></param>
    /// <param name="total">分母</param>
    /// <returns></returns>
    public static bool Random(this double p, float total = 1)
    {
        return UnityEngine.Random.Range(0, total) < p;
    }

    /// <summary>
    /// 水平翻转物体
    /// </summary>
    /// <param name="trans"></param>
    /// <param name="scaleMode">翻转scale还是翻转eulerAngles</param>
    public static T FlipHorizontal<T>(this T component, bool scaleMode = false) where T : Component
    {
        if (scaleMode)
        {
            Vector3 scale = component.transform.localScale;
            component.transform.SetScale(-scale.x);
        }
        else
        {
            Vector3 eulerAngles = component.transform.eulerAngles;
            component.transform.SetEulerAnglesY(eulerAngles.y == 180 ? 0 : 180);
        }
        return component;
    }

    /// <summary>
    /// 更加安全的销毁方式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="mono"></param>
    /// <param name="t"></param>
    /// <param name="delayTime">销毁延时</param>
    public static void DestroySafe<T>(this MonoBehaviour mono, T t, float delayTime) where T : Component
    {
        if (t != null)
        {
            if (delayTime != 0)
            {
                UnityEngine.Object.Destroy(t, delayTime);
            }
            else
            {
                UnityEngine.Object.Destroy(t);
            }
        }
    }

    /// <summary>
    /// 更加安全的销毁方式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="mono"></param>
    /// <param name="t"></param>
    /// <param name="delayTime">销毁延时</param>
    public static void DestroySafe(this MonoBehaviour mono, GameObject go, float delayTime)
    {
        if (go != null)
        {
            if (delayTime != 0)
            {
                UnityEngine.Object.Destroy(go, delayTime);
            }
            else
            {
                UnityEngine.Object.Destroy(go);
            }
        }
    }

    /// <summary>
    /// 更加安全地关闭某个协程函数
    /// </summary>
    /// <param name="mono"></param>
    /// <param name="coroutine"></param>
    public static void StopCoroutineSafe(this MonoBehaviour mono, Coroutine coroutine)
    {
        if (coroutine != null)
        {
            mono.StopCoroutine(coroutine);
        }
    }

    /// <summary>
    /// 显示某个名字的子物体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="component"></param>
    /// <param name="name">子物体名字</param>
    /// <returns></returns>
    public static T ShowChildByName<T>(this T component, string name) where T : Component
    {
        Transform child = component.transform.Find(name);
        if (child != null)
        {
            child.gameObject.Show();
            return component;
        }
        return null;
    }

    /// <summary>
    /// 隐藏某个名字的子物体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="component"></param>
    /// <param name="name">子物体名字</param>
    /// <returns></returns>
    public static T HideChildByName<T>(this T component, string name) where T : Component
    {
        Transform child = component.transform.Find(name);
        if (child != null)
        {
            child.gameObject.Hide();
            return component;
        }
        return null;
    }

}

public static class UIComponentExtension
{
    /// <summary>
    /// 便捷设置图片填充值
    /// </summary>
    /// <param name="image"></param>
    /// <param name="fillAmount"></param>
    /// <returns></returns>
    public static Image FillAmount(this Image image, float fillAmount)
    {
        image.fillAmount = fillAmount;
        return image;
    }

}

public static class RectTransExtension
{
    /// <summary>
    /// 设置锚点坐标X
    /// </summary>
    /// <param name="rectTrans"></param>
    /// <param name="anchorPosX"></param>
    /// <returns></returns>
    public static RectTransform AnchorPosX(this RectTransform rectTrans, float anchorPosX)
    {
        Vector2 pos = rectTrans.anchoredPosition;
        pos.x = anchorPosX;
        rectTrans.anchoredPosition = pos;
        return rectTrans;
    }
    /// <summary>
    /// 设置锚点坐标Y
    /// </summary>
    /// <param name="rectTrans"></param>
    /// <param name="anchorPosY"></param>
    /// <returns></returns>
    public static RectTransform AnchorPosY(this RectTransform rectTrans, float anchorPosY)
    {
        Vector2 pos = rectTrans.anchoredPosition;
        pos.y = anchorPosY;
        rectTrans.anchoredPosition = pos;
        return rectTrans;
    }
    /// <summary>
    /// 设置尺寸宽度
    /// </summary>
    /// <param name="rectTrans"></param>
    /// <param name="width"></param>
    /// <returns></returns>
    public static RectTransform SizeWidth(this RectTransform rectTrans, float width)
    {
        Vector2 size = rectTrans.sizeDelta;
        size.x = width;
        rectTrans.sizeDelta = size;
        return rectTrans;
    }
    /// <summary>
    /// 设置尺寸高度
    /// </summary>
    /// <param name="rectTrans"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public static RectTransform SizeHeight(this RectTransform rectTrans, float height)
    {
        Vector2 size = rectTrans.sizeDelta;
        size.y = height;
        rectTrans.sizeDelta = size;
        return rectTrans;
    }
}

public static class TransformExtension
{
    #region 属性归一化
    /// <summary>
    /// 属性归一化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="component"></param>
    /// <returns></returns>
    public static T Identity<T>(this T component) where T : Component
    {
        component.transform.position = Vector3.zero;
        component.transform.eulerAngles = Vector3.zero;
        component.transform.localScale = Vector3.one;
        return component;
    }

    /// <summary>
    /// 局部属性归一化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="component"></param>
    /// <returns></returns>
    public static T LocalIdentity<T>(this T component) where T : Component
    {
        component.transform.localPosition = Vector3.zero;
        component.transform.localEulerAngles = Vector3.zero;
        component.transform.localScale = Vector3.one;
        return component;
    }
    #endregion

    #region 坐标
    /// <summary>
    /// 获取组件坐标
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="component"></param>
    /// <returns></returns>
    public static Vector3 GetPostion<T>(this T component) where T : Component
    {
        return component.transform.position;
    }

    /// <summary>
    /// 获取组件局部坐标
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="component"></param>
    /// <returns></returns>
    public static Vector3 GetLocalPostion<T>(this T component) where T : Component
    {
        return component.transform.localPosition;
    }

    /// <summary>
    /// 归一化坐标
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="component"></param>
    /// <returns></returns>
    public static T PosIdentity<T>(this T component) where T : Component
    {
        component.transform.position = Vector3.zero;
        return component;
    }

    /// <summary>
    /// 归一化局部坐标
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="component"></param>
    /// <returns></returns>
    public static T LocalPosIdentity<T>(this T component) where T : Component
    {
        component.transform.localPosition = Vector3.zero;
        return component;
    }

    /// <summary>
    /// 设置坐标某些轴向的值(此方法不支持将XYZ的值设置成PI，填PI会被视作不修改此轴向的值)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="component"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public static T SetPosition<T>(this T component, float x = MathF.PI, float y = MathF.PI, float z = MathF.PI) where T : Component
    {
        component.transform.position = new Vector3
        (
           x == MathF.PI ? component.transform.position.x : x,
           y == Mathf.PI ? component.transform.position.y : y,
           z == MathF.PI ? component.transform.position.z : z
        );
        return component;
    }

    /// <summary>
    /// 设置局部坐标某些轴向的值(此方法不支持将XYZ的值设置成PI，填PI会被视作不修改此轴向的值)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="component"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public static T SetLocalPosition<T>(this T component, float x = MathF.PI, float y = MathF.PI, float z = MathF.PI) where T : Component
    {
        component.transform.localPosition = new Vector3
        (
           x == MathF.PI ? component.transform.localPosition.x : x,
           y == Mathf.PI ? component.transform.localPosition.y : y,
           z == MathF.PI ? component.transform.localPosition.z : z
        );
        return component;
    }
    #endregion

    #region 旋转
    /// <summary>
    /// 获取组件旋转
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="component"></param>
    /// <returns></returns>
    public static Quaternion GetRotation<T>(this T component) where T : Component
    {
        return component.transform.rotation;
    }

    /// <summary>
    /// 获取组件局部旋转
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="component"></param>
    /// <returns></returns>
    public static Quaternion GetLocalRotation<T>(this T component) where T : Component
    {
        return component.transform.localRotation;
    }

    /// <summary>
    /// 获取组件旋转
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="component"></param>
    /// <returns></returns>
    public static Vector3 GetEulerAngles<T>(this T component) where T : Component
    {
        return component.transform.eulerAngles;
    }

    /// <summary>
    /// 获取组件局部旋转
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="component"></param>
    /// <returns></returns>
    public static Vector3 GetLocalEulerAngles<T>(this T component) where T : Component
    {
        return component.transform.localEulerAngles;
    }

    /// <summary>
    /// 归一化旋转
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="component"></param>
    /// <returns></returns>
    public static T RotationIdentity<T>(this T component) where T : Component
    {
        component.transform.rotation = Quaternion.identity;
        return component;
    }

    /// <summary>
    /// 归一化局部旋转
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="component"></param>
    /// <returns></returns>
    public static T LocalRotationIdentity<T>(this T component) where T : Component
    {
        component.transform.localRotation = Quaternion.identity;
        return component;
    }

    /// <summary>
    /// 归一化旋转
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="component"></param>
    /// <returns></returns>
    public static T EulerAnglesIdentity<T>(this T component) where T : Component
    {
        component.transform.eulerAngles = Vector3.zero;
        return component;
    }

    /// <summary>
    /// 归一化局部旋转
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="component"></param>
    /// <returns></returns>
    public static T LocalEulerAnglesIdentity<T>(this T component) where T : Component
    {
        component.transform.localEulerAngles = Vector3.zero;
        return component;
    }

    /// <summary>
    /// 设置旋转(此方法不支持将XYZ的值设置成PI，填PI会被视作不修改此轴向的值)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="component"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public static T SetRotate<T>(this T component, float x = MathF.PI, float y = MathF.PI, float z = MathF.PI) where T : Component
    {
        component.transform.eulerAngles = new Vector3
        (
           x == MathF.PI ? component.transform.eulerAngles.x : x,
           y == Mathf.PI ? component.transform.eulerAngles.y : y,
           z == MathF.PI ? component.transform.eulerAngles.z : z
        );
        return component;
    }

    /// <summary>
    /// 设置局部旋转(此方法不支持将XYZ的值设置成PI，填PI会被视作不修改此轴向的值)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="component"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public static T SetLocalRotate<T>(this T component, float x = MathF.PI, float y = MathF.PI, float z = MathF.PI) where T : Component
    {
        component.transform.localEulerAngles = new Vector3
        (
           x == MathF.PI ? component.transform.localEulerAngles.x : x,
           y == Mathf.PI ? component.transform.localEulerAngles.y : y,
           z == MathF.PI ? component.transform.localEulerAngles.z : z
        );
        return component;
    }
    #endregion

    #region 缩放
    /// <summary>
    /// 获取缩放
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="component"></param>
    /// <returns></returns>
    public static Vector3 GetScale<T>(this T component) where T : Component
    {
        return component.transform.localScale;
    }

    /// <summary>
    /// 归一化缩放
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="component"></param>
    /// <returns></returns>
    public static T ScaleIdentity<T>(this T component) where T : Component
    {
        component.transform.localScale = Vector3.one;
        return component;
    }

    /// <summary>
    /// 设置缩放(此方法不支持将XYZ的值设置成PI，填PI会被视作不修改此轴向的值)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="component"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public static T SetScale<T>(this T component, float x = MathF.PI, float y = MathF.PI, float z = MathF.PI) where T : Component
    {
        component.transform.localScale = new Vector3
        (
           x == MathF.PI ? component.transform.localScale.x : x,
           y == Mathf.PI ? component.transform.localScale.y : y,
           z == MathF.PI ? component.transform.localScale.z : z
        );
        return component;
    }
    #endregion

    #region 销毁子物体
    /// <summary>
    /// 销毁全部子物体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="component"></param>
    /// <returns></returns>
    public static T DestroyAllChildren<T>(this T component) where T : Component
    {
        foreach (Transform childTrans in component.transform)
        {
            UnityEngine.Object.Destroy(childTrans.gameObject);
        }
        return component;
    }

    /// <summary>
    /// 销毁全部子物体
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>
    public static GameObject DestroyAllChildren(this GameObject go)
    {
        foreach (Transform childTrans in go.transform)
        {
            UnityEngine.Object.Destroy(childTrans.gameObject);
        }
        return go;
    }
    #endregion

    #region 层级设置
    /// <summary>
    /// 设置到最上层
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="component"></param>
    /// <returns></returns>
    public static T AsFirstSibling<T>(this T component) where T : Component
    {
        component.transform.SetAsFirstSibling();
        return component;
    }

    /// <summary>
    /// 设置到最下层
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="component"></param>
    /// <returns></returns>
    public static T AsLastSibling<T>(this T component) where T : Component
    {
        component.transform.SetAsLastSibling();
        return component;
    }

    /// <summary>
    /// 设置到目标层
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="component"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public static T SibilingIndex<T>(this T component, int index) where T : Component
    {
        component.transform.SetSiblingIndex(index);
        return component;
    }
    #endregion

    #region 仅修改或者增加   坐标、旋转、缩放的   某一个轴向的值
    /// <summary>
    /// 修改坐标X
    /// </summary>
    /// <param name="transform"><see cref="Transform" /> 对象。</param>
    /// <param name="newValue">x 坐标值。</param>
    public static void SetPositionX(this Transform transform, float newValue)
    {
        Vector3 v = transform.position;
        v.x = newValue;
        transform.position = v;
    }

    /// <summary>
    /// 修改坐标Y
    /// </summary>
    /// <param name="transform"><see cref="Transform" /> 对象。</param>
    /// <param name="newValue">y 坐标值。</param>
    public static void SetPositionY(this Transform transform, float newValue)
    {
        Vector3 v = transform.position;
        v.y = newValue;
        transform.position = v;
    }

    /// <summary>
    /// 修改坐标Z
    /// </summary>
    /// <param name="transform"><see cref="Transform" /> 对象。</param>
    /// <param name="newValue">z 坐标值。</param>
    public static void SetPositionZ(this Transform transform, float newValue)
    {
        Vector3 v = transform.position;
        v.z = newValue;
        transform.position = v;
    }

    /// <summary>
    /// 增加X坐标的值
    /// </summary>
    /// <param name="transform"><see cref="Transform" /> 对象。</param>
    /// <param name="deltaValue">x 坐标值增量。</param>
    public static void AddPositionX(this Transform transform, float deltaValue)
    {
        Vector3 v = transform.position;
        v.x += deltaValue;
        transform.position = v;
    }

    /// <summary>
    /// 增加Y坐标的值
    /// </summary>
    /// <param name="transform"><see cref="Transform" /> 对象。</param>
    /// <param name="deltaValue">y 坐标值增量。</param>
    public static void AddPositionY(this Transform transform, float deltaValue)
    {
        Vector3 v = transform.position;
        v.y += deltaValue;
        transform.position = v;
    }

    /// <summary>
    /// 增加Z坐标的值
    /// </summary>
    /// <param name="transform"><see cref="Transform" /> 对象。</param>
    /// <param name="deltaValue">z 坐标值增量。</param>
    public static void AddPositionZ(this Transform transform, float deltaValue)
    {
        Vector3 v = transform.position;
        v.z += deltaValue;
        transform.position = v;
    }

    /// <summary>
    /// 修改局部坐标X
    /// </summary>
    /// <param name="transform"><see cref="Transform" /> 对象。</param>
    /// <param name="newValue">x 局部坐标值</param>
    public static void SetLocalPositionX(this Transform transform, float newValue)
    {
        Vector3 v = transform.localPosition;
        v.x = newValue;
        transform.localPosition = v;
    }

    /// <summary>
    /// 修改局部坐标Y
    /// </summary>
    /// <param name="transform"><see cref="Transform" /> 对象。</param>
    /// <param name="newValue">y 局部坐标值</param>
    public static void SetLocalPositionY(this Transform transform, float newValue)
    {
        Vector3 v = transform.localPosition;
        v.y = newValue;
        transform.localPosition = v;
    }

    /// <summary>
    /// 修改局部坐标Z
    /// </summary>
    /// <param name="transform"><see cref="Transform" /> 对象。</param>
    /// <param name="newValue">z 局部坐标值</param>
    public static void SetLocalPositionZ(this Transform transform, float newValue)
    {
        Vector3 v = transform.localPosition;
        v.z = newValue;
        transform.localPosition = v;
    }

    /// <summary>
    /// 局部坐标X增加给定值
    /// </summary>
    /// <param name="transform"><see cref="Transform" /> 对象。</param>
    /// <param name="deltaValue">x 局部坐标值增量</param>
    public static void AddLocalPositionX(this Transform transform, float deltaValue)
    {
        Vector3 v = transform.localPosition;
        v.x += deltaValue;
        transform.localPosition = v;
    }

    /// <summary>
    /// 局部坐标Y增加给定值
    /// </summary>
    /// <param name="transform"><see cref="Transform" /> 对象。</param>
    /// <param name="deltaValue">y 局部坐标值增量</param>
    public static void AddLocalPositionY(this Transform transform, float deltaValue)
    {
        Vector3 v = transform.localPosition;
        v.y += deltaValue;
        transform.localPosition = v;
    }

    /// <summary>
    /// 局部坐标Z增加给定值
    /// </summary>
    /// <param name="transform"><see cref="Transform" /> 对象。</param>
    /// <param name="deltaValue">z 局部坐标值增量</param>
    public static void AddLocalPositionZ(this Transform transform, float deltaValue)
    {
        Vector3 v = transform.localPosition;
        v.z += deltaValue;
        transform.localPosition = v;
    }

    /// <summary>
    /// 修改旋转X
    /// </summary>
    /// <param name="transform"><see cref="Transform" /> 对象。</param>
    /// <param name="newValue">值</param>
    public static void SetEulerAnglesX(this Transform transform, float newValue)
    {
        Vector3 v = transform.eulerAngles;
        v.x = newValue;
        transform.eulerAngles = v;
    }

    /// <summary>
    /// 修改旋转Y
    /// </summary>
    /// <param name="transform"><see cref="Transform" /> 对象。</param>
    /// <param name="newValue">值</param>
    public static void SetEulerAnglesY(this Transform transform, float newValue)
    {
        Vector3 v = transform.eulerAngles;
        v.y = newValue;
        transform.eulerAngles = v;
    }

    /// <summary>
    /// 修改旋转Z
    /// </summary>
    /// <param name="transform"><see cref="Transform" /> 对象。</param>
    /// <param name="newValue">值</param>
    public static void SetEulerAnglesZ(this Transform transform, float newValue)
    {
        Vector3 v = transform.eulerAngles;
        v.z = newValue;
        transform.eulerAngles = v;
    }

    /// <summary>
    /// X轴角度增加给定值
    /// </summary>
    /// <param name="transform"><see cref="Transform" /> 对象。</param>
    /// <param name="deltaValue">增量</param>
    public static void AddEulerAnglesX(this Transform transform, float deltaValue)
    {
        Vector3 v = transform.eulerAngles;
        v.x += deltaValue;
        transform.eulerAngles = v;
    }

    /// <summary>
    /// Y轴角度增加给定值
    /// </summary>
    /// <param name="transform"><see cref="Transform" /> 对象。</param>
    /// <param name="deltaValue">增量</param>
    public static void AddEulerAnglesY(this Transform transform, float deltaValue)
    {
        Vector3 v = transform.eulerAngles;
        v.y += deltaValue;
        transform.eulerAngles = v;
    }

    /// <summary>
    /// Z轴角度增加给定值
    /// </summary>
    /// <param name="transform"><see cref="Transform" /> 对象。</param>
    /// <param name="deltaValue">增量</param>
    public static void AddEulerAnglesZ(this Transform transform, float deltaValue)
    {
        Vector3 v = transform.eulerAngles;
        v.z += deltaValue;
        transform.eulerAngles = v;
    }

    /// <summary>
    /// 修改局部旋转X
    /// </summary>
    /// <param name="transform"><see cref="Transform" /> 对象。</param>
    /// <param name="newValue">新的X轴旋转</param>
    public static void SetLocalEulerAnglesX(this Transform transform, float newValue)
    {
        Vector3 v = transform.localEulerAngles;
        v.x = newValue;
        transform.localEulerAngles = v;
    }

    /// <summary>
    /// 修改局部旋转Y
    /// </summary>
    /// <param name="transform"><see cref="Transform" /> 对象。</param>
    /// <param name="newValue">新的Y轴旋转</param>
    public static void SetLocalEulerAnglesY(this Transform transform, float newValue)
    {
        Vector3 v = transform.localEulerAngles;
        v.y = newValue;
        transform.localEulerAngles = v;
    }

    /// <summary>
    /// 修改局部旋转Z
    /// </summary>
    /// <param name="transform"><see cref="Transform" /> 对象。</param>
    /// <param name="newValue">新的Z轴旋转</param>
    public static void SetLocalEulerAnglesZ(this Transform transform, float newValue)
    {
        Vector3 v = transform.localEulerAngles;
        v.z = newValue;
        transform.localEulerAngles = v;
    }

    /// <summary>
    /// 局部旋转X增加给定值
    /// </summary>
    /// <param name="transform"><see cref="Transform" /> 对象。</param>
    /// <param name="add">增量X</param>
    public static void AddLocalEulerAnglesX(this Transform transform, float add)
    {
        Vector3 v = transform.localEulerAngles;
        v.x += add;
        transform.localEulerAngles = v;
    }

    /// <summary>
    /// 局部旋转Y增加给定值
    /// </summary>
    /// <param name="transform"><see cref="Transform" /> 对象。</param>
    /// <param name="add">增量Y</param>
    public static void AddLocalEulerAnglesY(this Transform transform, float add)
    {
        Vector3 v = transform.localEulerAngles;
        v.y += add;
        transform.localEulerAngles = v;
    }

    /// <summary>
    /// 局部旋转Z增加给定值
    /// </summary>
    /// <param name="transform"><see cref="Transform" /> 对象。</param>
    /// <param name="add">增量Z</param>
    public static void AddLocalEulerAnglesZ(this Transform transform, float add)
    {
        Vector3 v = transform.localEulerAngles;
        v.z += add;
        transform.localEulerAngles = v;
    }
    #endregion

    #region 获取组件 或者指定子物体
    /// <summary>
    /// 从子物体里递归获取对应名字游戏物体的Transform组件
    /// </summary>
    /// <param name="trans"></param>
    /// <param name="name">目标子物体名字</param>
    /// <returns></returns>
    public static Transform FindTransInChildren(this Transform trans, string name)
    {
        if (trans.name == name) return trans;

        //递归获取
        Transform target = null;
        foreach (Transform item in trans)
        {
            target = item.FindTransInChildren(name);
            if (target != null)
            {
                break;
            }
        }
        return target;
    }

    /// <summary>
    /// 从子物体里递归获取对应Tag对象的Transform组件
    /// </summary>
    /// <param name="trans"></param>
    /// <param name="name">目标子物体标签</param>
    /// <returns></returns>
    public static Transform FindTransWithTagInChildren(this Transform trans, string tag)
    {
        if (trans.tag == tag) return trans;

        Transform target = null;
        foreach (Transform item in trans)
        {
            target = item.FindTransWithTagInChildren(tag);
            if (target != null)
            {
                break;
            }
        }
        return target;
    }

    /// <summary>
    /// 递归获取子物体里目标标签的全部子物体Transform
    /// </summary>
    /// <param name="trans"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static List<Transform> FindTransAllWithTagInChildren(this Transform trans, string tag)
    {
        List<Transform> allTrans = new List<Transform>();
        if (trans.tag == tag)
        {
            allTrans.Add(trans);
        }

        foreach (Transform item in trans)
        {
            allTrans.AddRange(item.FindTransAllWithTagInChildren(tag));
        }
        return allTrans;
    }

    /// <summary>
    /// 从子物体里获取某个组件(包括隐藏着的子物体)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="trans"></param>
    /// <returns></returns>
    public static T GetTInChildrenIncludeHide<T>(this Transform trans) where T : UnityEngine.Object
    {
        //先从自身获取
        T t = trans.GetComponent<T>();
        if (t != default)
        {
            return t;
        }

        //递归获取
        foreach (Transform item in trans)
        {
            t = item.GetTInChildrenIncludeHide<T>();
            if (t != default)
            {
                break;
            }
        }
        return t;

    }

    /// <summary>
    /// 从子物体里获取某些组件(包括隐藏着的子物体)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="trans"></param>
    /// <param name="list"></param>
    public static List<T> GetTsInChildrenIncludeHide<T>(this Transform trans)
    {
        List<T> ts = trans.GetComponents<T>().ToList();

        foreach (Transform item in trans)
        {
            ts.AddRange(item.GetTsInChildrenIncludeHide<T>());
        }
        return ts;
    }

    private static readonly List<Transform> m_CachedTransforms = new List<Transform>();
    /// <summary>
    /// 递归设置游戏对象的层次。
    /// </summary>
    /// <param name="gameObject"><see cref="GameObject" /> 对象</param>
    /// <param name="layer">目标层级。</param>
    public static void SetLayerRecursively(this GameObject gameObject, int layer)
    {
        gameObject.GetComponentsInChildren(true, m_CachedTransforms);
        for (int i = 0; i < m_CachedTransforms.Count; i++)
        {
            m_CachedTransforms[i].gameObject.layer = layer;
        }
        m_CachedTransforms.Clear();

    }
    #endregion

    /// <summary>
    /// 复制Transform属性给另一个Transform
    /// </summary>
    /// <param name="self"></param>
    /// <param name="target"></param>
    public static void CopyTransfrom(this Transform self, Transform target)
    {
        self.position = target.position;
        self.eulerAngles = target.eulerAngles;
        self.localScale = target.localScale;
    }

}

public static class BehaviourExtension
{
    /// <summary>
    /// 启用组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="behaviour"></param>
    /// <returns></returns>
    public static T Enable<T>(this T behaviour) where T : Behaviour
    {
        behaviour.enabled = true;
        return behaviour;
    }

    /// <summary>
    /// 禁用组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="behaviour"></param>
    /// <returns></returns>
    public static T Disable<T>(this T behaviour) where T : Behaviour
    {
        behaviour.enabled = false;
        return behaviour;
    }
}

public static class GameObjectExtension
{
    /// <summary>
    /// 游戏对象淡入淡出(PS:淡出结束会隐藏或销毁物体)
    /// </summary>
    /// <param name="go"></param>
    /// <param name="fadeIn">是否是淡入</param>
    /// <param name="time">淡入淡出用时</param>
    /// <param name="autoDes">淡出结束自动销毁</param>
    public static void GameObjectFadeInOrOut(this GameObject go, bool fadeIn, float time, bool autoDes = true)
    {
        if (go == null) return;

        //先获取这个物体上的所有材质球
        List<Material> materials = go.GetMaterialsRecursion();
        if (fadeIn)
        {
            go.Show();
            foreach (var item in materials)
            {
                //记录原先的颜色
                Color oldColor = item.GetColor("_Color");
                //将初始透明度设置为0
                item.SetColor("_Color", oldColor.SetAlpha(0));
                float a = 0;
                //在给定时间内将透明度渐变到原先记录的透明度
                DOTween.To(() => a, b =>
                {
                    a = b;
                    Color color = oldColor.SetAlpha(a);
                    item.SetColor("_Color", color);
                }, oldColor.a, time);
            }
        }
        else
        {
            foreach (var item in materials)
            {
                //记录原先的颜色
                Color oldColor = item.GetColor("_Color");
                //在给定时间内将透明度渐变到0
                float a = oldColor.a;
                DOTween.To(() => a, b =>
                {
                    a = b;
                    Color color = oldColor.SetAlpha(a);
                    item.SetColor("_Color", color);

                }, 0, time).onComplete += () =>
                {
                    //淡入结束后 如果需要销毁对象就销毁 如果不需要销毁对象则隐藏对象以及将颜色还原回去
                    if (go != null && autoDes)
                    {
                        UnityEngine.Object.Destroy(go);
                    }
                    else if (go != null)
                    {
                        go.Hide();
                    }
                    if (item != null)
                    {
                        item.SetColor("_Color", oldColor);
                    }
                };
            }
        }
    }

    /// <summary>
    /// 递归获取这个游戏对象的全部材质球
    /// </summary>
    /// <param name="go"></param>
    /// <param name="materials"></param>
    public static List<Material> GetMaterialsRecursion(this GameObject go)
    {
        List<Material> materials = new List<Material>();
        Renderer renderer = go.GetComponent<Renderer>();
        if (renderer != null)
        {
            materials.AddRange(renderer.materials);
        }
        foreach (Transform trans in go.transform)
        {
            materials.AddRange(GetMaterialsRecursion(trans.gameObject));
        }
        return materials;
    }

    /// <summary>
    /// 递归获取这个游戏对象的全部材质球
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="materials"></param>
    public static List<Material> GetMaterialsRecursion(this Transform transform)
    {
        return transform.gameObject.GetMaterialsRecursion();
    }

    /// <summary>
    /// 修改层级
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="layer">目标层级</param>
    /// <param name="recursion">递归</param>
    public static void SetLayer(this GameObject obj, int layer, bool recursion = true)
    {
        obj.layer = layer;
        if (recursion)
        {
            foreach (Transform item in obj.transform)
            {
                item.gameObject.layer = layer;
                SetLayer(item.gameObject, layer, recursion);
            }
        }

    }

    /// <summary>
    /// 显示物体
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>
    public static GameObject Show(this GameObject go)
    {
        if (!go.activeSelf)
        {
            go.SetActive(true);
        }
        return go;
    }

    /// <summary>
    /// 隐藏物体
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>
    public static GameObject Hide(this GameObject go)
    {
        if (go.activeSelf)
        {
            go.SetActive(false);
        }
        return go;
    }

    /// <summary>
    /// 获取某个组件，如果不存在就添加一个
    /// </summary>
    /// <param name="go"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static T GetOrAddComponent<T>(this GameObject go) where T : Component
    {
        T t = go.GetComponent<T>();
        return t ? t : go.AddComponent<T>();
    }

    /// <summary>
    /// 获取某个组件，如果不存在就添加一个
    /// </summary>
    /// <param name="go"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static T GetOrAddComponent<T>(this Transform trans) where T : Component
    {
        T t = trans.gameObject.GetComponent<T>();
        return t ? t : trans.gameObject.AddComponent<T>();
    }

    /// <summary>
    /// 设置游戏物体层级
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="component"></param>
    /// <param name="layer"></param>
    /// <returns></returns>
    public static T Layer<T>(this T component, int layer) where T : Component
    {
        component.gameObject.layer = layer;
        return component;
    }

    /// <summary>
    /// 设置游戏物体层级
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="component"></param>
    /// <param name="layer"></param>
    /// <returns></returns>
    public static T Layer<T>(this T component, string layer) where T : Component
    {
        component.gameObject.layer = LayerMask.NameToLayer(layer);
        return component;
    }

    /// <summary>
    /// 设置游戏物体层级
    /// </summary>
    /// <param name="go"></param>
    /// <param name="layer"></param>
    /// <returns></returns>
    public static GameObject Layer(this GameObject go, int layer)
    {
        go.layer = layer;
        return go;
    }

    /// <summary>
    /// 设置游戏物体层级
    /// </summary>
    /// <param name="go"></param>
    /// <param name="layer"></param>
    /// <returns></returns>
    public static GameObject Layer(this GameObject go, string layer)
    {
        go.layer = LayerMask.NameToLayer(layer);
        return go;
    }

    /// <summary>
    /// 销毁游戏物体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="component"></param>
    /// <param name="delay">销毁延时</param>
    /// <returns></returns>
    public static T DestroyGameObjectSafe<T>(this T component, float delay = 0) where T : Component
    {
        if (component && component.gameObject)
        {
            UnityEngine.Object.Destroy(component.gameObject, delay);
        }
        return component;
    }

    /// <summary>
    /// 销毁游戏物体
    /// </summary>
    /// <param name="go"></param>
    /// <param name="delay">销毁延时</param>
    public static void DestroyGameObjectSafe(this GameObject go, float delay = 0)
    {
        if (go != null)
        {
            UnityEngine.Object.Destroy(go, delay);
        }
    }

}

public static class CameraExtension
{
    /// <summary>
    /// 截图
    /// </summary>
    /// <param name="camera"></param>
    /// <param name="rect">范围</param>
    /// <returns></returns>
    public static Texture2D ScreenShot(this Camera camera, Rect rect)
    {
        var renderTexture = new RenderTexture(Screen.width, Screen.height, 0);
        camera.targetTexture = renderTexture;
        camera.Render();
        RenderTexture.active = renderTexture;

        var screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
        screenShot.ReadPixels(rect, 0, 0);
        screenShot.Apply();

        camera.targetTexture = null;
        RenderTexture.active = null;
        UnityEngine.Object.Destroy(renderTexture);
        return screenShot;
    }
}

public static class ParticleExtension
{
    /// <summary>
    /// 播放一个游戏物体上的全部特效组件
    /// </summary>
    /// <param name="ps"></param>
    public static void RecursionPlayParticle(this GameObject ps)
    {
        ParticleSystem[] list = ps.GetComponentsInChildren<ParticleSystem>();

        for (int i = 0; i < list.Length; i++)
        {
            list[i].Play();
        }
    }

    /// <summary>
    /// 停止一个游戏物体上的全部特效组件
    /// </summary>
    /// <param name="ps"></param>
    public static void RecursionStopParticle(this GameObject ps)
    {
        ParticleSystem[] list = ps.GetComponentsInChildren<ParticleSystem>();

        for (int i = 0; i < list.Length; i++)
        {
            list[i].Stop();
        }
    }

    /// <summary>
    /// 暂停一个游戏物体上的全部特效组件
    /// </summary>
    /// <param name="ps"></param>
    public static void RecursionPauseParticle(this GameObject ps)
    {
        ParticleSystem[] list = ps.GetComponentsInChildren<ParticleSystem>();

        for (int i = 0; i < list.Length; i++)
        {
            list[i].Pause();
        }
    }
}
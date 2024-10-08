using System;
using UnityEngine;
using UnityEngine.Events;


namespace PixelsHub
{
    [Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    [Serializable]
    public class FloatEvent : UnityEvent<float> { }

    [Serializable]
    public class IntEvent : UnityEvent<int> { }

    [Serializable]
    public class StringEvent : UnityEvent<string> { }

    [Serializable]
    public class Vector2Event : UnityEvent<Vector2> { }

    [Serializable]
    public class Vector3Event : UnityEvent<Vector3> { }

    [Serializable]
    public class ColorEvent : UnityEvent<Color> { }

    [Serializable]
    public class GameObjectEvent : UnityEvent<GameObject> { }

    [Serializable]
    public class TransformEvent : UnityEvent<Transform> { }

    [Serializable]
    public class TextureEvent : UnityEvent<Texture> { }

}
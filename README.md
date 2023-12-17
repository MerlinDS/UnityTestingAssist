# Unity Testing Assist

### Helper methods for unit testing of the <a href="https://unity.com/">Unity</a> Objects.

This package contains a set of helper methods for unit testing of the Unity Objects, such as MonoBehaviour, ScriptableObject, etc.

[comment]: <> (Finish description)

## 💾 Installation
You can install Unity Testing Assist using any of the following methods:

### Unity Package Manager
```
https://github.com/merlinds/UnityTestingAssist.git?path=/Assets/UnityTestingAssist#v0.1.2
```

1. In Unity, open **Window** → **Package Manager**.
2. Press the **+** button, choose "**Add package from git URL...**"
3. Enter url above and press **Add**.

[comment]: <> (Add other installation methods)

## 📖 Usage

### Unity life cycle events

```csharp
class SomeMonoBehaviour : MonoBehaviour
{
    private void Awake()
    {
        // Do something
    }
    
    private void Start()
    {
        // Do something
    }
}
```
You can execute Unity life cycle events like this:
```csharp
[Test]
public void Test()
{
    SomeMonoBehaviour someMonoBehaviour = new GameObject().AddComponent<SomeMonoBehaviour>();
    someMonoBehaviour.ExecuteAwake();
}
```
Or you can execute chain of events:
```csharp
[Test]
public void Test()
{
    var someMonoBehaviour = new GameObject().AddComponent<SomeMonoBehaviour>();
    someMonoBehaviour.ExecuteAwake().ExecuteStart();
}
```

### Serialized fields and auto properties

```csharp
class SomeMonoBehaviour : MonoBehaviour
{
    [SerializeField] private int _serializeField;
    [field: SerializeField] public float SerializeProperty { get; private set; }
}
```

You can set value to serialized field or auto-property like this:
```csharp
[Test]
public void Test()
{
    var someMonoBehaviour = new GameObject().AddComponent<SomeMonoBehaviour>();
    someMonoBehaviour.EditSerializable()
        .Field("_serializeField", 42)
        .Field("SerializeProperty", 42.42f)
        .Apply();
}
```
**Don't forget to call `Apply()` method at the end of the chain.**




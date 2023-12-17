# Unity Testing Assist

### Helper methods for unit testing of the <a href="https://unity.com/">Unity</a> Objects.

This package contains a set of helper methods for unit testing of the Unity Objects, such as MonoBehaviour, ScriptableObject, etc.

## Work in progress! 👷

[comment]: <> (Finish description)

## 💾 Installation
You can install Unity Testing Assist using any of the following methods:

### Unity Package Manager
```
https://github.com/merlinds/UnityTestingAssist.git?path=/Assets/UnityTestingAssist#v0.1.0
```

1. In Unity, open **Window** → **Package Manager**.
2. Press the **+** button, choose "**Add package from git URL...**"
3. Enter url above and press **Add**.

[comment]: <> (Add other installation methods)

## Usage

### MonoBehaviour

Execute hidden unity event, such as `Awake`, `Start`, `Update`, etc.

Example:
```csharp
// Execute Awake event
new GameObject().AddComponent<SomeMonoBehaviour>().ExecuteAwake();

// Execute chaine of events
new GameObject().AddComponent<SomeMonoBehaviour>().ExecuteAwake().ExecuteStart().ExecuteUpdate();
```



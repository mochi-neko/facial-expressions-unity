# facial-expressions-unity

Provides facial expressions system of human models for Unity.

## Features

- [x] Blink system
- [x] LipSync system
- [x] Emotion system

## Extensions

- [x] VRM
- [x] VOICEVOX
- [ ] StateMachine

## How to import by Unity Package Manager

Minimal dependencies

```json
{
  "dependencies": {
    "com.mochineko.facial-expressions": "https://github.com/mochi-neko/facial-expressions-unity.git?path=/Assets/Mochineko/FacialExpressions#0.1.0",
    "com.mochineko.relent.result": "https://github.com/mochi-neko/Relent.git?path=/Assets/Mochineko/Relent/Result#0.1.1",
    "com.mochineko.relent.extensions.unitask": "https://github.com/mochi-neko/Relent.git?path=/Assets/Mochineko/Relent.Extensions/UniTask#0.1.3",
    "com.cysharp.unitask": "https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask",
    ...
  }
}
```

If you use VOICEVOX extension, please add

```json
{
  "dependencies": {
    "com.mochineko.facial-expressions.extensions.voicevox": "https://github.com/mochi-neko/facial-expressions-unity.git?path=/Assets/Mochineko/FacialExpressions.Extensions/VOICEVOX#0.1.0",
    "com.mochineko.voicevox-api": "https://github.com/mochi-neko/VOICEVOX-API-unity.git?path=/Assets/Mochineko/VOICEVOX_API#0.2.1",
    "com.mochineko.relent.uncertain-result": "https://github.com/mochi-neko/Relent.git?path=/Assets/Mochineko/Relent/UncertainResult#0.1.1",
    "com.mochineko.relent.resilience": "https://github.com/mochi-neko/Relent.git?path=/Assets/Mochineko/Relent/Resilience#0.1.1",
    ...
  }
}
```

If you use VRM extension, please add

```json
{
  "dependencies": {
    "com.mochineko.facial-expressions.extensions.vrm": "https://github.com/mochi-neko/facial-expressions-unity.git?path=/Assets/Mochineko/FacialExpressions.Extensions/VRM#0.1.0",
    "com.vrmc.gltf": "https://github.com/vrm-c/UniVRM.git?path=/Assets/UniGLTF#v0.108.0",
    "com.vrmc.vrm": "https://github.com/vrm-c/UniVRM.git?path=/Assets/VRM10#v0.108.0",
    "com.vrmc.vrmshaders": "https://github.com/vrm-c/UniVRM.git?path=/Assets/VRMShaders#v0.108.0",
    ...
  }
}
```

## Samples

If you use [sample with VOICEVOX and VRM](https://github.com/mochi-neko/facial-expressions-unity/blob/main/Assets/Mochineko/FacialExpressions.Samples/SampleForVoicevoxAndVRM.cs),
 all dependencies are

```json
{
  "dependencies": {
    "com.mochineko.facial-expressions": "https://github.com/mochi-neko/facial-expressions-unity.git?path=/Assets/Mochineko/FacialExpressions#0.1.0",
    "com.mochineko.facial-expressions.extensions.voicevox": "https://github.com/mochi-neko/facial-expressions-unity.git?path=/Assets/Mochineko/FacialExpressions.Extensions/VOICEVOX#0.1.0",
    "com.mochineko.facial-expressions.extensions.vrm": "https://github.com/mochi-neko/facial-expressions-unity.git?path=/Assets/Mochineko/FacialExpressions.Extensions/VRM#0.1.0",
    "com.mochineko.relent.result": "https://github.com/mochi-neko/Relent.git?path=/Assets/Mochineko/Relent/Result#0.1.1",
    "com.mochineko.relent.uncertain-result": "https://github.com/mochi-neko/Relent.git?path=/Assets/Mochineko/Relent/UncertainResult#0.1.3",
    "com.mochineko.relent.resilience": "https://github.com/mochi-neko/Relent.git?path=/Assets/Mochineko/Relent/Resilience#0.1.3",
    "com.mochineko.relent.extensions.unitask": "https://github.com/mochi-neko/Relent.git?path=/Assets/Mochineko/Relent.Extensions/UniTask#0.1.3",
    "com.mochineko.simple-audio-codec-unity": "https://github.com/mochi-neko/simple-audio-codec-unity.git?path=/Assets/Mochineko/SimpleAudioCodec#0.1.2",
    "com.mochineko.voicevox-api": "https://github.com/mochi-neko/VOICEVOX-API-unity.git?path=/Assets/Mochineko/VOICEVOX_API#0.2.1",
    "com.naudio.core": "https://github.com/mochi-neko/simple-audio-codec-unity.git?path=/Assets/NAudio/NAudio.Core#0.1.2",
    "com.vrmc.gltf": "https://github.com/vrm-c/UniVRM.git?path=/Assets/UniGLTF#v0.108.0",
    "com.vrmc.vrm": "https://github.com/vrm-c/UniVRM.git?path=/Assets/VRM10#v0.108.0",
    "com.vrmc.vrmshaders": "https://github.com/vrm-c/UniVRM.git?path=/Assets/VRMShaders#v0.108.0",
    "com.cysharp.unitask": "https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask",
    "com.unity.nuget.newtonsoft-json": "3.0.2",
    ...
  }
}
```

## Changelog

See [CHANGELOG](https://github.com/mochi-neko/facial-expressions-unity/blob/main/CHANGELOG.md).

## 3rd Party Notices

See [NOTICE](https://github.com/mochi-neko/facial-expressions-unity/blob/main/NOTICE.md).

## License

[MIT License](https://github.com/mochi-neko/facial-expressions-unity/blob/main/LICENSE).

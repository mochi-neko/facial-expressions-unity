# facial-expressions-unity

Abstract facial expressions system of human models for Unity.

## Features

- [x] Blink system
  - Procedural blink animation.
- [x] LipSync system 
  - Audio volume based lip animation.
- [x] Emotion system
  - Exclusive emotion animation.
  - Can define custom emotion.
  - Provide basic emotion defined by Paul Ekman. 

## Extensions for other libraries

- [x] [VRM](https://github.com/vrm-c/UniVRM)
  - Blink, lip and emotion animators for VRM model. 
- [x] [VOICEVOX](https://voicevox.hiroshiba.jp/)
  - VOICEVOX audio query based lip animation.
- [x] [uLipSync](https://github.com/hecomi/uLipSync)
  - Voice analysis based lip animation by uLipSync.

You can extend to other libraries e.g. Live2D.

## How to import by Unity Package Manager

Add following dependencies to your `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.mochineko.facial-expressions": "https://github.com/mochi-neko/facial-expressions-unity.git?path=/Assets/Mochineko/FacialExpressions#0.3.3",
    "com.mochineko.relent": "https://github.com/mochi-neko/Relent.git?path=/Assets/Mochineko/Relent#0.2.0",
    "com.mochineko.relent.extensions.unitask": "https://github.com/mochi-neko/Relent.git?path=/Assets/Mochineko/Relent.Extensions/UniTask#0.2.0",
    "com.cysharp.unitask": "https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask",
    ...
  }
}
```
.

All dependencies are in [package.json](./Assets/Mochineko/FacialExpressions/package.json).

## How to import extensions

If you use VOICEVOX extension, please add following dependencies:

```json
{
  "dependencies": {
    "com.mochineko.facial-expressions.extensions.voicevox": "https://github.com/mochi-neko/facial-expressions-unity.git?path=/Assets/Mochineko/FacialExpressions.Extensions/VOICEVOX#0.3.3",
    "com.mochineko.voicevox-api": "https://github.com/mochi-neko/VOICEVOX-API-unity.git?path=/Assets/Mochineko/VOICEVOX_API#0.2.2",
    ...
  }
}
```
.

If you use VRM extension, please add following dependencies:

```json
{
  "dependencies": {
    "com.mochineko.facial-expressions.extensions.vrm": "https://github.com/mochi-neko/facial-expressions-unity.git?path=/Assets/Mochineko/FacialExpressions.Extensions/VRM#0.3.3",
    "com.vrmc.gltf": "https://github.com/vrm-c/UniVRM.git?path=/Assets/UniGLTF#v0.108.0",
    "com.vrmc.vrm": "https://github.com/vrm-c/UniVRM.git?path=/Assets/VRM10#v0.108.0",
    "com.vrmc.vrmshaders": "https://github.com/vrm-c/UniVRM.git?path=/Assets/VRMShaders#v0.108.0",
    ...
  }
}
```
.

If you use uLipSync extension, please add following dependencies:

```json
{
  "dependencies": {
    "com.mochineko.facial-expressions.extensions.ulipsync": "https://github.com/mochi-neko/facial-expressions-unity.git?path=/Assets/Mochineko/FacialExpressions.Extensions/uLipSync#0.3.3",
    "com.hecomi.ulipsync": "https://github.com/hecomi/uLipSync.git#upm",
    "com.neuecc.unirx": "https://github.com/neuecc/UniRx.git?path=Assets/Plugins/UniRx/Scripts",
    ...
  }
}
```
.

## Samples

- [Sample to use VOICEVOX and VRM](./Assets/Mochineko/FacialExpressions.Samples/SampleForVoiceVoxAndVRM.cs).
- [Sample to use audio volume based lip sync](./Assets/Mochineko/FacialExpressions.Samples/VolumeBasedLipSyncSample.cs).
- [Sample to use uLipSync and VRM](./Assets/Mochineko/FacialExpressions.Samples/SampleForULipSyncAndVRM.cs).

## Change log

See [CHANGELOG](./CHANGELOG.md).

## 3rd Party Notices

See [NOTICE](./NOTICE.md).

## License

Licensed under the [MIT](./LICENSE) license.
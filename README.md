# facial-expressions-unity

Provides facial expressions system of human models for Unity.

## Features

- [x] Blink system
  - Procedural blink animation.
- [x] LipSync system 
  - Audio volume based lip animation.
- [x] Emotion system
  - Exclusive emotion animation.
  - Can define custom emotion.
  - Provide basic emotion defined by Paul Ekman. 

## Extensions

- [x] VRM
  - Blink, lip and emotion animators for VRM model. 
- [x] VOICEVOX
  - VOICEVOX audio query based lip animation. 

You can extend to other libraries e.g. Live2D.

## How to import by Unity Package Manager

Add following package to your `Packages/manifest.json`:

```
"com.mochineko.facial-expressions": "https://github.com/mochi-neko/facial-expressions-unity.git?path=/Assets/Mochineko/FacialExpressions#0.3.0",
```
.

All dependencies are in [package.json](./Assets/Mochineko/FacialExpressions/package.json).

## How to import extensions

If you use VOICEVOX extension, please add following dependency:

```
"com.mochineko.facial-expressions.extensions.voicevox": "https://github.com/mochi-neko/facial-expressions-unity.git?path=/Assets/Mochineko/FacialExpressions.Extensions/VOICEVOX#0.3.0",
```
.

If you use VRM extension, please add following dependency:

```
"com.mochineko.facial-expressions.extensions.vrm": "https://github.com/mochi-neko/facial-expressions-unity.git?path=/Assets/Mochineko/FacialExpressions.Extensions/VRM#0.3.0",
```
.

## Samples

- [Sample to use VOICEVOX and VRM](./Assets/Mochineko/FacialExpressions.Samples/SampleForVoiceVoxAndVRM.cs).
- [Sample to use audio volume based lip sync](./Assets/Mochineko/FacialExpressions.Samples/VolumeBasedLipSyncSample.cs).

## Change log

See [CHANGELOG](./CHANGELOG.md).

## 3rd Party Notices

See [NOTICE](./NOTICE.md).

## License

Licensed under the [MIT](./LICENSE) license.

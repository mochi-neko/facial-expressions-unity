# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [0.4.3] - 2023-08-09

### Added
- Add option to separate both blend shape for `SkinnedMeshEyelidMorpher`.

### Fixed
- Fix scale of weight for blend shape in `SkinnedMeshRenderer`.

## [0.4.3] - 2023-08-09

### Added
- Add implementation of emotion morpher for `Animator` and `SkinnedMeshRenderer`.

### Fixed
- Improve reset logic of implementations for `Animator` and `SkinnedMeshRenderer`.
- Fix not implemented method of `SkinnedMeshLipMorpher`.

## [0.4.2] - 2023-08-09

### Fixed
- Fix accessibility of some composition from internal to public.

## [0.4.1] - 2023-08-09

### Added
- Add compositions of each eyelid, lip and emotion morphers.

## [0.4.0] - 2023-08-09

### Added
- Add loop animator for lip and emotion.

### Changed
- Redesign lip, eyelid and emotion animator APIs.

## [0.3.3] - 2023-08-01

### Added

- Add extension for uLipSync.

## [0.3.2] - 2023-07-20

### Fixed

- Fix broken `package.json`s of extensions.

## [0.3.1] - 2023-07-12

### Fixed

- Fix dependencies in `package.json`.

## [0.3.0] - 2023-05-19

### Added

- Add volume-based lip animator: `VolumeBasedLipAnimator` with sample.

### Changed

- Update interfaces of animators.
- Rename `Emotion` to `BasicEmotion`.

## [0.2.0] - 2023-04-12

### Added

- Add emotion system.

### Changed

- Update Relent version to 0.2.0.

## [0.1.0] - 2023-04-04

### Added

- Add blink system.
- Add lip sync system.
- Add extension for VRM.
- Add extension for VOICEVOX.

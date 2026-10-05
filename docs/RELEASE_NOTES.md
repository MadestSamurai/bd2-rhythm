# BD2 Rhythm v0.2.3

## 简体中文

### 更新内容

- 同步公共连接组件，改善游戏关闭后的会话识别和工具交接稳定性。
- 完善统一宿主内的语言切换、窗口协作和连接复用。

### 下载

适用于 Windows x64。两版功能相同，均为单 EXE，内置简体中文／English。

| 版本 | 运行环境 | 建议 |
| --- | --- | --- |
| **Portable** | 内置 .NET 运行时 | 首次使用推荐，下载即用 |
| **Lite** | 需安装 .NET Desktop Runtime 8 x64 | 已安装运行时，下载更小 |

下载一种版本即可。ZIP 附中英文说明和许可证；SHA256SUMS.txt 可用于核对下载文件。

### 升级

暂停并关闭旧工具，再打开新版连接；本机设置保留。当前组件支持在游戏保持运行时更新和交接；如游戏本身仍显示断线或登录提示，请先恢复游戏连接。

[使用说明与风险声明](https://github.com/MadestSamurai/bd2-rhythm/blob/main/README.md)

## English

### Changes

- Updated shared connection components for more reliable session detection after game exit and tool handoff.
- Improved shared-host language switching, window integration and connection reuse.

### Downloads

For Windows x64. Both editions have the same features, run as a single EXE and include Simplified Chinese / English.

| Edition | Runtime requirement | Recommended for |
| --- | --- | --- |
| **Portable** | .NET included | Most users; download and run |
| **Lite** | .NET Desktop Runtime 8 x64 required | Smaller download if the runtime is installed |

Download one edition. ZIPs include both READMEs and licenses. Use SHA256SUMS.txt to verify downloaded files.

### Upgrade

Pause and close the old assistant, then connect with the new version. Local settings are retained. Current components support updates and handoff while the game stays open. If the game itself is disconnected or asking you to log in, restore its connection first.

[Usage and risk disclaimer](https://github.com/MadestSamurai/bd2-rhythm/blob/main/README.en.md)

---

# BD2 Rhythm v0.2.1

## 简体中文

### 更新内容

- 主窗口增加免费开源署名：GitHub MadestSamurai／B站 MadSamurai。
- 新增「来源与说明」，可查看并复制官方仓库与下载链接；随界面切换中英文。
- 统一双语 README、来源与风险说明，ZIP 附带完整说明；MIT 许可证保持不变。

### 下载

| 版本 | 运行环境 | 建议 |
| --- | --- | --- |
| **Portable** | 自带 .NET，无需另装运行库 | 大多数用户 |
| **Lite** | 需要 .NET Desktop Runtime 8 x64 | 已安装桌面运行库、希望减小下载体积 |

两版功能相同，内置简体中文／English。EXE 可独立使用；ZIP 附带双语说明与许可证。用 `SHA256SUMS.txt` 核对下载。

### 升级

停止自动操作并关闭旧工具，再打开新版。已有设置保留；本次主要更新来源与说明界面。

作者发布版免费。第三方收费不代表作者参与、背书或提供服务。[使用说明与风险提示](https://github.com/MadestSamurai/bd2-rhythm/blob/main/README.md)。

## English

### Changes

- Adds free-release attribution to the main window: GitHub MadestSamurai / Bilibili MadSamurai.
- Adds About & source with selectable official repository and download links, following the selected UI language.
- Standardizes bilingual READMEs and source/risk notices, also included in ZIPs. The MIT License is unchanged.

### Downloads

| Build | Runtime | Recommended for |
| --- | --- | --- |
| **Portable** | Includes .NET; no separate runtime needed | Most users |
| **Lite** | Requires .NET Desktop Runtime 8 x64 | Smaller download when the desktop runtime is installed |

Both builds have identical features and include Simplified Chinese / English. EXEs run independently; ZIPs include bilingual documentation and licenses. Verify downloads with `SHA256SUMS.txt`.

### Upgrade

Stop automation and close the old tool, then open the new version. Existing settings are retained; this update primarily changes attribution and source information.

Official releases are free. Third-party fees do not imply the author's involvement, endorsement or support. [Usage and risk notice](https://github.com/MadestSamurai/bd2-rhythm/blob/main/README.en.md).

---

# BD2 Rhythm v0.2.0

## 简体中文

### 更新内容

- 音游助手首次独立开源，使用 MIT 许可证。
- 内置简体中文和 English，可在窗口右上角切换并保存选择。
- 直接读取游戏当前谱面，无需额外导出或下载谱面包。
- 支持普通、长按、滑动、连打与 Fever；随机偏移默认 ±50ms，支持整体时间校准。
- 跟随游戏暂停，停止时释放长按；保留原始连接错误，便于排查问题。

### 下载

| 版本 | 运行环境 | 建议 |
| --- | --- | --- |
| **Portable** | 自带 .NET，无需另装运行库 | 大多数用户 |
| **Lite** | 需要 .NET Desktop Runtime 8 x64 | 已安装桌面运行库，希望减小下载体积 |

仅支持 Windows x64；两版均内置中英文且功能相同。EXE 可单独使用，ZIP 附带说明与许可。下载后可用 `SHA256SUMS.txt` 核对文件。

### 升级与使用

停止并关闭旧音游工具，正常重启游戏，再打开新版并连接。原有偏移设置保留。开启自动演奏后，在游戏内选曲开始；工具不负责自动续局。

使用前请阅读 [README 与免责声明](https://github.com/MadestSamurai/bd2-rhythm/blob/main/README.md)。

## English

### Changes

- First standalone open-source release of BD2 Rhythm, under the MIT License.
- Built-in Simplified Chinese and English with a saved language selector.
- Reads the current chart directly from the game; no chart export or download pack is needed.
- Supports tap, hold, slide, multi-tap and Fever. Random jitter defaults to ±50 ms, with adjustable timing calibration.
- Follows game pauses, releases holds when stopped and preserves original connection errors for troubleshooting.

### Downloads

| Edition | Runtime requirement | Recommended for |
| --- | --- | --- |
| **Portable** | Includes .NET; no separate runtime installation | Most users |
| **Lite** | Requires .NET Desktop Runtime 8 x64 | Users with the desktop runtime who want a smaller download |

Windows x64 only. Both editions include Chinese and English and have the same features. The EXE works on its own; the ZIP includes documentation and licenses. Verify files with `SHA256SUMS.txt`.

### Upgrade and usage

Stop and close the old rhythm app, restart the game normally, then open the new version and connect. Existing timing preferences are retained. Enable auto-play and select a song in the game; automatic round restarts are not included.

Read the [README and disclaimer](https://github.com/MadestSamurai/bd2-rhythm/blob/main/README.en.md) before use.

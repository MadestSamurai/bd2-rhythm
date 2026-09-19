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

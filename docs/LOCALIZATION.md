# Localization / 本地化

The application ships one bilingual executable. `localization/zh-CN.json` and `en-US.json` share the same source-message keys. Chinese source messages are retained in diagnostics; translation happens when text is displayed. Exact messages take priority over longest-first legacy fragments. Song names and raw system/compiler diagnostics are not translated.

`desktop/UiLabels.cs` supplies static WPF resource labels. Runtime state, validation and connection messages pass through `LanguageCatalog.Text`. Add corresponding entries to both catalogs for new app-authored messages. Avoid translating raw asset names.

`language.json` is saved separately under `%LOCALAPPDATA%\BD2Rhythm`. System Chinese defaults to `zh-CN`; other systems default to `en-US`. Selecting a language does not rewrite playback settings or control ownership.

Tests check catalog parity and dynamic connection messages. Packaged GUI smoke checks switch both languages during a simulated active song and check that settings and the control owner survive; screenshots include the regular and minimum window sizes.

界面使用同一个中英双语程序。新增文案应同时更新两个词典；运行日志、曲目名与原始异常保留原文。语言设置独立存储，不影响当前演奏。

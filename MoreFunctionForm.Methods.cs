using CustomYTPlayer.Common.Sets;

namespace CustomYTPlayer;

// 阻擋設計工具。
partial class DesignerBlocker { }

public partial class MoreFunctionForm
{
    /// <summary>
    /// 自定義初始化
    /// </summary>
    private void CustomInit()
    {
        Text = StringSet.MoreFormName;
        Icon = Properties.Resources.app_icon;

        BtnMoreFunction.Enabled = false;
        CBDiscordRichPresence.Checked = Properties.Settings.Default.EnableDiscordRichPresence;
        CBLogVerbose.Checked = Properties.Settings.Default.EnableLogVerbose;
    }
}
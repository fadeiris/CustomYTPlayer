namespace CustomYTPlayer.Models;

/// <summary>
/// 類別：ComboBoxItem
/// </summary>
public class ComboBoxItem
{
    /// <summary>
    /// 文字
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// 值
    /// </summary>
    public string Value { get; set; }

    public ComboBoxItem(string text, string value)
    {
        Text = text;
        Value = value;
    }

    /// <summary>
    /// 轉換成字串
    /// </summary>
    /// <returns>字串</returns>
    public override string ToString()
    {
        return Text;
    }
}
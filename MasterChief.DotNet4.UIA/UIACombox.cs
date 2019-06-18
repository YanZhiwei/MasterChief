using System;
using System.Windows.Automation;

namespace MasterChief.DotNet4.UIA
{
    /// <summary>
    ///     ComboBox 控件
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public static class UIACombox
    {
        /// <summary>
        /// 设置选中项
        /// </summary>
        /// <param name="comboBox">AutomationElement</param>
        /// <param name="item">需要选中的文本</param>
        /// <exception cref="ArgumentException">Element with AutomationId '{comboBox.Current.AutomationId}' and Name '{comboBox.Current.Name}' ControlType is not ComboBox.</exception>
        public static void SetSelectedItem(this AutomationElement comboBox, string item)
        {
            if (comboBox.Current.ControlType != ControlType.ComboBox)
                throw new ArgumentException(
                    $"Element with AutomationId '{comboBox.Current.AutomationId}' and Name '{comboBox.Current.Name}' ControlType is not ComboBox.");
            var automationPatternFromElement =
                GetSpecifiedPattern(comboBox, "ExpandCollapsePatternIdentifiers.Pattern");

            var expandCollapsePattern =
                comboBox.GetCurrentPattern(automationPatternFromElement) as ExpandCollapsePattern;


            var listItem = comboBox.FindFirst(TreeScope.Subtree,
                new PropertyCondition(AutomationElement.NameProperty, item));

            automationPatternFromElement = GetSpecifiedPattern(listItem, "SelectionItemPatternIdentifiers.Pattern");

            var selectionItemPattern = listItem.GetCurrentPattern(automationPatternFromElement) as SelectionItemPattern;

            selectionItemPattern?.Select();
        }

        private static AutomationPattern GetSpecifiedPattern(AutomationElement element, string patternName)
        {
            var supportedPattern = element.GetSupportedPatterns();

            foreach (var pattern in supportedPattern)
                if (pattern.ProgrammaticName == patternName)
                    return pattern;

            return null;
        }
    }
}
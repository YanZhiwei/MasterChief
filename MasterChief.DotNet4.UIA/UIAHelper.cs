using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;

namespace MasterChief.DotNet4.UIA
{
    /// <summary>
    /// UI Automation 辅助类
    /// </summary>
    public sealed class UIAHelper
    {
        public static AutomationElement FindElementById(AutomationElement parentElement, string automationId)
        {
            var findElement = parentElement.FindFirst(TreeScope.Descendants,
                new PropertyCondition(AutomationElement.AutomationIdProperty, automationId));
            return findElement;
        }
    }
}

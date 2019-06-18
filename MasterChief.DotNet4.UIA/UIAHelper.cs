using System;
using System.Windows.Automation;

namespace MasterChief.DotNet4.UIA
{
    /// <summary>
    ///     UI Automation 辅助类
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public static class UIAHelper
    {
        /// <summary>
        ///     根据ID查询AutomationElement
        /// </summary>
        /// <param name="parentElement">父AutomationElement.</param>
        /// <param name="automationId">Automation Id</param>
        /// <returns>AutomationElement</returns>
        public static AutomationElement FindElementById(this AutomationElement parentElement, string automationId)
        {
            var findElement = parentElement.FindFirst(TreeScope.Descendants,
                new PropertyCondition(AutomationElement.AutomationIdProperty, automationId));
            return findElement;
        }

        /// <summary>
        ///     根据Class Name查询AutomationElement
        /// </summary>
        /// <param name="parentElement">父AutomationElement.</param>
        /// <param name="className">Class Name</param>
        /// <returns>AutomationElement</returns>
        public static AutomationElement FindElementByClassName(this AutomationElement parentElement, string className)
        {
            var tarFindElement = parentElement.FindFirst(TreeScope.Descendants,
                new PropertyCondition(AutomationElement.ClassNameProperty, className));
            return tarFindElement;
        }

        /// <summary>
        ///     根据Name查询AutomationElement
        /// </summary>
        /// <param name="parentElement">父AutomationElement.</param>
        /// <param name="name">Name.</param>
        /// <returns>AutomationElement</returns>
        public static AutomationElement FindElementByName(this AutomationElement parentElement, string name)
        {
            var tarFindElement = parentElement.FindFirst(TreeScope.Descendants,
                new PropertyCondition(AutomationElement.NameProperty, name));
            return tarFindElement;
        }

        /// <summary>
        ///     根据Type查询AutomationElement
        /// </summary>
        /// <param name="parentElement">父AutomationElement.</param>
        /// <param name="type">ControlType</param>
        /// <returns>AutomationElementCollection</returns>
        public static AutomationElementCollection FindElementByType(this AutomationElement parentElement,
            ControlType type)
        {
            var tarFindElement = parentElement.FindAll(TreeScope.Descendants,
                new PropertyCondition(AutomationElement.ControlTypeProperty, type));
            return tarFindElement;
        }

        /// <summary>
        ///     获取ExpandCollapsePattern
        /// </summary>
        /// <param name="element">AutomationElement</param>
        /// <returns>ExpandCollapsePattern</returns>
        /// <exception cref="Exception">
        ///     AutomationId '{element.Current.AutomationId}' and Name '{element.Current.Name}' 不支持
        ///     ExpandCollapsePattern.
        /// </exception>
        public static ExpandCollapsePattern GetExpandCollapsePattern(this AutomationElement element)
        {
            if (!element.TryGetCurrentPattern(ExpandCollapsePattern.Pattern, out var currentPattern))
                throw new Exception(
                    $"AutomationId '{element.Current.AutomationId}' and Name '{element.Current.Name}' 不支持 ExpandCollapsePattern.");
            return currentPattern as ExpandCollapsePattern;
        }

        /// <summary>
        ///     获取SelectionItemPattern
        /// </summary>
        /// <param name="element">AutomationElement</param>
        /// <returns>SelectionItemPattern</returns>
        /// <exception cref="Exception">
        ///     AutomationId '{element.Current.AutomationId}' and Name '{element.Current.Name}' 不支持
        ///     SelectionItemPattern.
        /// </exception>
        public static SelectionItemPattern GetSelectionItemPattern(this AutomationElement element)
        {
            if (!element.TryGetCurrentPattern(SelectionItemPattern.Pattern, out var currentPattern))
                throw new Exception(
                    $"AutomationId '{element.Current.AutomationId}' and Name '{element.Current.Name}' 不支持 SelectionItemPattern.");
            return currentPattern as SelectionItemPattern;
        }

        /// <summary>
        ///     获取InvokePattern
        /// </summary>
        /// <param name="element">AutomationElement</param>
        /// <returns>InvokePattern</returns>
        /// <exception cref="Exception">
        ///     AutomationId '{element.Current.AutomationId}' and Name '{element.Current.Name}'
        ///     不支持InvokePattern.
        /// </exception>
        public static InvokePattern GetInvokePattern(this AutomationElement element)
        {
            if (!element.TryGetCurrentPattern(InvokePattern.Pattern, out var currentPattern))
                throw new Exception(
                    $"AutomationId '{element.Current.AutomationId}' and Name '{element.Current.Name}' 不支持InvokePattern.");
            return currentPattern as InvokePattern;
        }

        /// <summary>
        ///     获取WindowPattern
        /// </summary>
        /// <param name="element">AutomationElement</param>
        /// <returns>WindowPattern</returns>
        /// <exception cref="Exception">
        ///     AutomationId '{element.Current.AutomationId}' and Name '{element.Current.Name}'
        ///     不支持GetWindowPattern.
        /// </exception>
        public static WindowPattern GetWindowPattern(this AutomationElement element)
        {
            if (!element.TryGetCurrentPattern(WindowPattern.Pattern, out var currentPattern))
                throw new Exception(
                    $" AutomationId '{element.Current.AutomationId}' and Name '{element.Current.Name}'  不支持GetWindowPattern.");
            return currentPattern as WindowPattern;
        }

        /// <summary>
        ///     获取TogglePattern
        /// </summary>
        /// <param name="element">AutomationElement</param>
        /// <returns>TogglePattern</returns>
        /// <exception cref="Exception">
        ///     AutomationId '{element.Current.AutomationId}' and Name '{element.Current.Name}'
        ///     不支持TogglePattern.
        /// </exception>
        public static TogglePattern GetTogglePattern(this AutomationElement element)
        {
            if (!element.TryGetCurrentPattern(TogglePattern.Pattern, out var currentPattern))
                throw new Exception(
                    $"AutomationId '{element.Current.AutomationId}' and Name '{element.Current.Name}' 不支持TogglePattern.");
            return currentPattern as TogglePattern;
        }
    }
}
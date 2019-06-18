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
    }
}
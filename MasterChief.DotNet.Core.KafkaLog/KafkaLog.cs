using KafkaNet;
using log4net.Util;

namespace MasterChief.DotNet.Core.KafkaLog
{
    /// <summary>
    ///     KafkaLog
    /// </summary>
    /// <seealso cref="KafkaNet.IKafkaLog" />
    public class KafkaLog : IKafkaLog
    {
        /// <summary>
        ///     Debugs the format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public void DebugFormat(string format, params object[] args)
        {
            LogLog.Debug(GetType(), string.Format(format, args));
        }

        /// <summary>
        ///     Errors the format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public void ErrorFormat(string format, params object[] args)
        {
            LogLog.Debug(GetType(), string.Format(format, args));
        }

        /// <summary>
        ///     Fatals the format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public void FatalFormat(string format, params object[] args)
        {
            LogLog.Debug(GetType(), string.Format(format, args));
        }

        /// <summary>
        ///     Informations the format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public void InfoFormat(string format, params object[] args)
        {
            LogLog.Debug(GetType(), string.Format(format, args));
        }

        /// <summary>
        ///     Warns the format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public void WarnFormat(string format, params object[] args)
        {
            LogLog.Debug(GetType(), string.Format(format, args));
        }
    }
}
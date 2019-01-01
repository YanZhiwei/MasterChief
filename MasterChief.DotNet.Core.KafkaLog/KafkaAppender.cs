using KafkaNet;
using KafkaNet.Model;
using KafkaNet.Protocol;
using log4net.Appender;
using log4net.Core;
using System;
using System.IO;
using System.Text;

namespace MasterChief.DotNet.Core.KafkaLog
{
    /// <summary>
    /// Kafka Log4Net 自定义Appender
    /// </summary>
    public class KafkaAppender : AppenderSkeleton
    {
        #region Fields

        /// <summary>
        /// Kafka 生产者
        /// </summary>
        private Producer kafkaProducer;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Brokers
        /// </summary>
        public string Brokers
        {
            get;
            set;
        }

        /// <summary>
        /// Topic
        /// </summary>
        public string Topic
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Initialize the appender based on the options set
        /// </summary>
        /// <remarks>
        /// <para>
        /// This is part of the <see cref="T:log4net.Core.IOptionHandler" /> delayed object
        /// activation scheme. The <see cref="M:log4net.Appender.AppenderSkeleton.ActivateOptions" /> method must
        /// be called on this object after the configuration properties have
        /// been set. Until <see cref="M:log4net.Appender.AppenderSkeleton.ActivateOptions" /> is called this
        /// object is in an undefined state and must not be used.
        /// </para>
        /// <para>
        /// If any of the configuration properties are modified then
        /// <see cref="M:log4net.Appender.AppenderSkeleton.ActivateOptions" /> must be called again.
        /// </para>
        /// </remarks>
        public override void ActivateOptions()
        {
            base.ActivateOptions();
            InitKafkaProducer();
        }

        /// <summary>
        /// Subclasses of <see cref="T:log4net.Appender.AppenderSkeleton" /> should implement this method
        /// to perform actual logging.
        /// </summary>
        /// <param name="loggingEvent">The event to append.</param>
        /// <remarks>
        /// <para>
        /// A subclass must implement this method to perform
        /// logging of the <paramref name="loggingEvent" />.
        /// </para>
        /// <para>This method will be called by <see cref="M:DoAppend(LoggingEvent)" />
        /// if all the conditions listed for that method are met.
        /// </para>
        /// <para>
        /// To restrict the logging of events in the appender
        /// override the <see cref="M:PreAppendCheck()" /> method.
        /// </para>
        /// </remarks>
        protected override void Append(LoggingEvent loggingEvent)
        {
            try
            {
                string message = GetLogMessage(loggingEvent);
                string topic = GetTopic(loggingEvent);

                kafkaProducer.SendMessageAsync(topic, new[] { new Message(message) });
            }
            catch (Exception ex)
            {
                ErrorHandler.Error("KafkaProducer SendMessageAsync", ex);
            }
        }

        /// <summary>
        /// Raises the Close event.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Releases any resources allocated within the appender such as file handles,
        /// network connections, etc.
        /// </para>
        /// <para>
        /// It is a programming error to append to a closed appender.
        /// </para>
        /// </remarks>
        protected override void OnClose()
        {
            base.OnClose();
            StopKafkaProducer();
        }

        private string GetLogMessage(LoggingEvent loggingEvent)
        {
            StringBuilder builder = new StringBuilder();
            using (StringWriter writer = new StringWriter(builder))
            {
                Layout.Format(writer, loggingEvent);

                if (Layout.IgnoresException && loggingEvent.ExceptionObject != null)
                {
                    writer.Write(loggingEvent.GetExceptionString());
                }

                return writer.ToString();
            }
        }

        private string GetTopic(LoggingEvent loggingEvent)
        {
            return string.IsNullOrEmpty(Topic) == true ? Path.GetFileNameWithoutExtension(loggingEvent.Domain) : Topic;
        }

        /// <summary>
        /// 初始化Kafka 生产者
        /// </summary>
        private void InitKafkaProducer()
        {
            try
            {
                if (string.IsNullOrEmpty(Brokers))
                {
                    Brokers = "http://localhost:9200";
                }

                if (kafkaProducer == null)
                {
                    Uri brokers = new Uri(Brokers);
                    KafkaOptions kafkaOptions = new KafkaOptions(brokers)
                    {
                        Log = new KafkaLog()
                    };
                    kafkaProducer = new Producer(new BrokerRouter(kafkaOptions));
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Error("InitKafkaProducer", ex);
            }
        }

        /// <summary>
        /// 停止生产者
        /// </summary>
        private void StopKafkaProducer()
        {
            try
            {
                kafkaProducer?.Stop();
            }
            catch (Exception ex)
            {
                ErrorHandler.Error("StopKafkaProducer", ex);
            }
        }

        #endregion Methods
    }
}
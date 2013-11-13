using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using ElmahExtensions.Configuration;
using ElmahExtensions.ErrorActions;
using ElmahExtensions.ErrorConditions;
using ElmahExtensions.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ElmahExtensions
{
    [TestClass]
    public class BasicTests
    {
        /// <summary>
        /// Test serialization
        /// </summary>
        [TestMethod]
        public void TestConfigurationSerializeDeserialize()
        {
            var config = new CustomErrorHandlerConfiguration();
            config.ErrorHandlers = new List<ErrorHandlerSection>();
            config.ErrorHandlers.Add(new ErrorHandlerSection());
            config.ErrorHandlers[0].ErrorActions = new List<ErrorAction>();
            config.ErrorHandlers[0].ErrorActions.Add(new DummyErrorAction() { Name = "TestAction01" });
            config.ErrorHandlers[0].ErrorActions.Add(new WriteToEventLogErrorAction()
                {
                    EventLogEntryType = "Warning", 
                    Name = "EventLogger01", 
                    SourceName = "Test",
                    StringFormat = "Error occurred in ${ApplicationName}: ${Detail}"
                });
            config.ErrorHandlers[0].ErrorConditions = new List<ErrorCondition>();
            config.ErrorHandlers[0].ErrorConditions.Add(new DummyErrorCondition());
            config.ErrorHandlers[0].ErrorConditions.Add(new ElmahErrorMessageSubstringCondition(){Substring = "object reference not set"});
            config.ErrorHandlers[0].ErrorConditions.Add(new ExceptionTypeErrorCondition(){TypeName = "System.NullReferenceException"});
            config.ErrorHandlers[0].ErrorConditions.Add(new CatchAllErrorCondition());

            var xmls = new XmlSerializer(typeof (CustomErrorHandlerConfiguration));
            var serialized = string.Empty;
            using (var writer = new StringWriter())
            {
                xmls.Serialize(writer, config);
                serialized = writer.ToString();
            }

            var config2 = CustomErrorHandlerConfiguration.ReadFromString(serialized);
            Assert.AreEqual(config.ErrorHandlers.Count, config2.ErrorHandlers.Count);
            for (var i = 0; i < config.ErrorHandlers.Count; i++)
            {
                Assert.AreEqual(config.ErrorHandlers[i].ErrorActions.Count, config2.ErrorHandlers[i].ErrorActions.Count);
                Assert.AreEqual(config.ErrorHandlers[i].ErrorConditions.Count, config2.ErrorHandlers[i].ErrorConditions.Count);
                Assert.AreEqual(config.ErrorHandlers[i].Name, config2.ErrorHandlers[i].Name);
                for (var j = 0; j < config.ErrorHandlers[i].ErrorActions.Count; j++)
                {
                    Assert.AreEqual(config.ErrorHandlers[i].ErrorActions[j].Name, config2.ErrorHandlers[i].ErrorActions[j].Name);
                    Assert.AreEqual(config.ErrorHandlers[i].ErrorActions[j].GetType(), config2.ErrorHandlers[i].ErrorActions[j].GetType());
                }
                for (var j = 0; j < config.ErrorHandlers[i].ErrorConditions.Count; j++)
                {
                    Assert.AreEqual(config.ErrorHandlers[i].ErrorConditions[j].GetType(), config2.ErrorHandlers[i].ErrorConditions[j].GetType());
                }
            }
        }

        /// <summary>
        /// Test reading from a custom appsection
        /// </summary>
        [TestMethod]
        public void TestDeSerializeFromAppConfig()
        {
            var config = SettingsManager.Config;
        }

        /// <summary>
        /// Test deserialize from a string and execute
        /// </summary>
        [TestMethod]
        public void TestDeSerializeAndRun()
        {
            var config = CustomErrorHandlerConfiguration.ReadFromString(CommonCode.GetStringFromEmbeddedResource("ElmahExtensions.SampleConfiguration01.xml"));
            var handler = new CustomErrorHandler() {Configuration = config};
            var error = new Elmah.Error(new NullReferenceException());
            handler.HandleError(error);
        }
    }
}

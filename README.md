ElmahExtensions
===============

Built on top of the ELMAH platform, this library extends the way ELMAH reacts to errors
ELMAH itself does a wonderful job with detailed unobtrusive logging. The ElmahExtensions give tighter control over special conditions on how to react to specific errors using conditions and actions.

## Configuration settings

* **CustomErrorHandlerConfiguration node** - Required, single node. Has the optional attribute "*ErrorHandlerLoggingLevel*" which provides trace output debugging. Valid values are Debug, Info, Warn, Error. Default is Error.
  * **ErrorHandlers node** - Required, single node. No attributes. Contains a collection of ErrorHandlerSections.
     * **ErrorHandlerSection nodes** - Optional, 0-*. Represents a special error you want to handle, how to filter it and what actions to take. Contains a collection of conditions and actions. If a given error meets all the conditions then all the actions are performed on the error. Has the "*Name*" attribute which allows you to give a section an alias.
         * **ErrorConditions node** - Required. contains a collection of ErrorConditions
             * **ElmahErrorMessageSubstringCondition node** - Condition returns true when the error message contains a substring (case insensitive). The attrubite *Substring* is the substring to match.
             * **ExceptionTypeErrorCondition node** - Condition returns true when the error's exception type matches the specified type. The attribute *TypeName* specifies the type (for example, System.NullReferenceException)
             * **CatchAllErrorCondition node** - Condition always returns true.
         * **ErrorActions node** - required. Contains a collection of ErrorActions. ErrorActions will be performed in a sequential order. A chan of error actions will continue if one fails. All error actions have an optional "Name" attirbute for debugging/documentation purposes
             * **WriteToEventLogErrorAction node** - writes to application event log. has the following attributes:
                 * EventLogEntryType - Coincides with the Microsoft EventLogEntryTypes Debug/Error/Info/Warn
                 * SourceName - Name of the log source
                 * StringFormat - A template for the error message containing a mix of variables and text. The variables are wrapped in a ${ } block and are expected to be properties of the elmah error.
             * **SendEmailErrorAction node** - Sends email. Expects the system.Net/mailSettings section of the web.config is set up. it has the following attributes:
                 * EnableSsl - Compensates for a bug in System.Net.Mail :( enables TLS encryption. Default: false if not specified.
                 * From - From email address
                 * Recipients - comma-separated list of email addresses
                 * CcRecipients - comma-separated list of email addresses
                 * FormattedSubject - A template for the subject containing a mix of variables and text. The variables are wrapped in a ${ } block and are expected to be properties of the elmah error.
                 * FormattedBody - A template for the email body containing a mix of variables and text. Email body is HTML enabled. The variables are wrapped in a ${ } block and are expected to be properties of the elmah error.







## Installation

Step 1: place ELMAH dll and ElmahExtensions dll in the bin folder

Step 2: add the config section:
```xml
  <configSections>
    <section name="ElmahExtensions" type="ElmahExtensions.ElmahExtentsionsConfigurationSectionHandler, ElmahExtensions"/>
  </configSections>
```
and the actual config section. Below is an example of two sections. The first section executed when an error is encountered containing a match on a specific substring and it writes to the event log. 
The second section executes with all NullReferenceExceptions and sends a templated email to specific recipients.
```xml
  <ElmahExtensions>
    <CustomErrorHandlerConfiguration ErrorHandlerLoggingLevel="Debug">
      <ErrorHandlers>
        <ErrorHandlerSection Name="Section01">
          <ErrorConditions>
            <ElmahErrorMessageSubstringCondition Substring="object reference not set" />
          </ErrorConditions>
          <ErrorActions>
            <WriteToEventLogErrorAction
              Name="EventLogger01"
              EventLogEntryType="Warning"
              SourceName="Test"
              StringFormat="Error occurred in [${ApplicationName}]: Details: [${Detail}]" />
          </ErrorActions>
        </ErrorHandlerSection>
        <ErrorHandlerSection Name="Section02">
          <ErrorConditions>
            <CatchAllErrorCondition />
            <ExceptionTypeErrorCondition TypeName="System.NullReferenceException"/>
          </ErrorConditions>
          <ErrorActions>
            <SendEmailErrorAction
              EnableSsl="true"
              From="talktopete@gmail.com"
              Recipients="reallifedata@gmail.com,talktopete@gmail.com"
              CcRecipients="talktopete@gmail.com"
              FormattedSubject="Error occurred in application [${ApplicationName}]"
              FormattedBody="Details: [${Detail}]"
          />
          </ErrorActions>
        </ErrorHandlerSection>
      </ErrorHandlers>
    </CustomErrorHandlerConfiguration>
  </ElmahExtensions>
```

Step 3: register the http module. (the example below is in IIS 7 integrated pipeline mode)
```xml
  <system.webServer>
    <modules>
      <add name="ElmahExtensions" type="ElmahExtensions.CustomErrorModule, ElmahExtensions"/>
    </modules>
  </system.webServer>
```

OPTIONAL Step 4: Configure email (the example below is gmail example)
```xml
  <system.net>
    <mailSettings>
      <smtp from="talktopete@gmail.com">
        <network host="smtp.gmail.com" port="587" userName="talktopete@gmail.com" password="xxx" />
      </smtp>
    </mailSettings>
  </system.net>
```
OPTIONAL Step 5: Configure trace listening (Any errors in the configuration are reported to the trace. If you want to debug ElmahExtensions set the ErrorHandlerLoggingLevel to "Debug" in the config section and redirect trace output to a place where you can reach it.)
```xml
  <system.diagnostics>
    <trace autoflush="true">
      <listeners>
        <add name="logListener" type="System.Diagnostics.TextWriterTraceListener" initializeData="cat.log" />
      </listeners>
    </trace>
  </system.diagnostics>
```
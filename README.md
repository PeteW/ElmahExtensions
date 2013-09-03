ElmahExtensions
===============

Built on top of the ELMAH platform, this library extends the way ELMAH reacts to errors
Step 1: place ELMAH dll and ElmahExtensions dll in the bin folder
Step 2: add the config section:
```xml
  <configSections>
    <section name="ElmahExtensions" type="ElmahExtensions.ElmahExtentsionsConfigurationSectionHandler, ElmahExtensions"/>
  </configSections>
```
and
```xml
  <ElmahExtensions>
    <CustomErrorHandlerConfiguration ErrorHandlerLoggingLevel="Debug">
      <ErrorHandlers>
        <ErrorHandlerSection Name="Section01">
          <ErrorConditions>
            <DummyErrorCondition />
            <ElmahErrorMessageSubstringCondition Substring="object reference not set" />
          </ErrorConditions>
          <ErrorActions>
            <DummyErrorAction Name="TestAction01" />
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
OPTIONAL Step 5: Configure trace listening (Any errors in the configuration are reported to the trace)
```xml
  <system.diagnostics>
    <trace autoflush="true">
      <listeners>
        <!--        <add name="logListener" type="System.Diagnostics.TextWriterTraceListener" initializeData="cat.log" />-->
        <add name="consoleListener" type="System.Diagnostics.ConsoleTraceListener"/>
      </listeners>
    </trace>
  </system.diagnostics>
```
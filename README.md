## Done
- Start a barebones Logic App with `func host start` from the Aspire AppHost
- Emit logs - but not traces - into the OTel collector by setting the environment variables

## TODO
- Emit traces and view these in the dashboard
- Expose the Logic App SAS URL in the dashboard
- Start the dependencies in design-time as well as runtime

## Known issues
- Only works by passing userprofile paths to the Logic Apps extension-managed versions of the Dotnet SDK and Node, not the system-installed versions
- Hardcoded `OTEL_EXPORTER_OTLP_ENDPOINT` port
- Would like to add a Service Bus action, but the connector doesn't seem to support the SB emulator
- With a workspace open, F5 defaults to starting the Logic App project

## Questions
- Logic Apps insist on being contained in a code-workspace; how well will this play with Aspire? Presumably a C# project will naturally live in a solution file for the C# dev kit and for VS Enterprise. Logic Apps can be converted to .csproj, but this isn't the default, and they wouldn't lend themselves to being contained in a solution unless converted.
- Should I expect Advanced Telemetry to emit traces to the trace view in the dashboard, or do I need to call something that would start another span to make them appear as traces? Or do Logic Apps only emit structured logs and not traces? The documentation doesn't specify.

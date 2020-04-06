Muon.Runtime is the C# project that is baked (statically linked) into every assembly that Muon outputs.
Muon.Runtime.Bootstrap.BootRuntime is the first call of any Muon program.

# Implementation Details
- The Muon Runtime has this concept of "Compile-Time Data" which specifies the parameters of the runtime.
    - Muon must implement Bootstrap.GetCompileTimeConstants to return this data.

- In the loading namespace, there is something called the AllFunctionsVTable. As you may infer, this VTable will contain
every function in an application or library and is critical for enabling dynamic linking. However, the compiler may omit
data for the AllFunctionsVTable if dynamic linking has been disabled (which is the case by default)
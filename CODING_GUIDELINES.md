# Coding Guidelines

Coding guidelines for contributing to Suzerain Modding Kit.

## General Naming Conventions

- Private properties: `_underscorePrefixedCamelCase`.
- Local variables: `camelCase`.
- Everything else: `PascalCase`.
- Prefix interfaces with `I`.

## Variables

- Avoid using `var`, use explicit type names instead.

## Type Design

- Apply the most restrictive access modifiers for a feature to work.
    - Members within an `internal` class may use `public` instead of `internal`. They will be `internal` regardless of whether `public`/`internal` is specified because the class is `internal`.
- `internal` classes that are not inherited from should always be `sealed`.
- `public` classes should not be `sealed` unless necessary.
- Avoid exposing mutable public fields.

## File and Namespace Structure

- There should be one type per file, and the type should have the same name as the file.
    - **Except for patches.** A file containing patches may have multiple patch classes, and the name of the file may be different.
- Namespaces should match folder hierarchy.

## Patches

- Patch classes should be the patched class name, the patched method name, and the name of the patch (in `PascalCase`) or `Patch`, separated by underscores. For example: `ClassName_MethodName_Patch`, `ClassName_MethodName_PatchThatDoesSomethingSpecific`.
- Multiple patch classes may be contained in one file.
- Patch files should generally live next to their target (eg. `Events.cs` and `EventsPatches.cs`, `Conversation\*.cs` files and `Conversation\Patches.cs`).
- Prefer using `Postfix` over `Prefix`.

## Error Handling

- **Internal error handling:** _Suzerain Modding Kit failures are not the responsibility of the consumer._ Errors which are the result of a failure of Suzerain Modding Kit should be handled. An error or warning message should be logged instead of throwing an exception.
    - If the error is in an API method, the method should return a flag indicating to the caller that the method failed (usually `false` or `null`).
- **User error handling:** Errors which are caused by the consumers of the API should throw (eg. throw `ArgumentNullExceptions` and other user errors).
- **Avoiding corrupted state:** _An error caused by one mod should not affect other mods._ Avoid creating corrupted state (eg. halfway registering a custom story fragment) when an error occurs. Validate before performing the action so the method doesn't terminate halfway through the action. If something can't be validated beforehand, use default values or roll back the state before terminating.


# Coding Guidelines

Coding guidelines for contributing to Suzerain Modding Kit. **These guidelines are not complete yet.**

## Error Handling

- **Internal error handling:** _Suzerain Modding Kit failures are not the responsibility of the consumer._ Errors which are the result of a failure of Suzerain Modding Kit should be handled. An error or warning message should be logged instead of throwing an exception.
    - If the error is in an API method, the method should return a flag indicating to the caller that the method failed (usually `false` or `null`).
- **User error handling:** Errors which are caused by the consumers of the API should throw (eg. throw `ArgumentNullExceptions` and other user errors).
- **Avoiding corrupted state:** _An error caused by one mod should not affect other mods._ Avoid creating corrupted state (eg. halfway registering a custom story fragment) when an error occurs. Validate before performing the action so the method doesn't terminate halfway through the action. If something can't be validated beforehand, use default values or roll back the state before terminating.


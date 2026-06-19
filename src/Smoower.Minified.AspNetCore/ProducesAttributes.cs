using Microsoft.AspNetCore.Mvc;

namespace Smoower.Minified.AspNetCore;

// Compact [ProducesResponseType(...)] attributes for documented APIs. Stack them
// like the originals; the generic forms carry the response body type:
//   [P200<UserDto>, P404] public Tr Get(int id) => ...
// ProducesResponseTypeAttribute lives in the ASP.NET Core framework, so these add
// no extra dependency.

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public sealed class P200Attribute() : ProducesResponseTypeAttribute(200);

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public sealed class P200Attribute<T>() : ProducesResponseTypeAttribute(typeof(T), 200);

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public sealed class P201Attribute() : ProducesResponseTypeAttribute(201);

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public sealed class P201Attribute<T>() : ProducesResponseTypeAttribute(typeof(T), 201);

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public sealed class P202Attribute() : ProducesResponseTypeAttribute(202);

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public sealed class P204Attribute() : ProducesResponseTypeAttribute(204);

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public sealed class P400Attribute() : ProducesResponseTypeAttribute(400);

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public sealed class P401Attribute() : ProducesResponseTypeAttribute(401);

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public sealed class P403Attribute() : ProducesResponseTypeAttribute(403);

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public sealed class P404Attribute() : ProducesResponseTypeAttribute(404);

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public sealed class P409Attribute() : ProducesResponseTypeAttribute(409);

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public sealed class P422Attribute() : ProducesResponseTypeAttribute(422);

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public sealed class P500Attribute() : ProducesResponseTypeAttribute(500);

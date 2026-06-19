// Import each Smoower.Minified package's extension namespace once, here.
global using Smoower.Minified.Core;
global using Smoower.Minified.AspNetCore;
global using Smoower.Minified.EFCore;
global using Smoower.Minified.Hosting;
global using Smoower.Minified.Logging;
global using Smoower.Minified.Validation;

// Aliases are not transitive across assemblies, so re-declare the set here.
// Open generics (Log<T>, AR<T>) cannot be aliased; closed Task<IActionResult> can.
global using Ctl = Microsoft.AspNetCore.Mvc.ControllerBase;
global using Res = Microsoft.AspNetCore.Mvc.IActionResult;
global using AR = Microsoft.AspNetCore.Mvc.ActionResult;
global using CT = System.Threading.CancellationToken;
global using Cfg = Microsoft.Extensions.Configuration.IConfiguration;
global using Tr = System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult>;

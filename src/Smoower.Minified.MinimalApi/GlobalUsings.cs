// Minimal API consumer aliases. Using aliases are not transitive across
// assemblies, so consuming projects re-declare these in their own GlobalUsings.cs.
//
//   R  — the Results static factory, for the hand-written returns the terminators
//        don't cover: R.BadRequest(), R.Unauthorized(), R.NoContent().
//   Ir — the async handler return type, the minimal-API counterpart to Tr:
//        async Ir Get(int id) => ...
global using R = Microsoft.AspNetCore.Http.Results;
global using Ir = System.Threading.Tasks.Task<Microsoft.AspNetCore.Http.IResult>;

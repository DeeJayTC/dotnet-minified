#!/usr/bin/env python3
"""Token deltas for *candidate* libraries we might wrap next.

Each row is a realistic call-site: (library, long form, compact candidate).
The compact form is hypothetical - the point is to see which wraps actually pay.
o200k_base proxy. Run: pip install tiktoken; python bench/candidates.py
"""
import tiktoken
enc = tiktoken.get_encoding("o200k_base")
def d(long, short): return len(enc.encode(long)) - len(enc.encode(short))

C = [
 ("System.Text.Json", 'JsonSerializer.Serialize(x)', 'x.toJson()'),
 ("System.Text.Json", 'JsonSerializer.Deserialize<T>(s)', 's.fromJson<T>()'),
 ("Newtonsoft.Json", 'JsonConvert.SerializeObject(x)', 'x.toJson()'),
 ("Newtonsoft.Json", 'JsonConvert.DeserializeObject<T>(s)', 's.fromJson<T>()'),
 ("Newtonsoft.Json", 'JsonConvert.SerializeObject(x, Formatting.Indented)', 'x.toJson(pretty:true)'),

 ("FluentValidation", 'RuleFor(x => x.Name).NotEmpty().MaximumLength(100);', 'req(x=>x.Name).max(100);'),
 ("FluentValidation", 'RuleFor(x => x.Email).NotEmpty().EmailAddress();', 'req(x=>x.Email).email();'),
 ("FluentValidation", 'RuleFor(x => x.Age).GreaterThan(0).LessThanOrEqualTo(120);', 'req(x=>x.Age).gt(0).lte(120);'),

 ("Dapper", 'await conn.QueryAsync<User>(sql, p)', 'await conn.q<User>(sql, p)'),
 ("Dapper", 'await conn.QueryFirstOrDefaultAsync<User>(sql, p)', 'await conn.q1<User>(sql, p)'),
 ("Dapper", 'await conn.ExecuteAsync(sql, p)', 'await conn.ex(sql, p)'),
 ("Dapper", 'await conn.ExecuteScalarAsync<int>(sql, p)', 'await conn.scalar<int>(sql, p)'),

 ("AutoMapper", 'CreateMap<User, UserDto>();', 'map<User, UserDto>();'),
 ("AutoMapper", '_mapper.Map<UserDto>(user)', 'user.to<UserDto>()'),
 ("AutoMapper", 'config.CreateMap<User, UserDto>().ReverseMap();', 'map<User, UserDto>(both:true);'),

 ("MediatR", 'public class GetUser : IRequest<UserDto>', 'public class GetUser : Req<UserDto>'),
 ("MediatR", 'public async Task<UserDto> Handle(GetUser r, CancellationToken ct)', 'public async Task<UserDto> Handle(GetUser r, CT ct)'),
 ("MediatR", 'await _mediator.Send(new GetUser(id))', 'await m.send(new GetUser(id))'),

 ("FluentAssertions", 'result.Should().Be(expected);', 'result.shouldBe(expected);'),
 ("FluentAssertions", 'result.Should().NotBeNull();', 'result.shouldNotBeNull();'),
 ("FluentAssertions", 'act.Should().Throw<InvalidOperationException>();', 'act.shouldThrow<InvalidOperationException>();'),
 ("xUnit", 'Assert.Equal(expected, actual);', 'eq(expected, actual);'),
 ("xUnit", 'Assert.NotNull(result);', 'notNull(result);'),

 ("Polly", 'Policy.Handle<HttpRequestException>().WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(1))', 'retry<HttpRequestException>(3, 1)'),

 ("Swashbuckle", '[ProducesResponseType(StatusCodes.Status200OK)]', '[P200]'),
 ("Swashbuckle", '[ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]', '[P200<UserDto>]'),
 ("Swashbuckle", '[ProducesResponseType(StatusCodes.Status404NotFound)]', '[P404]'),

 ("Serilog/ILogger", 'Log.Information("created {Id}", id)', 'Log.inf("created {Id}", id)'),

 ("WPF (DependencyProperty)", 'public static readonly DependencyProperty FooProperty = DependencyProperty.Register(nameof(Foo), typeof(string), typeof(MyControl), new PropertyMetadata(null));', '[Dep] public string Foo { get; set; }'),
 ("WPF/MVVM", 'public string Name { get => _name; set { _name = value; OnPropertyChanged(); } }', '[ObservableProperty] string _name;'),

 ("EF provider setup", 'options.UseSqlServer(connectionString)', 'options.sql(connectionString)'),
 ("EF provider setup", 'options.UseNpgsql(connectionString)', 'options.pg(connectionString)'),
 ("ADO (SqlClient)", 'await using var cmd = new SqlCommand(sql, conn); var r = await cmd.ExecuteReaderAsync();', '// use Dapper instead'),
 ("Polly v8", 'new ResiliencePipelineBuilder().AddRetry(new RetryStrategyOptions { MaxRetryAttempts = 3 }).Build()', 'retry(3)'),
 ("OpenTelemetry", 'activity?.SetTag("user.id", id)', 'a?.tag("user.id", id)'),
 ("OpenTelemetry", 'using var activity = ActivitySource.StartActivity("GetUser")', 'using var a = src.span("GetUser")'),
 ("Google.Protobuf", 'GetUserResponse.Parser.ParseFrom(bytes)', 'bytes.proto<GetUserResponse>()'),
 ("Google.Protobuf", 'message.ToByteArray()', 'message.bytes()'),
 ("gRPC client", 'await client.GetUserAsync(new GetUserRequest { Id = id })', 'await client.GetUserAsync(new GetUserRequest { Id = id })'),
]

print(f"{'library':26}{'long':>5}{'short':>6}{'save':>6}  example")
by_lib = {}
for lib, long, short in C:
    delta = d(long, short)
    by_lib.setdefault(lib, []).append(delta)
    ex = long if len(long) < 42 else long[:39] + '...'
    print(f"{lib:26}{len(enc.encode(long)):5}{len(enc.encode(short)):6}{delta:+6}  {ex}")

print(f"\n{'library':26}{'avg save/call':>14}")
for lib, ds in by_lib.items():
    print(f"{lib:26}{sum(ds)/len(ds):+14.1f}")

var builder = WebApplication.CreateBuilder(args);


//Features in Gateway:

//1. Centralized JWT token validation
//2. Global rate limiting (via middleware)
//3. CORS enforcement
//4. Request/response logging
//5. Circuit breakers and retries (via Polly or YARP features)

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://localhost:5001"; // Auth service
        options.Audience = "sms-api";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapReverseProxy();

app.MapControllers();

app.Run();




 

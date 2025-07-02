var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var apiService = builder.AddProject<Projects.SMS_ApiService>("apiservice");

builder.AddProject<Projects.SMS_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();


//IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

//var postgres = builder.AddPostgres("contentplatform-db")
//    .WithPgAdmin();

//var rabbitMq = builder.AddRabbitMQ("contentplatform-mq")
//    .WithManagementPlugin();

//builder.AddProject<Projects.ContentPlatform_Api>("contentplatform-api")
//    .WithReference(postgres)
//    .WithReference(rabbitMq);

//builder.AddProject<Projects.ContentPlatform_Reporting_Api>("contentplatform-reporting-api")
//    .WithReference(postgres)
//    .WithReference(rabbitMq);

//builder.AddProject<Projects.ContentPlatform_Presentation>("contentplatform-presentation");

//builder.Build().Run();

//-----------------------------

//In your app that access db:

//builder.Services.AddDbContext<ApplicationDbContext>(o =>
//    o.UseNpgsql(builder.Configuration.GetConnectionString("contentplatform-db")));
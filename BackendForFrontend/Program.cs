using BackendForFrontend.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

builder.Services.AddSingleton<IPoeClient, PoeClient>();
builder.Services.AddHttpClient<IPoeClient, PoeClient>(client => {
    client.BaseAddress = new("https://www.pathofexile.com");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(cors => cors
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowAnyOrigin()
);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

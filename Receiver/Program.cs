using Receiver.Infra;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region [RabbitMQ]
builder.Services.AddMessaging(GetConfig<BusConfiguration>());
#endregion


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();


T GetConfig<T>(string section = null)
{
    T tObject = (T)Activator.CreateInstance(typeof(T));

    if (string.IsNullOrEmpty(section))
    {
        section = tObject.GetType().Name;
    }

    builder.Configuration.GetSection(section).Bind(tObject);
    return tObject;
}

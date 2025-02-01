using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SystemPayment.API.DataModels;
using SystemPayment.API.DataModels.Extensions;
using SystemPayment.API.Middleware;
using SystemPayment.API.Repositories.Implementation;
using SystemPayment.API.Repositories.Interface;
using SystemPayment.API.Response;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddDbContext<ApplicationDbContext>
	(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
	options.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
	options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(sw =>
{
	sw.SwaggerDoc("v1", new OpenApiInfo { Title = "Payment API", Version = "v1" });
});

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPaymentTypeRepository, PaymentTypeRepository>();
builder.Services.AddScoped<IBranchRepository, BranchRepository>();
builder.Services.AddScoped<IEducationTypeRepository, EducationTypeRepository>();
builder.Services.AddScoped<IPaymentSettingRepository, PaymentSettingRepository>();

builder.Services.AddAutoMapper(typeof(Program).Assembly);


builder.Services.Configure<ApiBehaviorOptions>(opt =>
{
	opt.InvalidModelStateResponseFactory = actionContext =>
	{
		var errors = actionContext.ModelState
			.Where(e => e.Value.Errors.Count > 0)
			.SelectMany(x => x.Value.Errors)
			.Select(x => x.ErrorMessage)
			.ToArray();


		var errorResponse = new ApiResponse<object>(errors, StatusCodes.Status400BadRequest);

		return new BadRequestObjectResult(errorResponse);
	};
});

var app = builder.Build();

await app.InitialiseDatabaseAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();

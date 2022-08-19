using System.Net;
using System.Reflection;
using Library.Context;
using Library.Helpers;
using Library.Repositories.Authors;
using Library.Repositories.Books;
using Library.Repositories.Customers;
using Library.Repositories.Rentals;
using Library.Repositories.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using ISession = Library.Helpers.ISession;

namespace Library
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.AddHttpContextAccessor();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var actionExecutingContext =
                        actionContext as Microsoft.AspNetCore.Mvc.Filters.ActionExecutedContext;
                    if (actionContext.ModelState.ErrorCount > 0
                        && actionExecutingContext?.ActionDescriptor.Parameters.Count ==
                        actionContext.ActionDescriptor.Parameters.Count)
                    {
                        return new UnprocessableEntityObjectResult(actionContext.ModelState);
                    }

                    return new BadRequestObjectResult(actionContext.ModelState);
                };
            });

            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromHours(10);//You can set Time   
            });
            services.AddMvc(setupAction =>
            {
                setupAction.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status400BadRequest));
                setupAction.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status406NotAcceptable));
                setupAction.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError));
                setupAction.Filters.Add(new ProducesDefaultResponseTypeAttribute());
                setupAction.ReturnHttpNotAcceptable = true;
                setupAction.Filters.Add(new ProducesAttribute("application/json", "application/xml"));
                setupAction.OutputFormatters.Add(new XmlSerializerOutputFormatter());
                var jsonOutputFormatter = setupAction.OutputFormatters.OfType<SystemTextJsonOutputFormatter>().FirstOrDefault();
                if (jsonOutputFormatter != null)
                {
                    if (jsonOutputFormatter.SupportedMediaTypes.Contains("text/json"))
                    {
                        jsonOutputFormatter.SupportedMediaTypes.Remove("text/json");
                    }
                }
            });

            var connectionString = Configuration["ConnectionStrings:Library"];

            services.AddDbContext<AppDbContext>(c => c.UseSqlServer(connectionString));

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<ISession, Session>();
            services.AddScoped<IUserAccess, UserAccess>();
            services.AddScoped<IRentalRepository, RentalRepository>();

            services.AddHttpClient("myServiceClient")
                .ConfigureHttpClient(client =>
                {
                    client.BaseAddress = new Uri("https://localhost:44384/api");
                    client.Timeout = TimeSpan.FromSeconds(5);
                })
                .ConfigurePrimaryHttpMessageHandler(
                    () => new HttpClientHandler() { CookieContainer = new CookieContainer() }
                );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseSession();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

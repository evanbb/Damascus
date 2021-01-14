using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Damascus.Example.Infrastructure;
using System.Collections.Generic;
using Damascus.Example.Contracts;
using static Damascus.Example.Infrastructure.RamMessageBus;

namespace Damascus.Example.Api
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
            services.AddControllers(opts =>
            {
                //opts.Filters.Add<ExceptionMappingFilter>();
            }).AddJsonOptions(cfg =>
            {
                cfg.JsonSerializerOptions.Converters.Add(new BookmarkItemConverter());
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api", Version = "v1" });
            });

            services.AddSingleton<IBookmarksQueryRepo, BookmarksCollectionQueryRepo>();
            services.AddSingleton<IMessageBus>(ctxt => new RamMessageBus(new Dictionary<string, IEnumerable<MessageHandler>>
            {
                {
                    typeof(BookmarksCollection).FullName,
                    new MessageHandler[]
                    {
                        e => ctxt.GetRequiredService<IBookmarksQueryRepo>().Handle(e)
                    }
                }
            }));
            services.AddSingleton<IMutableBookmarksCommandRepo, MutableBookmarksCommandRepo>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api v1"));
            }

            //app.UseExceptionStatusMap();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

# ExceptionHandler
 This is a global exception handler library for ASP.NET Core projects.
 
 Usage:
 
        public void ConfigureServices(IServiceCollection services)
        {
            ....................
            services.RegisterTanvirArjelExceptionHandler(connectionString);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseTanvirArjelExceptionHandler("/Home/Error");
            }
            else
            {
                app.UseTanvirArjelExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            ...............
        }

# ExceptionHandler
 This is a global exception handler library for ASP.NET Core projects.
 
 ### How do I get started?
 
 Configuring **TanvirArjel.ExceptionHandler** into your ASP.NET Core project is as simple as below:
 
 1. First install the `TanvirArjel.ExceptionHandler` nuget package into your project as follows:
 
    `Install-Package TanvirArjel.ExceptionHandler`
    
 2. Then in your `startup` class register the `TanvirArjel.ExceptionHandler` as follows:
 
        public void ConfigureServices(IServiceCollection services)
        {
            ....................
            services.RegisterTanvirArjelExceptionHandler(connectionString); <-- Here it is
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseTanvirArjelExceptionHandler("/Home/Error"); <-- Add here if you want to enable it in your development environment.
            }
            else
            {
                app.UseTanvirArjelExceptionHandler("/Home/Error"); <-- Add here if you want to enable it in your other environments except development.
                app.UseHsts();
            }

            ...............
        }
        
  **You are done!**

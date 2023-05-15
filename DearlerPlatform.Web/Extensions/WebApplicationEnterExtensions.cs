namespace DearlerPlatform.Web.Extensions
{
    public static class WebApplicationEnterExtensions
    {
        public static void initEnter(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();


            app.UseCors("any");
            //使用鉴权
            app.UseAuthentication();

            app.UseAuthorization();

            //app.UseHttpsRedirection();
        }

        public static void initMap(this IEndpointRouteBuilder app) 
        {
            app.MapControllers();
        }
    }
}

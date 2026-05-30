namespace Auth.API;

public class Program
{
    public static void Main( string[] args )
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder( args );

        WebApplication app = builder.Build();

        app.UseHttpsRedirection();

        app.Run();
    }
}

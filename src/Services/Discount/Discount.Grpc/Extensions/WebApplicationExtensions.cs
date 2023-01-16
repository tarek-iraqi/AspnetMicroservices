using Npgsql;

namespace Discount.Grpc.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication MigrateDatabase<TContext>(this WebApplication webApplication, int? retry = 0)
    {
        using var scope = webApplication.Services.CreateScope();

        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<TContext>>();

        try
        {
            logger.LogInformation("Start migrate postgres database");

            using var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            connection.Open();

            var command = new NpgsqlCommand { Connection = connection };

            command.CommandText = "DROP TABLE IF EXISTS Coupon;";
            command.ExecuteNonQuery();

            command.CommandText = @"CREATE TABLE Coupon(
		                            ID SERIAL PRIMARY KEY NOT NULL,
		                            ProductName VARCHAR(24) NOT NULL,
		                            Description TEXT,
		                            Amount INT);";
            command.ExecuteNonQuery();

            command.CommandText = "INSERT INTO Coupon (ProductName, Description, Amount) VALUES ('IPhone X', 'IPhone Discount', 150);";
            command.ExecuteNonQuery();

            command.CommandText = "INSERT INTO Coupon (ProductName, Description, Amount) VALUES ('Samsung 10', 'Samsung Discount', 100);";
            command.ExecuteNonQuery();

            logger.LogInformation("Finish migrate postgres database");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occured while migrating postgres database");

            if (retry < 50)
            {
                var retryMigrationCount = retry++;
                Thread.Sleep(2000);
                MigrateDatabase<TContext>(webApplication, retryMigrationCount);
            }
        }

        return webApplication;
    }
}

namespace dotnet_ef_rpg.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options) // ? what does base(options) do here
    {
    }

    /**
     * To be able to query and save RPG characters
     * The name will be the name of the corresponding db table
     */
    public DbSet<Character> Characters => Set<Character>();
}
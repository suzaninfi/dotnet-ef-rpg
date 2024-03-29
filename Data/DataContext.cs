﻿namespace dotnet_ef_rpg.Data;

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

    public DbSet<User> Users => Set<User>();

    public DbSet<Weapon> Weapons => Set<Weapon>();

    public DbSet<Skill> Skills => Set<Skill>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Skill>().HasData(
            new Skill { Id = 1, Name = "Fireball", Damage = 30 },
            new Skill { Id = 2, Name = "Frenzy", Damage = 20 },
            new Skill { Id = 3, Name = "Blizzard", Damage = 50 }
        );
    }
}
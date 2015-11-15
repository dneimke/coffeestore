using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using EF7Demo.CoffeeStore.Model;

namespace EF7Demo.CoffeeStore.Migrations
{
    [DbContext(typeof(CoffeeStoreContext))]
    [Migration("20151115000825_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc2-16340")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EF7Demo.CoffeeStore.Model.Coffee", b =>
                {
                    b.Property<int>("Coffee_Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("ImageUrl");

                    b.Property<string>("Name");

                    b.Property<decimal>("Retail");

                    b.HasKey("Coffee_Id");
                });

            modelBuilder.Entity("EF7Demo.CoffeeStore.Model.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.HasKey("Id");
                });
        }
    }
}

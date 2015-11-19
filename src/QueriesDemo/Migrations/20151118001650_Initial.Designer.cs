using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using ClassLibrary1.Models;

namespace QueriesDemo.Migrations
{
    [DbContext(typeof(CoffeeContext))]
    [Migration("20151118001650_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc2-16340")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ClassLibrary1.Models.Coffee", b =>
                {
                    b.Property<int>("Coffee_Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<int?>("ImageId");

                    b.Property<DateTime>("LastModified");

                    b.Property<string>("Name");

                    b.Property<decimal>("Price");

                    b.HasKey("Coffee_Id");
                });

            modelBuilder.Entity("ClassLibrary1.Models.CoffeeCustomerOrder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CoffeeId");

                    b.Property<int>("CustomerOrderId");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("ClassLibrary1.Models.CustomerOrder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("TradingDayId");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("ClassLibrary1.Models.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Coffee_Id");

                    b.Property<string>("Description");

                    b.Property<string>("ImageUrl");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("ClassLibrary1.Models.Store", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("ClassLibrary1.Models.TradingDay", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Day");

                    b.Property<int>("StoreId");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("ClassLibrary1.Models.Coffee", b =>
                {
                    b.HasOne("ClassLibrary1.Models.Image")
                        .WithOne()
                        .HasForeignKey("ClassLibrary1.Models.Coffee", "ImageId");
                });

            modelBuilder.Entity("ClassLibrary1.Models.CoffeeCustomerOrder", b =>
                {
                    b.HasOne("ClassLibrary1.Models.Coffee")
                        .WithMany()
                        .HasForeignKey("CoffeeId");

                    b.HasOne("ClassLibrary1.Models.CustomerOrder")
                        .WithMany()
                        .HasForeignKey("CustomerOrderId");
                });

            modelBuilder.Entity("ClassLibrary1.Models.CustomerOrder", b =>
                {
                    b.HasOne("ClassLibrary1.Models.TradingDay")
                        .WithMany()
                        .HasForeignKey("TradingDayId");
                });

            modelBuilder.Entity("ClassLibrary1.Models.TradingDay", b =>
                {
                    b.HasOne("ClassLibrary1.Models.Store")
                        .WithMany()
                        .HasForeignKey("StoreId");
                });
        }
    }
}

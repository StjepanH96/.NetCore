﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SolarApp.Data;

#nullable disable

namespace mySolarApi.Migrations
{
    [DbContext(typeof(SolarDbContext))]
    [Migration("20241201191527_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("SolarApp.Models.ProductionData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Production")
                        .HasColumnType("TEXT");

                    b.Property<int>("SolarPowerPlantId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("SolarPowerPlantId1")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SolarPowerPlantName")
                        .HasColumnType("TEXT");

                    b.Property<string>("TimeseriesType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("SolarPowerPlantId");

                    b.HasIndex("SolarPowerPlantId1");

                    b.ToTable("ProductionData");
                });

            modelBuilder.Entity("SolarApp.Models.SolarPowerPlant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateOfInstallation")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("InstalledPower")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Latitude")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Longitude")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("SolarPowerPlants");
                });

            modelBuilder.Entity("SolarApp.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SolarApp.Models.ProductionData", b =>
                {
                    b.HasOne("SolarApp.Models.SolarPowerPlant", null)
                        .WithMany()
                        .HasForeignKey("SolarPowerPlantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SolarApp.Models.SolarPowerPlant", null)
                        .WithMany("ProductionData")
                        .HasForeignKey("SolarPowerPlantId1");
                });

            modelBuilder.Entity("SolarApp.Models.SolarPowerPlant", b =>
                {
                    b.Navigation("ProductionData");
                });
#pragma warning restore 612, 618
        }
    }
}

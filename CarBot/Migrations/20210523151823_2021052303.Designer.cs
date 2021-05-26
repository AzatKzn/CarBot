﻿// <auto-generated />
using System;
using CarBot.DBContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CarBot.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20210523151823_2021052303")]
    partial class _2021052303
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.5");

            modelBuilder.Entity("CarBot.Models.Auto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Braking")
                        .HasColumnType("int");

                    b.Property<int>("Cost")
                        .HasColumnType("int");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsShow")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("Mobility")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<int>("Overclocking")
                        .HasColumnType("int");

                    b.Property<string>("Property")
                        .HasColumnType("longtext");

                    b.Property<int>("PropertyValue")
                        .HasColumnType("int");

                    b.Property<int>("Speed")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Autos");
                });

            modelBuilder.Entity("CarBot.Models.GroupRace", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Hash")
                        .HasColumnType("longtext");

                    b.Property<int>("RaceDivision")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("GroupRaces");
                });

            modelBuilder.Entity("CarBot.Models.GroupRaceParticipant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Experience")
                        .HasColumnType("int");

                    b.Property<int?>("GroupRaceId")
                        .HasColumnType("int");

                    b.Property<int>("Money")
                        .HasColumnType("int");

                    b.Property<int>("Place")
                        .HasColumnType("int");

                    b.Property<int>("Score")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("GroupRaceId");

                    b.HasIndex("UserId");

                    b.ToTable("GroupRaceParticipant");
                });

            modelBuilder.Entity("CarBot.Models.History", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("ActionType")
                        .HasColumnType("int");

                    b.Property<int?>("CarId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("CarId");

                    b.HasIndex("UserId");

                    b.ToTable("Histories");
                });

            modelBuilder.Entity("CarBot.Models.RewardHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Cost")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("FactUp")
                        .HasColumnType("int");

                    b.Property<Guid>("Guid")
                        .HasColumnType("char(36)");

                    b.Property<bool>("IsExp")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RewardHistories");
                });

            modelBuilder.Entity("CarBot.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("Attentiveness")
                        .HasColumnType("int");

                    b.Property<int>("Courage")
                        .HasColumnType("int");

                    b.Property<int>("Cunning")
                        .HasColumnType("int");

                    b.Property<long>("Experience")
                        .HasColumnType("bigint");

                    b.Property<int>("GroupRaceCount")
                        .HasColumnType("int");

                    b.Property<int>("GroupRaceVictories")
                        .HasColumnType("int");

                    b.Property<string>("Login")
                        .HasColumnType("longtext");

                    b.Property<int>("Luck")
                        .HasColumnType("int");

                    b.Property<long>("Money")
                        .HasColumnType("bigint");

                    b.Property<int>("RaceCountWithAI")
                        .HasColumnType("int");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("ScoreInGroupRace")
                        .HasColumnType("int");

                    b.Property<int>("SpeedReaction")
                        .HasColumnType("int");

                    b.Property<int>("TestDrivesCount")
                        .HasColumnType("int");

                    b.Property<int>("VictoriesWithAI")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CarBot.Models.UserCar", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("AutoId")
                        .HasColumnType("int");

                    b.Property<DateTime>("BuyDate")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<float>("Strength")
                        .HasColumnType("float");

                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("AutoId");

                    b.HasIndex("UserId");

                    b.ToTable("Cars");
                });

            modelBuilder.Entity("CarBot.Models.GroupRaceParticipant", b =>
                {
                    b.HasOne("CarBot.Models.GroupRace", "GroupRace")
                        .WithMany("Participants")
                        .HasForeignKey("GroupRaceId");

                    b.HasOne("CarBot.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("GroupRace");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CarBot.Models.History", b =>
                {
                    b.HasOne("CarBot.Models.UserCar", "Car")
                        .WithMany()
                        .HasForeignKey("CarId");

                    b.HasOne("CarBot.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Car");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CarBot.Models.RewardHistory", b =>
                {
                    b.HasOne("CarBot.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CarBot.Models.UserCar", b =>
                {
                    b.HasOne("CarBot.Models.Auto", "Auto")
                        .WithMany()
                        .HasForeignKey("AutoId");

                    b.HasOne("CarBot.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Auto");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CarBot.Models.GroupRace", b =>
                {
                    b.Navigation("Participants");
                });
#pragma warning restore 612, 618
        }
    }
}

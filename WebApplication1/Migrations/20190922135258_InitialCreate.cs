using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LR3Proj.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bank",
                columns: table => new
                {
                    BankID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Address = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bank", x => x.BankID);
                });

            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    ClientID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(nullable: false),
                    ClientType = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.ClientID);
                });

            migrationBuilder.CreateTable(
                name: "Condition",
                columns: table => new
                {
                    ConditionID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Mark = table.Column<string>(nullable: false),
                    MaxSum = table.Column<int>(nullable: false),
                    MinSum = table.Column<int>(nullable: false),
                    Special = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Condition", x => x.ConditionID);
                });

            migrationBuilder.CreateTable(
                name: "OfferType",
                columns: table => new
                {
                    OfferTypeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferType", x => x.OfferTypeID);
                });

            migrationBuilder.CreateTable(
                name: "Offer",
                columns: table => new
                {
                    OfferID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BankID = table.Column<int>(nullable: false),
                    ConditionID = table.Column<int>(nullable: false),
                    OfferTypeID = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Percentage = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offer", x => x.OfferID);
                    table.ForeignKey(
                        name: "FK_Offer_Bank_BankID",
                        column: x => x.BankID,
                        principalTable: "Bank",
                        principalColumn: "BankID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Offer_Condition_ConditionID",
                        column: x => x.ConditionID,
                        principalTable: "Condition",
                        principalColumn: "ConditionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Offer_OfferType_OfferTypeID",
                        column: x => x.OfferTypeID,
                        principalTable: "OfferType",
                        principalColumn: "OfferTypeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contract",
                columns: table => new
                {
                    ContractID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OfferID = table.Column<int>(nullable: false),
                    ClientID = table.Column<int>(nullable: false),
                    Sum = table.Column<float>(nullable: false),
                    SigningDate = table.Column<DateTime>(nullable: false),
                    FinishDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contract", x => x.ContractID);
                    table.ForeignKey(
                        name: "FK_Contract_Client_ClientID",
                        column: x => x.ClientID,
                        principalTable: "Client",
                        principalColumn: "ClientID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contract_Offer_OfferID",
                        column: x => x.OfferID,
                        principalTable: "Offer",
                        principalColumn: "OfferID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contract_ClientID",
                table: "Contract",
                column: "ClientID");

            migrationBuilder.CreateIndex(
                name: "IX_Contract_OfferID",
                table: "Contract",
                column: "OfferID");

            migrationBuilder.CreateIndex(
                name: "IX_Offer_BankID",
                table: "Offer",
                column: "BankID");

            migrationBuilder.CreateIndex(
                name: "IX_Offer_ConditionID",
                table: "Offer",
                column: "ConditionID");

            migrationBuilder.CreateIndex(
                name: "IX_Offer_OfferTypeID",
                table: "Offer",
                column: "OfferTypeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contract");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "Offer");

            migrationBuilder.DropTable(
                name: "Bank");

            migrationBuilder.DropTable(
                name: "Condition");

            migrationBuilder.DropTable(
                name: "OfferType");
        }
    }
}

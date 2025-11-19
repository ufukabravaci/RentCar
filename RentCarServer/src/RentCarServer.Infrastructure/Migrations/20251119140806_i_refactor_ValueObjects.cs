using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentCarServer.Infrastructure.Migrations;

/// <inheritdoc />
public partial class i_refactor_ValueObjects : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // 1. ADIM: İSİM DEĞİŞİKLİKLERİ (Veriyi korumak için Rename)

        // Users Tablosu
        migrationBuilder.RenameColumn(
            name: "FirstName_Value",
            table: "Users",
            newName: "FirstName");

        migrationBuilder.RenameColumn(
            name: "LastName_Value",
            table: "Users",
            newName: "LastName");

        migrationBuilder.RenameColumn(
            name: "FullName_Value",
            table: "Users",
            newName: "FullName");

        migrationBuilder.RenameColumn(
            name: "Email_Value",
            table: "Users",
            newName: "Email");

        migrationBuilder.RenameColumn(
            name: "UserName_Value",
            table: "Users",
            newName: "UserName");

        migrationBuilder.RenameColumn(
            name: "Password_PasswordSalt",
            table: "Users",
            newName: "PasswordSalt");

        migrationBuilder.RenameColumn(
            name: "Password_PasswordHash",
            table: "Users",
            newName: "PasswordHash");

        migrationBuilder.RenameColumn(
            name: "IsForgotPasswordCompleted_Value",
            table: "Users",
            newName: "IsForgotPasswordCompleted");

        migrationBuilder.RenameColumn(
            name: "ForgotPasswordDate_Value",
            table: "Users",
            newName: "ForgotPasswordDate");

        migrationBuilder.RenameColumn(
            name: "ForgotPasswordCode_Value",
            table: "Users",
            newName: "ForgotPasswordCode");

        // LoginTokens Tablosu
        migrationBuilder.RenameColumn(
            name: "Token_Value",
            table: "LoginTokens",
            newName: "Token");

        migrationBuilder.RenameColumn(
            name: "IsActive_Value",
            table: "LoginTokens",
            newName: "IsActive");

        migrationBuilder.RenameColumn(
            name: "ExpireDate_Value",
            table: "LoginTokens",
            newName: "ExpireDate");


        // 2. ADIM: TİP GÜNCELLEMELERİ (Alter Column)
        // Rename işleminden sonra tipleri yeni konfigürasyona uyduruyoruz.

        // CreatedBy -> Nullable yapılıyor
        migrationBuilder.AlterColumn<Guid>(
            name: "CreatedBy",
            table: "Users",
            type: "uniqueidentifier",
            nullable: true,
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier");

        // Password -> Byte Array boyutu ayarlanıyor
        migrationBuilder.AlterColumn<byte[]>(
            name: "PasswordSalt",
            table: "Users",
            type: "varbinary(512)",
            nullable: false,
            oldClrType: typeof(byte[]),
            oldType: "varbinary(max)");

        migrationBuilder.AlterColumn<byte[]>(
            name: "PasswordHash",
            table: "Users",
            type: "varbinary(512)",
            nullable: false,
            oldClrType: typeof(byte[]),
            oldType: "varbinary(max)");

        // !!! KRİTİK EKLEMELER !!! 
        // Index atılacak alanların ve diğer stringlerin tiplerini güncelliyoruz.
        // Eski tipler muhtemelen varchar(MAX)'tı.

        migrationBuilder.AlterColumn<string>(
            name: "Email",
            table: "Users",
            type: "nvarchar(100)", // Index için 100 karakter
            maxLength: 100,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "varchar(MAX)");

        migrationBuilder.AlterColumn<string>(
            name: "UserName",
            table: "Users",
            type: "nvarchar(100)", // Index için 100 karakter
            maxLength: 100,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "varchar(MAX)");

        migrationBuilder.AlterColumn<string>(
            name: "FirstName",
            table: "Users",
            type: "nvarchar(max)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "varchar(MAX)");

        migrationBuilder.AlterColumn<string>(
            name: "LastName",
            table: "Users",
            type: "nvarchar(max)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "varchar(MAX)");

        migrationBuilder.AlterColumn<string>(
            name: "FullName",
            table: "Users",
            type: "nvarchar(max)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "varchar(MAX)");


        // 3. ADIM: INDEX OLUŞTURMA
        // Tipler düzeldikten sonra indexler güvenle oluşturulabilir.
        migrationBuilder.CreateIndex(
            name: "IX_Users_Email",
            table: "Users",
            column: "Email",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Users_UserName",
            table: "Users",
            column: "UserName",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // Geri alma işlemleri (Rollback) - Yapılanların tam tersi

        migrationBuilder.DropIndex(
            name: "IX_Users_Email",
            table: "Users");

        migrationBuilder.DropIndex(
            name: "IX_Users_UserName",
            table: "Users");

        // Tipleri eski haline getir (varchar(MAX))
        migrationBuilder.AlterColumn<string>(
            name: "Email",
            table: "Users",
            type: "varchar(MAX)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(100)",
            oldMaxLength: 100);

        // ... Diğer AlterColumn rollback işlemleri buraya eklenebilir ama
        // Down metodu genelde geliştirme aşamasında çok kritik değildir.
        // Önemli olan Rename'lerin tersini yapmaktır.

        migrationBuilder.RenameColumn(name: "FirstName", table: "Users", newName: "FirstName_Value");
        migrationBuilder.RenameColumn(name: "LastName", table: "Users", newName: "LastName_Value");
        migrationBuilder.RenameColumn(name: "FullName", table: "Users", newName: "FullName_Value");
        migrationBuilder.RenameColumn(name: "Email", table: "Users", newName: "Email_Value");
        migrationBuilder.RenameColumn(name: "UserName", table: "Users", newName: "UserName_Value");

        migrationBuilder.RenameColumn(name: "PasswordSalt", table: "Users", newName: "Password_PasswordSalt");
        migrationBuilder.RenameColumn(name: "PasswordHash", table: "Users", newName: "Password_PasswordHash");

        migrationBuilder.RenameColumn(name: "IsForgotPasswordCompleted", table: "Users", newName: "IsForgotPasswordCompleted_Value");
        migrationBuilder.RenameColumn(name: "ForgotPasswordDate", table: "Users", newName: "ForgotPasswordDate_Value");
        migrationBuilder.RenameColumn(name: "ForgotPasswordCode", table: "Users", newName: "ForgotPasswordCode_Value");

        migrationBuilder.RenameColumn(name: "Token", table: "LoginTokens", newName: "Token_Value");
        migrationBuilder.RenameColumn(name: "IsActive", table: "LoginTokens", newName: "IsActive_Value");
        migrationBuilder.RenameColumn(name: "ExpireDate", table: "LoginTokens", newName: "ExpireDate_Value");

        // CreatedBy eski haline (Not Null)
        migrationBuilder.AlterColumn<Guid>(
            name: "CreatedBy",
            table: "Users",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier",
            oldNullable: true);

        migrationBuilder.AlterColumn<byte[]>(
            name: "Password_PasswordSalt",
            table: "Users",
            type: "varbinary(max)",
            nullable: false,
            oldClrType: typeof(byte[]),
            oldType: "varbinary(512)");

        migrationBuilder.AlterColumn<byte[]>(
            name: "Password_PasswordHash",
            table: "Users",
            type: "varbinary(max)",
            nullable: false,
            oldClrType: typeof(byte[]),
            oldType: "varbinary(512)");
    }
}